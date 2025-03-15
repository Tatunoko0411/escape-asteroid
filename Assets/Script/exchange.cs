using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class exchange : MonoBehaviour
{
    private int ReqNumberHigh = 1;//必要数(高ティア→低ティア)
    private int ReqNumberLow = 3;//必要数(低ティア→高ティア)

    public int ItemManagerTire;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool isuse = false;
        PlayerManager PlayerManager = GameObject.Find("MainPlayer").GetComponent<PlayerManager>();
            if (ItemManagerTire > 0)
            {
                if (ReqNumberLow <= PlayerManager.ItemManagers[ItemManagerTire - 1].Count)
                {
                    isuse = true;
                   
                } 
                else if (ReqNumberLow > PlayerManager.ItemManagers[ItemManagerTire - 1].Count)
                {

                    isuse = false;

                }

            }

            if (isuse)
            {
              GetComponent<Button>().interactable = true;
              return;
            }

        if (ItemManagerTire < 3)
            {
               if (ReqNumberHigh <= PlayerManager.ItemManagers[ItemManagerTire + 1].Count)
               {
                isuse = true;
               
              

               }
               else if (ReqNumberHigh > PlayerManager.ItemManagers[ItemManagerTire + 1].Count)
               {

                isuse = false;
               }

            }

        if (isuse)
        {
            GetComponent<Button>().interactable = true;
        }
        else
        {
            GetComponent<Button>().interactable = false;
        }

        


        
    }




}
