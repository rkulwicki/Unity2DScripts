using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAwayBuilding : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Once the dollar hits a player, it will be destroyed
    void OnTriggerEnter2D(Collider2D other)
    {

        Vector3 scale = transform.localScale;
        Vector3 tempScale = transform.localScale;

        if (other.tag == "Player")
        {

            StartCoroutine(FadeTo(0, 0.5f));
        }

    }

    
    void OnTriggerExit2D(Collider2D other)
    {

        Vector3 scale = transform.localScale;
        Vector3 tempScale = transform.localScale;

        if (other.tag == "Player")
        {

            StartCoroutine(FadeTo(1,0.5f));
        }

    }
    

    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = transform.GetComponent<SpriteRenderer>().material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            transform.GetComponent<SpriteRenderer>().material.color = newColor;
            yield return null;
        }
    }
}
