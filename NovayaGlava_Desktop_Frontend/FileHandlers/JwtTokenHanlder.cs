using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovayaGlava_Desktop_Frontend.FileHandlers
{
    public class JwtFileHandler
    {
        public const string _pathBase = "C:\\Users\\gamer\\source\\repos\\NovayaGlava-Desktop\\WPFForTest\\StaticFiles\\";
        private string _path { get; set; }

        public JwtFileHandler(string userId)
        {
            _path = _pathBase + userId.Substring(0, 5) + "jwt.txt";
        }

        public void SaveInFile(string token)
        {
            using StreamWriter writer = new StreamWriter(_path);
            writer.Write(token);
        }

        public async Task SaveInFileAsync(string token)
        {
            using StreamWriter writer = new StreamWriter(_path);
            await writer.WriteAsync(token);
        }

        public string LoadFromFile()
        {
            using StreamReader reader = new StreamReader(_path);
            return reader.ReadToEnd();
        }

        public async Task<string> LoadFromFileAsync()
        {
            using StreamReader reader = new StreamReader(_path);
            return await reader.ReadToEndAsync();
        }

        public void Update(string newToken)
        {
            using StreamWriter writer = new StreamWriter(_path, false);
            writer.Write(newToken);
        }

        public async Task UpdateAsync(string newToken)
        {
            using StreamWriter writer = new StreamWriter(_path, false);
            await writer.WriteAsync(newToken);
        }
    }
}
