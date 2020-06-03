using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class io_body : MonoBehaviour {
    //io_body contains our informational IO physics crap.
    public Rigidbody2D rigBod;
    public Collider2D collider2d;
    public CompositeCollider2D LocalComposite;
    public AreaEffector2D area2d;
    //io weight, define in Unity Inspector.
    public int obj_weight;

    //draggable joint shit.
    private GameObject instance;

    private bool draggable;
    public bool dragging_enabled = false; //ONLY OVERRIDE IN INSPECTOR.
    private bool dragging = false;

    //twig shit, jesus christ modularize this to a new script good LORD it is getting more spaghetti by the MINUTE
    public bool isTwig = false; //override on child objects

    public int depositValue = 0;//set on a per instance basis

    Vector2 presentLoc;
    Vector2 playerPos;
    Vector2 oldLoc;
    int playerUID;
    private bool holdingObj = false;


    private void Start() {
        bool freeBody = false;
        if (gameObject != null) {
            if (gameObject.scene.IsValid() && dragging_enabled == true) {
                draggable = true;
            }
        }
        else {
            freeBody = true;
        }
        if (freeBody == true) {
            Destroy(this);
        }
    }



    void Update() {

        if (dragging_enabled == true) {
            if (draggable == false) {//dummy check
                return;
            }
            else if (draggable == true) {
                presentLoc = gameObject.transform.position;//constant update
                if (dragging == true) {
                    moveWhileDragged();
                }
            }
        }
    }
    private void OnMouseDown() {
        //Debug.Log("fuck i got clicked");
        if (draggable == false) {
            return;
        }
        bool oldLocSet = false;
        if (oldLocSet == false) {
            oldLoc = presentLoc;
            oldLocSet = true;
        }
        if (Input.GetMouseButtonDown(0)) {
            //Debug.Log("io_body of " + gameObject.name.ToString() + " tried to OnMouseDown");
            if (checkDistance() == true && dragging == false && holdingObj == false) { //to-do with MP implementation, make sure the players can't multi-drag an object, or maybe, allow two dragging for sprinting? That'd be neat.
                dragging = true;
                //Debug.LogWarning("io_body dragging is now true"); //To do, prevent other players from sniping people dragging objects.
            }
            else if (holdingObj == true) {
                dragging = false;
                toggleColliders(true);
            }
        }
    }
    private void toggleColliders(bool fuck) {//todo move 'fuck' to global namespace
        if (fuck == true) {
            if (gameObject.GetComponent<Collider2D>()) {
                collider2d.enabled = true;
            }
            else if (gameObject.GetComponent<CompositeCollider2D>()) {
                LocalComposite.enabled = true;
            }
            else {
                return;
            }
        }
        else if (fuck == false) {
            if (gameObject.GetComponent<Collider2D>()) {
                collider2d.enabled = false;
            }
            else if (gameObject.GetComponent<CompositeCollider2D>()) {
                LocalComposite.enabled = false;
            }
            else {
                return;
            }
        }
    }
    private void getPlayer() {
        GameObject playerInstance = GameObject.Find("hash_bird_player");
        playerPos = playerInstance.transform.position;
        playerUID = playerInstance.GetComponent<playerController>().playerUID;
    }
    private bool checkDistance() {
        if (draggable == true) {
            getPlayer();
            if (Vector2.Distance(presentLoc, playerPos) < 1f) {
                Debug.LogWarning("io_body detected the player was less than 2f away! The player's UID was: " + playerUID);
                return true;
            }
            else {
                Debug.LogWarning("io_body couldn't find the player or they weren't close enough to be detected");
                return false;
            }
        }
        else return false;
    }
    private void moveWhileDragged() {
        if (draggable == false) {
            return;
        }
        if (draggable == true && dragging == true) {
            holdingObj = true;//this is used in GetMouseDown() during Update(), but we can't call it in FixedUpdate because mouse events are FPS dependent and can't be sent to the engine's clock
            GameObject playerInstance = GameObject.Find("hash_bird_player");
            bool playerMove = playerInstance.GetComponent<playerController>().moving;
            Vector2 mouseLoc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (dragging == true) {///stupid check, I know
                rigBod.MovePosition(mouseLoc);
                gameObject.layer = LayerMask.NameToLayer("DraggingOBJ");
                if (Input.GetMouseButtonUp(0) || playerMove == true) {
                    dragging = false;
                    Vector2 newLoc = mouseLoc;
                    rigBod.MovePosition(newLoc);
                    presentLoc = newLoc;
                    holdingObj = false;
                    gameObject.layer = LayerMask.NameToLayer("Props");
                }
                else if (Vector2.Distance(presentLoc, playerPos) > 2f) {
                    dragging = false;
                    rigBod.MovePosition(oldLoc);
                    holdingObj = false;
                    gameObject.layer = LayerMask.NameToLayer("Props");
                }
            }
            else {
                toggleColliders(true);
                draggable = true;
                gameObject.GetComponent<RelativeJoint2D>().enabled = false;
                return;
            }
        }
    }
    public void CheckForDeposit() {
        GameObject summonCircle = GameObject.Find("summon_circle");
        Debug.Log("twig io trying to find summon circle");
        if (isTwig == false) {
            Debug.Log("twig forgot it was a twig after checking for deposit");
            return;
        }
        else if (isTwig == true) {
            Debug.Log("twig managed to find summon circle and managed to touch it");
            //'destroy' the object and change its visuals, remove interaction, add one to the summon_circle_controller's internal deposit count
            draggable = false;//No longer interactable
            dragging_enabled = false;//**
            //tell the player it has deposited
            GameObject playerInstance = GameObject.Find("hash_bird_player");
            playerInstance.GetComponent<playerController>().deposits = playerInstance.GetComponent<playerController>().deposits + 1;
            //Debug.Log("summon circle deposits is now: " + dp);
            Destroy(gameObject);//yeet
        }
        else {
            Debug.Log("twig found the summon circle but wasn't touching it");
        }
    }
}
