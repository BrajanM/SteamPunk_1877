using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody PlayerRB;
    public Camera PlayerCamera;
    public float mouseSensitivity = 90f;
    public float playerSpeed = 10f;
    public float jumpForce = 10f;

    private Vector3 targetPosition;
    private float rotationOnX;
    private bool isJumping;
    private Vector3 prevoiusMousePosition;
    private float z = 0;

    // Start is called before the first frame update
    void Start()
    {
        setStartParameters();
        
    }

     
    void Update()
    {
        if (Input.mousePosition!=prevoiusMousePosition)
        {
            rotateCamera();
        }
        
        if (Input.GetKey(KeyCode.W))
        {
            PlayerMoveForward();
        }
        if (Input.GetKey(KeyCode.S))
        {
            PlayerMoveBackward();
        }
        if (Input.GetKey(KeyCode.D))
        {
            PlayerMoveRight();
        }
        if (Input.GetKey(KeyCode.A))
        {
            PlayerMoveLeft();
        }
        if (Input.GetKey(KeyCode.Space) && !isJumping)
        {
            PlayerJump();
            isJumping = true;
        }
        if (Input.GetMouseButton(0))
        {
            getTargetPosition();
        }
        moveCameraWhileWalking();
    }


    private void getTargetPosition()
    {
        var Ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(Ray, out hit))
        {
            targetPosition = hit.point;
            if (hit.collider.gameObject.tag == "Enemy")
            {
                Destroy(hit.collider.gameObject);
            }
            Debug.Log(targetPosition.ToString());
        }
    }

    private void rotateCamera()
    {
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;

        rotationOnX -= mouseY;
        rotationOnX = Mathf.Clamp(rotationOnX, -90f, 90f);
        PlayerCamera.transform.localEulerAngles = new Vector3(rotationOnX, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    private void PlayerMoveForward()
    {
        transform.position += transform.forward * Time.deltaTime*playerSpeed;
    }
    private void PlayerMoveBackward()
    {
        transform.position -= transform.forward * Time.deltaTime * playerSpeed;
    }
    private void PlayerMoveRight()
    {
        transform.Translate(Vector3.right* Time.deltaTime * playerSpeed);
    }
    private void PlayerMoveLeft()
    {
        transform.Translate(-Vector3.right * Time.deltaTime * playerSpeed);
    }
    private void PlayerJump()
    {
        PlayerRB.AddForce(Vector3.up*jumpForce,ForceMode.Impulse);
    }



    private void setStartParameters()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isJumping = false;
        prevoiusMousePosition = Input.mousePosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.position.y<transform.position.y)
        {
            isJumping = false;
        }
    }

    private void moveCameraWhileWalking()
    {
        float min = -1f;
        float max = 1f;
        bool up = true;
        bool down = false;


        if (up)
        {
            if (z<=max)
            {
                z = z + 0.01f;
            }
            else
            {
                up = false;
                down = true;
            }
        }
        if (down)
        {
            if (z>=min)
            {
                z = z - 0.01f;

            }
            else
            {
                up=true;
                down=false;
            }
        }
        Debug.Log(z);
        PlayerCamera.transform.localRotation = new Quaternion(PlayerCamera.transform.localRotation.x, 0, PlayerCamera.transform.localRotation.z+z, PlayerCamera.transform.localRotation.w);
    }
}
