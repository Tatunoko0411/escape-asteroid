using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using Newtonsoft.Json;
using ConsoleClient;
using MySqlConnector;
using System.IO;
using System.Data.SqlClient;

namespace ConsoleServer
{
    internal class Program
    {
        static public List<problem> problemList = new List<problem>();
        static public List<string> answerList = new List<string>();
        static public Random rnd = new Random();


        static MySqlConnectionStringBuilder connectionBilder =
                                   new MySqlConnectionStringBuilder()
                                   {
#if DEBUG
                                       Server = "localhost",
                                       Database = "quiz",
                                       UserID = "root",
                                       Password = "",
                                       //SslMode = MySqlSslMode.Required,
#else
                                               Server = "db-ge-202402.mysql.database.azure.com",//  AzureのMySQLサーバーのサーバー名
                                               Database = "azure_db",//Azure上データベース名
                                               UserID = "student",
                                               Password = "Yoshidajobi2024",
                                               SslMode = MySqlSslMode.Required
#endif
                                   };

        static async Task Main()
        {

            await RoadQuiz();

            //接続を待つエンドボイドを設定


            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 20001);

            //接続を受け付けるためのTcpListernerを作成
            TcpListener listener = new TcpListener(localEndPoint);

            try
            {
                listener.Start();
                while (true)
                {

                    Console.WriteLine("接続受付を開始");
                    //接続要求＆接続作成



                    TcpClient tcpClient = await listener.AcceptTcpClientAsync();
                    Thread thread = new Thread(new ParameterizedThreadStart(DoWork));
                    Console.WriteLine("クライアントが接続しました");
                    thread.Start(tcpClient);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        static async void DoWork(object param)
        {
            byte[] buffer = new byte[1024];
            TcpClient tcpClient = (TcpClient)param;
            NetworkStream stream = tcpClient.GetStream();
            int time = 0;
            int seikai = 0;
            try
            {
                while (true)
                {
                    //クライアントからリクエストを受信

                    int byteSize = await stream.ReadAsync(buffer, 0, buffer.Length);

                    //受信したデータを文字列に変換
                    string receiveString = Encoding.UTF8.GetString(buffer, 0, byteSize);

                    Console.WriteLine(receiveString);
                    //クライアントへのレスポンスを送信
                    string sendString = "";
                    if (receiveString == "init")
                    {
                        sendString = JsonConvert.SerializeObject(problemList);
                    }
                    else if (receiveString == "fin")
                    {
                        sendString = $"終了　正解数{seikai}";
                        buffer = Encoding.UTF8.GetBytes(sendString);
                        await stream.WriteAsync(buffer, 0, buffer.Length);
                        break;
                    }

                    else if (receiveString == answerList[time])
                    {
                        sendString = "正解";
                        seikai++;
                        time++;
                    }
                    else
                    {
                        sendString = "不正解";
                        time++;
                    }



                    buffer = Encoding.UTF8.GetBytes(sendString);

                    await stream.WriteAsync(buffer, 0, buffer.Length);

                    //接続終了


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            tcpClient.Close();
        }

        public static async Task RoadQuiz()
        {
            var conn = new MySqlConnection(connectionBilder.ConnectionString);
            await conn.OpenAsync();


            MySqlCommand command = conn.CreateCommand();


            command.CommandText = "select statement,choices1,choices2,choices3,answer from quiz;";
            MySqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                problem problem = new problem();

                problem.statement = (string)reader[0];
                problem.choices1 = (string)reader[1];
                problem.choices2 = (string)reader[2];
                problem.choices3 = (string)reader[3];
                problemList.Add(problem);
                answerList.Add((string)reader[4]);
            }

        }
    }
}
