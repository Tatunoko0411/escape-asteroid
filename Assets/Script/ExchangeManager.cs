using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ExchangeManager : MonoBehaviour
{
    //した二つは仮でstaticにしてます(後で変える)
    [SerializeField] public  GameObject CheckBoxPrefab;
    [SerializeField] public　 GameObject parentGameObject;

    [SerializeField] public GameObject ExchangeItem;//交換後のアイテム

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
        ExchangeItem = gameObject;
        Player player = GameObject.Find("Player").GetComponent<Player>();
        GameObject[] Items = GameObject.FindGameObjectsWithTag("CheckBox");
        if (Items.Length > 0)
        {
            foreach (GameObject r in Items)
            {
                Destroy(r);
            }
        }

        for (int i = 0; i < player.items.Count; i++)
        {
            if (player.items[i] != null)
            {
                foreach (GameObject item in player.items[i])
                {
                    Item item1 = item.GetComponent<Item>(); 
                    GameObject textObject = Instantiate(
                                             ExchangeButton[item1.Tire][item1.ID],
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
        
        switch (toggle.isOn)
        {
            case true:
                Cost.Add(cost);
                break;
            case false:
                Cost.Remove(cost);
                break;
        }
       
    }

    public void GetToggle(GameObject gameObject)
    {
        toggle = gameObject.GetComponent<Toggle>();
    }

    public void Exchange()//交換
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();
        for (int i = 0; i < Cost.Count; i++)
        {
            for (int z = 0; z < player.items.Count; z++)
            {
                if (player.items[z] != null)
                {
                     for(int x = 0;x < player.items[z].Count; x++)
                    {
                        if (player.items[z][x] == Cost[i])
                        {
                            player.items[z].Remove(Cost[i]);
                            player.items[ExchangeItem.GetComponent<Item>().Tire].Add(ExchangeItem);
                            return;
                        }
                    }
                }
            }

        }

       

    }
}
