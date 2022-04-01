using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used to name each landmark of the visualized hand (for easier access and identification in other scripts)

public class getPosition : MonoBehaviour
{
    public Vector3 position;
    public int landmarkNum;
    public interactionController control;

    string[] landmarks = new string[] {
        "WRIST", "THUMB_CMC", "THUMB_MCP", "THUMB_IP", "THUMB_TIP",
        "INDEX_FINGER_MCP", "INDEX_FINGER_PIP", "INDEX_FINGER_DIP", "INDEX_FINGER_TIP",
        "MIDDLE_FINGER_MCP", "MIDDLE_FINGER_PIP", "MIDDLE_FINGER_DIP", "MIDDLE_FINGER_TIP",
        "RING_FINGER_MCP", "RING_FINGER_PIP", "RING_FINGER_DIP", "RING_FINGER_TIP",
        "PINKY_MCP", "PINKY_PIP", "PINKY_DIP", "PINKY_TIP"
        };

    // Start is called before the first frame update
    void Start()
    {
        gameObject.name = landmarks[landmarkNum];
        if (control.isActiveAndEnabled == false)
        {
            control.enabled = true;
        }
    }
}
