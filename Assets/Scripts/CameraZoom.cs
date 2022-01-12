using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // Scrolling up = zoom in
            GetComponent<Camera>().fieldOfView--;
        else if (Input.GetAxis("Mouse ScrollWheel") < 0) // Scrolling down = zoom out
            GetComponent<Camera>().fieldOfView++;
    }
}
