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
    // Start is called before the first frame update
    void Start()
    {
        
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
            //はいを選択した場合
            GameObject.Find($"Player").GetComponent<Player>().items.Add(new Item() {Tire = 0,ID = 1});
            Debug.Log("アイテムを手に入れた!");
        }

        DiceRollButton.SetActive(true);
        BackButton.SetActive(true);
        GetItemUI.SetActive(false);
    }
}
