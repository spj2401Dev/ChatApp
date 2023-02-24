using ChatApp.Server.Hubs;
using ChatApp.Server.Interfaces;
using ChatApp.Server.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace ChatApp.Server.Services
{

    public class QuestionService : IQuestionService
    {

        private readonly IHubContext<ChatHub> _hubContext;
        public QuestionService(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }
        //private readonly IServiceScopeFactory _serviceScopeFactory;

        //public QuestionService(IServiceScopeFactory serviceScopeFactory)
        //{
        //    _serviceScopeFactory = serviceScopeFactory;
        //}

        private readonly string[] Questions = new[]
{
            "What is 1+1?",
            "What color is the sky?",
            "Ist weich ein synonym für flauschig?",
            "Ist C# besser als JS?",
            "Sind mehr als 40 chicken nuggets möglich?"
        };

        private readonly Dictionary<string, string[]> Answers = new Dictionary<string, string[]>
        {
            { "What is 1+1?", new[] { "2", "11" } },
            { "What color is the sky?", new[] { "blue", "light blue" } },
            { "Ist weich ein synonym für flauschig?", new[] { "nein", "nö", "ne", "ganz sicher nicht", "noch nicht mal ansatzweiße" } },
            { "Ist C# besser als JS?", new[] { "ja", "eventuell" } },
            { "Sind mehr als 40 chicken nuggets möglich?", new[] { "vieleicht" } },
        };


        private static string CurrentQuestion;

        public ActionResult<string> GetQuestion()
        {

            if (string.IsNullOrEmpty(CurrentQuestion))
            {
                CurrentQuestion = Questions[new Random().Next(0, Questions.Length)];
            }

            _hubContext.Clients.All.SendAsync("ReceiveQuetion", CurrentQuestion);

            //using (var scope = _serviceScopeFactory.CreateScope())
            //{
            //    ChatHub _chatHub = scope.ServiceProvider.GetRequiredService<ChatHub>();
            //    _chatHub.SendQuestion(CurrentQuestion);
            //    scope.Dispose();
            //}


            return new OkObjectResult(CurrentQuestion);
        }

        public Tuple<bool, string> CheckAnswer(string answer)
        {
            Tuple<bool, string> responseTupel;

            if (string.IsNullOrEmpty(CurrentQuestion))
            {
                responseTupel = new Tuple<bool, string>(false, "No question has been generated yet");
            }

            if (!Answers.TryGetValue(CurrentQuestion, out string[] possibleAnswers))
            {
                responseTupel = new Tuple<bool, string>(false, "No possible answers found for the current question");

            }

            if (possibleAnswers.Contains(answer.ToLower()))
            {
                CurrentQuestion = Questions[new Random().Next(0, Questions.Length)];
                responseTupel = new Tuple<bool, string>(true, CurrentQuestion);

                //return "true";
            }
            else
            {
                responseTupel = new Tuple<bool, string>(false, "Incorrect answer");

            }
            return responseTupel;
        }
    }

}


