using Newtonsoft.Json;
using System.Collections.ObjectModel;
using ClassLibForNovayaGlava_Desktop;
using ClassLibForNovayaGlava_Desktop.UserModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using NovayaGlava_Desktop_Frontend.FileHandlers;
using NovayaGlava_Desktop_Frontend.CacheHandlers;
using NovayaGlava_Desktop_Frontend.Utilities;


namespace NovayaGlava_Desktop_Frontend.MVVM.ViewModel
{
    class FriendsVM : INotifyPropertyChanged
    {
        private string _inputSearchString { get; set; }
        private UserModel _selectedFriend { get; set; }

        private int _friendsCount;
        public int FrindsCount
        {
            get => _friendsCount;
            set
            {
                _friendsCount = value;
                OnPropertyChanged(nameof(FrindsCount));
            }
        }


        public ObservableCollection<UserModel> Friends { get; set; }
        public ObservableCollection<UserModel> Users { get; set; }

        private List<UserModel> _users { get; set; }

        public RelayCommand GetUsers { get; set; }
        public RelayCommand PrintCurrentUser { get; set; }
        public RelayCommand SearchUsersCommand { get; set; }
        public RelayCommand ResetUsersCommand { get; set; }

        // Создание нового чата
        public RelayCommand CreateNewChatCommand { get; set; }


        UserIdHandler _userIdHandler;
        HttpClient _client;

        public FriendsVM()
        {
            _client = HttpClientSingleton.Client;
            Friends = new ObservableCollection<UserModel>();
            Users = new ObservableCollection<UserModel>();

            Task.Run(async () => await GetAllUsersLocalDb());
            SearchUsersCommand = new RelayCommand(async o => await SearchUsers());
            _userIdHandler = new UserIdHandler();
            CreateNewChatCommand = new RelayCommand(async o => await CreateNewChat());
        }

        // Выбранный юзер из списка Friends
        public UserModel SelectedFriend
        {
            get
            {
                return _selectedFriend;
            }
            set
            {
                _selectedFriend = value;
            }
        }

        // Строка ввода для поиска друзей или юзеров
        public string InputSearchString 
        {
            get
            {
                return _inputSearchString;
            } 
            set
            {
                _inputSearchString = value;
                OnPropertyChanged(nameof(InputSearchString));
            } 
        }

        // Получить список друзей по id
        private async Task GetFriendsListByIdLocalDb()
        {
            string userId = _userIdHandler.GetFromCache();
            HttpResponseMessage response = await _client.GetAsync($"https://localhost:7245/api/users/usersById/getFriendsList/localdb?userId={userId}");
            if (!response.IsSuccessStatusCode)
                MessageBox.Show("Не удалось получить список друзей");
            else
            {
                string jsonFriendsList = await response.Content.ReadAsStringAsync();
                List<UserModel> friendsList = JsonConvert.DeserializeObject<List<UserModel>>(jsonFriendsList);

                foreach(var friend in friendsList)
                    Friends.Add(friend);
            }
        }

        // Добавить юзера в список друзей
        private async Task AddFriendLocalDb()
        {

        }

        // Удалить юзера из списка друзей
        private async Task RemoveFriendLocalDb()
        {

        }

        // Получить всех пользователей из локальной базы данных
        public async Task GetAllUsersLocalDb()
        {
            HttpResponseMessage response = await _client.GetAsync("https://localhost:7245/api/users/allUsers/localdb");

            if (response.StatusCode != HttpStatusCode.OK)
                MessageBox.Show("Не удалось получить список из всех пользователей из локальной базы данных");

            string jsonAllUsers = await response.Content.ReadAsStringAsync();
            List<UserModel> allUsers = JsonConvert.DeserializeObject<List<UserModel>>(jsonAllUsers);
            foreach(var user in allUsers)
                Application.Current.Dispatcher.Invoke(() => Friends.Add(user));
        }

        // Получить пользователей, имя которых ссодержит/равно строке, вводимую в окне поиска
        public async Task SearchUsers()
        {
            if(string.IsNullOrEmpty(_inputSearchString))
            {
                if(Friends.Count == 0)
                    await GetAllUsersLocalDb();
            }
            else
            {
                ResetUsers();
                HttpResponseMessage response = await _client.GetAsync($"https://localhost:7245/api/users/searchFriends/localdb?input={_inputSearchString}");

                if (!response.IsSuccessStatusCode)
                    MessageBox.Show("Не удалось получить пользователей в процессе поиска");
                else
                {
                    string jsonUsers = await response.Content.ReadAsStringAsync();
                    List<UserModel> users = JsonConvert.DeserializeObject<List<UserModel>>(jsonUsers);
                    foreach (var user in users)
                    {
                        Application.Current.Dispatcher.Invoke(() => Users.Add(user));
                    }
                }
            }
        }

        public void ResetUsers()
        {
            Application.Current.Dispatcher.Invoke(() => Users.Clear());
        }

        private async Task CreateNewChat()
        {
            string userId = _userIdHandler.GetFromCache();
            List<string> users = new List<string> { userId, SelectedFriend._id };
            
            string jsonChat = JsonConvert.SerializeObject(users);
            HttpResponseMessage response = await _client.PostAsJsonAsync("https://localhost:7245/api/chat/addChat/localdb", jsonChat);
            if (response.StatusCode != HttpStatusCode.OK)
                MessageBox.Show("Не удалось добавить новый чат в локальную бд");
            else
            {
                MessageBox.Show(await response.Content.ReadAsStringAsync());
            }
        }

        private void LoadListOfPossibleFriends()
        {
            Task.Run(async () => await GetAllUsersLocalDb());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        public void GetChatWithSelectedUser(object sender, SelectionChangedEventArgs args)
        {
            ListBox lb = (ListBox)sender;
            var si = lb.SelectedItem;

            var text = (UserModel)si;
            MessageBox.Show(text._id);
        }

        private void SearchStringInputChanged(object sender, TextChangedEventArgs e)
        {
            MessageBox.Show(InputSearchString);
        }
    }
}
