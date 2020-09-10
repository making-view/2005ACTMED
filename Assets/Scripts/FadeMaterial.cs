using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Policy;
using UnityEngine;

public class FadeMaterial : MonoBehaviour
{
    public Renderer meshRenderer;
    public Material instancedMaterial;

    public float fadeTime = 3.0f;
    float timer = 0.0f;

    public List<string> texToFade;
    bool activated = false;

    [SerializeField]
    bool defaultTex = true;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = gameObject.GetComponent<Renderer>();
        instancedMaterial = meshRenderer.material;

        if(defaultTex)
        {
            texToFade.Add("_ColourSwitch");
            texToFade.Add("_NormalSwitch");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            timer += Time.deltaTime;


            //0 -> 1 smooth transition of float values in tex list

            if (timer <= fadeTime)
            {
                float fadeLevel = Mathf.Sin(timer / fadeTime * Mathf.PI / 2);

                foreach (string tex in texToFade)
                {
                    instancedMaterial.SetFloat(tex, fadeLevel);
                }
            }
            else
                activated = false;
        }
    }

    public void Activate()
    {
        activated = true;
    }
}
