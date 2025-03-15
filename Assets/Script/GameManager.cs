using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] public List<GameObject> trouts;
    [SerializeField]  public GameObject DiceRollButton; 
    [SerializeField] public GameObject BackButton;
    [SerializeField] public GameObject GetItemManagerUI;
    [SerializeField] public  List<GameObject> Tire0List;
    [SerializeField] public  List<GameObject> Tire1List;
    [SerializeField] public  List<GameObject> Tire2List;
    [SerializeField] public  List<GameObject> Tire3List;


    public List<List<GameObject>> ItemManagerList = new List<List<GameObject>>();

    [SerializeField] public GameObject parentGameObject;
    public int activePlayerManager = 0;
    public int turn = 0;   //どのプレイヤーのターンかを判別
    public bool isDiceRoll=false;
 
    // Start is called before the first frame update
    void Start()
    {
        ItemManagerList.Add(Tire0List);
        ItemManagerList.Add(Tire1List);
        ItemManagerList.Add(Tire2List);
        ItemManagerList.Add(Tire3List);
        
    } 

    // Update is called once per frame
    void Update()
    {
        
    }



    public void GetItem(bool Get)
    {
        GameObject PlayerManagers = GameObject.Find("MainPlayer");
        PlayerManager PlayerManager = PlayerManagers.GetComponent<PlayerManager>();
        if (Get)
        {
            float result;
            int Tire =0;
            int kinds = 0;
         

               
                
            if (turn == PlayerManager.id)
            {


                //はいを選択した場合
                result = Random.Range(0, 100);
                if (result <= PlayerManager.probability[0])
                {//ティア0取得
                    Tire = 0;
                    kinds = Random.Range(0, 4);
                }
                else if (result > PlayerManager.probability[0] && result <= (100 - (PlayerManager.probability[2] + PlayerManager.probability[3])))
                {//ティア1取得
                    Tire = 1;
                    kinds = Random.Range(0, 4);
                }
                else if (result > (PlayerManager.probability[1] + PlayerManager.probability[0]) && result <= (100 - PlayerManager.probability[3]))
                {//ティア2取得
                    Tire = 2;
                    kinds = Random.Range(0, 4);
                }
                else if (result > (PlayerManager.probability[0] + PlayerManager.probability[1] + PlayerManager.probability[2]) && result <= 100)
                {//ティア3取得
                    Tire = 3;
                    kinds = Random.Range(0, 3);
                }
                ExchangeManager exchangeManager = GameObject.Find("ExchangeManager").GetComponent<ExchangeManager>();
                PlayerManager.ItemManagers[Tire].Add(ItemManagerList[Tire][kinds]);
                Debug.Log($"アイテムを手に入れた!Tire{Tire},{kinds}");
                GameObject textObject = Instantiate(
                                         ItemManagerList[Tire][kinds],
                                         parentGameObject.transform.position,
                                         Quaternion.identity,
                                         parentGameObject.transform
                                         );
            }
        }
        

        PlayerManager.isGetItemManager = false;

        //次の人のターンに
        PlayerManagers.GetComponent<Client>().SendComment((int)Event.Event_ID.Turn_End);
        
    }
}
