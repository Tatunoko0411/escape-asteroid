using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    private int trout = 0;
    public int Move = 0;
    public List<int> probability = new List<int>()
    {0,0,0,0};
    [SerializeField]public List<Texture> textures;
    [SerializeField]public SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] Animator PlayerAnim;
    public int direction = 1;
    private int waitTime = 0;
    private Vector3 TargetPos;
    private int troutTier = 0;
    public bool isSetDice = false;
    public bool isDiceRollUI = false;
    public bool isGetItemManager = false;
    public bool isStart = false;
   public bool isGoal = false;
    public bool isGoalSend = false;
    public bool Clear = false;
    public bool isSetExchange = false;
    public bool isDiceRoll = false;
    public int id  = 99;

    public int hand;

    public int handCard_id;

    public int handCard_id_2;

    public int handCard_id_3;

    [SerializeField] public List<List<GameObject>> HaveItemManagers = new List<List<GameObject>>()
   {
       new List<GameObject>(), new List<GameObject>(),new List<GameObject>(),new List<GameObject>()
   };

    [SerializeField] public  List<List<GameObject>> ItemManagers =new List<List<GameObject>>()
   {
       new List<GameObject>(), new List<GameObject>(),new List<GameObject>(),new List<GameObject>()
   };

    [SerializeField] public GameObject Dice;
    [SerializeField] Text oxText;
  
    public int oxygen = 50;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.name == "MainPlayer")
        {
            id = Client.MainPlayerID;
            GetComponent<Client>().SetAnothreID(id);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

     
        if (!isSetExchange || transform.name != "MainPlayer")
        {
            if (oxygen <= 0)
            {
                oxygen = 0;
                //強制終了
                GameEnd();
                return;
            }
            if (id == gameManager.turn || transform.name != "MainPlayer")
            {
                if (!isGoal)
                {
                    if (!isSetDice)
                    {
                        isDiceRollUI = true;

                        isSetDice = true;
                    }

                    if (isDiceRoll)
                    {
                        //プレイヤーの移動
                        if (Move > 0 )
                        {
                            if (waitTime <= 0)
                            {

                                if ((gameManager.trouts.Count <= trout + direction) ||
                                    (0 > trout + direction))
                                {
                                    Move = 0;
                                    ChangeDirection();
                                    //素材取得ターンに入る
                                    if (transform.name == "MainPlayer")
                                    {
                                        ChoiceGetItemManager();
                                    }
                                    return;
                                }
                                else
                                {
                                    TargetPos = (Vector3)(GameObject.Find("GameManager").GetComponent<GameManager>().trouts[trout + direction].transform.position);
                                    //TargetPos.y = 0.3f;
                                    jump();
                                }

                                transform.position = Vector3.MoveTowards
                                      (transform.position,
                                      TargetPos,
                                      0.1f + Time.deltaTime
                                      );


                                if (transform.position == TargetPos)
                                {
                                    Move--;
                                    trout += direction;
                                    waitTime = 30;
                                    if (Move == 0)
                                    {
                                        if (transform.name == "MainPlayer")
                                        {
                                            ChoiceGetItemManager();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                waitTime--;
                            }

                        }
                    }
                }
                else if (!isGoalSend)
                {
                    if (transform.name == "MainPlayer")
                    {
                        GetComponent<Client>().SendComment((int)Event.Event_ID.Turn_End);
                        isGoalSend = true;
                    }
                }
            }
            else
            {
                isGoalSend = false;
            }
            if (transform.name == "MainPlayer")
            {
                oxText.text = $"{oxygen}";

                if (oxygen >= 30)
                {
                    oxText.color = new Color(0.4213836f, 0.8542393f, 1, 1);
                }
                else if (oxygen < 30 && oxygen >= 10)
                {
                    oxText.color = new Color(0.990566f, .9220721f, 0.2865788f, 1);
                }
                else if (oxygen <= 10)
                {
                    oxText.color = new Color(0.9528302f, 0.2037497f, 0.2037497f, 1);
                }
            }
        }
    }

    public void ChangeDirection()
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        GetComponent<Client>().SendComment((int)Event.Event_ID.Change_Direction);
       // direction = -direction;
       gameManager.BackButton.GetComponent<Button>().interactable = false;
    }

    public void MovePlayerManager(int roll,int dice1,int dice2)
    {
        Dice dice = Dice.GetComponent<Dice>();
        Move = roll;
        isDiceRollUI = false;
        dice.DiceAnimation(dice1,dice2,roll);
    }

    public void BackPlayer(int roll)
    {
        Move = -roll;
    }

    public void ChoiceGetItemManager()
    {
        //ダイスロール系のボタンを無効にする
        GameObject gameObject = GameObject.Find("GameManager");
        isGetItemManager = true;
    }


    public void SetHaveItemManager()
    {//
        GameObject[] ActivItemManagers = GameObject.FindGameObjectsWithTag("Item");
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        foreach (GameObject r in ActivItemManagers)
        {
            Destroy(r);
        }

        for (int i = 0; i < HaveItemManagers.Count; i++)
        {
            if (HaveItemManagers[i] != null)
            {
                foreach (GameObject Item in HaveItemManagers[i])
                {
                    GameObject textObject = Instantiate(
                                             Item,
                                             gameManager.transform.position,
                                             Quaternion.identity,
                                             gameManager.parentGameObject.transform
                                             );
                }
            }
        }
    }
    public void SetItemManager()
    {//
        GameObject[] ActivItemManagers = GameObject.FindGameObjectsWithTag("Item");
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        foreach (GameObject r in ActivItemManagers)
        {
            Destroy(r);
        }

        for (int i = 0; i < ItemManagers.Count; i++)
        {
            if (ItemManagers[i] != null)
            {
                foreach (GameObject Item in ItemManagers[i])
                {
                    GameObject textObject = Instantiate(
                                             Item,
                                             gameManager.transform.position,
                                             Quaternion.identity,
                                             gameManager.parentGameObject.transform
                                             );
                }
            }
        }
    }
    public void LockPlayer(int PlayerID)
    {
       Client client = GetComponent<Client>();
        client.lookAtID = PlayerID;
        client.SendComment((int)Event.Event_ID.Look);
    }

    public void reduceOxygen()
    {
        CardManager cardManager =GameObject.Find("CardManager").GetComponent<CardManager>();
        int ItemCount = 0;
        for(int i = 0;i< HaveItemManagers.Count;i++)
        {
            ItemCount += HaveItemManagers[i].Count;
        }

        if (cardManager.breathHold == true)
        {
            ItemCount = 0;
            cardManager.breathHold = false;
        }
        oxygen -= ItemCount;
        GetComponent<Client>().SendComment((int)Event.Event_ID.Oxygen);

    }

    public void DiceRoll()
    {
        CardManager cardManager = GameObject.Find("CardManager").GetComponent<CardManager>();
        int ItemCount = 0;
        for (int i = 0; i < HaveItemManagers.Count; i++)
        {
            ItemCount += HaveItemManagers[i].Count;
        }

        if (cardManager.breathHold == true)
        {
            ItemCount = 0;
            cardManager.breathHold = false;
        }
        oxygen -= ItemCount;
        GetComponent<Client>().SendComment((int)Event.Event_ID.Dice);
    }

    public void  AddItems()
    {
        for (int i = 0; i < HaveItemManagers.Count; i++)
        {
            if (HaveItemManagers[i] != null)
            {
                for (int j = 0; j < HaveItemManagers[i].Count; j++)
                {
                    ItemManagers[i].Add(HaveItemManagers[i][j]);
                }
            }
        }
    }
    public void jump()
    {
        PlayerAnim.SetTrigger("jump");
    }

    public void GameEnd()
    {
        this.transform.position = (Vector3)(GameObject.Find("GameManager").GetComponent<GameManager>().trouts[0].transform.position);
        HaveItemManagers = new List<List<GameObject>>
        {
            new List<GameObject>(), new List<GameObject>(),new List<GameObject>(),new List<GameObject>()
        };
        GetComponent<Client>().SendComment((int)Event.Event_ID.Goal);
    }

        
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "based")
        {
            if (isStart)
            {
                isGoal = true;
                if (transform.name == "MainPlayer")
                {
                    GetComponent<Client>().SendComment((int)Event.Event_ID.Goal);
                   
                    AddItems();
                    SetItemManager();
                }
                Debug.Log("ゴールしました");
            }
            else
            {
                isStart = true;
                Debug.Log("スタートしました");
            }
            
        }

        if (other.gameObject.tag == "Level0")
        {
            probability[0] = 80;
            probability[1] = 20;
            probability[2] = 0;
            probability[3] = 0;
        }

        if (other.gameObject.tag == "Level1")
        {
            probability[0] = 30;
            probability[1] = 60;
            probability[2] = 10;
            probability[3] = 0;
        }

        if (other.gameObject.tag == "Level2")
        {
            probability[0] = 0;
            probability[1] = 30;
            probability[2] = 60;
            probability[3] = 10;
        }

        if (other.gameObject.tag == "Level3")
        {
            probability[0] = 0;
            probability[1] = 0;
            probability[2] = 20;
            probability[3] = 80;
        }
    }
}
