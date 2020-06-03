using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class summon_circle_controller : MonoBehaviour {
    public int depositAmt = 0;
    public Collider2D locCollider;

    public GameObject splatter1;
    public GameObject splatter2;
    void Start() {

    }

    // Update is called once per frame
    void FixedUpdate() {
        //objectIsOver = false;   
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<io_body>() != null) {
            if (collision.gameObject.GetComponent<io_body>().isTwig == true) {
                List<GameObject> twigs = new List<GameObject>();
                if (collision.gameObject.GetComponent<io_body>().isTwig == true) {
                    twigs.Add(collision.gameObject);
                }
                int newDeposits = 0;
                //twigs.Add(collision.gameObject);
                for (int i = 0; i < twigs.Count; i++) {
                    if (twigs[i].GetComponent<io_body>().isTwig == false) {
                        twigs.RemoveAt(i);
                    }
                    else if (twigs[i].GetComponent<io_body>().isTwig == true) {
                        
                        io_body nDp = twigs[i].gameObject.GetComponent<io_body>();
                        newDeposits += nDp.depositValue;//combined value of all depositable objects. If zero
                        /*if (Random.Range(0, 100) > 50) {
                            GameObject S = Instantiate(splatter1, twigs[i].transform, twigs[i].transform) as GameObject;
                            //Debug.Log("tried to instantiate rand1");
                        }
                        else {
                            GameObject S = Instantiate(splatter2, twigs[i].transform, twigs[i].transform) as GameObject;
                            //Debug.Log("tried to instantiate rand2");
                        }*///This shit just does NOT WORK, why are the TRANSFORMS ALL WRONG AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA

                        twigs[i].gameObject.GetComponent<io_body>().CheckForDeposit();
                        
                    }
                }
                depositAmt = depositAmt + newDeposits;
                //Debug.Log("summon circle has a twig over it");
                GameObject eggBoi = GameObject.FindGameObjectWithTag("SummonObj");
                eggBoi.GetComponent<egg_controller>().CheckForGrowth();//this sets the eggs depositAMT as well.
            }

        } else {
            return;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {//for some reason only this works so what the fuck?
        if (collision.gameObject.GetComponent<io_body>().isTwig == true) {
            //Debug.Log("summon circle has stopped detecting a twig over it");
        }
    }
}
