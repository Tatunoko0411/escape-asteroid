using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class exchange : MonoBehaviour
{
    public int ReqNumberHigh;//必要数(高ティア→低ティア)
    public int ReqNumberLow;//必要数(低ティア→高ティア)

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TryExchenge()
    {
        Player player = GameObject.Find("Player").GetComponent<Player>(); 
        for (int i = 0; i < 4; i++)
        {
            if (i > 0)
            {
                if (ReqNumberHigh > player.items[i + 1].Count)
                {
                    for (int f = 0;f < 4;f++)
                    {
                        GameObject.Find("GameManager").GetComponent<GameManager>().ExchangeButton[i][f].GetComponent<Button>().enabled = false;
                    }
                    
                }
            }

            if (i < 4)
            {
                if (ReqNumberHigh > player.items[i + 1].Count && ReqNumberLow > player.items[i - 1].Count)
                {
                    for (int f = 0; f < 4; f++)
                    {
                        GameObject.Find("GameManager").GetComponent<GameManager>().ExchangeButton[i][f].GetComponent<Button>().enabled = false;
                    }
                }
            }

            
        }
    }





}
