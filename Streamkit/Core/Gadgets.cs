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
            this.id = id;
            this.user = user;
        }

        public string Id {
            get { return this.id; }
        }

        public User User {
            get { return this.user; }
        }

        public abstract void Update();
    }


    public class Bitbar : Gadget {
        private int value = 0;
        private int maxValue = 1000;
        private byte[] image = null;
        private string targetColor = "#000000";
        private string fillColor = "#00FF00";

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

        public string TargetColor {
            get { return this.targetColor.Replace("#", ""); }
            set {
                if (value.Replace("#", "").Length != 6) {
                    this.targetColor = "#000000";
                }
                this.targetColor = value;
            }
        }

        public string FillColor {
            get { return this.fillColor.Replace("#", ""); }
            set {
                if (value.Replace("#", "").Length != 6) {
                    this.fillColor = "#00FF00";
                }
                this.fillColor = value;
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

        public static Bitbar GetBitbar(string id) {
            using (DatabaseConnection conn = new DatabaseConnection()) {
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "user_id, value, max_value, image, target_color, fill_color "
                                + "FROM gadget_bitbar WHERE id = @id";


                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows) {
                    throw new Exception("Bitbar " + id + " does not exist.");
                }

                reader.Read();

                User user = UserManager.GetUser(reader.GetString("user_id"));

                Bitbar bitbar = new Bitbar(reader.GetString("id"), user);
                bitbar.Value = reader.GetInt32("value");
                bitbar.MaxValue = reader.GetInt32("max_value");
                bitbar.Image = reader.GetBytes("image");
                bitbar.TargetColor = reader.GetString("target_color");
                bitbar.FillColor = reader.GetString("fill_color");

                return bitbar;
            }
        }

        public static Bitbar GetBitbar(User user) {
            using (DatabaseConnection conn = new DatabaseConnection()) {
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT id, value, max_value, image, target_color, fill_color "
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
                bitbar.TargetColor = reader.GetString("target_color");
                bitbar.FillColor = reader.GetString("fill_color");

                return bitbar;
            }
        }

        public static Bitbar CreateBitbar(User user) {
            using (DatabaseConnection conn = new DatabaseConnection()) {
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO gadget_bitbar "
                                + "(id, user_id, value, max_value, image, target_color, fill_color) "
                                + "VALUES (@id, @userid, @val, @maxval, @img, @tcolor, @fcolor)";

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
                cmd.Parameters.AddWithValue("@tcolor", bitbar.TargetColor);
                cmd.Parameters.AddWithValue("@fcolor", bitbar.FillColor);

                cmd.ExecuteNonQuery();
                return bitbar;
            }
        }

        public static void UpdateBitbar(Bitbar bitbar) {
            using (DatabaseConnection conn = new DatabaseConnection()) {
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE gadget_bitbar "
                                + "SET value = @val, max_value = @maxval, image = @img, "
                                + "target_color = @tcolor, fill_color = @fcolor "
                                + "WHERE id = @id";
                cmd.Parameters.AddWithValue("@val", bitbar.Value);
                cmd.Parameters.AddWithValue("@maxval", bitbar.MaxValue);
                cmd.Parameters.AddWithValue("@img", bitbar.Image);
                cmd.Parameters.AddWithValue("@tcolor", bitbar.TargetColor);
                cmd.Parameters.AddWithValue("@fcolor", bitbar.FillColor);
                cmd.Parameters.AddWithValue("@id", bitbar.Id);
                cmd.ExecuteNonQuery();
            }
        }

        public static void AddBits(User user, int count) {
            using (DatabaseConnection conn = new DatabaseConnection()) {
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE gadget_bitbar "
                                + "SET value = value + @count "
                                + "WHERE user_id = @userid";
                cmd.Parameters.AddWithValue("@count", count);
                cmd.Parameters.AddWithValue("@userid", user.UserId);
                cmd.ExecuteNonQuery();

                Logger.Log("Added " + count + " to bitbar of " + user.UserId);
            }
        }
    }
}