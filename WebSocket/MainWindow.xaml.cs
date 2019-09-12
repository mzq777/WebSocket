using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WebSocket
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 所有socket连接
        /// </summary>
        List<IWebSocketConnection> allSockets = new List<IWebSocketConnection>();

        WebSocketServer server;
        public MainWindow()
        {
            InitializeComponent();
        }

        public WebSocketServer Server { get => server; set => server = value; }

        /// <summary>
        /// 开始监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnListen_Click(object sender, RoutedEventArgs e)
        {
            this.txtReceive.Text = "开始监听";
            Server = new WebSocketServer("ws://0.0.0.0:" + this.txtPort.Text + "");
            Server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    this.txtReceive.Dispatcher.Invoke(() => {
                        this.txtReceive.Text = this.txtReceive.Text + "\n" + socket.ConnectionInfo.ClientIpAddress + "连接成功";
                    });
                    allSockets.Add(socket);
                };
                socket.OnClose = () =>
                {                    
                    allSockets.Remove(socket);
                };
                socket.OnMessage = message =>
                {
                    this.txtReceive.Dispatcher.Invoke(() => {
                        this.txtReceive.Text = this.txtReceive.Text + "\n" + socket.ConnectionInfo.ClientIpAddress + "\n" + message;
                    });
                };
            });
        }
        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            allSockets[0].Send(this.txtSend.Text);
            this.txtReceive.Text = this.txtReceive.Text + "\n" +"self\n"+ this.txtSend.Text;
            this.txtSend.Text = "";
        }
        /// <summary>
        /// 停止监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDisListen_Click(object sender, RoutedEventArgs e)
        {
            Server.Dispose();
            this.txtReceive.Text = this.txtReceive.Text + "\n停止监听";
        }
    }
}
