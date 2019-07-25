using StackOverflow.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackOverflow.Models
{
    public class QuestionViewModel
    {
        public Question Question { get; set; }
        public User CurrentUser { get; set; }
    }
}
