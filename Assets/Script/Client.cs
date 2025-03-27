using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Linq;
using Newtonsoft.Json;
using Assets.Script;
using Assembly_CSharp;
using static Event;
using Unity.VisualScripting;

public class Client : MonoBehaviour
{
    [SerializeField] GameObject systemMessage;
    [SerializeField] GameObject commentPrefab;
    [SerializeField] GameObject parentObject;
    Text answerText;
    SynchronizationContext context; // ワーカースレッドからメインスレッドに処理を渡すのに使用する
    static TcpClient tcpClient;
    static bool isConnected = false; // サーバーとの接続処理が一通り終わったらtrueにする
    public int lookAtID;//情報を見たい人のID
    bool waitSend= false;
    static bool first = false;
    static public int MainPlayerID;
    public int CardId;
    static public List<bool> ClearList = new List<bool>() { false,false,false,false};
    static PlayerManager player;
    static GameManager gameManager;
    static CardManager cardManager;
    // 接続先のサーバー情報を設定
#if DEBUG
    public string host = "127.0.0.1";
    public int port = 20001;
#else
    public string host = "4.216.50.103";

    public int port = 20001;
#endif

    /// <summary>
    /// 開始時の処理
    /// </summary>
    void Start()
    {
      // player = GameObject.Find("MainPlayer").GetComponent<PlayerManager>();
        if (!isConnected) 
        {
        // ワーカースレッドからメインスレッドに処理を渡す準備
        context = SynchronizationContext.Current;

        // 回答入力フォームからデータを見られるようにする
        //answerText = GameObject.Find("Comment").GetComponent<Text>();

    
            // サーバーと接続
            ConnectServer();
        }
        else
        {
            cardManager = GameObject.Find("CardManager").GetComponent<CardManager>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            player = GameObject.Find("MainPlayer").GetComponent<PlayerManager>();
            //プレイヤーのIDに合わせてテクスチャを変更
            player.skinnedMeshRenderer.material.SetTexture("_MainTex", player.textures[player.id]);
            player.id = MainPlayerID;
            //他のプレイヤー（キャラクター）のテクスチャ変更
            SetAnothreID(player.id);

            if (MainPlayerID == 0)
            {//プレイヤーIDが0のプレイヤーのみがサーバーに目標設備の設定を要求
                SendComment((int)Event.Event_ID.Set_Target);
            }
        }
    }

    /// <summary>
    /// サーバーとの接続処理
    /// </summary>
    async void ConnectServer()
    {
        // クライアント作成
        tcpClient = new TcpClient();
        // host = "4.216.50.103";
        host = "127.0.0.1";
        // 送受信タイムアウト設定（msec）
        tcpClient.SendTimeout = 1000;
        tcpClient.ReceiveTimeout = 1000;

        // サーバーへ接続要求
        await tcpClient.ConnectAsync(host, port);

        // サーバーからメッセージを受信
        byte[] sendBuffer = new byte[1024];
        NetworkStream stream = tcpClient.GetStream();
        int length = await stream.ReadAsync(sendBuffer, 0, sendBuffer.Length);
        string receiveString = Encoding.UTF8.GetString(sendBuffer, 0, length);
        
        Debug.Log(receiveString);
        // 受信用の処理をワーカースレッドで起動
        Thread thread = new Thread(new ThreadStart(ReceiveProcess));
        thread.Start();
        isConnected = true;

            // 接続したらプレイヤーIDを設定
           
            MainPlayerID = int.Parse(receiveString);
            first = true;
            GameObject.Find("ConectManager").GetComponent<ConectManager>().SetConectedPlayer(MainPlayerID);
            SendComment((int)Event.Event_ID.Conect);




       // 接続処理が一通り完了したらtrueに更新
    }

