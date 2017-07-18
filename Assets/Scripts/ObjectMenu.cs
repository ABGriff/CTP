using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class ObjectMenu : MonoBehaviour
{


    public void SpawnSphere()
    {
        GameObject go = Instantiate(Resources.Load("Sphere")) as GameObject;
    }

    public void SpawnCube()
    {
        GameObject go = Instantiate(Resources.Load("Cube")) as GameObject;
    }
}

