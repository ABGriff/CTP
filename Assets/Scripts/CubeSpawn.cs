using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawn : MonoBehaviour
{
    public GameObject bullet;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            GameObject instance = Instantiate(bullet, transform.position,
                transform.rotation)
                as GameObject;
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            //instance.getcomponent<rigidbody>().addforce(fwd * power);
        }
    }
}
