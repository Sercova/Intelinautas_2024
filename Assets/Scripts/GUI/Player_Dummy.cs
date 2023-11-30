using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Dummy : MonoBehaviour {

    public GameObject Dummy;
    public GameController GCtrller;

    void Start()
    {
        this.transform.position = Dummy.transform.position;
    }

	void LateUpdate () {
        // Turn back to the player position
        //if (GCtrller.watchingFinished)
        //    this.transform.position = Vector2.Lerp(this.transform.position,Dummy.transform.position,0.25f);
    }
}
