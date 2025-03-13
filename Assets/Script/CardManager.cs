using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    
    public bool usedCard1 = false;  //手札の使用判定
    public bool usedCard2 = false;
    public bool usedCard3= false;
    public int hand_1;  //カードのランダム判定
    public int hand_2;
    public int hand_3;

    public int randMax = 50;
    public int randMin = 0;
    public int randAns;
    


    public bool luckyCharm;
    public bool homingInstinct;
    public bool goodShoping;
    public bool breathHold;

    Dice dice;
    GameManager gameManager;
    Player player;
    ExchangeManager exchangeManager;
    



    Card[] Card = new Card[]
       {
            new Card
            {
                id = 1,
                name = "幸運のサイコロ",
                
            },
            new Card
            {

                id = 2,
                name = "息止め",
                
            },
            new Card
            {
                id = 3,
                name = "緊急補給",
                
            },
            new Card
            {
                id = 4,
                name = "忍び足",
                

            },
            new Card
            {
                id = 5,
                name = "終末時計",
               
            },
            new Card
            {
                id = 6,
                name = "幸運のお守り",
               
            },
            new Card
            {
                id = 7,
                name = "帰巣本能",
               
            },
            new Card
            {
                id = 8,
                name = "買い物上手",
               
            },
            new Card
            {
                id = 9,
                name = "猫の手",
               

            },
            new Card
            {
                id = 10,
                name = "テセウスの船",
               
            },
            new Card
            {
                id = 11,
                name = "速度制限"
            },
            
       };

    List<Player> players = new List<Player>();

    private void Start()
    {

        SetHand();
        


    }




    // Update is called once per frame
    void Update()
    {

    }

    public void CardAction(int CardNum)
    {
        int haveCardSet = 0;
        int playCard  = 0;
        if (players[0].hand > 0)    //カードの残数確認
        {
            if(CardNum == 1)    //各カードの使用判定
            {
                if(usedCard1 == false)
                {
                    //使用するカードのidを処理用の変数に代入
                    playCard = players[0].handCard_id;
                    usedCard1 = true;//カードを使用した状態にする
                }
                else
                {
                    Debug.Log("このカードはもう使用されました");
                    return;
                }
            }
            else if(CardNum ==2)
            {   if(usedCard2 ==false)
                {
                    playCard = players[0].handCard_id_2;
                    usedCard2 = true;
                }
                else
                {
                    Debug.Log("このカードはもう使用されました");
                    return;
                }
               
            }
            else if(CardNum == 3)
            {
                if (usedCard3 == false)
                {
                    playCard = players[0].handCard_id_3;
                    usedCard3 = true;
                }
                else
                {
                    Debug.Log("このカードはもう使用されました");
                    return;
                }
            }

            
            Debug.Log($"{Card[playCard - 1].id}使用、{Card[playCard - 1].name}");
            switch (playCard)    //使うカードの効果判定、処理
            {
                //各カードの効果処理(カードのidにて判定)
                case 1:
                    //456賽はdice2で変更
                    dice.Dice2Max = 6;
                    dice.Dice2Min = 4;  

                    break;
                
                case 2:

                    breathHold = true;

                    break;
                case 3:

                    player.oxygen += 3;

                    break;
               
                case 4:

                    GameObject.Find($"Player").GetComponent<Player>().MovePlayer(-1);

                    break;
               
                case 5:

                    randAns = Random.Range(1, 3);
                    player.oxygen -= randAns;
                    Debug.Log($"{randAns}の酸素が放出された");

                    break;
                case 6:
                    //gamemanagerにて処理
                    luckyCharm = true;
                    break;
               
                case 7:
                    //diceにて処理
                    //帰還時のみ
                    if(gameManager.BackButton == false)
                    {
                        homingInstinct = true;
                    }

                    break;
                
                case 8:
                    //exchangeにて処理
                    goodShoping = true;

                    break;
                case 9:
                    int Tire = 0;
                    int kinds = 0;
                     player = GameObject.Find($"Player").GetComponent<Player>();

                    randAns = Random.Range(randMin, randMax);

                    
                    //約25%でティア1の素材獲得
                    if(randAns >= 14)//25%は12.5なので13まで獲得とする
                    {
                        Tire = 0;
                    }
                    else
                    {
                        Tire = 1;
                    }

                    kinds = Random.Range(0, 4);

                    exchangeManager = GameObject.Find("ExchangeManager").GetComponent<ExchangeManager>();
                    GameObject.Find($"Player").GetComponent<Player>().items[Tire].Add(gameManager.ItemList[Tire][kinds]);
                    Debug.Log($"アイテムを手に入れた!Tire{Tire},{kinds}");
                    GameObject textObject = Instantiate(
                                             gameManager.ItemList[Tire][kinds],
                                             gameManager.parentGameObject.transform.position,
                                             Quaternion.identity,
                                             gameManager.parentGameObject.transform
                                             );

                    

                    break;
               
                case 10:
                    if (players[0].hand == 0)//使用可能な手札がないときは変更しない
                    {
                        Debug.Log("手札がありません");
                    }
                    else
                    {
                        if (usedCard1 == false)
                        {
                            haveCardSet = players[0].handCard_id;
                            hand_1 = Random.Range(1, 24);//1枚目
                            while (true)
                            {
                                if (hand_1 == haveCardSet)
                                {
                                    hand_1 = Random.Range(1, 24);//1枚目
                                }
                                else
                                {
                                    players[0].handCard_id = hand_1;
                                    break;
                                }
                            }
                        }

                        if (usedCard2 == false)
                        {
                            haveCardSet = players[0].handCard_id_2;
                            hand_2 = Random.Range(1, 24);

                            while (true)
                            {
                                if (hand_1 == hand_2 || hand_2 == haveCardSet)//２枚目の変更
                                {
                                    hand_2 = Random.Range(1, 24);

                                }
                                else
                                {
                                    players[0].handCard_id_2 = hand_2;
                                    break;
                                }
                            }


                        }

                        if (usedCard3 == false)
                        {
                            haveCardSet = players[0].handCard_id_3;
                            hand_2 = Random.Range(1, 24);

                            while (true)
                            {
                                if (hand_1 == hand_2 || hand_2 == haveCardSet)//２枚目の変更
                                {
                                    hand_2 = Random.Range(1, 24);

                                }
                                else
                                {
                                    players[0].handCard_id_3 = hand_3;

                                    break;
                                }
                            }


                        }

                        Debug.Log($"手札 1枚目:{players[0].handCard_id} ２枚目:{players[0].handCard_id_2} ３枚目:{players[0].handCard_id_3}");

                    }



                    break;
                case 11:

                    dice.Dice1Max = 2;
                    dice.Dice2Max = 2;


                    break;
               
            }

            players[0].hand -= 1;
           
        }
        else
        {//手札を全て使用された場合
            Debug.Log("手札がありません");
        }

        Debug.Log($"残り手札{players[0].hand}");

    }

    public void changeround()
    {
        
       
    }

    public void SetHand()   //ラウンド開始時の手札交換
    {
        
        hand_1 = Random.Range(1, 24);//1枚目
        hand_2 = Random.Range(1, 24);//２枚目
        hand_3 = Random.Range(1, 24);//3枚目

        while (true)
        {           
            if(hand_1 == hand_2)//２枚目の変更
            {
                hand_2 = Random.Range(1, 24);
                
            }
            else
            {
                break;
            }
        }

        while(true)
        {
            if(hand_1 == hand_3 || hand_2 == hand_3)//3枚目の変更
            {
                hand_3 = Random.Range(1, 24);
            }
            else
            {
                break;
            }
        }

        //変更した値を手札に代入
        players[0].handCard_id = hand_1;
        players[0].handCard_id_2 = hand_2;
        players[0].handCard_id_3 = hand_3;

        Debug.Log($"手札 1枚目{players[0].handCard_id}２枚目{players[0].handCard_id_2}３枚目{players[0].handCard_id_3}");

        players[0].hand = 3;//手札の枚数カウントリセット

        //各手札を使用可能の状態にする
        usedCard1 = false;
        usedCard2 = false;
        usedCard3 = false;

        //カードの効果を無効にする
        
        luckyCharm = false;
        homingInstinct = false;
        goodShoping = false;




    }

}
