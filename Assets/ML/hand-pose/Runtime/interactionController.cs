using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// main feature of our design
// gets location of important hand portions during the start method
// calculates the relation between these portions to determine the gesture
// if a gesture is found, the nearest 'interactable' tagged object is 'grabbed' or 'pinched',
// while it's physics and collisions are disabled

public class interactionController : MonoBehaviour
{
    public float pinchThreshold = 30.0f;
    public float grabDistThreshold = 40.0f;
    public float grabThreshold = 3;

    public GameObject pinchLine;
    public GameObject indexLine;
    public GameObject middleLine;
    public GameObject ringLine;
    public GameObject pinkyLine;
    
    GameObject wrist;
    GameObject thumb;
    GameObject index;
    GameObject middle;
    GameObject ring;
    GameObject pinky;

    GameObject rootIndex;
    GameObject rootPinky;

    GameObject[] gos;

    float pinchDist;
    float indexWristDist;
    float middleWristDist;
    float ringWristDist;
    float pinkyWristDist;

    LineRenderer indexlr;
    LineRenderer middlelr;
    LineRenderer ringlr;
    LineRenderer pinkylr;
    LineRenderer pinchlr;

    // Start is called before the first frame update
    void Start()
    {
        wrist = GameObject.Find("/sceneController/WRIST");
        thumb = GameObject.Find("/sceneController/THUMB_TIP");
        index = GameObject.Find("/sceneController/INDEX_FINGER_TIP");
        middle = GameObject.Find("/sceneController/MIDDLE_FINGER_TIP"); 
        ring = GameObject.Find("/sceneController/RING_FINGER_TIP");
        pinky = GameObject.Find("/sceneController/PINKY_TIP");
        rootIndex = GameObject.Find("/sceneController/INDEX_FINGER_MCP");
        rootPinky = GameObject.Find("/sceneController/PINKY_MCP");

        indexlr = indexLine.GetComponent<LineRenderer>();
        middlelr = middleLine.GetComponent<LineRenderer>();
        ringlr = ringLine.GetComponent<LineRenderer>();
        pinkylr = pinkyLine.GetComponent<LineRenderer>();
        pinchlr = pinchLine.GetComponent<LineRenderer>();

        gos = GameObject.FindGameObjectsWithTag("interactable");
    }

    // Update is called once per frame
    void Update()
    {
        float[] distances = new float[4];
        int count = 0;

        pinchDist = Vector3.Distance(thumb.transform.position, index.transform.position);
        
        indexWristDist = Vector3.Distance(wrist.transform.position, index.transform.position);
        middleWristDist = Vector3.Distance(wrist.transform.position, middle.transform.position);
        ringWristDist = Vector3.Distance(wrist.transform.position, ring.transform.position);
        pinkyWristDist = Vector3.Distance(wrist.transform.position, pinky.transform.position);

        distances[0] = indexWristDist;
        distances[1] = middleWristDist;
        distances[2] = ringWristDist;
        distances[3] = pinkyWristDist;

        if (pinchDist <= pinchThreshold && wrist.activeSelf == true && count < 3)
        {
            pinchLine.SetActive(true);
            pinchlr.SetPosition(0, index.transform.position);
            pinchlr.SetPosition(1, thumb.transform.position);

            Vector3 avgPos =  new Vector3((thumb.transform.position.x + index.transform.position.x) / 2, (thumb.transform.position.y + index.transform.position.y) / 2, (thumb.transform.position.z + index.transform.position.z) / 2);

            foreach (GameObject go in gos)
            {
                float dist = Vector3.Distance(go.transform.position, avgPos);
                if (dist <= 90)
                {
                    go.transform.position = avgPos;
                    go.gameObject.GetComponent<BoxCollider>().enabled = false;
                    go.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    go.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                }
            }
        }
        else
        {
            pinchLine.SetActive(false);
            foreach (GameObject go in gos)
            {
                go.gameObject.GetComponent<BoxCollider>().enabled = true;
            }
        }

        foreach (float i in distances)
        {
            if (i <= grabDistThreshold)
            {
                count++;
            }
        }

        if (count >= grabThreshold && wrist.activeSelf == true)
        {
            indexLine.SetActive(true);
            middleLine.SetActive(true);
            ringLine.SetActive(true);
            pinkyLine.SetActive(true);

            indexlr.SetPosition(0, index.transform.position);
            indexlr.SetPosition(1, wrist.transform.position);

            middlelr.SetPosition(0, middle.transform.position);
            middlelr.SetPosition(1, wrist.transform.position);

            ringlr.SetPosition(0, ring.transform.position);
            ringlr.SetPosition(1, wrist.transform.position);

            pinkylr.SetPosition(0, pinky.transform.position);
            pinkylr.SetPosition(1, wrist.transform.position);

            Vector3 avgPos = new Vector3((wrist.transform.position.x + rootIndex.transform.position.x + rootPinky.transform.position.x) / 3,
            (wrist.transform.position.y + rootIndex.transform.position.y + rootPinky.transform.position.y) / 3,
            (wrist.transform.position.z + rootIndex.transform.position.z + rootPinky.transform.position.z) / 3);

            foreach (GameObject go in gos)
            {
                float dist = Vector3.Distance(go.transform.position, avgPos);
                if (dist <= 90)
                {
                    go.transform.position = avgPos;
                }
            }
        }
        else
        {
            indexLine.SetActive(false);
            middleLine.SetActive(false);
            ringLine.SetActive(false);
            pinkyLine.SetActive(false);
        }
    }
}
