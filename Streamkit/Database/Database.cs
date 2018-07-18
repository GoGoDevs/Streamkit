using System;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Streamkit.Database {
    public class DatabaseConnection : IDisposable {
        private static DatabaseConnection instance;

        private MySqlConnection connection;
        private MySqlTransaction transaction;

        public DatabaseConnection() {
            this.connection = new MySqlConnection(
                    Config.DatabaseCredentials.ConnectionString);

            this.connection.Open();
        }

        public MySqlConnection Connection {
            get { return this.connection; }
        }

        public MySqlCommand CreateCommand() {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = this.connection;
            return cmd;
        }

        public void BeginTransaction() {
            this.transaction = this.connection.BeginTransaction();
        }

        public void Rollback() {
            this.transaction.Rollback();
        }

        public void Commit() {
            this.transaction.Commit();
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

    public static class MySqlDataReaderExtensions {
        public static byte[] GetBlob(this MySqlDataReader reader, string col) {
            // TODO.
            throw new NotImplementedException();
        }
    }
}