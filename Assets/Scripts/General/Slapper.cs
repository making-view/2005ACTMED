using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slapper : MonoBehaviour
{
    public enum InteractionMode
    {
        Bonkers, 
        Poppers, 
        Changers
    }

    private BubbleSpawner spawner = null;
    private Vector3 prevPosition = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] public InteractionMode interactionmode = InteractionMode.Bonkers;

    [SerializeField] private float bonkVel = 1.0f;

    void Start()
    {
        ChangeInteractionMode(interactionmode);
        spawner = GameObject.FindObjectOfType<BubbleSpawner>();
        prevPosition = transform.position;
    }

    void Update()
    {
        velocity = (transform.position - prevPosition) / Time.fixedDeltaTime;
        prevPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.ToLower().Equals("gaybubble"))
        {
            switch (interactionmode)
            {
                case InteractionMode.Bonkers:
                    Bonk(other);
                    break;
                case InteractionMode.Changers:
                    Change(other);
                    break;
                case InteractionMode.Poppers:
                    Pop(other);
                    break;
            }
        }
    }

    private void Change(Collider other)
    {
        var index = spawner.MakePositive(other.gameObject);
        if (index >= 0)
        {
            spawner.SpawnBuuuble(other.gameObject.transform.position, "default", index);
            other.gameObject.GetComponent<Bubble>().ChangeBehaviour("positive");
        }
    }

    public void ChangeInteractionMode(InteractionMode newMode)
    {
        interactionmode = newMode;
        var grabber = GetComponentInParent<OVRGrabber>();
        if (grabber.grabbedObject != null)
            grabber.ForceRelease(grabber.grabbedObject);
       
        grabber.enabled = interactionmode != InteractionMode.Poppers;
    }

    private void Pop(Collider other)
    {
        spawner.PopBubble(other.gameObject);
    }

    private void Bonk(Collider other)
    {
        if (velocity.magnitude > bonkVel)
            other.GetComponent<Rigidbody>()
                .velocity = ((other.gameObject.transform.position - transform.position).normalized * velocity.magnitude);
    }
}
