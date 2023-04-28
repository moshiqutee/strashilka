using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    Vector2 moveInput;
    bool jumpInput;
    public float jumpHeight = 1.5f;
    public Rigidbody body;
    bool isGrounded;
    RaycastHit groundHit;
    public LayerMask rayMask;
    public float rayDistance = 2.5f;
    public float rayRadius = 0.4999999f;
    public float mouseSpeed = 5.0f;
    public Transform playerCamera;
    Vector2 mouseInput;
    Vector3 mouseRot;
    public float groundDrag = 0.01f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        mouseRot = playerCamera.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetButtonDown("Jump"))
        {
            jumpInput = true;
        }
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
    void FixedUpdate()
    {
        isGrounded = Physics.SphereCast(transform.position, rayRadius, Vector3.down, out groundHit, rayDistance, rayMask);
        
        var moveForce = new Vector3(moveInput.x * moveSpeed, 0, moveInput.y * moveSpeed);
        moveForce = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * moveForce;
        body.AddForce(moveForce);

        //mouseRot = new Vector3(moveInput.x * mouseSpeed, 0, moveInput.y * mouseSpeed);
        mouseRot.x += -mouseInput.y * mouseSpeed;
        mouseRot.x = Mathf.Clamp(mouseRot.x, -90, 90);
        mouseRot.y += mouseInput.x * mouseSpeed;
        playerCamera.rotation = Quaternion.Euler(mouseRot);

        if (jumpInput && isGrounded) 
        {
            jumpInput = false;
            body.AddForce(0,jumpHeight,0);
        }

        var vel = body.velocity;
        if (isGrounded)
        {
            QuadraticDrag(ref vel, groundDrag);
        }
        body.velocity = vel;

    }
    public static void QuadraticDrag(ref Vector3 vel, float dragFactor)
    {
        var velMag = vel.magnitude;
        vel += (velMag * velMag) * dragFactor * -vel.normalized;
    }
}
