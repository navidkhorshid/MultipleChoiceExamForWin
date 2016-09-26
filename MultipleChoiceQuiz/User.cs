using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultipleChoiceQuiz
{
    static class User
    {
        private static int user_id = 0;
        private static string user_name = "";

        public static int User_id { get { return user_id; } set { user_id = value; } }
        public static string User_name { get { return user_name; } set { user_name = value; } }
    }
}
