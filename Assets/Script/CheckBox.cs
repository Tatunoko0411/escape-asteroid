using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckBox : MonoBehaviour
{
    // Start is called before the first frame update
    public int ItemTire;
    [SerializeField] GameObject item;


    public void Add()
    {
        Toggle toggle = GetComponent<Toggle>();
         GameObject.Find("ExchangeManager").GetComponent<ExchangeManager>().AddCost(item, toggle.isOn);
    }
}
