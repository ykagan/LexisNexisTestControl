var LNQuiz = function ($) {
    var _static = {
        currentQuestionId: '',
        currentQuestionIndex:0,
        questionList: []
    }

    return {
        Init: function () {
            //set the first question to the current question
            _static.currentQuestionId = $(".questionList .question:first").data("id");
            _static.currentQuestionIndex = 0;
            //show the first question
            $(".questionList .question:first").show();
            $(".questionList .question").each(function (elem) {
                _static.questionList.push($(this).data("id"));
            });

            //bind controls
            $(".questionList .btnNextQuestion").bind("click", LNQuiz.ShowNextQuestion)
        },
        ShowNextQuestion: function () {
            $(".questionList .question[data-id=" + _static.currentQuestionId + "]").slideUp();
            _static.currentQuestionIndex++;
            _static.currentQuestionId = _static.questionList[_static.currentQuestionIndex];
            if (_static.currentQuestionIndex == _static.questionList.length - 1) {
                //if this is the last question, show the submit button, hide the next button
                $(".btnSubmitQuiz").show();
                $(".btnNextQuestion").hide();
            }
            $(".questionList .question[data-id=" + _static.currentQuestionId + "]").slideDown();
            
        }
    }
}(jQuery);