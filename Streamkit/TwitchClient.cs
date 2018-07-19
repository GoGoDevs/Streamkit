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
            Logger.Log("Message received: " + e.ChatMessage);
        }

        private void onNewSubscriber(object sender, OnNewSubscriberArgs e) {
            Logger.Log("New subscriber in " + e.Channel);
        }

        private void onResubsriber(object sender, OnReSubscriberArgs e) {
            Logger.Log("Resubscriber in " + e.Channel);
        }

        private void onGiftedSubscription(object sender, OnGiftedSubscriptionArgs e) {
            Logger.Log("Gifted subscription in " + e.Channel);
        }
    }
}