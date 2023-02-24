using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using System.Security.Cryptography;

namespace ChatApp.Client.Pages
{
    public partial class Index
    {
        private HubConnection? hubConnection;
        private List<string> messages = new List<string>();
        private string? userInput;
        private string? messageInput;
        public string QuestionText { get; set; } = "Loading Question...";


        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(Navigation.ToAbsoluteUri("/chathub"))
                .Build();

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var encodedMsg = $"{user}: {message}";
                messages.Insert(0, encodedMsg);

                if (messages.Count > 30)
                {
                    messages.RemoveAt(messages.Count - 1);
                }

                StateHasChanged();
            });

            hubConnection.On<string>("ReceiveQuetion", (question) =>
            {
                QuestionText = question;
            });

            await hubConnection.StartAsync();
        }

        private async Task Send()
        {
            if (hubConnection is not null)
            {
                await hubConnection.InvokeAsync("SendMessage", userInput, messageInput);
            }
        }

        public bool IsConnected =>
            hubConnection?.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }
    }
}