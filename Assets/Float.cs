using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    public float verticalRange = 1.0f;
    public float spin = 10.0f;
 
    Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float newY = position.y + Mathf.Sin(Time.timeSinceLevelLoad) * verticalRange;
        float newRot = transform.rotation.eulerAngles.y + spin * Time.deltaTime;

        transform.position = new Vector3(position.x, newY, position.z);
        transform.rotation = Quaternion.Euler(0, newRot, 0);
    }
}
