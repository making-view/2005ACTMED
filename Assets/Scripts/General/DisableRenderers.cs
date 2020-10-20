using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableRenderers : MonoBehaviour
{
    [SerializeField] bool visibleInEditor = true;
    // Start is called before the first frame update
    void Start()
    {
        if(!visibleInEditor || !Application.isEditor)
        {
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
                r.enabled = false;
        }
    }

    private void OnValidate()
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = visibleInEditor;
    }
}
