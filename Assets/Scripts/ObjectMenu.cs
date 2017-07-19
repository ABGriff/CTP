using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class ObjectMenu : MonoBehaviour
{


    public void SpawnSphere()
    {
        GameObject go = Instantiate(Resources.Load("Sphere"), new Vector3(0.1F,0.01F,3F), transform.rotation) as GameObject;
    }

    public void SpawnCube()
    {
        GameObject go = Instantiate(Resources.Load("Cube"), new Vector3(0.2F, 0.02F, 3F), transform.rotation) as GameObject;
    }

    public void SpawnCar()
    {
        GameObject go = Instantiate(Resources.Load("Car"), new Vector3(0.2F, 0.02F, 3F), Quaternion.Euler(new Vector3(-90, 0, 0))) as GameObject;
    }
}

