using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIntoBoat : MonoBehaviour
{
    [SerializeField] OVRPlayerController startRig = null;
    [SerializeField] GameObject boatPosition = null;

    private bool movedToBoat = false;


    // Start is called before the first frame update
    void Start()
    {
        if (startRig == null || boatPosition == null)
            throw new System.Exception(gameObject.name + " null reference error");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!movedToBoat && other.gameObject.tag.Equals("Player"))
        {
            StartCoroutine(GoIntoBoat());
            movedToBoat = true;
        }
    }

    IEnumerator GoIntoBoat()
    {
        float fadeTimer = 1.5f;
        var fade = startRig.GetComponentInChildren<OVRScreenFade>();

        fade.fadeTime = fadeTimer / 3;

        fade.FadeIn();
        startRig.GetComponent<CharacterController>().enabled = false;       
        yield return new WaitForSeconds(fade.fadeTime * 2);

        var camPos = startRig.GetComponentInChildren<Camera>().transform.position;
        var offset = new Vector3 (camPos.x - startRig.transform.position.x, 0, camPos.z - startRig.transform.position.z);

        startRig.transform.position = boatPosition.transform.position/* + offset*/;
        startRig.transform.rotation = boatPosition.transform.rotation;
        startRig.transform.parent = boatPosition.transform;

        fade.FadeOut();
        //TODO play narrator soundclip and send boat sailing
    }
}
