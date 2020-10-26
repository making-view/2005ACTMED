using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleThing : MonoBehaviour
{
    [SerializeField] private Vector3 newScale;
    private Vector3 oldscale;
    [SerializeField] private float timeToScale = 1.0f;
    private float progress;

    //Fraudulent stuff
    [SerializeField] private bool ImAFraud = false;
    [SerializeField] private float newXpos = 16.565f;
    private Vector3 oldPos;
    private Vector3 newPos;

    private void Start()
    {
        oldscale = transform.localScale;
        oldPos = transform.localPosition;
        newPos = oldPos;

        if (ImAFraud)
            newPos = new Vector3(newXpos, oldPos.y, oldPos.z);
        
    }

    public void StartScale()
    {
        StopCoroutine(ScaleBack());
        StopCoroutine(ScaleToNew());

        StartCoroutine(ScaleToNew());
    }

    public void GoBack()
    {
        StopCoroutine(ScaleBack());
        StopCoroutine(ScaleToNew());

        StartCoroutine(ScaleBack());
    }

    IEnumerator ScaleBack()
    {
        while (progress > 0)
        {
            progress -= Time.deltaTime;
            this.transform.localScale = Vector3.Lerp(oldscale, newScale, progress / timeToScale);
            this.transform.localPosition = Vector3.Lerp(oldPos, newPos, progress / timeToScale);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ScaleToNew()
    {
        while (progress <= timeToScale)
        {
            progress += Time.deltaTime;
            this.transform.localScale = Vector3.Lerp(oldscale, newScale, progress / timeToScale);
            this.transform.localPosition = Vector3.Lerp(oldPos, newPos, progress / timeToScale);
            yield return null;
        }
    }
}
