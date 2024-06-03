using Newtonsoft.Json;
using System.Collections.ObjectModel; // For ObservableCollection
using System.IO; //Application object
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32; // For OpenFileDialog 
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Net.Http;
using System.Net.Http.Json;
using ClassLibForNovayaGlava_Desktop;
using ClassLibForNovayaGlava_Desktop.UserModel;
using Microsoft.AspNetCore.SignalR.Client;
using NovayaGlava_Desktop_Frontend.FileHandlers;
using NovayaGlava_Desktop_Frontend.CacheHandlers;
using NovayaGlava_Desktop_Frontend.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace NovayaGlava_Desktop_Frontend.MVVM.ViewModel
{
    class ChatVM : INotifyPropertyChanged
    {
        public HorizontalAlignment Alignment;
        private HubConnection _connection;
        private ChatUserModel _selectedChat;
        private ChatModel _currentChat;

        public string UserName { get; set; }
        public string Message { get; set; }
        public string FilePath { get; set; }

        // При добавлении или удалении объекта из данной коллекци он уведомляет об этом все элементы привязки;
        public ObservableCollection<UserModel> Users { get; set; }
        public ObservableCollection<ChatMessageModel> Messages { get; set; }

        // p2p чаты.
        // Название чатов - никнеймы собеседников
        public ObservableCollection<ChatUserModel> Chats { get; set; }

        public RelayCommand ConnectToServerCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }
        public RelayCommand OpenFileDialogCommand { get; set; }
        public RelayCommand SaveFileDialogCommand { get; set; }

        CredentialHandler _credential;
        HttpClient _client;
        public IServiceProvider ServiceProvider { get; set; }
        IHttpClientFactory _clientFactory;


        public ChatVM()
        {
            ServiceProvider = ServiceProviderContainer.ServiceProvider;
            _clientFactory = ServiceProvider.GetRequiredService<IHttpClientFactory>();
            _client = _clientFactory.CreateClient("ApiClient");
            _credential = new CredentialHandler();

            Chats = new ObservableCollection<ChatUserModel>();
            Users = new ObservableCollection<UserModel>();
            Messages = new ObservableCollection<ChatMessageModel>();

            _connection = ChatHubConnectionHandler.Connection;


            Task.Run(async () => await GetChatsByIdLocalDb());

            SendMessageCommand = new RelayCommand(async o =>
            {
                
                await AddMessageLocalDb();


            }); //, o => !string.IsNullOrEmpty(Message) еще это здесь было

            OpenFileDialogCommand = new RelayCommand(async o => await OpenFileDialog());
        }

        //Выбранный юзер из списка
        public ChatUserModel SelectedChat
        {
            get => _selectedChat;
            set
            {
                _selectedChat = value;
                //Task.Run(async () => await GetSelectedChatLocalDb());
            }
        }

        // Получение списка чатов по userId из локальной бд
        private async Task GetChatsByIdLocalDb()
        {
            string userId = _credential.GetToken("userId");
            //string jwt = _credential.GetToken("jwt");

            //_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwt);
            HttpResponseMessage response = await _client.GetAsync($"https://localhost:7245/api/chats/get?userId={userId}");

            if (!response.IsSuccessStatusCode)
                MessageBox.Show($"Не удалось получить чаты по след id - [{userId}], {response.StatusCode}");
            else
            {
                List<ChatUserModel> chats = await response.Content.ReadFromJsonAsync<List<ChatUserModel>>();

                foreach (ChatUserModel chat in chats)
                    Application.Current.Dispatcher.Invoke(() => Chats.Add(chat));
            }
        }

        // Подключиться к хабу ChatHub
        public async Task ConnectToChutHub()
        {
            string userId = _credential.GetToken("userId");

            HttpResponseMessage response = await _client.GetAsync($"https://localhost:7245/api/connectionIds/getConnectionId/localdb?chatId={_selectedChat._id}&userId={userId}");

            if (!response.IsSuccessStatusCode)
                MessageBox.Show($"Не удалось получить connectionId к хабу SignalR. Error - {await response.Content.ReadAsStringAsync()}");
            else
            {
                string dbConnectionId = await response.Content.ReadAsStringAsync();

                // Первое добавление id подключения в локальную бд
                if (string.IsNullOrEmpty(dbConnectionId))
                {
                    await SendConnectionInformationWithCurrentUserLocalDb();
                }

                // Если уже подключался ранее
                else
                {
                    // Текущий id подключения
                    ChatHubConnectionHandler chatConnection = new ChatHubConnectionHandler();
                    string newConnectionId = chatConnection.GetConnectionId();

                    // Если айдишки не равны
                    if (newConnectionId != dbConnectionId)
                    {
                        UpdateConnectionIdModel updateConnectionIdModel = new UpdateConnectionIdModel(_selectedChat._id, newConnectionId, userId);
                        string jsonUpdateConnectionIdModel = JsonConvert.SerializeObject(updateConnectionIdModel);

                        HttpResponseMessage patchResponse = await _client.PatchAsJsonAsync("https://localhost:7245/api/connectionIds/update/localdb", jsonUpdateConnectionIdModel);

                        if (!patchResponse.IsSuccessStatusCode)
                            MessageBox.Show($"Не удалось обновить id подключения {await patchResponse.Content.ReadAsStringAsync()}");
                        else
                        {
                            MessageBox.Show("id подключения успешно обновлён");
                        }
                    }
                }
            }
        }

        // Получения списка из всех юзеров из локальной бд
        private async Task GetAllUsersFromLocalDb()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://localhost:7245/api/users/allUsers/localdb");

            string jsonUsers = await response.Content.ReadAsStringAsync();
            List<UserModel> users = JsonConvert.DeserializeObject<List<UserModel>>(jsonUsers);

            foreach (var user in users)
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }
        }

        // Получение чата с выбранным пользователем
        // Создание или получение уже созданного чата из локальной бд
        private async Task GetSelectedChatLocalDb()
        {
            if (Messages.Count > 0)
            {
                Application.Current.Dispatcher.Invoke(() => Messages.Clear());
            }
            HttpClient client = new HttpClient();

            string currentUserId = _credential.GetToken("userId");
            List<string> usersId = new List<string> { currentUserId, _selectedChat.CompanionId };
            string jsonUsersId = JsonConvert.SerializeObject(usersId);

            HttpResponseMessage responseMessage = await client.PostAsJsonAsync("https://localhost:7245/api/chat/addChat/localdb", jsonUsersId);

            string response = await responseMessage.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(response))
                MessageBox.Show("Ошибка получения ответа от сервера. В теле запроса отсутствует объект чата");
            ChatModel chat = JsonConvert.DeserializeObject<ChatModel>(response);
            _currentChat = chat;

            // Получение сообщений для чата с выбранным пользователем
            await GetMessagesByChatIdLocalDb();
        }

        // Добавить новое сообщение в локальную бд
        private async Task AddMessageLocalDb()
        {
            HttpClient client = new HttpClient();
            ChatMessageModel messageModel = new ChatMessageModel
            {
                _id = Guid.NewGuid().ToString(),
                Author = _credential.GetToken("userId"),
                ChatId = _currentChat._id,
                TimeStamp = DateTime.Now.ToString(),
                Content = Message,

                CommentId = null,
                Attachments = null,
                __v = 0,
                ReplyComment = false,

            };

            string jsonMessage = JsonConvert.SerializeObject(messageModel);
            HttpResponseMessage response = await client.PostAsJsonAsync("https://localhost:7245/api/messages/add/localdb", jsonMessage);

            if (response.StatusCode != HttpStatusCode.OK)
                MessageBox.Show("Ошибка добавления нового сообщения в локальную бд");

            Application.Current.Dispatcher.Invoke(() => Messages.Add(messageModel));
        }

        // Получение списка сообщений для текущего чата по id чата из локальной бд
        private async Task GetMessagesByChatIdLocalDb()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"https://localhost:7245/api/messages/get/{_currentChat._id}/localdb");

            if (response.StatusCode != HttpStatusCode.OK)
                MessageBox.Show($"Не удалось получить все сообщения чата {_currentChat}");
            else
            {
                string jsonMessages = await response.Content.ReadAsStringAsync();
                List<ChatMessageModel> chatMessages = JsonConvert.DeserializeObject<List<ChatMessageModel>>(jsonMessages);

                foreach (var message in chatMessages)
                    Application.Current.Dispatcher.Invoke(() => Messages.Add(message));
            }
        }

        // Функция открытия окна для загрузки файлов
        // filter - фильтр для изображений и gif
        private async Task OpenFileDialog(string filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF")
        {
            List<string> filesPath = new List<string>();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = filter;

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    filesPath.Add(fileName);
                }
            }
            await AddFilesToLocalDb(filesPath);
            MessageBox.Show("Success? Method is enDead");
        }

        // Метод для выбора типа файла для загрузки на сервер
        public void GetFilterToFileDialog()
        {

        }

        // Метод для загрузки файлов на сервер
        private async Task AddFilesToLocalDb(List<string> filesPath)
        {
            HttpClient client = new HttpClient();

            // Ничего не делает, так как загружать ничего и не надо
            if (filesPath.Count == 0) { }

            // Загрузка одного файла на локальный сервер
            else if (filesPath.Count == 1)
            {
                string filePath = filesPath.First();
                FileStream stream = new FileStream(filePath, FileMode.Open);

                // Максимальный размер для загрузки файлов на сервер - 10 мб
                byte[] buffer = new byte[10000000];
                int bufferLength = await stream.ReadAsync(buffer, 0, buffer.Length);

                AddingFileToServerModel addingFileToServerModel = new AddingFileToServerModel(_credential.GetToken("userId"), buffer, filePath);
                string jsonModel = JsonConvert.SerializeObject(addingFileToServerModel);
                HttpResponseMessage response = await client.PostAsJsonAsync("https://localhost:7245/api/files/loadFile/localdb", jsonModel);

                if (response.IsSuccessStatusCode)
                    MessageBox.Show("Файл успешно добавлен");
            }

            // Загрузка нескольких файлов на локальный сервер
            else
            {
                List<AddingFileToServerModel> addingFileToServerModelList = new List<AddingFileToServerModel>();

                for (int i = 0; i < filesPath.Count; i++)
                {
                    FileStream stream = new FileStream(filesPath[i], FileMode.Open);
                    byte[] buffer = new byte[100000000];
                    await stream.ReadAsync(buffer, 0, buffer.Length);
                    addingFileToServerModelList.Add(new AddingFileToServerModel(_credential.GetToken("userId"), buffer, filesPath[i]));
                }

                string jsonModel = JsonConvert.SerializeObject(addingFileToServerModelList);
                HttpResponseMessage response = await client.PostAsJsonAsync("https://localhost:7245/api/files/loadFiles/localdb", jsonModel);

                if (response.IsSuccessStatusCode)
                    MessageBox.Show("Файлы успешно добавлены");
            }
        }

        // Секция с сокетами SignalR

        // Добавление информации о подключении в локальную бд
        private async Task SendConnectionInformationWithCurrentUserLocalDb()
        {
            HttpClient client = new HttpClient();
            ChatHubConnectionHandler chatConnection = new ChatHubConnectionHandler();

            string connectionString = chatConnection.GetConnectionId();
            if (string.IsNullOrEmpty(connectionString))
            {
                MessageBox.Show("Вы не подключены к хабу для обмена сообщениями");
            }

            ConnectionIdModel connectionIdModel = new ConnectionIdModel(_credential.GetToken("userId"), _selectedChat._id, connectionString);
            string jsonConnectionIdModel = JsonConvert.SerializeObject(connectionIdModel);

            HttpResponseMessage response = await client.PostAsJsonAsync("https://localhost:7245/api/connectionIds/add/localdb", jsonConnectionIdModel);
            if (!response.IsSuccessStatusCode)
                MessageBox.Show("Не удалось добавить запись с информацией о подключении");
            else
            {
                MessageBox.Show("Данные о подключении успешно добавлены");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }

}
