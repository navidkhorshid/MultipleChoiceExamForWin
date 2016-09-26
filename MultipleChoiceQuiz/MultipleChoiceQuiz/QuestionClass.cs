using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultipleChoiceQuiz
{
    class QuestionClass
    {
        public QuestionClass() { }
        public string TEXT  {get;set;}
        public string A {get;set;}
        public string B {get;set;}
        public string C {get;set;}
        public string D {get;set;}
        public int ANS { get; set; }
        public int StudentAnswer { get; set; }
    }
}
