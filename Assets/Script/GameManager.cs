using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] public List<GameObject> trouts;
    [SerializeField]  public GameObject DiceRollButton; 
    [SerializeField] public GameObject BackButton;
    [SerializeField] public GameObject GetItemUI;
    [SerializeField] public  List<GameObject> Tire0List;
    [SerializeField] public  List<GameObject> Tire1List;
    [SerializeField] public  List<GameObject> Tire2List;
    [SerializeField] public  List<GameObject> Tire3List;


    public List<List<GameObject>> ItemList = new List<List<GameObject>>();

    [SerializeField] public GameObject parentGameObject;

    CardManager cardManager;
    public int randMax = 50;
    public int randMin = 0;
    public int randAns;

    // Start is called before the first frame update
    void Start()
    {
        ItemList.Add(Tire0List);
        ItemList.Add(Tire1List);
        ItemList.Add(Tire2List);
        ItemList.Add(Tire3List);

    } 

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeDirection()   //帰還ボタン使用時
    {
        GameObject.Find($"Player").GetComponent<Player>().direction = - GameObject.Find($"Player").GetComponent<Player>().direction ;
        BackButton.GetComponent<Button>().interactable = false ;
    }

    public void GetItem(bool Get)
    {
        if (Get)
        {
            float result;
            int Tire =0;
            int kinds = 0;
            Player player = GameObject.Find($"Player").GetComponent<Player>();
            //はいを選択した場合
            result = Random.Range(0, 100);
            if (result <= player.probability[0])
            {//ティア0取得
                Tire = 0;
                kinds = Random.Range(0, 4);
            }
            else if (result > player.probability[0] && result <= (100-(player.probability[2] + player.probability[3])))
            {//ティア1取得
                Tire = 1;
                kinds = Random.Range(0, 4);
            }
            else if (result > (player.probability[1]+ player.probability[0]) && result <= (100 - player.probability[3]))
            {//ティア2取得
                Tire = 2;
                kinds = Random.Range(0, 4);
            }
            else if (result > (player.probability[0] + player.probability[1]+player.probability[2]) && result <= 100)
            {//ティア3取得
                Tire = 3;
                kinds = Random.Range(0, 3);
            }


            ExchangeManager exchangeManager = GameObject.Find("ExchangeManager").GetComponent<ExchangeManager>();   
            GameObject.Find($"Player").GetComponent<Player>().items[Tire].Add(ItemList[Tire][kinds]);
            Debug.Log($"アイテムを手に入れた!Tire{Tire},{kinds}");
            GameObject textObject = Instantiate(
                                     ItemList[Tire][kinds],
                                     parentGameObject.transform.position,
                                     Quaternion.identity,
                                     parentGameObject.transform
                                     );

            //幸運のお守り発動中
            if (cardManager.luckyCharm == true)
            {
                randAns = Random.Range(randMin, randMax);
               

                //30%の確率で複製
                if(randAns <= 15)
                {

                    exchangeManager = GameObject.Find("ExchangeManager").GetComponent<ExchangeManager>();
                    GameObject.Find($"Player").GetComponent<Player>().items[Tire].Add(ItemList[Tire][kinds]);
                    Debug.Log($"アイテムを手に入れた!Tire{Tire},{kinds}");
                    textObject = Instantiate(
                                            ItemList[Tire][kinds],
                                            parentGameObject.transform.position,
                                            Quaternion.identity,
                                            parentGameObject.transform
                                            );
                }
                
            }



        }

        DiceRollButton.SetActive(true);
        BackButton.SetActive(true);
        GetItemUI.SetActive(false);
    }
}
