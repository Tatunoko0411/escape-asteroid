using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    
    public bool usedCard1 = false;  //手札の使用判定
    public bool usedCard2 = false;
    public bool usedCard3= false;
    public int hand_1;  //カードのランダム判定
    public int hand_2;
    public int hand_3;
    


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
                name = "酸素ボンベ",
                
            },
            new Card
            {
                id = 3,
                name = "不法投棄",
                
            },
            new Card
            {
                id = 4,
                name = "息止め",
                

            },
            new Card
            {
                id = 5,
                name = "緊急補給",
               
            },
            new Card
            {
                id = 6,
                name = "すり替え",
               
            },
            new Card
            {
                id = 7,
                name = "妨害工作",
               
            },
            new Card
            {
                id = 8,
                name = "忍び足",
               
            },
            new Card
            {
                id = 9,
                name = "未知への恐怖",
               

            },
            new Card
            {
                id = 10,
                name = "格上げ",
               
            },
            new Card
            {
                id = 11,
                name = "3Dプリンター"
            },
            new Card
            {
                id = 12,
                name = "宇宙の気まぐれ"
            },
            new Card
            {
                id = 13,
                name = "終末時計"
            },
            new Card
            {
                id = 14,
                name = "幸運のお守り"
            },
            new Card
            {
                id = 15,
                name = "無関心"
            },
            new Card
            {
                id = 16,
                name = "帰巣本能"
            },
            new Card
            {
                id = 17,
                name = "イカサマ"
            },
            new Card
            {
                id = 18,
                name = "買い物上手"
            },
            new Card
            {
                id = 19,
                name = "猫の手"
            },
            new Card
            {
                id = 20,
                name = "七変化"
            },
            new Card
            {
                id = 21,
                name = "テセウス"
            },
           new Card
            {
                id = 22,
                name = "速度制限"
            },
           new Card
            {
                id = 23,
                name = "宇宙の奇跡"
            },
           new Card
            {
                id = 24,
                name = "等価交換"
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

                    break;
                case 2:

                    break;
                case 3:

                    break;
                case 4:

                    break;
                case 5:

                    break;
                case 6:

                    break;
                case 7:

                    break;
                case 8:

                    break;
                case 9:

                    break;
                case 10:

                    break;
                case 11:

                    break;
                case 12:

                    break;
                case 13:

                    break;
                case 14:

                    break;
                case 15:

                    break;
                case 16:

                    break;
                case 17:

                    break;
                case 18:

                    break;
                case 19:

                    break;
                case 20:

                    break;
                case 21:

                    break;
                case 22:

                    break;
                case 23:

                    break;
                case 24:

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

    }

}
