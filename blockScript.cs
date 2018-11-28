using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockScript : MonoBehaviour {

    public float bounceHeight, bounceSpeed;
    public int blockHits;
    public float finalHitSpeed;
    public float finalHitEmission;

    public ParticleSystem woodParticles;

    private Vector2 originalPosition;

    private bool canBounce = true;

    private string blockType;

    private BoxCollider2D col2D;
    private SpriteRenderer renderer2D;
    private ParticleSystem particles2D;

    private void Awake()
    {
        woodParticles.enableEmission = false;
    }
    // Use this for initialization
    void Start () {

        blockType = transform.tag;
        originalPosition = transform.localPosition;
        col2D = GetComponent<BoxCollider2D>();
        renderer2D = GetComponent<SpriteRenderer>();
        particles2D = GetComponentInChildren<ParticleSystem>();
      
    }

    public void BlockBounce() {

        if (canBounce)
        {

            if (blockHits > 1)
            {
                StartCoroutine(Bounce());
                blockHits = blockHits - 1;
            }
            else if (blockHits == 1)
            {
                //at the final hit, make the particle big!
                var ma = particles2D.main;
                ma.startSpeed = finalHitSpeed;
                var em = particles2D.emission;
                em.rateOverTime = finalHitEmission;
                StartCoroutine(Bounce());
                blockHits = blockHits - 1;
                StartCoroutine(FadeToInactive(0, 0.2f, 0));
            }
            else
            {

                canBounce = false;
                woodParticles.enableEmission = false;
            }
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Bounce() {

        woodParticles.enableEmission = true;

        while (true) {

            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + bounceSpeed * Time.deltaTime);

            if (transform.localPosition.y >= originalPosition.y + bounceHeight) {

                break;
            }

            yield return null;
        }

        while (true) {

            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - bounceSpeed * Time.deltaTime);

            if (transform.localPosition.y <= originalPosition.y)
            {
                transform.localPosition = originalPosition;
                break;
            }

            yield return null;
        }

        woodParticles.enableEmission = false;
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

        col2D.enabled = false;
        renderer2D.enabled = false;

        //this is here so that way the particles have time to do their thing
        for (float s = 0.0f; s < 1.0f; s += Time.deltaTime / 4.0f)
        {

            yield return null;
        }
        gameObject.SetActive(false);

    }
}
