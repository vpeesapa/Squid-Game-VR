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
    float startTime;
    float jumpDelay = 5f;

    void Start()
    {
        startTime = Time.time;
    }

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
            float currentTime = Time.time;
            
            if(currentTime - startTime >= jumpDelay)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                startTime = currentTime;
            }
            
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

        if(other.gameObject.tag == "EndPlatform") {
            StartCoroutine(DelayWin(2f));
        }

        if(other.gameObject.tag == "BrokenGlass") {
            StartCoroutine(DestroyGlass(1.3f, other));
        }
    }

    IEnumerator DestroyGlass(float time, Collider other){
        yield return new WaitForSeconds(time);
        
        // Code to execute after the delay
        Destroy(other.gameObject);
     }

    IEnumerator DelayWin(float time){
        yield return new WaitForSeconds(time);
        
        // Code to execute after the delay
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