    /// <summary>
    /// 【ワーカースレッドで起動】サーバーからデータを受信するための処理
    /// </summary>
    private async void ReceiveProcess()
    {
        NetworkStream stream = tcpClient.GetStream();
        while (true)
        {
            // ReadAsyncでサーバーからのメッセージを受信待機
            byte[] receiveBuffer = new byte[1024];
            int length = await stream.ReadAsync(receiveBuffer, 0, receiveBuffer.Length);

            // lengthが0なら接続されていないので処理終了
            if (length <= 0)
            {
                break;
            }

            // 受け取ったデータを文字列に変換
            
            byte[] buffer = new byte[1024];
            // 受信したデータからイベントIDを取り出す
            int eventID = receiveBuffer[0];
           // byte[] bufferString = new byte[1024]; // 1バイト目をスキップ
            string receiveString ;


            switch (eventID)
            {
                case (int)Event.Event_ID.Conect:
                    //プレイヤー接続時に受信、接続したプレイヤーに対応したキャラクターがアクティブになる
                    int conectId = receiveBuffer[1];
                    buffer = receiveBuffer.Skip(2).ToArray(); // 1バイト目をスキップ
                    receiveString = Encoding.UTF8.GetString(buffer, 0, length - 2);
                    Debug.Log($"受信文字列: {receiveString}");
                    context.Post(_ =>
                    {
                        ConectManager conectManager =  GameObject.Find("ConectManager").GetComponent<ConectManager>();
                    switch(conectId)
                    {
                        case 0:
                            conectManager.PlayerActive1 = true;
                            break;
                        case 1:
                            conectManager.PlayerActive2 = true;
                            break;
                        case 2:
                            conectManager.PlayerActive3 = true;
                            break;
                        case 3:
                            conectManager.PlayerActive4 = true;
                            break;
                    }
                    }, null);
                    break;
                case (int)Event.Event_ID.Dice:
                    //各プレイヤーがダイスロールをするときに受信、対応したキャラクターを動かす
                    int MovePlayerManagerId = receiveBuffer[1];
                    int ox = receiveBuffer[2];
                    // 受信したデータを文字列に変換
                    buffer = receiveBuffer.Skip(3).ToArray(); // ３バイトスキップ
                    receiveString = Encoding.UTF8.GetString(buffer, 0, length - 3);
                    Debug.Log($"受信文字列: {receiveString}");

                    //ダイスロール結果をデシリアライズ
                    Roll roll = JsonConvert.DeserializeObject<Roll>(receiveString);

                    Debug.Log($"{roll.rollNum}");

     
                    context.Post(_ =>
                    {
                        //プレイヤーのタグが付いているオブジェクトを取得
                        GameObject[] PlayerManagers = GameObject.FindGameObjectsWithTag("Player");
                        //要求者のIDと同じIDのキャラクターを動かす
                        foreach (GameObject PlayerManager in PlayerManagers)
                        {
                            PlayerManager PlayerManagerScript = PlayerManager.GetComponent<PlayerManager>();
                            if (PlayerManagerScript.id == MovePlayerManagerId)
                            {
                                PlayerManagerScript.MovePlayerManager(roll.rollNum,roll.dice1,roll.dice2);
                            }
                        }
                        //酸素量の変更
                        player.oxygen = ox;
                        Debug.Log($"残り{player.oxygen}");
                    }, null);
                    break;
                case (int)Event.Event_ID.Card:



                    context.Post(_ =>
                    {
                        //ここにおかないとなぜかバッファーが読み込めない
                        int usePlayerId = (int)receiveBuffer[1];
                        int ActiveCardId = (int)receiveBuffer[2];

                         cardManager.CardAction(ActiveCardId,usePlayerId);
                         
                        Debug.Log($"カードID{usePlayerId}のカードが使われました");
                    }, null);
                    break;
                case (int)Event.Event_ID.Change_Direction:
                    int ChangePlayrrId = receiveBuffer[1];
                    // 受信したデータを文字列に変換
                    buffer = receiveBuffer.Skip(2).ToArray(); // 1バイト目をスキップ
                    receiveString = Encoding.UTF8.GetString(buffer, 0, length - 2);
                    Debug.Log($"受信文字列: {receiveString}");

                    //  PlayerManager1.GetComponent<PlayerManager>().MovePlayerManager(roll);
                    context.Post(_ =>
                    {
                        GameObject[] PlayerManagers = GameObject.FindGameObjectsWithTag("Player");
                        foreach (GameObject PlayerManager in PlayerManagers)
                        {
                            PlayerManager PlayerManagerScript = PlayerManager.GetComponent<PlayerManager>();
                            if (PlayerManagerScript.id == ChangePlayrrId)
                            {
                                PlayerManagerScript.direction = -PlayerManagerScript.direction;
                                Debug.Log($"方向が変わりました");
                            }
                        }
                    }, null);
                    break;
                case (int)Event.Event_ID.Turn_End:
                    //プレイヤーIDと所持アイテム数を取得
                    int PlayerID = receiveBuffer[1];
                    int ItemNum = receiveBuffer[2];
                    // 受信したデータを文字列に変換
                    
                    buffer = receiveBuffer.Skip(3).ToArray(); // ３バイトをスキップ
                    receiveString = Encoding.UTF8.GetString(buffer, 0, length - 1);
                    Debug.Log($"受信文字列: {receiveString}");

                    context.Post(_ =>
                    {
                        //ターンを変更6
                        Debug.Log($"{receiveString}番目の人のターン");
                        gameManager.GetComponent<GameManager>().turn = int.Parse(receiveString);
                      
                        player.isSetDice = false;
                        player.isDiceRoll = false;

                        //プレイヤーIDに合わせてテキスト変更
                        gameManager.HaveItemTexts[PlayerID].text = $"プレイヤー{PlayerID + 1}:{ItemNum}個";

                    }, null);

                    break;
                case (int)Event.Event_ID.Look:
                    int lookPlayerId = receiveBuffer[1];
                    // 受信したデータを文字列に変換
                    buffer = receiveBuffer.Skip(2).ToArray(); // 1バイト目をスキップ
                    receiveString = Encoding.UTF8.GetString(buffer, 0, length - 2);
                    Debug.Log($"受信文字列: {receiveString}");
                    context.Post(_ =>
                    {
                       

                        waitSend = true;
                        if (player.id == lookPlayerId)
                        {
                            SendComment((int)Event.Event_ID.Send);
                        }
                    }, null);
                    break;
                case (int)Event.Event_ID.Send:
                    buffer = receiveBuffer.Skip(1).ToArray();
                    receiveString = Encoding.UTF8.GetString(buffer, 0, length - 1);
                    
                    if (waitSend)
                    {
                        context.Post(_ =>
                        {
                            Debug.Log($"相手の情報が送られたよ！{receiveString}");
                        }, null);

                    }
                    break;
                    case (int)Event.Event_ID.Oxygen:
                    // 受信したデータを文字列に変換
                    buffer = receiveBuffer.Skip(1).ToArray(); // 1バイト目をスキップ
                    receiveString = Encoding.UTF8.GetString(buffer, 0, length - 1);
                    context.Post(_ =>
                    {
                       
                        player.oxygen = int.Parse(receiveString);
                        Debug.Log($"残り{player.oxygen}");
                    }, null);
                    break;
                    case (int)Event.Event_ID.Round_End:
                    context.Post(_ =>
                    {
                        Debug.Log("ラウンドが終了しました");
                        //ココから交換に移る
                         player.isSetExchange = true;
                    }, null);
                    break;
                case (int)Event.Event_ID.Next_Round:
                    context.Post(_ =>
                    {
                        player.isSetExchange = false;
                        player.oxygen = 50;
                        player.isGoal = false;
                        player.isStart = true;
                        player.isDiceRoll = false;
                        player.isDiceRollUI = true;
                        player.direction = 1;
                      
                        gameManager.turn = 0;
                        gameManager.BackButton.GetComponent<Button>().interactable = true;

                        cardManager.SetHand();
                    }, null);
                    break;
                    case (int) Event.Event_ID.Game_End:
                    context.Post(_ =>
                    {
                        Initiate.Fade("ResultScene", Color.black, 1.0f);
                    }, null);
                    break;
                    case (int)Event.Event_ID.Clear:
                    int ClearPlayerId = receiveBuffer[1];
                    ClearList[ClearPlayerId] = true;
                    buffer = receiveBuffer.Skip(2).ToArray();
                    receiveString = Encoding.UTF8.GetString(buffer, 0, length - 2);
                    int scondEventId = 0;
                    bool AllClear= true;
                    for (int i = 0;i<ClearList.Count;i++)
                    {
                        if(ClearList[i] != true)
                        {
                            AllClear = false;
                            break;
                        }
                    }
                    if(AllClear)
                    {
                        context.Post(_ =>
                        {
                            Initiate.Fade("ResultScene", Color.black, 1.0f);
                        }, null);
                    }
                    bool result = int.TryParse(receiveString,out scondEventId);
                    if (result )
                    {
                        if(scondEventId == (int)Event.Event_ID.Game_End)
                        {
                            context.Post(_ =>
                            {
                                 Initiate.Fade("ResultScene", Color.black, 1.0f);
                            }, null);
                        }
                        else if(scondEventId == (int)Event.Event_ID.Next_Round)
                        {
                            context.Post(_ =>
                            {
                                player.isSetExchange = false;
                                player.oxygen = 50;
                                player.isGoal = false;
                                player.isStart = true;
                                player.isDiceRoll = false;
                                player.isDiceRollUI = true;
                                player.direction = 1;
                              
                                gameManager.turn = 0;
                                gameManager.BackButton.GetComponent<Button>().interactable = true;

                                cardManager.SetHand();
                            }, null);
                        }
                    }

                    break;
                    case(int)Event.Event_ID.Quit_Player:
                    int quitPlayerID = receiveBuffer[1];
                    context.Post(_ =>
                    {
                       ConectManager conectManager =   GameObject.Find("ConectManager").GetComponent<ConectManager>();
                        switch(quitPlayerID)
                        {
                            case 0:
                                conectManager.PlayerActive1 = false;
                                break;
                            case 1:
                                conectManager.PlayerActive2 = false;
                                break;
                            case 2:
                                conectManager.PlayerActive3 = false;
                                break;
                            case 3:
                                conectManager.PlayerActive4 = false;
                                break;

                        }
                    }, null);
                    break;
                    case (int)Event.Event_ID.Set_Target:
                    context.Post(_ =>
                    {

                       
                        gameManager.target = receiveBuffer[1];
                        TarGetItemsUIManager tarGetItemsUIManager = GameObject.Find("TarGetItems").GetComponent<TarGetItemsUIManager>();
                        tarGetItemsUIManager.SetTarget();
                    }, null);
                    break;
            }
            // Unityで用意しているメソッドは、ワーカースレッド内で起動不可なものもある。
            // Instantiateはメインスレッドでないと実行不可。
            // contextを介して、ワーカースレッドからメインスレッドに処理を依頼する。
            // TODO: 何故かコメントがおかしい動きをする。。要修正

        }
    }

