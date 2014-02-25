using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LexisNexisTestControl.UserControls
{
    public partial class QuizQuestions : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(this.ViewState["Questions"] != null)
            {
                this.Questions = this.ViewState["Questions"] as List<IDMv2.Question>;
            }
            
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            this.questionContainer.Visible = this.Questions != null;
        }

        public List<IDMv2.Question> Questions { get; set; }

        public override void DataBind(){
            base.DataBind();
            if(this.Questions != null)
            {
                this.questionList.DataSource = this.Questions;
                this.questionList.DataBind();
            }
            this.ViewState.Add("Questions", Questions);
        }

        public class QuestionsSubmittedEventArgs
        {
            public List<IDMv2.IdentityQuizAnswer> Answers { get; set; }
        }
        public delegate void QuestionSubmission(object sender, QuestionsSubmittedEventArgs e);
        public event QuestionSubmission QuestionsSubmitted;

        protected void submitQuestions_Click(object sender, EventArgs e)
        {
            var answers = new List<IDMv2.IdentityQuizAnswer>();
            foreach (var question in Questions)
            {
                var answer = Request.Form["question_" + question.questionId];
                if (answer != null)
                {

                    answers.Add(new IDMv2.IdentityQuizAnswer()
                    {
                        questionId = question.questionId,
                        choiceId = answer
                    });
                }
            }
            //trigger event to notify parent control that questions have been submitted
            this.QuestionsSubmitted(this, new QuestionsSubmittedEventArgs() { Answers = answers });
        }
    }
}