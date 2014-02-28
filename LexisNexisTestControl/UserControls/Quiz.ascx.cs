using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using  LexisNexisTestControl.Models;
using LexisNexisTestControl.Services;

namespace LexisNexisTestControl.UserControls
{
    public partial class Quiz : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          if(User == null)
          {
              User = QuizUser.GetTestUser();
          }

        }

        public QuizUser User
        {
            get;
            set;
        }

        
        public void GenerateQuizUI(QuizUser user)
        {
            User = user;
            var transactionId = System.Guid.NewGuid().ToString();
            var response = LNQuizService.GetQuizQuestions(user, transactionId);

            this.lblName.Text = User.FirstName + " " + User.LastName;
            this.pnlIntro.Visible = true;

            this.QuizQuestions.Questions = response.questions.question.ToList();
            this.QuizQuestions.DataBind();

            this.lblAlert.Text = "Please answer the following questions:";

            this.ViewState.Add("quizId", response.questions.quizId);
            this.ViewState.Add("transactionId", transactionId);
            
            //foreach(var question in response.questions.question)
            //{
            //    this.txtResponse.InnerText += question.questionId + ":" + question.text + "\n";
            //    foreach(var choice in question.choice)
            //    {
            //        this.txtResponse.InnerText += choice.text + "\n";
            //    }
            //}


        }

        protected void questions_Submitted(object sender, QuizQuestions.QuestionsSubmittedEventArgs e)
        {
            var answers = e.Answers;
            var scoreRequest = new IDMv2.IdentityScoreRequest()
            {
                answer = answers.ToArray(),
                quizId = ViewState["quizId"].ToString()
            };
            var response = LNQuizService.GetProductResponse(scoreRequest, ViewState["transactionId"].ToString());
            this.lblScore.Text = response.Score;
            this.lblStatus.Text = response.Status.ToString();
            
            this.QuizQuestions.Visible = false;
            this.pnlResult.Visible = true;
            this.pnlIntro.Visible = false;

            switch(response.Status)
            {
                case IDMv2.InstantAuthenticateProductStatus.PASS:
                    this.lblAlert.Text = "Thanks you. Your identity has been verified.";
                    break;
                case IDMv2.InstantAuthenticateProductStatus.PENDING:
                    this.lblAlert.Text = "Please answer this additional question";
                    this.QuizQuestions.Questions = response.BonusQuestions;
                    this.QuizQuestions.DataBind();
                    this.QuizQuestions.Visible = true;
                    break;
                case IDMv2.InstantAuthenticateProductStatus.OMITTED:
                    this.lblAlert.Text = "Please answer all the provided questions";
                    //TODO: This should never happen, client-side checks should ensure all answers
                    this.QuizQuestions.Visible = true;
                    break;
                case IDMv2.InstantAuthenticateProductStatus.FAIL:
                    this.lblAlert.Text = "We're sorry but we cannot verify your identity.";
                    break;
                case IDMv2.InstantAuthenticateProductStatus.UNABLE_TO_GENERATE:
                case IDMv2.InstantAuthenticateProductStatus.NOT_ENOUGH_DATA:
                case IDMv2.InstantAuthenticateProductStatus.IDENTITY_NOT_LOCATED:
                    this.lblAlert.Text = "We do not have enough information to generate a quiz.";
                    break;
                case IDMv2.InstantAuthenticateProductStatus.TIMED_OUT:
                case IDMv2.InstantAuthenticateProductStatus.ABANDONED:
                    this.lblAlert.Text = "Your session has timed out. Please try again.";
                    break;
                case IDMv2.InstantAuthenticateProductStatus.SYSTEM_ERROR:
                default:
                    this.lblAlert.Text = "An unexpected error occured. Please try again.";
                    break;
            }
        }
    }
}