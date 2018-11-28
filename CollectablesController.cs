using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesController : MonoBehaviour {

	public int money = 0;
    //chance is 0 to 100.
    public int chanceIndex, chanceCommon, chanceUncommon, chanceRare, chanceSuperRare, chanceUltraRare;
    public bool lucky;

    private bool playerWantsItem;


    #region All Objects and Object Pooler ;)

    ObjectPooler objectPooler;

    [System.Serializable]
    public class rankedItems
    {

        public string tag;
        public List<string> objPoolerPrefabTags;
        
    }

    public List<rankedItems> allItemsPrefabs;

    #endregion







    // Use this for initialization
    void Start () {

        objectPooler = ObjectPooler.Instance;
	}

    void Update(){

        CheckPlayerInput();

        GenerateItem();

    }

    //1, 5, and 10 dollar bills are the only ones that exists now
    void OnTriggerEnter2D(Collider2D other){

		if (other.tag == "1dollar")
			money = money + 1;
		else if (other.tag == "5dollar")
			money = money + 5;
		else if (other.tag == "10dollar")
			money = money + 10;

        if (money > 100) {
            money = 100;
        }
	}

    //as of right now, the generate item key is SPACE. Might Change
    void CheckPlayerInput(){

        bool input_Space = Input.GetKey(KeyCode.Space);

        playerWantsItem = input_Space;
    }


    //generates items based on # of money. Don't access directly in update.
    //is accessed in generate item.
    void GenerateChance() {

        if (money == 0)
        {

            chanceCommon = 0;
            chanceUncommon = 0;
            chanceRare = 0;
            chanceSuperRare = 0;
            chanceUltraRare = 0;
        }
        else if (money > 0 && money < 10)
        {

            chanceCommon = 100;
            chanceUncommon = 0;
            chanceRare = 0;
            chanceSuperRare = 0;
            chanceUltraRare = 0;
        }
        else if (money >= 10 && money < 20)
        {

            chanceCommon = 50;
            chanceUncommon = 100;
            chanceRare = 0;
            chanceSuperRare = 0;
            chanceUltraRare = 0;
        }
        else if (money >= 20 && money < 30)
        {

            chanceCommon = 30;
            chanceUncommon = 90;
            chanceRare = 100;
            chanceSuperRare = 0;
            chanceUltraRare = 0;
        }
        else if (money >= 30 && money < 40)
        {

            chanceCommon = 10;
            chanceUncommon = 50;
            chanceRare = 95;
            chanceSuperRare = 100;
            chanceUltraRare = 0;
        }
        else if (money >= 40 && money < 50)
        {

            chanceCommon = 5;
            chanceUncommon = 20;
            chanceRare = 80;
            chanceSuperRare = 100;
            chanceUltraRare = 0;
        }

        //over 50 means you wont get a common item
        else if (money >= 50 && money < 60)
        {

            chanceCommon = 0;
            chanceUncommon = 20;
            chanceRare = 70;
            chanceSuperRare = 99;
            chanceUltraRare = 100;
        }
        else if (money >= 60 && money < 70)
        {

            chanceCommon = 0;
            chanceUncommon = 1;
            chanceRare = 30;
            chanceSuperRare = 95;
            chanceUltraRare = 100;
        }
        else if (money >= 70 && money < 80)
        {

            chanceCommon = 0;
            chanceUncommon = 1;
            chanceRare = 20;
            chanceSuperRare = 90;
            chanceUltraRare = 100;
        }
        else if (money >= 80 && money < 90)
        {

            chanceCommon = 0;
            chanceUncommon = 1;
            chanceRare = 10;
            chanceSuperRare = 80;
            chanceUltraRare = 100;
        }
        else if (money >= 90 && money < 100)
        {

            chanceCommon = 0;
            chanceUncommon = 1;
            chanceRare = 10;
            chanceSuperRare = 40;
            chanceUltraRare = 100;
        }
        else if (money >= 100)
        {

            chanceCommon = 0;
            chanceUncommon = 0;
            chanceRare = 1;
            chanceSuperRare = 20;
            chanceUltraRare = 100;
        }
    }

    void GenerateItem() {

        if (playerWantsItem && money > 0)
        {
            GenerateChance();

            chanceIndex = Random.Range(1, 100);

            //take a look at where the random number is between 1 and 100 (inclusive)
            //and then compare it to the odds that were generated based on money


            //common--------------------------------------------------------------
            if (chanceIndex > 0 && chanceIndex <= chanceCommon)
            {

                print("We got a common item.");

                //get a random index between the size of the obj tags list
                int ri = Random.Range(0, allItemsPrefabs[0].objPoolerPrefabTags.Count);

                //A list of common items within the list of lists.
                string s = allItemsPrefabs[0].objPoolerPrefabTags[ri];
                objectPooler.SpawnFromPool(s, transform.position, Quaternion.identity);
            }
            //uncommon-------------------------------------------------------------
            else if (chanceIndex > chanceCommon && chanceIndex <= chanceUncommon)
            {

                print("We got an uncommon item.");

                //get a random index between the size of the obj tags list
                int ri = Random.Range(0, allItemsPrefabs[1].objPoolerPrefabTags.Count);

                //A list of common items within the list of lists.
                string s = allItemsPrefabs[1].objPoolerPrefabTags[ri];
                objectPooler.SpawnFromPool(s, transform.position, Quaternion.identity);
            }
            //rare-------------------------------------------------------------------
            else if (chanceIndex > chanceUncommon && chanceIndex <= chanceRare)
            {

                print("We got a rare item.");

                //get a random index between the size of the obj tags list
                int ri = Random.Range(0, allItemsPrefabs[2].objPoolerPrefabTags.Count);

                //A list of common items within the list of lists.
                string s = allItemsPrefabs[2].objPoolerPrefabTags[ri];
                objectPooler.SpawnFromPool(s, transform.position, Quaternion.identity);
            }
            //super rare------------------------------------------------------------
            else if (chanceIndex > chanceRare && chanceIndex <= chanceSuperRare)
            {

                print("We got a super rare item.");

                //get a random index between the size of the obj tags list
                int ri = Random.Range(0, allItemsPrefabs[3].objPoolerPrefabTags.Count);

                //A list of common items within the list of lists.
                string s = allItemsPrefabs[3].objPoolerPrefabTags[ri];
                objectPooler.SpawnFromPool(s, transform.position, Quaternion.identity);
            }
            //ultra rare-------------------------------------------------------------
            else if (chanceIndex > chanceSuperRare && chanceIndex <= chanceUltraRare)
            {

                print("We got an ultra rare item.");

                //get a random index between the size of the obj tags list
                int ri = Random.Range(0, allItemsPrefabs[4].objPoolerPrefabTags.Count);

                //A list of common items within the list of lists.
                string s = allItemsPrefabs[4].objPoolerPrefabTags[ri];
                objectPooler.SpawnFromPool(s, transform.position, Quaternion.identity);
            }
            else {
                print("Something went wrong.");
            }

            //spend the money and reset to 0
            money = 0;

        }
    }
}
