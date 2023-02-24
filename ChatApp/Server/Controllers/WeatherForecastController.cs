using ChatApp.Server.Interfaces;
using ChatApp.Server.Services;
using ChatApp.Shared;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionsController : ControllerBase
    {
        IQuestionService _questionService;
        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }



        [HttpGet]
        public ActionResult<string> GetQuestion()
        {
            return _questionService.GetQuestion();
        }

        [HttpPost]
        public ActionResult<string> CheckAnswer([FromBody] string answer)
        {
            var result = _questionService.CheckAnswer(answer);

            if (result.Item1)
            {
                return Ok(result.Item2);
            }
            else
            {
                return BadRequest(result.Item2);
            }
        }

    }
}


