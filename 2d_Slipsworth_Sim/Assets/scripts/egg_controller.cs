using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class egg_controller : MonoBehaviour
{
    private int requiredDeposits;
    public int currentDeposits;//gotten from summon_circle_controller
    public GameObject me;

    private int reqStage1 = 0; //we start here
    private int reqStage2; //I'm dynamic!
    private int reqStage3; //I'm static.

    public Sprite stage_1;
    public Sprite stage_2;
    public Sprite stage_3;

    private GameObject[] twigs;
    private bool growable = true;
    void Start()
    {
        UpdateReqDep();
        InitializeGrowValues();
    }
    void UpdateReqDep() {//expensive orange. Use when a new player connects.
        twigs = GameObject.FindGameObjectsWithTag("twig");
        requiredDeposits = twigs.Length;
        Array.Clear(twigs, 0, twigs.Length);
        if (requiredDeposits == 0) {
            Debug.LogError("Egg controller asserts that it does not require twigs to evolve. The controller will now commit suicide. Goodbye.");
            Destroy(this);
        }
        Debug.Log("Required deposits for summon is: " + requiredDeposits);
    }
    void InitializeGrowValues() {
        reqStage2 = (requiredDeposits / 2) * 1; //half
        Debug.Log("Egg grow stage 2 requires: " + reqStage2 + " twigs total.");
        reqStage3 = (requiredDeposits / 6) * 5;//most sticks
        Debug.Log("Egg grow stage 3 requires: " + reqStage3 + " twigs total.");
    }
    public void CheckForGrowth() {
        currentDeposits = GameObject.FindGameObjectWithTag("summon_circle").GetComponent<summon_circle_controller>().depositAmt;
        if (growable == true) {
            if (currentDeposits > reqStage1) {
                me.gameObject.GetComponent<SpriteRenderer>().sprite = stage_1;
            }
            if (currentDeposits > reqStage2) {
                me.gameObject.GetComponent<SpriteRenderer>().sprite = stage_2;
            }
            if (currentDeposits > reqStage3) {
                me.gameObject.GetComponent<SpriteRenderer>().sprite = stage_3;
                growable = false;
            }
        }
    }
}
