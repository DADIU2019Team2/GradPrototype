using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translating : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float speed = 5;

        //transform.Translate(speed*Time.deltaTime, 0, 0);
        GetComponent<CharacterController>().Move(new Vector3(speed * Time.deltaTime, 0, 0));

    }
}
