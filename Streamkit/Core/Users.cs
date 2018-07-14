using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Streamkit.Database;
using MySql.Data.MySqlClient;

namespace Streamkit.Core {
    public class User {
        private string userId;
        private string twitchId;
        private string twitchUsername;
    }

    
    public static class UserManager {
        public static User GetUser(string id) {
            throw new NotImplementedException();
        }

        public static void AddUser(string twitchId, string twitchUsername) {
            using (DatabaseConnection conn = new DatabaseConnection()) {
                // TODO...
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "";
                cmd.Parameters.AddWithValue("", null);
                throw new NotImplementedException();
            }
        }
    }
}