    /// <summary>
    /// 【「送信」ボタン押下で呼び出すメソッド】メッセージ送信
    /// </summary>
    public async void SendComment(int event_ID)
    {
       
        // サーバーと接続できていない場合はデータを送らないようにする
        if (!isConnected)
        {
            Debug.Log("サーバーと接続できていないため送信不可");
        }

        // 入力フォームの文字列を読み込み

        // 送信用データを用意
        string sendString = "";
        byte[] buffer = new byte[1024];
        byte[] sendBuffer = new byte[1024];
        switch (event_ID)
        {
            case (int)Event.Event_ID.Conect:

                sendBuffer = Encoding.UTF8.GetBytes(sendString);
                sendBuffer = sendBuffer.Prepend((byte)MainPlayerID).ToArray();
                break;
            case (int)Event.Event_ID.Dice:
                DIce dIce = new DIce();
                if (GameObject.Find("CardManager").GetComponent<CardManager>().luckyDice)
                {

                    dIce.Dice1Max = 3;
                    dIce.Dice1Min = 1;
                    dIce.Dice2Max = 6;
                    dIce.Dice2Min = 4;
                    GameObject.Find("CardManager").GetComponent<CardManager>().luckyDice =false;
                }
                else if ( true)
                { 
                        dIce.Dice1Max = 3;
                        dIce.Dice1Min = 1;
                        dIce.Dice2Max = 3;
                        dIce.Dice2Min = 1;
                 
                }

                sendString = JsonConvert.SerializeObject(dIce);
                sendBuffer = Encoding.UTF8.GetBytes(sendString);
                sendBuffer = sendBuffer.Prepend((byte)GetComponent<PlayerManager>().oxygen).ToArray();
                sendBuffer = sendBuffer.Prepend((byte)GetComponent<PlayerManager>().id).ToArray();
                break;
            case (int)Event.Event_ID.Card:
                sendBuffer = Encoding.UTF8.GetBytes(sendString);
                sendBuffer = sendBuffer.Prepend((byte)CardId).ToArray();
                sendBuffer = sendBuffer.Prepend((byte)GetComponent<PlayerManager>().id).ToArray();
                break;
            case (int)Event.Event_ID.Change_Direction:
                sendBuffer = Encoding.UTF8.GetBytes(sendString);
                sendBuffer = sendBuffer.Prepend((byte)GetComponent<PlayerManager>().id).ToArray();
                break;
            case (int)Event.Event_ID.Look:
                sendBuffer = Encoding.UTF8.GetBytes(sendString);
                sendBuffer = sendBuffer.Prepend((byte)lookAtID).ToArray();
                break;
            case (int)Event.Event_ID.Send:
                PlayerManager playerManager = GetComponent<PlayerManager>();
       
                    Player sendPlayer = new Player();
                sendPlayer.id = playerManager.id;
                sendPlayer.hand = playerManager.hand;
                sendPlayer.handCard_id = playerManager.handCard_id;
                sendPlayer.handCard_id_2 = playerManager.handCard_id_2;
                sendPlayer.handCard_id_3 = playerManager.handCard_id_3;
                    for (int i = 0; i < playerManager.ItemManagers.Count; i++)
                    {
                        for (int j = 0; j > playerManager.ItemManagers[i].Count; j++)
                        {
                        ItemManager itemManager = playerManager.ItemManagers[i][j].GetComponent<ItemManager>();
                        sendPlayer.Items[i].Add(new Item(itemManager.Tire, itemManager.ID));

                        }
                    }
                sendString = JsonConvert.SerializeObject(player);
                sendBuffer = Encoding.UTF8.GetBytes(sendString);
                break;
            case (int)Event.Event_ID.Oxygen:

                sendString = player.oxygen.ToString();
                sendBuffer = Encoding.UTF8.GetBytes(sendString);
                break;
            case (int)Event.Event_ID.Turn_End:
                int ItemCount = 0;
                for (int i = 0; i < player.HaveItemManagers.Count; i++)
                {
                    ItemCount += player.HaveItemManagers[i].Count;
                }
                sendBuffer = Encoding.UTF8.GetBytes(sendString);
                sendBuffer = sendBuffer.Prepend((byte)ItemCount).ToArray();
                sendBuffer = sendBuffer.Prepend((byte)MainPlayerID).ToArray();
       
                break;
            case (int)Event.Event_ID.Goal:
                sendBuffer = Encoding.UTF8.GetBytes(sendString);
                break;
                case (int)Event.Event_ID.Clear:
                
                sendBuffer = Encoding.UTF8.GetBytes(sendString);
                sendBuffer = sendBuffer.Prepend((byte)MainPlayerID).ToArray();
                break;
            default:
                sendBuffer = Encoding.UTF8.GetBytes(sendString);
                break;
        }
       

        // サーバーへメッセージを送信
       
        // 送信用データの先頭にイベントIDを付与
        sendBuffer = sendBuffer.Prepend((byte)event_ID).ToArray();

        NetworkStream stream = tcpClient.GetStream();
        await stream.WriteAsync(sendBuffer, 0, sendBuffer.Length);
        Debug.Log("");
    }

