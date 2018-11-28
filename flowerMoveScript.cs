using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flowerMoveScript : MonoBehaviour {


    public float rustleDistance, rustleSpeed;
    public int timesToRustle;

    private Vector2 originalPosition;

    // Use this for initialization
    void Start () {

        originalPosition = transform.localPosition;
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player") {

            StartCoroutine(Rustle());
        }
    }


    IEnumerator Rustle()
    {

        for (int i = 0; i < timesToRustle; i++) {

            while (true)
            {

                transform.localPosition = new Vector2(transform.localPosition.x + rustleSpeed * Time.deltaTime, transform.localPosition.y);

                if (transform.localPosition.x >= originalPosition.x + rustleDistance)
                {

                    break;
                }

                yield return null;
            }



            while (true)
            {

                transform.localPosition = new Vector2(transform.localPosition.x - rustleSpeed * Time.deltaTime, transform.localPosition.y);

                if (transform.localPosition.x <= originalPosition.x - rustleDistance)
                {

                    break;
                }

                yield return null;
            }

            while (true)
            {

                transform.localPosition = new Vector2(transform.localPosition.x + rustleSpeed * Time.deltaTime, transform.localPosition.y);

                if (transform.localPosition.x >= originalPosition.x)
                {
                    transform.localPosition = originalPosition;
                    break;
                }

                yield return null;
            }
        }
    }
}
