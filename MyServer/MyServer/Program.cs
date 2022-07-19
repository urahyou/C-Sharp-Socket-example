using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace socket
{
    class Program
    {

        static Socket serverSocket;
       
        

        static void Main(string[] args)
        {
            SetListen();  //开始监听

        }

        static void SetListen()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                serverSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888));
                serverSocket.Listen(2);

                Thread thread = new Thread(Listen);
                thread.Start(serverSocket);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        static void Listen(object so)
        {
            Socket serverSocket = so as Socket;

            while (true)
            {
                try
                {
                    Socket clientSocket = serverSocket.Accept(); //接受客户端接入
                    // 获取链接IP地址
                    string clientPoint = clientSocket.RemoteEndPoint.ToString();

                    //开启新线程来不停接受信息
                    Thread rec = new Thread(Receive);
                    rec.Start(clientSocket);

                    //开启新线程来不停发送信息
                    Thread sen = new Thread(Send);
                    sen.Start(clientSocket);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message, ex);
                }
            }
        }

        static void Receive(object so)
        {
            Socket clientSocket = so as Socket;

            string clientPoint = clientSocket.RemoteEndPoint.ToString();
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int len = clientSocket.Receive(buffer);
                    //Console.WriteLine(len.ToString(), buffer);
                    if (len == 0) break;

                    string msg = Encoding.UTF8.GetString(buffer, 0, len);
                    Console.WriteLine("客户端说："+ msg);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        static void Send(object so)
        {
            Socket clientSocket = so as Socket;

            while (true)
            {
                //获取控制台输入
                string input = Console.ReadLine();

                byte[] msg = Encoding.UTF8.GetBytes(input);
                clientSocket.Send(msg);
                
            }
        }
    }
}
