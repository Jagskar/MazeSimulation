using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMouseLook : MonoBehaviour
{
    public float sensitivity = 100f;
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public Vector2 bounds;

    void Update()
    {
        Vector3 camPos = transform.position;

        if (Input.GetKey("up") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            camPos.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("down") || Input.mousePosition.y <= panBorderThickness)
        {
            camPos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("left") || Input.mousePosition.y <= panBorderThickness)
        {
            camPos.x -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("right") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            camPos.x += panSpeed * Time.deltaTime;
        }

        camPos.x = Mathf.Clamp(camPos.x, -bounds.x, bounds.x);
        camPos.z = Mathf.Clamp(camPos.z, -bounds.y, bounds.y);

        transform.position = camPos;
    }
}
