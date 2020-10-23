using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class FadeToPoint : MonoBehaviour
{
    [Tooltip("OVRCameraRig w OVRScreenFade on player head")]
    [SerializeField] OVRPlayerController startRig = null;

    [Tooltip("Where to place player")]
    [SerializeField] GameObject endPosition = null;
    [SerializeField] UnityEvent eventAfterMove = null;

    [Tooltip("Total time for transition to happen")]
    [SerializeField] float moveTimer = 1.5f;

    [SerializeField] bool reEnableController = true;
    private bool moved = false;


    // Start is called before the first frame update
    void Start()
    {
        CheckforRequiredComponents();
    }

    //looks for needed scripts in children and variables in script
    private void CheckforRequiredComponents()
    {
        if (startRig == null || endPosition == null)
            throw new System.Exception(gameObject.name + " rig or end position not set!");

        if (startRig.GetComponentInChildren<Camera>() == null)
            throw new System.Exception(gameObject.name + " player head/camera not found!");

        if (startRig.GetComponentInChildren<OVRScreenFade>() == null)
            throw new System.Exception(gameObject.name + " no OVRScreenFade object attached to camera!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!moved && other.gameObject.tag.Equals("Player"))
        {
            StartCoroutine(GoToPoint());
            moved = true;
        }
    }

    IEnumerator GoToPoint()
    {
        var fade = startRig.GetComponentInChildren<OVRScreenFade>();

        //divide fade and movement into 3 chunks (fade-in, wait, fade-out)
        fade.fadeTime = moveTimer / 3;

        //start fading and disable controller if there is one
        fade.FadeOut();
        if (startRig.GetComponent<CharacterController>() != null)
            startRig.GetComponent<CharacterController>().enabled = false;

        //fade out to black
        yield return new WaitForSeconds(fade.fadeTime * 2);

        //move rig to point w rotation
        var camPos = startRig.GetComponentInChildren<Camera>().transform.position;
        var offset = new Vector3(camPos.x - startRig.transform.position.x, 0, camPos.z - startRig.transform.position.z);

        startRig.transform.position = endPosition.transform.position - offset;
        startRig.transform.rotation = endPosition.transform.rotation;
        startRig.transform.parent = endPosition.transform;

        //fade in and wait
        fade.FadeIn();
        yield return new WaitForSeconds(fade.fadeTime);

        //enable controller if there is one and if it should be re-enabled
        if (startRig.GetComponent<CharacterController>() != null)
            startRig.GetComponent<CharacterController>().enabled = reEnableController;

        //do fancy shit after you've moved if you want to, idk
        if (eventAfterMove != null)
            eventAfterMove.Invoke();
            
    }
}