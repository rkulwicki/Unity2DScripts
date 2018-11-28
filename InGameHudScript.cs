using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameHudScript : MonoBehaviour {

    bool showText = true;
    public int maxHealthRef, currentHealthRef, maxMoney, currentMoneyRef;
    public Font FRFont;

    private string[] maxHealthBubbles;

    // Use this for initialization
    void Start () {
        maxHealthRef = FindObjectOfType<Player>().maxHealth;
        maxMoney = 100;

    }

	// Update is called once per frame
	void Update () {
		currentHealthRef = FindObjectOfType<Player>().currentHealth;
        currentMoneyRef = FindObjectOfType<CollectablesController>().money;

    }

    void OnGUI()
    {
        Texture2D heartTexture = Resources.Load<Texture2D>("heart");
        Texture2D moneyTexture = Resources.Load<Texture2D>("moneySign");

        //Font FRFont = Resources.Load<Font>("Noturnal Hand");

        GUIStyle FRStyle = new GUIStyle();
        FRStyle.font = FRFont;
        FRStyle.fontSize = Screen.height/15;
        FRStyle.normal.textColor = Color.black;

        GUIStyle FRStyle2 = new GUIStyle();
        FRStyle2.font = FRFont;
        FRStyle2.fontSize = Screen.height / 15;
        FRStyle2.normal.textColor = Color.green;

        string[] maxHealthBubbles = new string[maxHealthRef];
        for (int i = 0; i < currentHealthRef; i++)
        {
            maxHealthBubbles[i] = ".";
        }

        if (showText)
        {

            //health
            string[] healthBubbles = new string[currentHealthRef];
            for (int i = 0; i < currentHealthRef; i++){
                healthBubbles[i] = ".";
            }

            //health GUI
            GUI.Label(new Rect(Screen.width / 17,Screen.height/30, 100, 20), "LIFE: " + currentHealthRef + "/"+ maxHealthRef, FRStyle);

            //money GUI
            GUI.Label(new Rect(Screen.width / 17, Screen.height / 8, 100, 20), "MONEY: " + currentMoneyRef + "/" + maxMoney, FRStyle);

            //this may be used for text chat!!!
            //GUI.TextArea(textArea, "Here is a block of text\nlalalala\nanother line\nI could do this all day!");
        }

    }

}
