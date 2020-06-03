using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class io_door : MonoBehaviour {
    public int requiredConnections = 1;//override in inspector as needed.
    public int incomingConnections = 0;
    public GameObject me;

    public Collider2D locLider;

    public Sprite door_0;
    public Sprite door_1;

    //todo add method to be controlled by unity event in floor_button.cs

    public void changeIncomingConnections(int ic) {
        incomingConnections = incomingConnections + ic;
        if (incomingConnections < 0) {
            incomingConnections = 0;
            Debug.LogError("io_door connections was less than 0");
        }
        doDoor();
    }
    

    public void doDoor() {
        //incomingConnections = 0;
            if (incomingConnections >= requiredConnections) {
                locLider.enabled = false;
                me.gameObject.GetComponent<SpriteRenderer>().sprite = door_1;
            }
            else {
                locLider.enabled = true;
                me.gameObject.GetComponent<SpriteRenderer>().sprite = door_0;
        }
    }
}
