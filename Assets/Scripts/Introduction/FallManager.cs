using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FallManager : MonoBehaviour
{

    private bool onBridge = true;
    private bool offBridgeLastFrame = false;
    private bool fallingOff = false;
    
    [SerializeField] private string tagString = "";

    [SerializeField] CharacterController rig;
    [SerializeField] Camera playerHead;

    OVRScreenFade fader = null;

    [SerializeField] float saveCooldown = 1.0f;
    private float maxSaveCooldown = 1.0f;

    public float fadeTime = 1.5f;

    [SerializeField] Transform bridgeMiddle = null;

    Transform lastSafeLocationDebugTransform = null;
    Vector3 lastSafeLocation = Vector3.zero;
    Vector3 lastSafeLocationOffset = Vector3.zero;

    Transform nextSafeLocationDebugTransform = null;
    Vector3 nextSafeLocation = Vector3.zero;
    Vector3 nextSafeLocationOffset = Vector3.zero;


    private void Start()
    {
        maxSaveCooldown = saveCooldown;

        if (rig == null)
            rig = this.GetComponent<CharacterController>();

        if (playerHead == null)
            playerHead = GameObject.Find("CenterEyeAnchor").GetComponent<Camera>();

        if (fader == null)
            fader = playerHead.GetComponent<OVRScreenFade>();


        if (lastSafeLocationDebugTransform == null)
            lastSafeLocationDebugTransform = GameObject.Find("SafeLocation").GetComponent<Transform>();

        if (nextSafeLocationDebugTransform == null)
            nextSafeLocationDebugTransform = GameObject.Find("NextSafeLocation").GetComponent<Transform>();

        nextSafeLocation = rig.transform.position;
        nextSafeLocationOffset = CalculateOffset();
        lastSafeLocation = rig.transform.position;
        lastSafeLocationOffset = CalculateOffset();
    }

    private Vector3 CalculateOffset()
    {
        return new Vector3(
                    playerHead.transform.position.x - rig.transform.position.x,
                    0,
                    playerHead.transform.position.z - rig.transform.position.z);
    }

    // Update is called once per frame
    private void Update()
    {

        if (!fallingOff)
        {
            saveCooldown -= Time.deltaTime;

            if (!onBridge && !offBridgeLastFrame)
            {
                //if true next frame. Player fell off bridge
                offBridgeLastFrame = true;
            }
            else if (!onBridge && offBridgeLastFrame)
            {
                //TODO player fell off bridge. Fade and reset position          
                saveCooldown = maxSaveCooldown;
                fallingOff = true;
                //fade in, slow down time and move player to the proper position
                StartCoroutine(MovePlayerBackToBridge());
            }
            else //Player is safely on the bridge
            {
                offBridgeLastFrame = false;
                if (saveCooldown <= 0.0f) //if it's time to store a new safe position
                {
                    lastSafeLocation = nextSafeLocation;
                    lastSafeLocationOffset = nextSafeLocationOffset;
                    nextSafeLocation = rig.transform.position;
                    nextSafeLocationOffset = CalculateOffset();
                    UpdateDebug();
                    saveCooldown = maxSaveCooldown;
                }
            }
            onBridge = false;
        }
        else
        {
            offBridgeLastFrame = false;
            onBridge = true;
        }
    }

    //let the player fall for a little while, then fade out. Transport them back up and fade in.
    IEnumerator MovePlayerBackToBridge()
    {
        Debug.Log("player fell off");

        fader.fadeTime = fadeTime / 3;

        offBridgeLastFrame = false;


        //play falling sound or some stuff idk
        var audio = GetComponent<AudioSource>();
        audio.Stop();
        audio.Play();

        yield return new WaitForSeconds(fadeTime / 3);
        fader.FadeOut();
        yield return new WaitForSeconds(fadeTime / 3);
        //problem for neste uke. This shit fuck yo
        rig.enabled = false;
        rig.gameObject.transform.position =
            new Vector3(bridgeMiddle.position.x, lastSafeLocation.y, lastSafeLocation.z)
            - CalculateOffset();

        foreach (Rigidbody r in rig.gameObject.GetComponentsInChildren<Rigidbody>())
            r.velocity = Vector3.zero;

        yield return new WaitForSeconds(fadeTime / 5);

        fader.fadeTime = fadeTime;
        fader.FadeIn();
        onBridge = true;
        offBridgeLastFrame = false;
        rig.enabled = true;
        yield return new WaitForSeconds(fadeTime / 3);
        fallingOff = false;


    }

    public void SetFadetime(float fadeTime)
    {
        this.fadeTime = fadeTime;
    }

        private void UpdateDebug()
    {
        lastSafeLocationDebugTransform.position = lastSafeLocation;
        nextSafeLocationDebugTransform.position = nextSafeLocation;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!onBridge && other.gameObject.tag.Equals(tagString))
        {
            offBridgeLastFrame = false;
            onBridge = true;
        }
    }
}

