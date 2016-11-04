using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {

    public float sprintMultiplier = 3.0F;
    public float jumpForce = 5.0f;
    public float speed = 3.0f;
    public float rotateSpeed = 3.0F;
    // public Rigidbody rb;
    CharacterController controller;
    Vector3 moveDirection = Vector3.zero;

    // public float speed = 3.0f;
    // public float gravity = 20.0f;
    // public float rotateSpeed = 3.0f;
    // public float jumpSpeed = 8.0f;
    // private Vector3 moveDirection = Vector3.zero;

	// Use this for initialization
	void Start () {
        // Remove cursor, confine to game window
		Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        // transform.Rotate(0, (Input.GetAxis("Horizontal")  + Input.GetAxis("Left Thumb X")) * rotateSpeed, 0);
        // float curSpeed = speed * (Input.GetAxis("Vertical") + Input.GetAxis("Left Thumb Y"));

        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
        float curSpeed = speed * Input.GetAxis("Vertical");
        
        if (Input.GetButton("Sprint"))
        {
            // moveDirection.x *= sprintMultiplier;
            // moveDirection.z *= sprintMultiplier;
            curSpeed *= sprintMultiplier;
        }

        // Debug.Log(rb.position.y);
        // if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
        // if (Input.GetButtonDown("Jump") && !(rb.velocity.y > 2) && !(rb.velocity.y < -2))
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            Debug.Log("pressed Jump");
            // this.GetComponent<CharacterController>().AddForce(new Vector3(0, jumpForce, 0), ForceMode.Force);
            moveDirection.y = jumpForce;
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);

        // controller.Move(moveDirection + (forward * curSpeed));        
        controller.SimpleMove((forward * curSpeed) + moveDirection);




        // Vector3 forward = transform.TransformDirection(Vector3.forward);
        // float curSpeed = speed * Input.GetAxis ("Vertical");

        // transform.Rotate(0,0,(float)(Input.GetAxis ("Horizontal") * rotateSpeed));

        // // transform.Rotate(0,0,(float)Input.GetAxis ("Unhorizontal") * -rotateSpeed);


        // controller.SimpleMove(forward * curSpeed);


        // if (Input.GetButton ("Jump") && controller.isGrounded) 
        // { 
        //     moveDirection.y= jumpSpeed;
        // }


        // moveDirection.y -= gravity * Time.deltaTime;


        // controller.Move(moveDirection * Time.deltaTime);

	}


}
