using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class floor_button : MonoBehaviour {
    // Start is called before the first frame update
    public GameObject button;
    //rendering references
    public Animator animController;
    public SpriteRenderer spriteRend;
    //physics references
    public Rigidbody2D rigBod;
    public BoxCollider2D collider2d;
    public AreaEffector2D area2d;

    public int requiredMass = 10;
    public int combinedMass {
        get {
            int count = 0;
            foreach (GameObject g in collidingObjects) {
                io_body body = g.GetComponent<io_body>();
                count += body.obj_weight;
            }
            return count;
        }
    }

    public bool pressed = false;
    public List<GameObject> collidingObjects = new List<GameObject>();
    //game io
    public UnityEvent onPressed;
    public UnityEvent onReleased;

    private void Start() {

    }

    // Update is called once per frame
    private void Update() {
        //Debug.Log(combinedMass);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //Debug.Log("fuck I got stepped on");
        Debug.Log("trigger enter " + collision.gameObject.name);
        if (collision.gameObject.GetComponent<io_body>() != null && collision.IsTouching(collider2d) == true) {
            collidingObjects.Add(collision.gameObject);
            Debug.LogError("there are" + collidingObjects.Count + "on the floor button");
            //Debug.LogWarning(combinedMass);
            depress();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Debug.Log("trigger exit " + collision.gameObject.name);
        if (collision.gameObject.GetComponent<io_body>() != null && collidingObjects.Contains(collision.gameObject)) {
            unpress(collision);
        }
    }
    private void depress() {
        if (combinedMass > requiredMass) {
            pressed = true;
            animController.SetBool("depressed", true);
            processLinks();
        }
        else if (combinedMass < requiredMass) {
            return;
        }
    }
    private void unpress(Collider2D collision) {
        if (collision.gameObject.GetComponent<io_body>() != null) {
            collidingObjects.Remove(collision.gameObject);

            if (combinedMass < requiredMass) {
                pressed = false;
                animController.SetBool("depressed", false);
                processLinks();
            }
        }
    }
    private void processLinks() {
        if (combinedMass > requiredMass) {
            onPressed.Invoke();
        }
        else {
            onReleased.Invoke();
        }
    }
}
