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
    public partial class Student : Form
    {
        List<QuestionClass> questionlist = new List<QuestionClass>();
        DateTime endTime = new DateTime();
        int AnsNow = 0;
        int quiz_id;
        public Student()
        {
            InitializeComponent();
        }

        private void Results() 
        {
            DBLINQDataContext db = new DBLINQDataContext();
            int correct = 0;
            int wrong = 0;
            int notsolved = 0;
            double score = 0;
            DateTime date = new DateTime();
            date = System.DateTime.Now;
            for (int i = 0; i < questionlist.Count(); i++) 
            {
                if (questionlist[i].StudentAnswer == questionlist[i].ANS) 
                {
                    correct++;
                }
                else if (questionlist[i].StudentAnswer == 0)
                {
                    notsolved++;
                }
                else 
                {
                    wrong++;
                }
            }
            score = ((double)(correct * 3 - wrong) / (double)(3 * (wrong + correct + notsolved))) * 100;
            QUIZ_USER qu = new QUIZ_USER()
            {
                U_ID = User.User_id,
                QZ_ID = quiz_id,
                QU_DATE = date,
                QU_CORRECT = correct,
                QU_WRONG = wrong,
                QU_SCORE = (int)score,
                QU_NOTSOLVED = notsolved
            };

            db.QUIZ_USERs.InsertOnSubmit(qu);
            db.SubmitChanges();

            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = true;
            button1.Visible = true;

            MessageBox.Show("Your Score is : " + score.ToString(),"Well-done",MessageBoxButtons.OK);
            Student st = new Student();
            this.Hide();
            this.Close();
            st.ShowDialog();
            
        }

        private void Student_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'quizDBDataSet.QUIZ' table. You can move, or remove it, as needed.
            this.qUIZTableAdapter.Fill(this.quizDBDataSet.QUIZ);
          
            DBLINQDataContext db = new DBLINQDataContext();

            var query = from qzu in db.QUIZ_USERs
                                 where qzu.U_ID == User.User_id
                                 select new
                                 {
                                     ValueMember = qzu.QU_ID,
                                     Text = from q in db.QUIZs
                                             where qzu.QZ_ID == q.QZ_ID
                                             select q.QZ_TITLE
                                 };

            foreach(var q in query)
            {
                listBox2.Items.Add(q.ValueMember);
            }

            label1.Text =  " Hello " + User.User_name + " !";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            try
            {
                quiz_id = (int)listBox1.SelectedValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + "Choose an exam !");
                return;
            }
            questionlist.Clear();
            DBLINQDataContext db = new DBLINQDataContext();
            
            try
            {
                
                var query = from qq in db.QUIZ_QUESTIONs
                            where qq.QZ_ID == quiz_id
                            select new
                            {
                                quest = from q in db.QUESTIONs
                                        where q.Q_ID == qq.Q_ID
                                        select new
                                        {
                                            TEXT = q.Q_TEXT,
                                            A = q.Q_A,
                                            B = q.Q_B,
                                            C = q.Q_C,
                                            D = q.Q_D,
                                            ANS = q.Q_ANSWER,
                                            StudentAnswer = 0
                                        }
                            };
                int i = 0;
                if (query.Count() > 0)
                {
                    foreach (var qc in query)
                    {
                        //questionlist[i].TEXT = qc.quest.First().TEXT;
                        //questionlist[i].A = qc.quest.Single().A;
                        //questionlist[i].B = qc.quest.Single().B;
                        //questionlist[i].C = qc.quest.Single().C;
                        //questionlist[i].D = qc.quest.Single().D;
                        //questionlist[i].ANS = (int)qc.quest.Single().ANS;
                        //questionlist[i].StudentAnswer = (int)qc.quest.Single().StudentAnswer;
                        QuestionClass qqcc = new QuestionClass();
                        qqcc.TEXT = qc.quest.First().TEXT;
                        qqcc.A = qc.quest.Single().A;
                        qqcc.B = qc.quest.Single().B;
                        qqcc.C = qc.quest.Single().C;
                        qqcc.D = qc.quest.Single().D;
                        qqcc.ANS = (int)qc.quest.Single().ANS;
                        qqcc.StudentAnswer = (int)qc.quest.Single().StudentAnswer;
                        questionlist.Add(qqcc);
                        i++;
                    }
                //age 2 ta bashe i ham 2 mishe == questionlist.count
                    if (i == questionlist.Count && i > 0)
                    {
                        var minutes = questionlist.Count();
                        var start = DateTime.UtcNow;
                        endTime = start.AddMinutes(minutes + 0.02);
                        timer1.Enabled = true;
                 
                        pictureBox1.Visible = true;
                        pictureBox2.Visible = true;
                        button1.Visible = false;
                        pictureBox3.Visible = false;
                        textBox6.Text = questionlist[AnsNow].TEXT;
                        label9.Text = questionlist[AnsNow].A;
                        label10.Text = questionlist[AnsNow].B;
                        label11.Text = questionlist[AnsNow].C;
                        label12.Text = questionlist[AnsNow].D;
                        label13.Text = (AnsNow + 1).ToString() + " / " + i.ToString();
                    }
                }
                else
                {
                    MessageBox.Show("No Questions Found...");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DBLINQDataContext db = new DBLINQDataContext();
                var query = from qu in db.QUIZ_USERs
                            where qu.QU_ID == Convert.ToInt32(listBox2.SelectedItem)
                            select new
                            {
                                QuizTitle = (from q in db.QUIZs
                                             where q.QZ_ID == qu.QZ_ID
                                             select q.QZ_TITLE).Single(),
                                Score = qu.QU_SCORE,
                                Correct = qu.QU_CORRECT,
                                Wrong = qu.QU_WRONG,
                                NotSolved = qu.QU_NOTSOLVED,
                                Date = qu.QU_DATE
                            };
                textBox1.Text = query.First().Score.ToString();
                textBox2.Text = query.First().Correct.ToString();
                textBox3.Text = query.First().Wrong.ToString();
                textBox4.Text = query.First().NotSolved.ToString();
                textBox5.Text = query.First().Date.ToString();
                label3.Text = query.First().QuizTitle.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Select a quiz ...");
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan remainingtime = endTime - DateTime.UtcNow;
            if (remainingtime < TimeSpan.Zero)
            {
                label14.Text = "Done!";
                timer1.Enabled = false;
                System.Threading.Thread.Sleep(2000);
                //form to show results
                Results();
            }
            else 
            {
                label14.Text = Convert.ToInt32(remainingtime.TotalHours).ToString() + " : " + remainingtime.Minutes.ToString() + " : " + remainingtime.Seconds.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button3.Visible = true; button3.Enabled = true;
            if(radioButton1.Checked){questionlist[AnsNow].StudentAnswer = 1;}
            else if(radioButton2.Checked){questionlist[AnsNow].StudentAnswer = 2;}
            else if(radioButton3.Checked){questionlist[AnsNow].StudentAnswer = 3;}
            else if(radioButton4.Checked){questionlist[AnsNow].StudentAnswer = 4;}
            else{questionlist[AnsNow].StudentAnswer = 0;}
            AnsNow++;
            if (AnsNow == questionlist.Count)
            {
                timer1.Stop();
                //show results
                Results();
            }
            else 
            {
                textBox6.Text = questionlist[AnsNow].TEXT;
                label9.Text = questionlist[AnsNow].A;
                label10.Text = questionlist[AnsNow].B;
                label11.Text = questionlist[AnsNow].C;
                label12.Text = questionlist[AnsNow].D;
                label13.Text = (AnsNow + 1).ToString() + " / " + questionlist.Count.ToString();
                switch (questionlist[AnsNow].StudentAnswer)
                {
                    case 1:
                        radioButton1.Checked = true;
                        break;
                    case 2:
                        radioButton2.Checked = true;
                        break;
                    case 3:
                        radioButton3.Checked = true;
                        break;
                    case 4:
                        radioButton4.Checked = true;
                        break;
                    default:
                        {
                            radioButton1.Checked = false;
                            radioButton2.Checked = false;
                            radioButton3.Checked = false;
                            radioButton4.Checked = false;
                        }
                        break;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked) { questionlist[AnsNow].StudentAnswer = 1; }
            else if (radioButton2.Checked) { questionlist[AnsNow].StudentAnswer = 2; }
            else if (radioButton3.Checked) { questionlist[AnsNow].StudentAnswer = 3; }
            else if (radioButton4.Checked) { questionlist[AnsNow].StudentAnswer = 4; }
            else { questionlist[AnsNow].StudentAnswer = 0; }
            AnsNow--;
            if (AnsNow == -1)
            {
                button3.Enabled = false;
                AnsNow++;
            }
            else
            {
                textBox6.Text = questionlist[AnsNow].TEXT;
                label9.Text = questionlist[AnsNow].A;
                label10.Text = questionlist[AnsNow].B;
                label11.Text = questionlist[AnsNow].C;
                label12.Text = questionlist[AnsNow].D;
                label13.Text = (AnsNow + 1).ToString() + " / " + questionlist.Count.ToString();
                switch (questionlist[AnsNow].StudentAnswer)
                {
                    case 1:
                        radioButton1.Checked = true;
                        break;
                    case 2:
                        radioButton2.Checked = true;
                        break;
                    case 3:
                        radioButton3.Checked = true;
                        break;
                    case 4:
                        radioButton4.Checked = true;
                        break;
                    default: 
                        {
                            radioButton1.Checked = false;
                            radioButton2.Checked = false;
                            radioButton3.Checked = false;
                            radioButton4.Checked = false;
                        }
                        break;
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide(); this.Close();
            Login l = new Login();
            l.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Application.Exit();
        }
    }
}
