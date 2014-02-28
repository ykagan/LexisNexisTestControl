using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LexisNexisTestControl.Models
{
    /// <summary>
    /// Super class of IDMv2 Question with additonal choice info
    /// </summary>
    public class QuizQuestion: IDMv2.Question
    {
        public QuizQuestion(IDMv2.Question question)
        {
            this.questionId = question.questionId;
            this.sequenceId = question.sequenceId;
        }

        public List<QuizChoice> choices {
            get{
                return choice.Select(c => 
                    new QuizChoice{
                        text = c.text,
                        choiceId = c.choiceId,
                        isCorrect = c.isCorrect,
                        isCorrectSpecified = c.isCorrectSpecified,
                        sequenceId = c.sequenceId,
                        sequenceIdSpecified = c.sequenceIdSpecified,
                        QuestionId = this.questionId,
                    }).ToList();
            }
        }
    }
    /// <summary>
    /// IDMv2 choice with the question id
    /// </summary>
    public class QuizChoice: IDMv2.Choice
    {
        public string QuestionId
        {
            get;
            set;
        }
    }

}