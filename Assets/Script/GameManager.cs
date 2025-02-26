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
     public static List<List<GameObject>> ItemList;
    [SerializeField] public static List<GameObject> Tire0List;
    [SerializeField] public static List<GameObject> Tire1List;
    [SerializeField] public static List<GameObject> Tire2List;
    [SerializeField] public static List<GameObject> Tire3List;
    public static List<List<GameObject>> ExchangeButton;
    [SerializeField] public static List<GameObject> Tire0Button;
    [SerializeField] public static List<GameObject> Tire1Button;
    [SerializeField] public static List<GameObject> Tire2Button;
    [SerializeField] public static List<GameObject> Tire3Button;

    // Start is called before the first frame update
    void Start()
    {
        ItemList.Add(Tire0List);
        ItemList.Add(Tire1List);
        ItemList.Add(Tire2List);
        ItemList.Add(Tire3List);

        ExchangeButton.Add(Tire0Button);
        ExchangeButton.Add(Tire1Button);
        ExchangeButton.Add(Tire2Button);
        ExchangeButton.Add(Tire3Button);
    } 

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeDirection()
    {
        GameObject.Find($"Player").GetComponent<Player>().direction = - GameObject.Find($"Player").GetComponent<Player>().direction ;
    }

    public void GetItem(bool Get)
    {
        if (Get)
        {
            int Tire = 0;
            int ItemID = 0;
            //はいを選択した場合
            GameObject.Find($"Player").GetComponent<Player>().items[Tire].Add(ItemList[Tire][ItemID]);
            Debug.Log("アイテムを手に入れた!");
        }

        DiceRollButton.SetActive(true);
        BackButton.SetActive(true);
        GetItemUI.SetActive(false);
    }
}
