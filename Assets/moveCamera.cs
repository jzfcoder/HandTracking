using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Is used in conjuction with a slider to manually calibrate virtual & real life scaling

public class moveCamera : MonoBehaviour
{
    public GameObject camera;
    public Slider slider;
    public void OnValueChanged()
    {
        camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, slider.value);
    }
}
