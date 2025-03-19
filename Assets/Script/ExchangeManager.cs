using Assembly_CSharp;
using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ExchangeManager : MonoBehaviour
{
    //した二つは仮でstaticにしてます(後で変える)
    [SerializeField] public  GameObject CheckBoxPrefab;
    [SerializeField] public　 GameObject parentGameObject;

    [SerializeField] public GameObject ExchangeItemManager;//交換後のアイテム

    [SerializeField]public Image　CostColorIMGLow;
    [SerializeField]public Image　CostColorIMGHigh;
    [SerializeField]public Text CostColorTextHigh;
    [SerializeField] public Text CostColorTextLow;

   [SerializeField] public List<GameObject> Tire0Button;
    [SerializeField] public List<GameObject> Tire1Button;
    [SerializeField] public List<GameObject> Tire2Button;
    [SerializeField] public List<GameObject> Tire3Button;
    //各目標アイテム作成の有無の確認
    public bool boosterItem = false;
    public bool cockpitItem = false;
    public bool parachuteItem = false;
    public bool toiletItem = false;
    public bool controllerItem = false;
    public bool isSelect = false;
    public bool isHigh = false;//高いアイテムで交換するとき
    public bool isLow = false;//低いアイテムで交換するとき
   public int ReqNumberHigh = 1;//必要数(高ティア→低ティア)
   public int ReqNumberLow = 3;//必要数(低ティア→高ティア)

    //作成した目標アイテムの量
    public int clearItemCont = 0;
    public List<List<GameObject>> ExchangeButton = new List<List<GameObject>>();


    public List<GameObject> Cost;
    private Toggle toggle;
    private PlayerManager player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("MainPlayer").GetComponent<PlayerManager>();
        ExchangeButton.Add(Tire0Button);
        ExchangeButton.Add(Tire1Button);
        ExchangeButton.Add(Tire2Button);
        ExchangeButton.Add(Tire3Button);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCheckBox(GameObject gameObject)
    {
        ExchangeItemManager = gameObject;
        Cost  = new List<GameObject>();
        CostColorTextHigh.color =Color.black;
        CostColorTextLow.color =Color.black;
        CostColorTextHigh.text = $"×{Cost.Count}/{ReqNumberLow}";
        CostColorTextLow.text = $"×{Cost.Count}/{ReqNumberHigh}";
        switch (gameObject.GetComponent<ItemManager>().Tire)
        {
            case 0:
                CostColorIMGHigh.color = Color.blue;
                CostColorIMGLow.color = Color.white;
                break;
                case 1:
                CostColorIMGHigh.color = new Color(0.55f,0,0.84f);
                CostColorIMGLow.color = Color.gray;
                break;
                case 2:
                CostColorIMGHigh.color = Color.yellow;
                CostColorIMGLow.color = Color.blue;
                break;
                case 3:
                CostColorIMGHigh.color = Color.white;
                CostColorIMGLow.color = new Color(0.55f, 0, 0.84f);
                break;
        }
        PlayerManager PlayerManager = GameObject.Find("MainPlayer").GetComponent<PlayerManager>();
        GameObject[] ItemManagers = GameObject.FindGameObjectsWithTag("CheckBox");
        if (ItemManagers.Length > 0)
        {
            foreach (GameObject r in ItemManagers)
            {
                Destroy(r);
            }
        }
        ItemManager itemManager = ExchangeItemManager.GetComponent<ItemManager>();
        for (int i = 0; i < PlayerManager.ItemManagers.Count; i++)
        {
            if (itemManager.Tire != i&&itemManager.Tire+1 >= i&& itemManager.Tire-1 <= i)
            {
                if (PlayerManager.ItemManagers[i] != null)
                {
                    foreach (GameObject ItemManager in PlayerManager.ItemManagers[i])
                    {
                        ItemManager ItemManager1 = ItemManager.GetComponent<ItemManager>();
                        GameObject textObject = Instantiate(
                                                 ExchangeButton[ItemManager1.Tire][ItemManager1.ID],
                                                 parentGameObject.transform.position,
                                                 Quaternion.identity,
                                                 parentGameObject.transform
                                                 );
                    }
                }
            }
        }
    }

    public void UpdateCheckBox()
    {
        ExchangeManager exchangeManager = GameObject.Find("ExchangeManager").GetComponent<ExchangeManager>();
        GameObject[] checks = GameObject.FindGameObjectsWithTag("CheckBox");
        if (checks.Length > 0)
        {
            foreach (GameObject r in checks)
            {
                if (isHigh)
                {
                   if(r.GetComponent<CheckBox>().ItemTire<exchangeManager.ExchangeItemManager.GetComponent<ItemManager>().Tire)
                   {
                        r.GetComponent<Toggle>().interactable = false;
                   }
                }
                else if (isLow)
                {
                    if (r.GetComponent<CheckBox>().ItemTire > exchangeManager.ExchangeItemManager.GetComponent<ItemManager>().Tire)
                    {
                        r.GetComponent<Toggle>().interactable = false;
                    }
                }
            }
        }
    }

    public void AddCost(GameObject cost,bool isOn)//指定したアイテムをリストコストに指定する
    {
        ExchangeManager exchangeManager = GameObject.Find("ExchangeManager").GetComponent<ExchangeManager>();
        switch (isOn)
        {
            case true:
                exchangeManager.Cost.Add(cost);
                if(cost.GetComponent<ItemManager>().Tire >= exchangeManager.ExchangeItemManager.GetComponent<ItemManager>().Tire)
                {
                    exchangeManager.isHigh = true;
                    exchangeManager.isLow = false;
                }
                else if(cost.GetComponent<ItemManager>().Tire <= exchangeManager.ExchangeItemManager.GetComponent<ItemManager>().Tire)
                {
                    exchangeManager.isLow = true;
                    exchangeManager.isHigh = false;
                }
                break;
            case false:
                exchangeManager.Cost.Remove(cost);
                break;
        }
        

        exchangeManager.UpdateCheckBox();

        if (isHigh)
        {
            CostColorTextHigh.text = $"×{exchangeManager.Cost.Count}/1";
            if (exchangeManager.Cost.Count == ReqNumberHigh)
            {
                CostColorTextHigh.color = Color.green;
                GameObject.FindWithTag("Exchange").GetComponent<Button>().interactable = true;
            }
        }
        else if (isLow)
        {
            CostColorTextLow.text = $"×{exchangeManager.Cost.Count}/3";
            if (exchangeManager.Cost.Count == ReqNumberLow)
            {
                CostColorTextLow.color = Color.green;
                GameObject.FindWithTag("Exchange").GetComponent<Button>().interactable = true;
            }
        }
    }

    public void GetToggle(GameObject gameObject)
    {
        toggle=GameObject.Find(gameObject.transform.name).GetComponent<Toggle>();
    }

    public void Exchange()//交換
    {
        ExchangeManager exchangeManager = GameObject.Find("ExchangeManager").GetComponent<ExchangeManager>();
        for (int i = 0; i < exchangeManager.Cost.Count; i++)
        {
            for (int z = 0; z < player.ItemManagers.Count; z++)
            {
                if (player.ItemManagers[z] != null)
                {
                     for(int x = 0;x < player.ItemManagers[z].Count; x++)
                     {
                        if (player.ItemManagers[z][x] == exchangeManager.Cost[i])
                        {
                            player.ItemManagers[z].Remove(exchangeManager.Cost[i]);
                           
                            break;
                        }
                     }
                }
            }

        }

        if (isHigh)
        {

            player.ItemManagers[ExchangeItemManager.GetComponent<ItemManager>().Tire].Add(exchangeManager.ExchangeItemManager);
            player.ItemManagers[ExchangeItemManager.GetComponent<ItemManager>().Tire].Add(exchangeManager.ExchangeItemManager);

        }
        else if (isLow)
        {
            player.ItemManagers[ExchangeItemManager.GetComponent<ItemManager>().Tire].Add(exchangeManager.ExchangeItemManager);
        }
        GameObject[] ItemManagers = GameObject.FindGameObjectsWithTag("CheckBox");
        if (ItemManagers.Length > 0)
        {
            foreach (GameObject r in ItemManagers)
            {
                Destroy(r);
            }
        }

    }


    public void clearCheck()
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        // アイテムの所持状況を管理する辞書（ティアごとにIDを管理）
        List<List<GameObject>>  playerhaveItemList = player.ItemManagers;

        Target target =  gameManager.TarGetItems[gameManager.target].GetComponent<Target>();

        for (int i = 0; i < target.targetCosts.Count; i++)
        {
            for (int z = 0; z < playerhaveItemList.Count; z++)
            {
                if (playerhaveItemList[z] != null)
                {
                    for (int x = 0; x < playerhaveItemList[z].Count; x++)
                    {
                        if (playerhaveItemList[z][x] == target.targetCosts[i])
                        {
                            playerhaveItemList[z].Remove(target.targetCosts[i]);
                            target.allCosts[i] = true;
                            break;
                        }
                    }
                }
            }

        }


        for (int i = 0;i < target.allCosts.Count;i++)
        {
            if (!target.allCosts[i])
            {
                return;
            }
        }

        player.Clear = true;

        // ゲームクリア条件の確認
        if (player.Clear)
        {
            // ゲームクリアフラグを立てる
            Debug.Log("ゲームクリア");
        }
    }

    // プレイヤーのインベントリからアイテムを削除する処理
    void RemoveItemFromPlayer(GameObject item)
    {
        // プレイヤーのインベントリを走査し、該当するアイテムを削除
        for (int y = 0; y < player.ItemManagers.Count; y++)
        {
            if (player.ItemManagers[y] != null)
            {
                player.ItemManagers[y].Remove(item); // インベントリからアイテムを削除
            }
        }
    }
}
