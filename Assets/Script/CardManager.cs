using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardManager : MonoBehaviour
{

    public bool usedCard1 = false;  //手札の使用判定
    public bool usedCard2 = false;
    public bool usedCard3 = false;
    public int hand_1;  //カードのランダム判定
    public int hand_2;
    public int hand_3;

    [SerializeField] GameObject CardNum1;
    [SerializeField] GameObject CardNum2;
    [SerializeField] GameObject CardNum3;
    [SerializeField] GameObject CardNum4;
    [SerializeField] GameObject CardNum5;
    [SerializeField] GameObject CardNum6;
    [SerializeField] GameObject CardNum7;
    [SerializeField] GameObject CardNum8;
    [SerializeField] GameObject CardNum9;
    [SerializeField] GameObject CardNum10;
    [SerializeField] GameObject CardNum11;

    public int randMax = 50;
    public int randMin = 0;
    public int randAns;



    public bool luckyCharm;
    public bool homingInstinct;
    public bool goodShoping;
    public bool breathHold;
    public bool luckyDice;
    public bool speedlimit;


    [SerializeField] public GameObject playerManagerObj;
    [SerializeField] public GameObject DiceObj;
    [SerializeField] public GameObject GameManagerObj;
    [SerializeField] public GameObject ExchangeObj;

    DIce dice;
    GameManager gameManager;
    PlayerManager player;
    ExchangeManager exchangeManager;
    exchange exchange;



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



  

    private void Start()
    {
        player = playerManagerObj.GetComponent<PlayerManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        exchangeManager = GameObject.Find("ExchangeManager").GetComponent<ExchangeManager>();
      //  dice = GameObject.Find("Dice").GetComponent<DIce>();
        SetHand();



    }




    // Update is called once per frame
    void Update()
    {

    }

    public void CardAction(int CardNum,int playerId)
    {
        int haveCardSet = 0;
        //int playCard = 0;
        if (player.hand > 0)    //カードの残数確認
        {



           //Debug.Log($"{Card[playCard].id}使用、{Card[playCard].name}");
            switch (CardNum)    //使うカードの効果判定、処理
            {
                //各カードの効果処理(カードのidにて判定)
                case 1:
                    if (player.id == playerId)
                    {
                        //片方のサイコロを456賽にする
                        //456賽はdice2で

                        luckyDice = true;
                        CardNum1.SetActive(false);
                    }
                    break;

                case 2:

                    //酸素を消費せず行動可能
                    //breathHold = true;
                    if (player.id == playerId)
                    {
                        breathHold = true;
                        CardNum2.SetActive(false);
                    }
                    break;
                case 3:

                    player.oxygen += 3;
                    if (player.id == playerId)
                    {
                        CardNum3.SetActive(false);
                    }
                    break;

                case 4:

                    GameObject[] playerManagers = GameObject.FindGameObjectsWithTag("Player");
                    for (int i = 0; i < playerManagers.Length; i++)
                    {
                        PlayerManager player = playerManagers[i].GetComponent<PlayerManager>();
                        //他プレイヤーを選択肢人マス戻す
                        if (player.id != playerId)
                        {
                            player.BackPlayer(1);
                        }
                    }
                    if (player.id == playerId)
                    {
                        CardNum4.SetActive(false);
                    }
                    break;

                case 5:
                    if (player.id == playerId)
                    {
                        randAns = Random.Range(1, 3);
                        player.oxygen -= randAns;
                        Debug.Log($"{randAns}の酸素が放出された");
                        Client client = GameObject.Find("MainPlayer").GetComponent<Client>();
                        client.SendComment((int)Event.Event_ID.Oxygen);
                        CardNum5.SetActive(false);
                    }
                    break;
                case 6:
                    //gamemanagerにて処理

                    //持続効果、アイテム獲得時一定確率でアイテムを複製
                    if (player.id == playerId)
                    {
                        luckyCharm = true;
                        CardNum6.SetActive(false);
                    }
                    break;

                case 7:

                    //diceにて処理
                    //帰還時のみ使用可能、ダイスの合計値*2進める
                    if (player.id == playerId)
                    {
                        if (player.direction <= -1)
                        {
                            homingInstinct = true;//カード使用フラグ
                        }
                        else
                        {

                            if (CardNum == 1)
                            {
                                usedCard1 = false;
                            }
                            else if (CardNum == 2)
                            {
                                usedCard2 = false;
                            }
                            else if (CardNum == 3)
                            {
                                usedCard3 = false;
                            }
                            Debug.Log("条件を満たしてないので使用できません");
                            player.hand++;
                        }
                        CardNum7.SetActive(false);
                    }
                    break;

                case 8:

                    //継続効果、高ティアへのアイテム交換に必要な素材数を減少
                    if (player.id == playerId)
                    {
                        goodShoping = true;
                        exchangeManager.ReqNumberLow = 2;
                        Debug.Log($"変換必要個数{exchangeManager.ReqNumberLow}");
                        CardNum8.SetActive(false);
                    }
                    break;
                case 9:
                    if (player.id == playerId)
                    {
                        int Tire = 0;
                        int kinds = 0;
                        //player = GameObject.Find($"Player").GetComponent<Player>();


                        //ティア0素材アイテムを獲得、一定確率でティア1アイテム
                        randAns = Random.Range(randMin, randMax);

                        //約25%でティア1の素材獲得
                        if (randAns >= 14)//25%は12.5なので13まで獲得とする
                        {
                            Tire = 0;
                        }
                        else
                        {
                            Tire = 1;
                        }

                        kinds = Random.Range(0, 4);


                        player.HaveItemManagers[Tire].Add(gameManager.ItemManagerList[Tire][kinds]);
                        Debug.Log($"アイテムを手に入れた!Tire{Tire},{kinds}");
                        GameObject textObject = Instantiate(
                                                 gameManager.ItemManagerList[Tire][kinds],
                                                 gameManager.parentGameObject.transform.position,
                                                 Quaternion.identity,
                                                 gameManager.parentGameObject.transform
                                                 );


                        CardNum9.SetActive(false);
                    }
                    break;

                case 10:
                    if (player.id == playerId)
                    {
                        if (player.hand == 1)//使用可能な手札がないときは変更しない
                        {
                            Debug.Log("入れ替える手札がありません");
                            return;
                        }
                        else
                        {
                            CardNum1.SetActive(false);
                            CardNum2.SetActive(false);
                            CardNum3.SetActive(false);
                            CardNum4.SetActive(false);
                            CardNum5.SetActive(false);
                            CardNum6.SetActive(false);
                            CardNum7.SetActive(false);
                            CardNum8.SetActive(false);
                            CardNum9.SetActive(false);
                            CardNum10.SetActive(false);
                            CardNum11.SetActive(false);

                            for (int i = 0; i < player.hand ; i++)
                            {
                                int HandNum = Random.Range(1, Card.Length + 1);


                                switch (HandNum)
                                {
                                    case 1:
                                        CardNum1.SetActive(true);
                                        CardNum1.transform.position = new Vector3(760 + (230 * i), 150, 1);
                                        break;
                                    case 2:
                                        CardNum2.SetActive(true);
                                        CardNum2.transform.position = new Vector3(760 + (230 * i), 150, 1);
                                        break;
                                    case 3:
                                        CardNum3.SetActive(true);
                                        CardNum3.transform.position = new Vector3(760 + (230 * i), 150, 1);
                                        break;
                                    case 4:
                                        CardNum4.SetActive(true);
                                        CardNum4.transform.position = new Vector3(760 + (230 * i), 150, 1);
                                        break;
                                    case 5:
                                        CardNum5.SetActive(true);
                                        CardNum5.transform.position = new Vector3(760 + (230 * i), 150, 1);
                                        break;
                                    case 6:
                                        CardNum6.SetActive(true);
                                        CardNum6.transform.position = new Vector3(760 + (230 * i), 150, 1);
                                        break;
                                    case 7:
                                        CardNum7.SetActive(true);
                                        CardNum7.transform.position = new Vector3(760 + (230 * i), 150, 1);
                                        break;
                                    case 8:
                                        CardNum8.SetActive(true);
                                        CardNum8.transform.position = new Vector3(760 + (230 * i), 150, 1);
                                        break;
                                    case 9:
                                        CardNum9.SetActive(true);
                                        CardNum9.transform.position = new Vector3(760 + (230 * i), 150, 1);
                                        break;
                                    case 10:
                                        CardNum10.SetActive(true);
                                        CardNum10.transform.position = new Vector3(760 + (230 * i), 150, 1);
                                        break;
                                    case 11:
                                        CardNum11.SetActive(true);
                                        CardNum11.transform.position = new Vector3(760 + (230 * i), 150, 1);
                                        break;

                                }




                            }

                        }
                        CardNum10.SetActive(false);
                    }
                    break;
                case 11:
                    if (player.id != playerId)
                    {
                        //2d2に変更
                        dice.Dice1Max = 2;
                        dice.Dice2Max = 2;

                        speedlimit = true;//カード使用フラグ

                        Debug.Log($"dice１最大値{dice.Dice1Max}:dice２最大値{dice.Dice2Max}");
                    }
                    if (player.id == playerId)
                    {
                        CardNum11.SetActive(false);
                    }
                    break;

            }

            player.hand -= 1;

        }
        else
        {//手札を全て使用された場合
            Debug.Log("手札がありません");
        }

        Debug.Log($"残り手札{player.hand}");

    }

    public void changeround()
    {


    }

    public void SetHand()   //ラウンド開始時の手札交換
    {

        int[] hundCrd = new int[3];

        CardNum1.SetActive(false);
        CardNum2.SetActive(false);
        CardNum3.SetActive(false);
        CardNum4.SetActive(false);
        CardNum5.SetActive(false);
        CardNum6.SetActive(false);
        CardNum7.SetActive(false);
        CardNum8.SetActive(false);
        CardNum9.SetActive(false);
        CardNum10.SetActive(false);
        CardNum11.SetActive(false);

        hand_1 = Random.Range(1, Card.Length + 1);//1枚目
        hand_2 = Random.Range(1, Card.Length + 1);//２枚目
        hand_3 = Random.Range(1, Card.Length + 1);//3枚目

        while (true)
        {
            if (hand_1 == hand_2)//２枚目の変更
            {
                hand_2 = Random.Range(1, Card.Length + 1);

            }
            else
            {
                break;
            }
        }

        while (true)
        {
            if (hand_1 == hand_3 || hand_2 == hand_3)//3枚目の変更
            {
                hand_3 = Random.Range(1, Card.Length + 1);
            }
            else
            {
                break;
            }
        }

        hundCrd[0] = hand_1;
        hundCrd[1] = hand_2;
        hundCrd[2] = hand_3;


        for (int i = 0; i < hundCrd.Length; i++)
        {
            switch (hundCrd[i])
            {
                case 1:
                    CardNum1.SetActive(true);
                    CardNum1.transform.position = new Vector3(760 + (230 * i), 150, 1);
                    break;
                case 2:
                    CardNum2.SetActive(true);
                    CardNum2.transform.position = new Vector3(760 + (230 * i), 150, 1);
                    break;
                case 3:
                    CardNum3.SetActive(true);
                    CardNum3.transform.position = new Vector3(760 + (230 * i), 150, 1);
                    break;
                case 4:
                    CardNum4.SetActive(true);
                    CardNum4.transform.position = new Vector3(760 + (230 * i), 150, 1);
                    break;
                case 5:
                    CardNum5.SetActive(true);
                    CardNum5.transform.position = new Vector3(760 + (230 * i), 150, 1);
                    break;
                case 6:
                    CardNum6.SetActive(true);
                    CardNum6.transform.position = new Vector3(760 + (230 * i), 150, 1);
                    break;
                case 7:
                    CardNum7.SetActive(true);
                    CardNum7.transform.position = new Vector3(760 + (230 * i), 150, 1);
                    break;
                case 8:
                    CardNum8.SetActive(true);
                    CardNum8.transform.position = new Vector3(760 + (230 * i), 150, 1);
                    break;
                case 9:
                    CardNum9.SetActive(true);
                    CardNum9.transform.position = new Vector3(760 + (230 * i), 150, 1);
                    break;
                case 10:
                    CardNum10.SetActive(true);
                    CardNum10.transform.position = new Vector3(760 + (230 * i), 150, 1);
                    break;
                case 11:
                    CardNum11.SetActive(true);
                    CardNum11.transform.position = new Vector3(760 + (230 * i), 150, 1);
                    break;

            }


            //変更した値を手札に代入
            player.handCard_id = hand_1;
            player.handCard_id_2 = hand_2;
            player.handCard_id_3 = hand_3;

            Debug.Log($"手札 1枚目{player.handCard_id}２枚目{player.handCard_id_2}３枚目{player.handCard_id_3}");

            player.hand = 3;//手札の枚数カウントリセット

            //各手札を使用可能の状態にする
            usedCard1 = false;
            usedCard2 = false;
            usedCard3 = false;

            //カードの継続効果を無効にする
            if (goodShoping != false)//買い物上手
            {
                goodShoping = false;
                exchangeManager.ReqNumberLow = 3;
                Debug.Log($"{Card[7].name}の効果が失われた\n変換必要個数{exchangeManager.ReqNumberLow}");
            }

            if (luckyCharm != false)//幸運のお守り
            {
                luckyCharm = false;
                Debug.Log($"{Card[5].name}の効果が失われた");
            }

            //これらのフラグはダイスロール後すぐに効果無効
            homingInstinct = false;//帰巣本能
            speedlimit = false;//速度制限
            luckyDice = false;//幸運のサイコロ

            //酸素計算処理終了後に効果無効
            breathHold = false;//息止め
        }
    }

    //こっちで送信してからアクションする
    public void SendCard(int cardNum)
    {
        Client client = GameObject.Find("MainPlayer").GetComponent<Client>();
        client.CardId = cardNum;
        client.SendComment((int)Event.Event_ID.Card);
    }
}