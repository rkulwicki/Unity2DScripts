using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dollar : MonoBehaviour {

	public int respawnTime;

	public float maxSize;
	public float growFactor;
	//public float waitTime;

	public float angle;
	public float maxAngle;
	public float minAngle;
	public float rotationFactor;
	private Vector3 respawnPosition;

	void Start () {
		
		StartCoroutine (Scale());
		StartCoroutine (Rotate ());
		respawnPosition = transform.localPosition;
	}

	void Update () {
		
	}

	//Once the dollar hits a player, it will be destroyed
	void OnTriggerEnter2D(Collider2D other){

		Vector3 scale = transform.localScale;
		Vector3 tempScale = transform.localScale;

		if (other.tag == "Player") {

            //plays sound. This is in the SoundManager script, not game object.
            SoundManager.PlaySound("dollarCollect");

			//we want to disable the collider asap so no other players can
			//collect it until it respawns again.
			gameObject.GetComponent<Collider2D> ().enabled = false;

			StartCoroutine(despawn (other.gameObject.transform.localPosition));
			StartCoroutine (respawnCollider ());
			StartCoroutine (respawnRenderer ());
		}
	
	}

	//I want to make it so the dollar does some sort of animation
	//to indicate that it was collected but I don't have that yet.
	private IEnumerator despawn(Vector3 other){


		float timer = 0;

		//calculate the angle at which the dollar goes toward the player.
		//float slope = (other.y - respawnPosition.y)/(other.x - respawnPosition.y);
		//print (slope);
		while(transform.localScale.x > 0)
		{
			timer += Time.deltaTime;
			transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime * growFactor * 60;
			transform.localPosition += new Vector3(0, 1, 0) * Time.deltaTime * growFactor * 60;

			yield return null;
		}

		//after enlarging, the dollar renderer goes away
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		transform.localPosition = respawnPosition;
	}

	//enable collider
	private IEnumerator respawnCollider(){

		yield return new WaitForSeconds (respawnTime);
		gameObject.GetComponent<Collider2D> ().enabled = true;
	}

	//enable renderer
	private IEnumerator respawnRenderer(){

		yield return new WaitForSeconds (respawnTime);
		gameObject.GetComponent<SpriteRenderer> ().enabled = true;
	}


	//Gives a cute little zoom in and out animation. That's it
	IEnumerator Scale()
	{
		float timer = 0;

		while(true) // this could also be a condition indicating "alive or dead"
		{
			// we scale all axis, so they will have the same value, 
			// so we can work with a float instead of comparing vectors
			while(maxSize > transform.localScale.x)
			{
				timer += Time.deltaTime;
				transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * growFactor;
				yield return null;
			}

			//I left this in here just in case I want to pause in between rotations
			//or scaling...
			//yield return new WaitForSeconds(waitTime);

			timer = 0;
			while(1 < transform.localScale.x)
			{
				timer += Time.deltaTime;
				transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime * growFactor;
				yield return null;
			}

			timer = 0;
		}
	}

	//Rotates the dollars for a cute little addition to the animation. Again, that's all.
	IEnumerator Rotate()
	{
		float timer = 0;

		while(true) // this could also be a condition indicating "alive or dead"
		{
			// we scale all axis, so they will have the same value, 
			// so we can work with a float instead of comparing vectors
			while(maxAngle > angle)
			{
				timer += Time.deltaTime;
				angle += Time.deltaTime * rotationFactor;
				transform.localRotation = Quaternion.Euler(0, 0, angle);
				yield return null;
			}
			// reset the timer

			timer = 0;
			while(minAngle < angle)
			{
				timer += Time.deltaTime;
				angle -= Time.deltaTime * rotationFactor;
				transform.localRotation = Quaternion.Euler(0, 0, angle);
				yield return null;
			}

			timer = 0;
		}
	}
}
