using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController Controller;

    public float speed = 8f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    Vector3 velocity;
    Vector3 move;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position,groundDistance,groundMask);

        if(isGrounded && velocity.y < 0){
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        move = transform.right * x + transform.forward * z;

        Controller.Move(move * speed * Time.deltaTime);

        if(isGrounded && Input.GetKeyDown(KeyCode.Space)){
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        Controller.Move(velocity * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "DeathPlane") {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }
    }
}
