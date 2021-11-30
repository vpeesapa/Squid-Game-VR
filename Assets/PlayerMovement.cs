using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController Controller;

    public GameObject Camera;

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

    InputDevice leftController;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        move = Camera.transform.right * x + Camera.transform.forward * z;

        Controller.Move(move * speed * Time.deltaTime);

        if (isGrounded && OVRInput.GetDown(OVRInput.Button.One))
        //if(isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        Controller.Move(velocity * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DeathPlane")
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (other.gameObject.tag == "EndPlatform")
        {
            StartCoroutine(DelayWin(2f));
        }

        if (other.gameObject.tag == "BrokenGlass")
        {
            StartCoroutine(DestroyGlass(1.3f, other));
        }
    }

    IEnumerator DestroyGlass(float time, Collider other)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        Destroy(other.gameObject);
    }

    IEnumerator DelayWin(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
