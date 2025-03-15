using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExchangeManager : MonoBehaviour
{
    //した二つは仮でstaticにしてます(後で変える)
    [SerializeField] public  GameObject CheckBoxPrefab;
    [SerializeField] public　 GameObject parentGameObject;

    [SerializeField] public GameObject ExchangeItemManager;//交換後のアイテム

    [SerializeField] public List<GameObject> Tire0Button;
    [SerializeField] public List<GameObject> Tire1Button;
    [SerializeField] public List<GameObject> Tire2Button;
    [SerializeField] public List<GameObject> Tire3Button;

    public List<List<GameObject>> ExchangeButton = new List<List<GameObject>>();

    public List<GameObject> Cost;
    private Toggle toggle;
    // Start is called before the first frame update
    void Start()
    {

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
        PlayerManager PlayerManager = GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>();
        GameObject[] ItemManagers = GameObject.FindGameObjectsWithTag("CheckBox");
        if (ItemManagers.Length > 0)
        {
            foreach (GameObject r in ItemManagers)
            {
                Destroy(r);
            }
        }

        for (int i = 0; i < PlayerManager.ItemManagers.Count; i++)
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

    public void AddCost(GameObject cost)//指定したアイテムをリストコストに指定する
    {
        ExchangeManager exchangeManager = GameObject.Find("ExchangeManager").GetComponent<ExchangeManager>();
        switch (toggle.isOn)
        {
            case true:
                exchangeManager.Cost.Add(cost);
                break;
            case false:
                exchangeManager.Cost.Remove(cost);
                break;
        }
       
    }

    public void GetToggle(GameObject gameObject)
    {
        toggle = gameObject.GetComponent<Toggle>();
    }

    public void Exchange()//交換
    {
        PlayerManager PlayerManager = GameObject.Find("MainPlayer").GetComponent<PlayerManager>();
        for (int i = 0; i < Cost.Count; i++)
        {
            for (int z = 0; z < PlayerManager.ItemManagers.Count; z++)
            {
                if (PlayerManager.ItemManagers[z] != null)
                {
                     for(int x = 0;x < PlayerManager.ItemManagers[z].Count; x++)
                     {
                        if (PlayerManager.ItemManagers[z][x] == Cost[i])
                        {
                            PlayerManager.ItemManagers[z].Remove(Cost[i]);
                           
                            return;
                        }
                     }
                }
            }

        }

        PlayerManager.ItemManagers[ExchangeItemManager.GetComponent<ItemManager>().Tire].Add(ExchangeItemManager);


    }
}
