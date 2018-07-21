using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

using Newtonsoft.Json.Linq;

using Streamkit.Core;
using Streamkit.Utils;

namespace Streamkit.Hubs {
    public class BitbarHub : Hub {
        private Dictionary<User, IClientProxy> users = new Dictionary<User, IClientProxy>();

        public override async Task OnConnectedAsync() {
            await RequestSource(Clients.Caller);
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception) {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task RequestSource(IClientProxy client) {
            await client.SendAsync("request_source", "");
        }

        public async Task ReceiveSource(string user, string source) {
            JObject json = JObject.Parse(source);
            Bitbar bitbar = BitbarManager.GetBitbar((string)json["source_id"]);
            users[bitbar.User] = Clients.Caller;

            await UpdateSource(bitbar);
        }

        public async Task UpdateSource(Bitbar bitbar) {
            JObject source = new JObject();
            source["source_id"] = bitbar.Id;
            source["value"] = bitbar.Value;
            source["max_value"] = bitbar.MaxValue;
            source["image"] = Base64.Encode(bitbar.Image);
            source["target_color"] = bitbar.TargetColor;
            source["fill_color"] = bitbar.FillColor;

            await this.users[bitbar.User].SendAsync("update_source", source.ToString());
        }
    }
}