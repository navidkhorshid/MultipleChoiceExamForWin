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
    public partial class Administrator : Form
    {
        DBLINQDataContext db = new DBLINQDataContext();
        public Administrator()
        {
            InitializeComponent();
        }

        private void Administrator_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'quizDBDataSet.QUESTION' table. You can move, or remove it, as needed.
            this.qUESTIONTableAdapter.Fill(this.quizDBDataSet.QUESTION);
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var query = from p in db.QUESTIONs
                        where p.Q_ID == (int)listBox1.SelectedValue
                        select new
                        {
                            text = p.Q_TEXT,
                            a  = p.Q_A,
                            b = p.Q_B,
                            c = p.Q_C,
                            d = p.Q_D,
                            ca = p.Q_ANSWER
                        };

            if (query.Count()>0)
            {
                textBox10.Text = query.First().text;
                textBox9.Text = query.First().a;
                textBox8.Text = query.First().b;
                textBox7.Text = query.First().c;
                textBox6.Text = query.First().d;
                switch (query.First().ca)
                {
                    case 1:
                        radioButton8.Checked = true;
                        break;
                    case 2:
                        radioButton7.Checked = true;
                        break;
                    case 3:
                        radioButton6.Checked = true;
                        break;
                    case 4:
                        radioButton5.Checked = true;
                        break;
                    default: 
                        {
                            radioButton5.Checked = false;
                            radioButton6.Checked = false;
                            radioButton7.Checked = false;
                            radioButton8.Checked = false;
                        }
                        break;
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox3.Items.Add(listBox2.SelectedValue);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox3.Items.Remove(listBox3.SelectedItem);
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var query = from p in db.QUESTIONs
                        where p.Q_ID == (int)listBox2.SelectedValue
                        select new
                        {
                            text = p.Q_TEXT
                        };

            if (query.Count()>0)
            {
                textBox11.Text = query.First().text;
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var query = from p in db.QUESTIONs
                        where p.Q_ID == Convert.ToInt32(listBox3.SelectedItem)
                        select new
                        {
                            text = p.Q_TEXT
                        };

            if (query.Count()>0)
            {
                textBox11.Text = query.First().text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int correct_answer = 0;

            if (radioButton1.Checked)
                correct_answer = 1;
            else if (radioButton2.Checked)
                correct_answer = 2;
            else if (radioButton3.Checked)
                correct_answer = 3;
            else if (radioButton4.Checked)
                correct_answer = 4;

            QUESTION q = new QUESTION()
            {
                Q_TEXT = textBox1.Text,
                Q_A = textBox2.Text,
                Q_B = textBox3.Text,
                Q_C = textBox4.Text,
                Q_D = textBox5.Text,
                Q_ANSWER = correct_answer
            };

            db.QUESTIONs.InsertOnSubmit(q);
            db.SubmitChanges();
            Administrator ad = new Administrator();
            this.Close();
            ad.ShowDialog();
            //db.Insert_Question(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, correct_answer);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            QUESTION thisQ = db.QUESTIONs.Single(q => q.Q_ID == (int)listBox1.SelectedValue);
            thisQ.Q_TEXT = textBox10.Text;
            thisQ.Q_A = textBox9.Text;
            thisQ.Q_B = textBox8.Text;
            thisQ.Q_C = textBox7.Text;
            thisQ.Q_D = textBox6.Text;
            int correct_answer = 0;
            if (radioButton8.Checked)
                correct_answer = 1;
            else if (radioButton7.Checked)
                correct_answer = 2;
            else if (radioButton6.Checked)
                correct_answer = 3;
            else if (radioButton5.Checked)
                correct_answer = 4;
            thisQ.Q_ANSWER = correct_answer;

            db.SubmitChanges();
            Administrator ad = new Administrator();
            this.Close();
            ad.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            QUIZ qz = new QUIZ();
            qz.QZ_TITLE = textBox12.Text;
            db.QUIZs.InsertOnSubmit(qz);
            db.SubmitChanges();
            for (int i = 0; i < listBox3.Items.Count; i++)
            {
                QUIZ_QUESTION qq = new QUIZ_QUESTION()
                {
                    Q_ID = Convert.ToInt32(listBox3.Items[i]),
                    QZ_ID = qz.QZ_ID
                };
                db.QUIZ_QUESTIONs.InsertOnSubmit(qq);
            }
            db.SubmitChanges();
            Administrator ad = new Administrator();
            this.Close();
            ad.ShowDialog();
        }

        private void Administrator_FormClosed(object sender, FormClosedEventArgs e)
        {
            Login l = new Login();
            l.Show();
        }

       
    }
}
