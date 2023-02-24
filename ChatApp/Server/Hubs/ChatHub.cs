using ChatApp.Server.Interfaces;
using ChatApp.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;
using System.Text;

namespace ChatApp.Server.Hubs
{
    public class ChatHub : Hub
    {
        //string result = string.Empty;
        private readonly IQuestionService _questionService;
        public ChatHub(IQuestionService questionService)
        {
            _questionService = questionService;
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
            _questionService.GetQuestion();

            //result = Convert.ToString(_questionService.CheckAnswer(message));
            var result = _questionService.CheckAnswer(message);

            if (result.Item1 == true)
            {
                await Clients.All.SendAsync("ReceiveMessage", user, "Is right");
                _questionService.GetQuestion();
            }
        }

        public async Task SendQuestion(string question)
        {
            await Clients.All.SendAsync("ReceiveQuetion", question);
        }

    }



}
