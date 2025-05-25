using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Reference")]
    public Transform orientation;
    public MeshRenderer model;

    [Header("Movement")]
    public float moveForce = 10f;
    public float moveSpeed = 10f;
    public float jumpForce = 7f;
    public float jumpCooldown = 0.2f;
    public LayerMask whatIsGround;
    public float fallMultiplier = 2f;
    public float coyoteTime = 0.2f;
    public float groundLayLength = 0.2f;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private float horizontalInput;
    private float verticalInput;
    private Camera cam;
    private bool grounded = true;
    private bool readyToJump = true;
    private float coyoteTimeCounter;
    private Vector3 startPos;
    private float deadZoneY = 30;

    private void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        //model.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        if (coyoteTime > jumpCooldown)
        {
            Debug.Log("Double Jump Error may occur");
        }
    }
    private void Update()
    {
        MyInput();
        SpeedControl();
        GroundCheck();
        CoyoteTime();
        ResetIfVoid();
    }
    private void ResetIfVoid()
    {
        if (transform.position.y < -deadZoneY)
        {
            rb.linearVelocity = Vector3.zero;
            transform.position = startPos + Vector3.up * deadZoneY;
        }
    }
    private void FixedUpdate()
    {
        MovePlayer();
        FallFaster();
    }
    private void FallFaster()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime;
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveForce * 10f, ForceMode.Force);
    }
    private void GroundCheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, groundLayLength, whatIsGround);
    }
    private void MyInput()
    {
        if (Input.GetKey(KeyCode.Space) && readyToJump && coyoteTimeCounter > 0) //Jump
        {
            Jump();
            coyoteTimeCounter = 0;
            readyToJump = false;
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }
    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
        if (horizontalInput == 0 && verticalInput == 0)
        {
            rb.linearVelocity = new Vector3(Mathf.Lerp(rb.linearVelocity.x, 0, Time.deltaTime * 4), rb.linearVelocity.y, Mathf.Lerp(rb.linearVelocity.z, 0, Time.deltaTime * 4));
        }
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
    private void CoyoteTime()
    {
        if (grounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }
}
