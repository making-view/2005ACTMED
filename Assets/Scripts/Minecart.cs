using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Minecart : MonoBehaviour
{

    [SerializeField]
    float duration = 20;
    float timeElapsed = 0;
    float wait = 5;

    [SerializeField]
    float fadeTimer = 1.5f;

    bool fading = false;

    [SerializeField]
    OVRScreenFade fade = null;

    [SerializeField]
    GameObject endposGO;

    Vector3 startPos;
    Vector3 endPos;


    // Start is called before the first frame update
    void Start()
    {
        startPos = this.gameObject.transform.position;
        endPos = endposGO.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;


        if (timeElapsed > duration + wait - fadeTimer)
        {
            this.transform.position = Vector3.Lerp(startPos, endPos, (timeElapsed - wait) / duration);

            if (fade == null)
            {
                fade = GameObject.Find("CenterEyeAnchor").GetComponent<OVRScreenFade>();

                if (!fading)
                {
                    StartCoroutine(Travel());
                    fading = true;
                }
            }
        }
        else if (!(timeElapsed > duration + wait))
            this.transform.position = Vector3.Lerp(startPos, endPos, (timeElapsed - wait) / duration);
        else
            SceneManager.LoadScene("Scandinavian_Forest");

    }

    IEnumerator Travel()
    {
        if (fade != null)
        {
            Debug.Log("fading out of cave");

            fade.fadeOnStart = true;
            fade.fadeColor = Color.white;
            fade.fadeTime = fadeTimer;
            fade.FadeOut();

            yield return new WaitForSeconds(fadeTimer + 0.5f);
        }
        else
            Debug.Log("fade == null - " + this.gameObject.name);

        //SceneManager.LoadScene("Scandinavian_Forest");
    }
}
