using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using USSD.App.Models;
using USSD.App.Settings;
using USSD.App.TCP;
using USSD_Tester.ViewModels;

namespace USSD_Tester
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get; private set; }

        public void HandleTCP()
        {

        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            USSDViewModel VM = ServiceProvider.GetRequiredService<USSDViewModel>();
            mainWindow.DataContext = VM;
            mainWindow.Show();
           

            //handle TCP
            var tcpClient = ServiceProvider.GetRequiredService<ITCPClient>();
            var socket = await tcpClient.StartClient();

            byte[] msg = Encoding.ASCII.GetBytes($"?^{-1}^{0}^{1}^{2}^{3}\n");

            // Send the data through the socket.
            int bytesSent = socket.Send(msg);



            byte[] messageReceived = new byte[1024];
            while (true)
            {
                var result = await Task.Run(() =>
                {
                    return socket.Receive(messageReceived);
                    

                });
                string msgReceived = Encoding.ASCII.GetString(messageReceived, 0, result);

                var resp = JsonConvert.DeserializeObject<EngineRequest>(msgReceived);
               
                VM.Data.Response = resp.Message + "\n";

                //msg = Encoding.ASCII.GetBytes($"?^{0}^{0}^{1}^{2}^{3}\n");
                //bytesSent = socket.Send(msg);


               // Encoding.ASCII.GetString(ReadBySize(socket));
            }

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
        byte[] ReadBySize(Socket socket,int size = 1024)
        {
            var readEvent = new AutoResetEvent(false);
            var buffer = new byte[size]; //Receive buffer
            var totalRecieved = 0;
            do
            {
                var recieveArgs = new SocketAsyncEventArgs()
                {
                    UserToken = readEvent
                };
                recieveArgs.SetBuffer(buffer, totalRecieved, size - totalRecieved);//Receive bytes from x to total - x, x is the number of bytes already recieved
                recieveArgs.Completed += recieveArgs_Completed;
                socket.ReceiveAsync(recieveArgs);
                readEvent.WaitOne();//Wait for recieve

                if (recieveArgs.BytesTransferred == 0)//If now bytes are recieved then there is an error
                {
                    if (recieveArgs.SocketError != SocketError.Success)
                        throw new Exception("Unexpected Disconnect");
                    //throw new Exception("Unexpected disconnect");
                }
                totalRecieved += recieveArgs.BytesTransferred;

            } while (totalRecieved != size);//Check if all bytes has been received
            return buffer;
        }

        private void recieveArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            var are = (AutoResetEvent)e.UserToken;
            are.Set();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // ...

            services.AddTransient(typeof(MainWindow));
            services.AddTransient(typeof(USSDViewModel));
            services.AddTransient<IUSSDSettings, USSDSettings>();
            services.AddSingleton<ITCPClient, TCPClient>();
            services.AddTransient<IConfiguration>(sp =>
            {
                IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
                configurationBuilder.AddJsonFile("appsettings.json");
                return configurationBuilder.Build();
            });
        }
    }
}
