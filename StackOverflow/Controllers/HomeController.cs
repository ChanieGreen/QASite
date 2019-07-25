using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StackOverflow.Data;
using StackOverflow.Models;

namespace StackOverflow.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString;
        private UserRepository _userRepo;
        private QASiteRepository _questionRepo;

        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _userRepo = new UserRepository(_connectionString);
            _questionRepo = new QASiteRepository(_connectionString);
        }

        public IActionResult Index()
        {
            return View(_questionRepo.GetQuestions());
        }

        public IActionResult Question(int id)
        {
            var vm = new QuestionViewModel();
            vm.Question = _questionRepo.GetQuestionById(id);
            vm.CurrentUser = _userRepo.GetUserByEmail(User.Identity.Name);
            return View(vm);
        }

        [Authorize]
        public IActionResult AddQuestion()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddQuestion(Question question, IEnumerable<string> tags)
        {
            question.DatePosted = DateTime.Now;
            var user = _userRepo.GetUserByEmail(User.Identity.Name);
            question.UserId = user.Id;
            _questionRepo.AddQuestion(question, tags);
            return Redirect("/");
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddAnswer(Answer answer)
        {
            answer.DatePosted = DateTime.Now;
            var user = _userRepo.GetUserByEmail(User.Identity.Name);
            answer.UserId = user.Id;
            _questionRepo.AddAnswer(answer);
            return Redirect($"/home/question?id={answer.QuestionId}");
        }

        [HttpPost]
        public void LikeQuestion(int questionId)
        {
            var user = _userRepo.GetUserByEmail(User.Identity.Name);
            var like = new Like
            {
                QuestionId = questionId,
                UserId = user.Id
            };
            _questionRepo.LikeQuestion(like);
        }

        public IActionResult GetLikes(int questionId)
        {
            return Json(new { likes = _questionRepo.GetLikes(questionId) });
        }
    }
}
