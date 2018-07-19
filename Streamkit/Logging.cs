using System;
using System.IO;

namespace Streamkit {
    public class Logger {
        public static Logger Instance;

        private string path;

        public Logger() {
            this.path = Directory.GetCurrentDirectory() + "/Logs/" 
                      + DateTimeOffset.UtcNow.ToUnixTimeSeconds() + ".log";
        }

        public void LogMessage(string message) {
            string log = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + " | " + message;
            Console.WriteLine(log);

            if (!File.Exists(this.path)) {
                File.WriteAllText(this.path, "START OF LOG" + Environment.NewLine);
            }
            File.AppendAllText(this.path, log + Environment.NewLine);
        }

        public static void Log(string message) {
            Instance.LogMessage(message);
        }
    }
}