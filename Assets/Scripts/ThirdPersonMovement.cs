using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    private float smoothVelocity;

    public float movementSpeed = 8f;
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;
    public float smoothing = 0.1f;

    public CharacterController controller;
    public Transform cam;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVelocity, smoothing);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 movementVector = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(movementVector.normalized * movementSpeed * Time.deltaTime);
        }
    }
}
