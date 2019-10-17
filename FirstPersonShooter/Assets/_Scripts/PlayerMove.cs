using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //variables//
    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;

    private float movementSpeed;

    [SerializeField] private float characterHeight;
    [SerializeField] private float isGroundedRayLength;

    [SerializeField] private float walkSpeed, runSpeed;
    [SerializeField] private float runBuildUpSpeed;
    [SerializeField] private KeyCode runKey;

    [SerializeField] private float slopeForce;
    [SerializeField] private float slopeForceRayLength;

    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private float jumpingMovement;

    [SerializeField] private float crouchSpeed;
    [SerializeField] private float crouchRayLength;
    [SerializeField] private KeyCode crouchKey;


    private Vector3 moveDirection = Vector3.zero;
    [SerializeField] private float speed;
    [SerializeField] public float gravity = 20.0f;
    [SerializeField] bool jumping = false;
    [SerializeField] float gravityMultiplyer;
    [SerializeField] private float gravityOrigin = 7f;

    //-----Bools-----Bools-----Bools-----Bools-----Bools------
    /*////////////////////////////////////////////////////////*/

    [SerializeField] private bool isGrounded = true;
    [SerializeField] private bool isStanding = true;

    [SerializeField] private bool isRunning;

    [SerializeField] private bool isJumping;

    [SerializeField] private bool isCrouching;
    [SerializeField] private bool canStand;

    /*////////////////////////////////////////////////////////*/
    //-----Bools-----Bools-----Bools-----Bools-----Bools------

    private CharacterController playerController;
    [SerializeField] private Transform playerCamera;

    private void Awake()
    {
        playerController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerMoving()
    {
        //float horiInput = Input.GetAxis(horizontalInputName);
        //float vertInput = Input.GetAxis(verticalInputName);

        //Vector3 forwardMovement = transform.forward * vertInput;
        //Vector3 rightMovement = transform.right * horiInput;

        //playerController.Move(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * (movementSpeed * jumpingMovement));
        //playerController.transform.Translate(((forwardMovement + rightMovement)* jumpingMovement) * Time.deltaTime) ;

        if (jumping == false)
        {
            
            gravity = gravityOrigin;
            return;
        }
        else
        {
            gravity += 0.5f;

            /*moveDirection = new Vector3(Input.GetAxis(horizontalInputName), 0.0f, Input.GetAxis(verticalInputName));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection = moveDirection * speed;

            moveDirection.y = moveDirection.y - ((gravity * 20) * Time.deltaTime);

            playerController.Move(moveDirection * Time.deltaTime);*/

            /*float horiInput = Input.GetAxis(horizontalInputName);
            float vertInput = Input.GetAxis(verticalInputName);

            Vector3 forwardMovement = transform.forward * vertInput;
            Vector3 rightMovement = transform.right * horiInput;*/

            //playerController.Move(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * (movementSpeed * jumpingMovement));
            //playerController.transform.Translate(((forwardMovement + rightMovement)* jumpingMovement) * Time.deltaTime) ;
        }





    }


    private void PlayerMovement()
    {

        /*float horiInput = Input.GetAxis(horizontalInputName);
        float vertInput = Input.GetAxis(verticalInputName);

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horiInput;

        transform.Translate((horiInput * Time.deltaTime), 0, (vertInput * Time.deltaTime));

        playerController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * (movementSpeed * jumpingMovement) * 0);
        //playerController.transform.Translate(((forwardMovement + rightMovement) * jumpingMovement) * Time.deltaTime);*/



        /*float horiInput = Input.GetAxis(horizontalInputName);
        float vertInput = Input.GetAxis(verticalInputName);

        moveDirection = new Vector3(Input.GetAxis(horizontalInputName), 0.0f, Input.GetAxis(verticalInputName));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection = moveDirection * speed;

        moveDirection.y = moveDirection.y - ((gravity * 20) * Time.deltaTime);
        
        playerController.Move(moveDirection * Time.deltaTime);*/



        //Input Data
        
        

        float horiInput = Input.GetAxis(horizontalInputName);
        float vertInput = Input.GetAxis(verticalInputName);

        moveDirection = new Vector3(horiInput, 0.0f, vertInput);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection = moveDirection * speed;

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horiInput;


        //original movement that worked just no midair movement speed
        //playerController.Move(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * (movementSpeed * Time.deltaTime));

        moveDirection.y = moveDirection.y - (gravity *  gravityMultiplyer) * Time.deltaTime;
        playerController.Move(moveDirection * Time.deltaTime);

        if ((vertInput != 0 || horiInput != 0) && OnSlope())
        {
            playerController.Move(Vector3.down * playerController.height / 2 * slopeForce * Time.deltaTime);
        }

        
        

        CheckIfGrounded();
        SetMovementSpeed();
        JumpInput();
        IsCrouching();
        PlayerMoving();

    }

    private void SetMovementSpeed()
    {
        // make sure when crouch and running is pressed defalut walking speed is all that happens
        if (Input.GetKey(runKey) && Input.GetKey(crouchKey))
        {
            movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, Time.deltaTime * runBuildUpSpeed);
        }
        //when shift is pressed player will move at a running speed
        else if (Input.GetKey(runKey))
        {
            isRunning = true;
            movementSpeed = Mathf.Lerp(movementSpeed, runSpeed, Time.deltaTime * runBuildUpSpeed);
        }
        else if (Input.GetKeyUp(runKey))
        {
            isRunning = false;
        }
        //when crouch key is pressed player will move at a slower speed
        else if (Input.GetKey(crouchKey))
        {
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
            movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, Time.deltaTime * runBuildUpSpeed);
        }
    }


    //-----------------------------------------------------

    private void CheckIfGrounded()
    {
        IsGrounded();
        if (IsGrounded())
        {
            isGrounded = true;
            jumping = false;
        }
        else
        {
            
            isGrounded = false;
            jumping = true;
        }
    }

    private bool IsGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, characterHeight))
        {
            print("Hit something");

            return true;
        }
        print("Hit nothing");

        return false;

    }

    private void Standing()
    {
        canStand = false;
        isStanding = true;
        playerController.height = 2;
        playerController.center = new Vector3(0, 0, 0);
        playerCamera.transform.localPosition = new Vector3(0, (playerController.height / 2), 0);
    }
    private void Crouching()
    {
        isCrouching = true;
        playerController.height = 1;
        playerController.center = new Vector3(0, -0.5f, 0);
        playerCamera.transform.localPosition = new Vector3(0, ((playerController.height / 2) - 0.5f), 0);
        movementSpeed = Mathf.Lerp(movementSpeed, crouchSpeed, Time.deltaTime * runBuildUpSpeed);
    }

    private bool CanStand(float value)
    {
        if (!CanStandTest(value))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private bool CanStandTest(float rayLength)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, rayLength))
        {
            isCrouching = true;
            canStand = false;

            if (!canStand)
            {
                print("cannot stand");
            }
            return false;
        }
        else
        {
            //isGrounded = true;
            canStand = true;
            return true;
        }
    }

    private void IsCrouching()
    {
        CanStand(crouchRayLength);
        isStanding = false;
        if (!isCrouching && canStand)
        {
            Standing();
        }
        else if (isCrouching)
        {
            Crouching();
            if (!CanStand(crouchRayLength))
            {

                isCrouching = true;
                return;
            }
            else
            { }
        }
    }


    private bool OnSlope()
    {
        if (isJumping)
        {
            return false;
        }

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, playerController.height / 2 * slopeForceRayLength))
        {
            if (hit.normal != Vector3.up)
            {
                return true;
            }
        }
        return false;
    }

    private void JumpInput()
    {
        if (!IsGrounded())
        {

            return;

        }
        else
        {
            if (Input.GetKeyDown(jumpKey) && !isJumping)
            {
                if (!isGrounded)
                {
                    return;
                }
                else
                {
                    isJumping = true;
                    StartCoroutine(JumpEvent());
                }

            }
        }

    }
    private IEnumerator JumpEvent()
    {
        playerController.slopeLimit = 90.0f;
        float timeInAir = 0.0f;
        do
        {
            
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            playerController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;
            isGrounded = false;
            jumping = true;
            gravity += 0.5f;
            yield return null;
        } while (!playerController.isGrounded && playerController.collisionFlags != CollisionFlags.Above);
        gravity = 8;
        playerController.slopeLimit = 45.0f;
        isJumping = false;
        jumping = false;
        //isGrounded = true;
    }
    //-----------------------------------------------------
}