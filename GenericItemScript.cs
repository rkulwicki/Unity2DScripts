using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericItemScript : MonoBehaviour, IPooledObject
{

    public float upForce, sideForce, timeBeforeFade, angle, rotationFactor;
    public bool goingRight, fadeAfterSpawn, stopMovementWhenGround;

    private GameObject activePlayer;

    private float xForce, yForce;

    public void OnObjectSpawn()
    {

        //reset back to normal
        transform.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, 1);

        StartCoroutine(Rotate());
        //start fade coroutine after spawn
        if (fadeAfterSpawn)
        {
            StartCoroutine(FadeToInactive(0, 0.5f, timeBeforeFade));
        }
        /*
        //if player facing right, kick ball right. If not, kick left
        float xForce = sideForce; //Random.Range(-sideForce, sideForce);
        float yForce = upForce; //Random.Range(upForce / 2f, upForce);
        Vector2 force = new Vector2(xForce, yForce);

        GetComponent<Rigidbody2D>().velocity = force;
        */
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {

            activePlayer = other.gameObject;
            goingRight = activePlayer.GetComponent<Player>().isFacingRight;

            //if player facing right, kick ball right. If not, kick left

            if (goingRight)
            {

                xForce = sideForce;
            }
            else
            {

                xForce = -sideForce;
            }


            //float xForce = sideForce; //Random.Range(-sideForce, sideForce);
            float yForce = upForce; //Random.Range(upForce / 2f, upForce);
            Vector2 force = new Vector2(xForce, yForce);

            GetComponent<Rigidbody2D>().velocity = force;
        }
        else if (other.tag == "Ground")
        {

            //if stops movement all together when hits the ground
            if (stopMovementWhenGround)
            {

                Rigidbody2D body2d = GetComponent<Rigidbody2D>();
                body2d.constraints = RigidbodyConstraints2D.FreezeAll;
            }

            //fades and then deactivates
            //xForce = 0;
            yForce = upForce / 2;
            Vector2 force = new Vector2(xForce, yForce);
            GetComponent<Rigidbody2D>().velocity = force;

            //fade if hits ground
            if (!fadeAfterSpawn)
            {
                StartCoroutine(FadeToInactive(0, 0.5f, timeBeforeFade));
            }
        }
        else if (other.tag == "Block")
        {
            //add a random force if it's anything else
            float xForce = Random.Range(-sideForce, sideForce);
            float yForce = Random.Range(-upForce, upForce);
            Vector2 force = new Vector2(xForce, yForce);
            GetComponent<Rigidbody2D>().velocity = force;

        }


        Vector2 scale = transform.localScale;
        Vector2 tempScale = transform.localScale;
    }


    IEnumerator FadeToInactive(float aValue, float aTime, float timeBeforeFade)
    {
        for (float s = 0.0f; s < 1.0f; s += Time.deltaTime / timeBeforeFade)
        {

            yield return null;
        }

        float alpha = transform.GetComponent<SpriteRenderer>().material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            transform.GetComponent<SpriteRenderer>().material.color = newColor;
            yield return null;
        }

        gameObject.SetActive(false);

    }

    //Rotates in circles
    IEnumerator Rotate()
    {

        while (true) // this could also be a condition indicating "alive or dead"
        {
            // we scale all axis, so they will have the same value, 
            // so we can work with a float instead of comparing vectors
            angle += Time.deltaTime * rotationFactor;
            transform.localRotation = Quaternion.Euler(0, 0, angle);
            yield return null;
        }
    }
}
