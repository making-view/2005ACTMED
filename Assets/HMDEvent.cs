using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HMDEvent : MonoBehaviour
{

    [SerializeField] UnityEvent onUnmounted = null;


    private void Start()
    {
        OVRManager.HMDUnmounted += UnmountedEvent;
    }

    private void UnmountedEvent ()
    {
        onUnmounted.Invoke();
    }
}
