using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultipleChoiceQuiz
{
    class Questions
    {
        public Questions()
        {
            Answers = new List<string>();
        }
        public string QuestionText { get; set; }
        public List<string> Answers { get; set; }
        public int CorrectAnswer { get; set; }
        public int AddAnswer(int indx,string ans) 
        {
            Answers[indx] = ans;
            return indx;
        }
    }
}
