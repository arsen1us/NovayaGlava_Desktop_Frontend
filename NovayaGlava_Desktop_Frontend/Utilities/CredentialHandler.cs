using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CredentialManagement;

namespace NovayaGlava_Desktop_Frontend.Utilities
{
    public class CredentialHandler
    {
        public void SaveToken(string target, string userName, string token)
        {
            using Credential credential = new Credential
            {
                Target = target,
                Username = userName,
                Password = token,
                PersistanceType = PersistanceType.LocalComputer
            };
            credential.Save();
        }

        public string GetToken(string target)
        {
            using Credential credential = new Credential();
            credential.Target = target;
            
            return credential.Load() ? credential.Password : null;
        }

        public void DeleteToken(string target)
        {
            using Credential credential= new Credential();
            credential.Target = target;
            credential.Delete();
        }
    }
}
