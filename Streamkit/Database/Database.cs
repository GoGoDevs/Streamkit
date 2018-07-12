using MySql.Data;
using MySql.Data.MySqlClient;

namespace Streamkit.Database {
    public class DatabaseConnection {
        private static DatabaseConnection instance;

        private MySqlConnection connection;

        public MySqlConnection Connection {
            get { return this.connection; }
        }

        public static DatabaseConnection Connect() {
            if (instance == null) instance = new DatabaseConnection();
            return instance;
        }
    }
}