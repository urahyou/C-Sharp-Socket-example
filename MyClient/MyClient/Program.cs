using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;



namespace socket_client
{
    class Program
    {
        static Socket clientSocket;

        static void Main(string[] args)
        {
            
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            //连接
            Connect(); 
        }

        //连接客户端，连接后开始监听发送过来的消息并且把输入的信息发给服务端
        static void Connect()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); 
            
            try
            {
                //连接
                clientSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888));

                //独立线程来接受来自服务端的数据
                Thread receive = new Thread(Receive);
                receive.Start(clientSocket);

                //独立线程来发送数据给服务端
                Thread send = new Thread(Send);
                send.Start(clientSocket); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void Receive(object so)
        {
            Socket clientSocket = so as Socket;


            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;  // 修改文字颜色为绿色
                try
                {
                    byte[] buffer = new byte[1024];
                    int len = clientSocket.Receive(buffer);
                    if (len > 0)
                    {
                        string msg = Encoding.UTF8.GetString(buffer);
                        
                        Console.WriteLine("服务端说："+ msg);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        static void Send(object so)
        {
            Socket clientSocket = so as Socket;

            while (true)  // 监听键盘输入
            {
                Console.ForegroundColor = ConsoleColor.Red;
                //获取键盘输入
                string input = Console.ReadLine();
                //编码
                byte[] buffer = Encoding.UTF8.GetBytes(input);
                //发送
                clientSocket.Send(buffer);
            }
        }
    }
}
