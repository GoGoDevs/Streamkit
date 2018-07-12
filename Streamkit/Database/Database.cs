using System;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Streamkit.Database {
    public class DatabaseConnection : IDisposable {
        private static DatabaseConnection instance;

        private MySqlConnection connection;

        public DatabaseConnection() {
            this.connection = new MySqlConnection(
                    Config.DatabaseCredentials.ConnectionString);

            this.connection.Open();
        }

        public MySqlConnection Connection {
            get { return this.connection; }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: dispose managed state (managed objects).
                    this.connection.Close();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}