    public void SetAnothreID(int mainId)
    {
        GameObject[] playerManagers = GameObject.FindGameObjectsWithTag("Player");
        int id = 0;
        for (int i = 0;i < playerManagers.Length;i++)
        {
            PlayerManager anothtePlayer = playerManagers[i].GetComponent<PlayerManager>();

            if (mainId == id)
            {
                id++;
            }

            if (anothtePlayer.id > playerManagers.Length)
            {
                    anothtePlayer.id = id;
                anothtePlayer.skinnedMeshRenderer.material.SetTexture("_MainTex", anothtePlayer.textures[anothtePlayer.id]);
                id++;
            }
     
            

        }
    }

    public void SetLookAtID(int id)
    {
        lookAtID = id;
        SendComment((int)Event.Event_ID.Look);
    }
    /// <summary>
    /// ゲーム終了時にサーバーとの接続を切断する
    /// ※OnApplicationQuitは、StartやUpdateと同様Unityが用意してくれているメソッド。
    /// 　終了時に自動的に呼ばれる。
    /// </summary>
    async void OnApplicationQuit()
    {
        // サーバーへ接続終了用の文字列を送信
        string sendString = "__end";
        byte[] sendBuffer = new byte[1024];
        sendBuffer = Encoding.UTF8.GetBytes(sendString);
        sendBuffer = sendBuffer.Prepend((byte)MainPlayerID).ToArray();
        NetworkStream stream = tcpClient.GetStream();
        await stream.WriteAsync(sendBuffer, 0, sendBuffer.Length);

        // 接続終了
        tcpClient.Close();
    }
}