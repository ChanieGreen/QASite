using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackOverflow.Data
{
    public class QASiteRepository
    {
        private string _connectionString;

        public QASiteRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private Tag GetTag(string name)
        {
            using (var ctx = new QASiteContext(_connectionString))
            {
                return ctx.Tags.FirstOrDefault(t => t.Name.ToLower() == name.ToLower());
            }
        }

        private int AddTag(string name)
        {
            using (var ctx = new QASiteContext(_connectionString))
            {
                var tag = new Tag { Name = name };
                ctx.Tags.Add(tag);
                ctx.SaveChanges();
                return tag.Id;
            }
        }

        public void AddQuestion(Question question, IEnumerable<string> tags)
        {
            using (var ctx = new QASiteContext(_connectionString))
            {
                ctx.Questions.Add(question);
                foreach (string tag in tags)
                {
                    Tag t = GetTag(tag);
                    int tagId;
                    if (t == null)
                    {
                        tagId = AddTag(tag);
                    }
                    else
                    {
                        tagId = t.Id;
                    }
                    ctx.QuestionsTags.Add(new QuestionsTags
                    {
                        QuestionId = question.Id,
                        TagId = tagId
                    });
                }

                ctx.SaveChanges();
            }
        }
        
        public IEnumerable<Question> GetQuestions()
        {
            using(var context = new QASiteContext(_connectionString))
            {
                return context.Questions
                    .Include(q => q.Answers)
                    .ThenInclude(a => a.User)
                    .Include(q => q.QuestionsTags)
                    .ThenInclude(qt => qt.Tag)
                    .Include(q => q.Likes)
                    .Include(q => q.User)
                    .OrderByDescending(q => q.DatePosted)
                    .ToList();
            }
        }

        public Question GetQuestionById (int id)
        {
            using (var context = new QASiteContext(_connectionString))
            {
                return context.Questions
                    .Include(q => q.Answers)
                    .ThenInclude(a => a.User)
                    .Include(q => q.QuestionsTags)
                    .ThenInclude(qt => qt.Tag)
                    .Include(q => q.Likes)
                    .Include(q => q.User)
                    .FirstOrDefault(q => q.Id == id);
            }
        }

        public void AddAnswer (Answer answer)
        {
            using(var context = new QASiteContext(_connectionString))
            {
                context.Answers.Add(answer);
                context.SaveChanges();
            }
        }

        public void LikeQuestion(Like like)
        {
            using(var context = new QASiteContext(_connectionString))
            {
                context.Likes.Add(like);
                context.SaveChanges();
            }
        }

        public int GetLikes (int questionId)
        {
            using(var context = new QASiteContext(_connectionString))
            {
                return context.Likes.Where(l => l.QuestionId == questionId).Count();
            }
        }
    }
}
