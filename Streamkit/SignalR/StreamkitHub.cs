using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

using Newtonsoft.Json.Linq;

using Streamkit.Core;
using Streamkit.Utils;

namespace Streamkit.Hubs {
    public class BitbarHub : Hub {
        // TODO: Move this logic up one level when we create a new hub!
        // One to many!.
        private static Dictionary<string, HashSet<BitbarHub>> userConnections 
                = new Dictionary<string, HashSet<BitbarHub>>();

        // One to one!
        private static Dictionary<BitbarHub, string> connectionUsers 
                = new Dictionary<BitbarHub, string>();

        public override async Task OnConnectedAsync() {
            await Clients.Caller.SendAsync("request_source", "test");
            await RequestSource(Clients.Caller);
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception) {
            if (connectionUsers.ContainsKey(this)) {
                string userid = connectionUsers[this];

                if (userConnections.ContainsKey(userid)) {
                    userConnections[userid] = null;
                    userConnections.Remove(userid);
                }

                connectionUsers.Remove(this);
            }

            return base.OnDisconnectedAsync(exception);
        }
  
        // TODO: Move this logic up one level when we create a new hub!
        public async Task RequestSource(IClientProxy client) {
            await client.SendAsync("request_source", "");
        }

        // TODO: Move this logic up one level when we create a new hub!
        public async Task ReceiveSource(string source) {
            JObject json = JObject.Parse(source);
            Bitbar bitbar = BitbarManager.GetBitbar((string)json["source_id"]);

            string userid = bitbar.User.UserId;

            connectionUsers[this] = userid;
            if (!userConnections.ContainsKey(userid)) {
                userConnections[userid] = new HashSet<BitbarHub>();
            }
            userConnections[userid].Add(this);

            await UpdateSource(bitbar);
        }

        public static async Task UpdateSource(Bitbar bitbar) {
            if (!userConnections.ContainsKey(bitbar.User.UserId)) return;

            JObject source = new JObject();
            source["source_id"] = bitbar.Id;
            source["value"] = bitbar.Value;
            source["max_value"] = bitbar.MaxValue;
            source["image"] = Base64.Encode(bitbar.Image);
            source["target_color"] = "#" + bitbar.TargetColor;
            source["fill_color"] = "#" + bitbar.FillColor;

            List<BitbarHub> remList = new List<BitbarHub>();

            foreach (BitbarHub hub in userConnections[bitbar.User.UserId]) {
                try {
                    // TODO: Some connection hubs are disposed, is there a better way to tell?
                    await hub.Clients.Caller.SendAsync("update_source", source.ToString());
                }
                catch {
                    remList.Add(hub);
                }
            }

            foreach (BitbarHub hub in remList) {
                userConnections[bitbar.User.UserId].Remove(hub);
            }
        }
    }
}