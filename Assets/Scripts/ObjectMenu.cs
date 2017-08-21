using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class ObjectMenu : MonoBehaviour
{
    public void SpawnSphere()
    {
        //First and Third line ensure that the game object is instantiated in the correct area.
        GameObject instantiateLoc = GameObject.Find("Holograms");
        GameObject instance = Instantiate(Resources.Load("Sphere"), new Vector3(0.1F, 0.01F, 3F), transform.rotation) as GameObject;
        instance.transform.parent = instantiateLoc.transform;
        //Give each instance a new name to avoid world anchor issues
        instance.name = "Sphere" + System.DateTime.Now.ToString("yyyyMMddhhmmss");
    }

    public void spawnCube()
    {
        GameObject instantiateLoc = GameObject.Find("Holograms");
        GameObject instance = Instantiate(Resources.Load("Cube"), new Vector3(0.2F, 0.02F, 3F), transform.rotation) as GameObject;
        instance.transform.parent = instantiateLoc.transform;
        instance.name = "Cube" + System.DateTime.Now.ToString("yyyyMMddhhmmss");
    }

    public void spawnCar()
    {
        //due to the way that 3d modelling programs and unity defines what the "up" axis is, we have to rotate the car -90 degrees around the x-axis to ensure it st.
        GameObject instantiateLoc = GameObject.Find("Holograms");
        GameObject instance = Instantiate(Resources.Load("Car"), new Vector3(0.2F, 0.02F, 3F), Quaternion.Euler(new Vector3(-90, 0, 0))) as GameObject;
        instance.transform.parent = instantiateLoc.transform;
        instance.name = "Car" + System.DateTime.Now.ToString("yyyyMMddhhmmss");
    }

    public void spawnBridgeOne()
    {
        GameObject instance = Instantiate(Resources.Load("BridgeOne"), new Vector3(0.2F, 0.02F, 3F), transform.rotation) as GameObject;
        instance.name = "BridgeOne" + System.DateTime.Now.ToString("yyyyMMddhhmmss");
    }

	public void LevelOne() {
		// Create a temporary reference to the current scene.
		Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("Level One");
	}

    public void LevelTwo() {
        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("Level Two");
    }

    public void LevelThree() {
        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("Level Three");
    }
}
