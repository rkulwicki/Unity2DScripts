using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowEffect : MonoBehaviour {

    public Vector2 offset = new Vector2(0.3f, -0.3f);

    private SpriteRenderer sprRndCaster;
    private SpriteRenderer sprRndShadow;

    private Transform transCaster;
    private Transform transShadow;

    public Material shadowMaterial;
    public Color shadowColor;

    

    // Use this for initialization
    void Start () {

        transCaster = transform;
        transShadow = new GameObject().transform;
        transShadow.parent = transCaster;
        transShadow.gameObject.name = "Shadow";
        transShadow.localRotation = Quaternion.identity;

        sprRndCaster = GetComponent<SpriteRenderer>();
        sprRndShadow = transShadow.gameObject.AddComponent<SpriteRenderer>();

        sprRndShadow.material = shadowMaterial;
        sprRndShadow.color = shadowColor;
        sprRndShadow.sortingLayerName = sprRndCaster.sortingLayerName;
        sprRndShadow.sortingOrder = sprRndCaster.sortingOrder - 10;

        sprRndShadow.color = new Color(1, 1, 1, 0.5f);
        transShadow.localScale = new Vector3(1,1,1);


    }
	
	// Update is called once per frame
	void LateUpdate () {

        //basically taking all the characteristics of the caster
        transShadow.position = new Vector2(transCaster.position.x + offset.x,
            transCaster.position.y + offset.y);

        sprRndShadow.sprite = sprRndCaster.sprite;
        sprRndShadow.enabled = sprRndCaster.enabled;


        //gameObject.GetComponent<SpriteRenderer>().enabled = false;

    }
}
