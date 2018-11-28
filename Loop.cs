using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop : MonoBehaviour {

	public bool isRightPortal;
	public GameObject leftPortal;
	public GameObject rightPortal;
	public float buffer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
   {
		
		if (isRightPortal) {

			col.gameObject.transform.position = new Vector3 (leftPortal.transform.position.x + buffer, 
                col.gameObject.transform.position.y, col.gameObject.transform.position.z);
		} else {

			col.gameObject.transform.position = new Vector3 (rightPortal.transform.position.x - buffer, 
                col.gameObject.transform.position.y, col.gameObject.transform.position.z);
		}
	}
}
