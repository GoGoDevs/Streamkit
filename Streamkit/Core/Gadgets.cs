using System;
using System.Drawing;
using System.Globalization;

using MySql.Data.MySqlClient;

using Streamkit.Database;
using Streamkit.Crypto;

namespace Streamkit.Core {
    public abstract class Gadget {
        protected string id;
        protected User user;

        public Gadget(string id, User user) {

        }

        public abstract void Update();
    }


    public class Bitbar : Gadget {
        private int value = 0;
        private int maxValue = 1000;
        private byte[] image = null;
        private string color = "#00FF00";

        public Bitbar(string id, User user) : base(id, user) {

        }

        public int Value {
            get { return this.value; }
            set { this.value = value; }
        }

        public int MaxValue {
            get { return this.maxValue; }
            set { this.maxValue = value; }
        }

        public byte[] Image {
            get { return this.image; }
            set { this.image = value; }
        }

        public string Color {
            get { return this.color.Replace("#", ""); }
            set {
                if (value.Replace("#", "").Length != 6) {
                    throw new Exception("Invalid color code.");
                }
                this.color = value;
            }
        }

        public override void Update() {
            throw new NotImplementedException();
        }
    }


    public static class BitbarManager {
        public static bool BitbarExists(string id) {
            using (DatabaseConnection conn = new DatabaseConnection()) {
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT id FROM gadget_bitbar WHERE id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                return reader.HasRows;
            }
        }

        public static Bitbar GetBitbar(User user) {
            using (DatabaseConnection conn = new DatabaseConnection()) {
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT id, value, max_value, image, color "
                                + "FROM gadget_bitbar WHERE user_id = @userid";
                cmd.Parameters.AddWithValue("@userid", user.UserId);
                MySqlDataReader reader = cmd.ExecuteReader();
                
                // Create bitbar for user if one does not already exist.
                if (!reader.HasRows) {
                    return CreateBitbar(user);
                }

                reader.Read();

                Bitbar bitbar = new Bitbar(reader.GetString("id"), user);
                bitbar.Value = reader.GetInt32("value");
                bitbar.MaxValue = reader.GetInt32("max_value");
                bitbar.Image = reader.GetBytes("image");
                bitbar.Color = reader.GetString("color");

                return bitbar;
            }
        }

        public static Bitbar CreateBitbar(User user) {
            using (DatabaseConnection conn = new DatabaseConnection()) {
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO gadget_bitbar "
                                + "(id, user_id, value, max_value, image, color) "
                                + "VALUES (@id, @userid, @val, @maxval, @img, @color)";

                string id = TokenGenerator.Generate();
                while (BitbarExists(id)) {
                    id = TokenGenerator.Generate();
                }

                Bitbar bitbar = new Bitbar(id, user);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@userid", user.UserId);
                cmd.Parameters.AddWithValue("@val", bitbar.Value);
                cmd.Parameters.AddWithValue("@maxval", bitbar.MaxValue);
                cmd.Parameters.AddWithValue("@img", bitbar.Image);
                cmd.Parameters.AddWithValue("@color", bitbar.Color);

                cmd.ExecuteNonQuery();
                return bitbar;
            }
        }

        public static void UpdateBitbar(Bitbar bitbar) {
            using (DatabaseConnection conn = new DatabaseConnection()) {
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE gadget_bitbar "
                                + "SET value = @val, max_value = @maxval, image = @img, color = @color";
                cmd.Parameters.AddWithValue("@val", bitbar.Value);
                cmd.Parameters.AddWithValue("@maxval", bitbar.MaxValue);
                cmd.Parameters.AddWithValue("@img", bitbar.Image);
                cmd.Parameters.AddWithValue("@color", bitbar.Color);

                cmd.ExecuteNonQuery();
            }
        }

        public static void AddBits(User user, int count) {
            using (DatabaseConnection conn = new DatabaseConnection()) {
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE gadget_bitbar "
                                + "SET value = value + @count";
                cmd.Parameters.AddWithValue("@count", count);     
                cmd.ExecuteNonQuery();

                Logger.Log("Added " + count + " to bitbar of " + user.UserId);
            }
        }
    }
}