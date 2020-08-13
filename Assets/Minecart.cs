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

        if (!(timeElapsed > duration + wait))
            this.transform.position = Vector3.Lerp(startPos, endPos, (timeElapsed - wait) / duration);
        else
        {
            SceneManager.LoadScene("Scandinavian_Forest");
        }
    }
}
