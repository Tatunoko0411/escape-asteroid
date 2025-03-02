using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ExchangeManager : MonoBehaviour
{
    //した二つは仮でstaticにしてます(後で変える)
    [SerializeField] public  GameObject CheckBoxPrefab;
    [SerializeField] public　 GameObject parentGameObject;

    [SerializeField] public GameObject ExchangeItem;//交換後のアイテム

    public List<GameObject> Cost;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCheckBox(int Tire)
    {
        GameObject textObject = Instantiate(
          CheckBoxPrefab,
          parentGameObject.transform.position,
          Quaternion.identity,
          parentGameObject.transform
        );
    }

    public void AddCost(GameObject cost)//指定したアイテムをリストコストに指定する
    {
        Cost.Add(cost);
    }

    public void Exchange()//交換
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();
        for (int i = 0; i < Cost.Count; i++)
        {
            for (int z = 0; z < player.items[ExchangeItem.GetComponent<Item>().Tire].Count; z++)
            {
                if (player.items[ExchangeItem.GetComponent<Item>().Tire][z].GetComponent<Item>().ID == Cost[i].GetComponent<Item>().ID)
                {
                    player.items[ExchangeItem.GetComponent<Item>().Tire].Remove(player.items[ExchangeItem.GetComponent<Item>().Tire][z]);
                    break;
                }
            }

        }

        player.items[ExchangeItem.GetComponent<Item>().Tire].Add(ExchangeItem);
    }
}
