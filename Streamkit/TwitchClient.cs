using System;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;

namespace Streamkit.Twitch {
    public class TwitchBot {
        public static TwitchBot Instance;

        private TwitchClient client;

        public TwitchBot() {
            ConnectionCredentials credentials = new ConnectionCredentials(
                    "BotZura", Config.TwitchChatToken);

            client = new TwitchClient();
            client.Initialize(credentials);

            client.OnMessageReceived += onMessageReceived;
            client.OnNewSubscriber += onNewSubscriber;
            client.OnReSubscriber += onResubsriber;
            client.OnGiftedSubscription += onGiftedSubscription;

            client.Connect();

            // For testing
            this.JoinChannel("gogomic");

            Instance = this;
        }

        public void JoinChannel(string channelName) {
            this.client.JoinChannel(channelName);
        }

        private void onMessageReceived(object sender, OnMessageReceivedArgs e) {
            throw new NotImplementedException();
        }

        private void onNewSubscriber(object sender, OnNewSubscriberArgs e) {
            throw new NotImplementedException();
        }

        private void onResubsriber(object sender, OnReSubscriberArgs e) {
            throw new NotImplementedException();
        }

        private void onGiftedSubscription(object sender, OnGiftedSubscriptionArgs e) {
            throw new NotImplementedException();
        }
    }
}