using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Server.Interfaces
{
    public interface IQuestionService
    {
        public ActionResult<string> GetQuestion();

        public Tuple<bool, string> CheckAnswer(string answer);

    }
}
