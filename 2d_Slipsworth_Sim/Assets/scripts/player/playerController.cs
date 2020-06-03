using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class playerController : MonoBehaviourPun
{
    //
    public int playerUID = 1;//replace with photon component


    //movespeed & vector
    private float minMoveSpeed = 2;
    private float maxMoveSpeed = 3;
    public float presentMoveSpeed;
    private float acceleration = 2;
    private bool speedLimit = false;
    private bool sprinting = false;

    Vector2 movement;

    //references
    public GameObject player;
    public Animator animController;
    public SpriteRenderer spriteRend;

    //physics stuff
    public Rigidbody2D rigBod;
    public bool draggingObj;

    //io_stuff
    public int deposits = 0;

    //render vars
    private bool defaultFace = true;

    //conditional vars
    public bool isAlive = true;
    public bool moving = false;

    private void Start() {
        spriteRend.flipX = false;
        presentMoveSpeed = minMoveSpeed;
    }

    private void Update() {
        if (!photonView.IsMine && PhotonNetwork.IsConnected) {
            return;
        }
        else {
            handleUpdateMovement();
        }
        
    }

    private void FixedUpdate() {
        handleFixedUpdateMovement();
    }

    //private void OnMouseDown() {
    //    GameObject clickedInstance;
    //    clickedInstance = 
    //    if (clickedInstance.GetComponent<io_draggable>() != null) {
    //        clickedInstance.GetComponent<io_draggable>();
    //    }
    //}

    //movement stuff
    private void handleUpdateMovement() {
        if (isAlive == true)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            animController.SetFloat("Speed", movement.sqrMagnitude);
        }
    }
    private void handleFixedUpdateMovement() {
        if (isAlive == false) {
            return;
        } else {
            rigBod.MovePosition(rigBod.position + movement * presentMoveSpeed * Time.fixedDeltaTime);//maybe move to own method
            getMoving();
            flipSprite();
            doSprint();
            accelerate();
        }
    }

    private void getMoving() {
        Vector2 noMovement;
        noMovement = new Vector2(0, 0);

        if (movement != noMovement)
        {
            moving = true;
            //Debug.Log("movement true");
        }
        else if (movement == noMovement)
        {
            moving = false;
            animController.SetBool("Sprinting", false);
            //Debug.Log("movement false");
        }
    }

    private void flipSprite() {
        //attempting to flip da sprite//
        if (movement.x == -1)
        {
            spriteRend.flipX = true;
            defaultFace = false;
        }
        else if (movement.x == 1)
        {
            spriteRend.flipX = false;
            defaultFace = true;
        }
    }

    private void accelerate() {
        //acceleration//
        float sp_maxMoveSpeed = 5f;
        float sp_acceleration = 3f;

        if (presentMoveSpeed >= minMoveSpeed && moving == false)
        {
            presentMoveSpeed = minMoveSpeed;
            speedLimit = false;
            //Debug.Log("Reset move speed");
        }
        else if (presentMoveSpeed <= maxMoveSpeed && presentMoveSpeed >= minMoveSpeed && moving == true && speedLimit == false)
        {
            if (sprinting == true)
            {
                maxMoveSpeed = sp_maxMoveSpeed;
                acceleration = sp_acceleration;
            }
            presentMoveSpeed = presentMoveSpeed + (acceleration * Time.deltaTime);
            //Debug.Log("Attempted to accelerate");
            //Debug.Log("Max speed is:" + maxMoveSpeed.ToString());
            if (presentMoveSpeed >= maxMoveSpeed)
            {
                speedLimit = true;
                //Debug.Log("Speed limit reached");
                //Debug.Log(presentMoveSpeed.ToString());
                presentMoveSpeed = maxMoveSpeed;
            }
        }
    }
    private void doSprint() {
        float og_maxMoveSpeed = 3;
        float og_acceleration = 2;
        if (isAlive == true && moving == true && Input.GetKey("left shift"))
        {
            sprinting = true;
            speedLimit = false;
            animController.SetBool("Sprinting", true);
        }
        else
        {
            sprinting = false;
            maxMoveSpeed = og_maxMoveSpeed;
            acceleration = og_acceleration;
            //Force deceleration if shift is released during movement.
            if (presentMoveSpeed > maxMoveSpeed)
            {
                bool decelerating = true;
                if (decelerating == true)
                {
                    presentMoveSpeed = presentMoveSpeed - (acceleration * Time.deltaTime);
                    if (presentMoveSpeed <= maxMoveSpeed)
                    {
                        decelerating = false;
                        animController.SetBool("Sprinting", false);
                        //Debug.Log("decelerated, maxspeed is now:" + maxMoveSpeed.ToString() + " | presentmove is:" + presentMoveSpeed.ToString());
                    }
                }
            }
            else if (presentMoveSpeed <= maxMoveSpeed)
            {
                return;
            }
        }
    }
}

