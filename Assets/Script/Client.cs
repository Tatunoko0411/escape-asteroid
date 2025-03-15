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

public class Client : MonoBehaviour
{
    [SerializeField] GameObject systemMessage;
    [SerializeField] GameObject commentPrefab;
    [SerializeField] GameObject parentObject;
    Text answerText;
    SynchronizationContext context; // ワーカースレッドからメインスレッドに処理を渡すのに使用する
    TcpClient tcpClient;
    bool isConnected = false; // サーバーとの接続処理が一通り終わったらtrueにする
    public int lookAtID;//情報を見たい人のID
    bool waitSend= false;
    // 接続先のサーバー情報を設定
#if DEBUG
    public string host = "127.0.0.1";
    public int port = 20001;
#else
    public string host = "127.0.0.1";
    public int port = 20001;
#endif

    /// <summary>
    /// 開始時の処理
    /// </summary>
    void Start()
    {
        // ワーカースレッドからメインスレッドに処理を渡す準備
        context = SynchronizationContext.Current;

        // 回答入力フォームからデータを見られるようにする
       //answerText = GameObject.Find("Comment").GetComponent<Text>();

        // サーバーと接続
        ConnectServer();
    }

    /// <summary>
    /// サーバーとの接続処理
    /// </summary>
    async void ConnectServer()
    {
        // クライアント作成
        tcpClient = new TcpClient();

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

        // システムメッセージとして表示
        PlayerManager playerManager = GetComponent<PlayerManager>();
        playerManager.id = int.Parse(receiveString);


        SetAnothreID(playerManager.id);
        

        // 受信用の処理をワーカースレッドで起動
        Thread thread = new Thread(new ThreadStart(ReceiveProcess));
        thread.Start();

        isConnected = true; // 接続処理が一通り完了したらtrueに更新
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
            byte[] bufferString ; // 1バイト目をスキップ
            string receiveString ;


            switch (eventID)
            {
                case (int)Event.Event_ID.Dice:
                    int MovePlayerManagerId = receiveBuffer[1];
                    // 受信したデータを文字列に変換
                    bufferString = receiveBuffer.Skip(2).ToArray(); // 1バイト目をスキップ
                    receiveString = Encoding.UTF8.GetString(bufferString, 0, length - 2);
                    Debug.Log($"受信文字列: {receiveString}");

                    Roll roll = JsonConvert.DeserializeObject<Roll>(receiveString);
                  //  int roll = int.Parse(receiveString);
                    Debug.Log($"{roll.rollNum}");

                    //  PlayerManager1.GetComponent<PlayerManager>().MovePlayerManager(roll);
                    context.Post(_ =>
                    {
                        GameObject[] PlayerManagers = GameObject.FindGameObjectsWithTag("Player");
                        foreach (GameObject PlayerManager in PlayerManagers)
                        {
                            PlayerManager PlayerManagerScript = PlayerManager.GetComponent<PlayerManager>();
                            if (PlayerManagerScript.id == MovePlayerManagerId)
                            {
                                PlayerManagerScript.MovePlayerManager(roll.rollNum);
                            }
                        }
                    }, null);
                    break;
                case (int)Event.Event_ID.Change_Direction:
                    int ChangePlayrrId = receiveBuffer[1];
                    // 受信したデータを文字列に変換
                    bufferString = receiveBuffer.Skip(2).ToArray(); // 1バイト目をスキップ
                    receiveString = Encoding.UTF8.GetString(bufferString, 0, length - 2);
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
                    // 受信したデータを文字列に変換
                    bufferString = receiveBuffer.Skip(1).ToArray(); // 1バイト目をスキップ
                    receiveString = Encoding.UTF8.GetString(bufferString, 0, length - 1);
                    Debug.Log($"受信文字列: {receiveString}");

                    context.Post(_ =>
                    {
                        GameObject gameManager = GameObject.Find("GameManager");
                        Debug.Log($"{receiveString}番目の人のターン");
                        gameManager.GetComponent<GameManager>().turn = int.Parse(receiveString);
                        GetComponent<PlayerManager>().isSetDice = false;

                    }, null);
                    break;
                case (int)Event.Event_ID.Look:
                    int lookPlayerId = receiveBuffer[1];
                    // 受信したデータを文字列に変換
                    bufferString = receiveBuffer.Skip(2).ToArray(); // 1バイト目をスキップ
                    receiveString = Encoding.UTF8.GetString(bufferString, 0, length - 2);
                    Debug.Log($"受信文字列: {receiveString}");
                    context.Post(_ =>
                    {
                        PlayerManager playerManager = GetComponent<PlayerManager>();

                        waitSend = true;
                        if (playerManager.id == lookPlayerId)
                        {
                            SendComment((int)Event.Event_ID.Send);
                        }
                    }, null);
                    break;
                case (int)Event.Event_ID.Send:
                    bufferString = receiveBuffer.Skip(1).ToArray();
                    receiveString = Encoding.UTF8.GetString(bufferString, 0, length - 1);
                    
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
                    bufferString = receiveBuffer.Skip(1).ToArray(); // 1バイト目をスキップ
                    receiveString = Encoding.UTF8.GetString(bufferString, 0, length - 1);
                    context.Post(_ =>
                    {
                        PlayerManager playerManager = GetComponent<PlayerManager>();
                        playerManager.oxygen = int.Parse(receiveString);
                    }, null);
                    break;
                    case (int)Event.Event_ID.Round_End:
                    context.Post(_ =>
                    {
                        Debug.Log("ラウンドが終了しました");
                        //ココから交換に移る
                        GetComponent<PlayerManager>().isSetExchange = true;
                    }, null);
                    break;
            }
            // Unityで用意しているメソッドは、ワーカースレッド内で起動不可なものもある。
            // Instantiateはメインスレッドでないと実行不可。
            // contextを介して、ワーカースレッドからメインスレッドに処理を依頼する。
            // TODO: 何故かコメントがおかしい動きをする。。要修正
            context.Post(_ =>
            {
                // UI上にコメント用のオブジェクトを生成後、テキストの内容を書き換え
               // GameObject comment = Instantiate(commentPrefab, parentObject.transform.position, Quaternion.identity, parentObject.transform);
                // comment.GetComponent<Text>().text = receiveString;
            }, null);
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
            case (int)Event.Event_ID.Dice:
                DIce dIce = new DIce();
                if (/*ここにダイス変化の条件を入れる*/false)
                {

                    dIce.Dice1Max = 6;
                    dIce.Dice1Min = 4;
                    dIce.Dice2Max = 6;
                    dIce.Dice2Min = 4;
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
                sendBuffer = sendBuffer.Prepend((byte)GetComponent<PlayerManager>().id).ToArray();
                break;
            case (int)Event.Event_ID.Change_Direction:
                sendBuffer = sendBuffer.Prepend((byte)GetComponent<PlayerManager>().id).ToArray();
                break;
            case (int)Event.Event_ID.Look:
                sendBuffer = Encoding.UTF8.GetBytes(sendString);
                sendBuffer = sendBuffer.Prepend((byte)lookAtID).ToArray();
                break;
            case (int)Event.Event_ID.Send:
                PlayerManager playerManager = GetComponent<PlayerManager>();
       
                    Player player = new Player();
                    player.id = playerManager.id;
                    player.hand = playerManager.hand;
                    player.handCard_id = playerManager.handCard_id;
                    player.handCard_id_2 = playerManager.handCard_id_2;
                    player.handCard_id_3 = playerManager.handCard_id_3;
                    for (int i = 0; i < playerManager.ItemManagers.Count; i++)
                    {
                        for (int j = 0; j > playerManager.ItemManagers[i].Count; j++)
                        {
                        ItemManager itemManager = playerManager.ItemManagers[i][j].GetComponent<ItemManager>();
                         player.Items[i].Add(new Item(itemManager.Tire, itemManager.ID));

                        }
                    }
                sendString = JsonConvert.SerializeObject(player);
                sendBuffer = Encoding.UTF8.GetBytes(sendString);
                break;
            case (int)Event.Event_ID.Oxygen:

                PlayerManager playerManager1 = GetComponent<PlayerManager>();
                sendString = playerManager1.oxygen.ToString();
                sendBuffer = Encoding.UTF8.GetBytes(sendString);
                break;
            case (int)Event.Event_ID.Goal:
                break;
        }
       

        // サーバーへメッセージを送信
       
        // 送信用データの先頭にイベントIDを付与
        sendBuffer = sendBuffer.Prepend((byte)event_ID).ToArray();
        NetworkStream stream = tcpClient.GetStream();
        await stream.WriteAsync(sendBuffer, 0, sendBuffer.Length);
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
        NetworkStream stream = tcpClient.GetStream();
        await stream.WriteAsync(sendBuffer, 0, sendBuffer.Length);

        // 接続終了
        tcpClient.Close();
    }
}