using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MultipleChoiceQuiz
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private bool IsValidUser(string userName, string password)
        {
            DBLINQDataContext db = new DBLINQDataContext();
            var query = from u in db.USERs
                        where u.U_UN == userName && u.U_PASS == password
                        select u;

            if (query.Count()>0)
            {
                User.User_id = query.First().U_ID;
                User.User_name = query.First().U_FNAME + " " + query.First().U_LNAME;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true) 
            {
                if (IsValidUser(textBox1.Text, textBox2.Text))
                {
                    Student st = new Student();
                    this.Hide();
                    st.ShowDialog();
                    
                }
            }
            else if(radioButton2.Checked == true)
            {
                if (textBox1.Text == "admin" && textBox2.Text == "admin")
                {
                    Administrator ad = new Administrator();
                    this.Hide();
                    ad.ShowDialog();
                    
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }


    }
}
