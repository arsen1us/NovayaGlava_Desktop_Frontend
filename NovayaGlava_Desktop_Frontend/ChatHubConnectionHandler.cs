using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NovayaGlava_Desktop_Frontend.CacheHandlers;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows;

namespace NovayaGlava_Desktop_Frontend
{
    public class ChatHubConnectionHandler
    {
        private static HubConnection _connection;
        static UserIdHandler _userIdHandler;

        public ChatHubConnectionHandler()
        {
            _userIdHandler = new UserIdHandler();
        }

        // Метод для получения объекта HubConnection
        public static HubConnection Connection
        {
            get
            {
                if (_connection != null)
                    return _connection;
                else
                {
                    _connection = new HubConnectionBuilder().WithUrl("wss://localhost:7245/chatHub").Build();

                    // Обработка полученного ответа от сервера
                    _connection.On<string, string>("ReceiveMessage", (userNickName, message) =>
                    {
                        MessageBox.Show(userNickName + ": " + message);
                    });

                    Task.Run(async () => await _connection.StartAsync()).Wait();

                    return _connection;
                }
            }
        }

        public string GetConnectionId()
        {
            if (_connection != null)
                return _connection.ConnectionId;
            return string.Empty;
        }

        public void AddUserToConnectedUsers(string userId)
        {
            _connection.SendAsync("AddUserToConnectedUsers", userId, _connection.ConnectionId);
        }
    }
}
