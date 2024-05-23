using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovayaGlava_Desktop_Frontend.FileHandlers
{
    public class UserIdFileHandler
    {
        public const string _pathBase = "C:\\Users\\gamer\\source\\repos\\NovayaGlava-Desktop\\WPFForTest\\StaticFiles\\";
        private string _path { get; set; }

        public UserIdFileHandler(string userId)
        {
            _path = _pathBase + userId.Substring(0, 5) + "userId.txt";
        }

        public void SaveInFile(string userId)
        {
            using StreamWriter writer = new StreamWriter(_path);
            writer.Write(userId);
        }

        public async Task SaveInFileAsync(string userId)
        {
            using StreamWriter writer = new StreamWriter(_path);
            await writer.WriteAsync(userId);
        }

        public string LoadFromFile(string userId)
        {
            using StreamReader reader = new StreamReader(_path);
            return reader.ReadToEnd();
        }

        public async Task<string> LoadFromFileAsync(string userId)
        {
            using StreamReader reader = new StreamReader(_path);
            return await reader.ReadToEndAsync();
        }

        public void Update(string newUserId)
        {
            using StreamWriter writer = new StreamWriter(_path, false);
            writer.Write(newUserId);
        }

        public async Task UpdateAsync(string newUserId)
        {
            using StreamWriter writer = new StreamWriter(_path, false);
            await writer.WriteAsync(newUserId);
        }
    }
}
