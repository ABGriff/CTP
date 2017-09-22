using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeCommands : MonoBehaviour {

    /// <summary>
    /// This script will contains our BridgeCommands for the GUI
    /// Alexander Burey 2017
    /// Common Time Playbox
    /// </summary>
    
    //A list of all our pieces for doing the commands later
    List<GameObject> pieces = new List<GameObject>();

    //Start the test
    public void startTest() {
        //for each object in our pieces (Each piece we created during build phase)
        foreach (GameObject obj in pieces) {
            //if object is a plank
            if (obj.transform.Find("Plank")) {
                //essentially same thing as before but ensure its a plank with comparetag
                if (obj.transform.Find("Plank").gameObject.CompareTag("Plank")) {
                    GameObject go = obj.transform.Find("Plank").gameObject;
                    go.GetComponent<BridgeTest>().startTest();
                }
            }
        }
    }

    //Reset the test
    public void resetTest() {
        foreach (GameObject obj in pieces) {
            if (obj.transform.Find("Plank")) {
                if (obj.transform.Find("Plank").gameObject.CompareTag("Plank")) {
                    GameObject go = obj.transform.Find("Plank").gameObject;
                    go.GetComponent<BridgeTest>().resetTest();
                }
            }
        }
    }
    //Pause/continue the test
    public void pauseContinueTest() {
        foreach (GameObject obj in pieces) {
            if (obj.transform.Find("Plank")) {
                if (obj.transform.Find("Plank").gameObject.CompareTag("Plank")) {
                    GameObject go = obj.transform.Find("Plank").gameObject;
                    go.GetComponent<BridgeTest>().pauseContinueTest();
                }
            }
        }
    }

    //An undo button for deleting the last item created.
    public void undo() {
        int num = 0;
        foreach (GameObject obj in pieces) {
            num += 1;
        }
        GameObject item = pieces[num - 1];
        Destroy(item);
        pieces.Remove(pieces[num - 1]);
    }

    //Add a Plank to the world and add it to our list to start the test later
    public void addPlank() {
        GameObject instance = Instantiate(Resources.Load("Basic_Plank"), new Vector3(0f, -0.1f, 4.9f), Quaternion.identity) as GameObject;
        pieces.Add(instance);
    }

    //Spawn a Left Ramp
    public void SpawnRampL() {
        GameObject instance = Instantiate(Resources.Load("RampL"), new Vector3(-.7f, 0f, .75f), Quaternion.identity) as GameObject;
        pieces.Add(instance);
    }

    //Spawn a Right Ramp
    public void SpawnRampR() {
        GameObject instance = Instantiate(Resources.Load("RampR"), new Vector3(-0.3f, 0f, 0.8f), Quaternion.identity) as GameObject;
        pieces.Add(instance);
    }

    //Spawn a Car
    public void SpawnCar() {
        GameObject instance = Instantiate(Resources.Load("Car"), new Vector3(0.15f, 0.2f, 3.625f), Quaternion.Euler(0, 90, 0)) as GameObject;
        pieces.Add(instance);
    }

    //Spawn a Support
    public void addSupport() {
        GameObject instance = Instantiate(Resources.Load("Support"), new Vector3(-.4f, -0.25f, 3.3f), Quaternion.identity) as GameObject;
        pieces.Add(instance);
    }
}
