using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour {

    public bool moveRight;
    public Vector2 velocity;
    public int leftBound, rightBound;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 pos = transform.localPosition;

        if (moveRight){

            pos.x += velocity.x * Time.deltaTime;
        }
        else{

            pos.x -= velocity.x * Time.deltaTime;
        }

        transform.localPosition = pos;

        //did the cloud get to the left end?
        if (transform.localPosition.x < leftBound) {

            transform.localPosition = new Vector2(rightBound-1, transform.localPosition.y);
        }

        //did the cloud get to the right end?
        if (transform.localPosition.x > rightBound)
        {

            transform.localPosition = new Vector2(leftBound+1, transform.localPosition.y);
        }
    }
}
