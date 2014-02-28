<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuizQuestions.ascx.cs" Inherits="LexisNexisTestControl.UserControls.QuizQuestions"  %>


<div id="questionContainer"  class="questionList" runat="server">
    <asp:Repeater runat="server" ID="questionList" ItemType="LexisNexisTestControl.IDMv2.Question">
        <ItemTemplate>
            <div class="question" data-id='<%# Item.questionId %>' runat="server" id='question'>
                <p class="body" id="divBody"><%# Item.text %></p>
                <asp:Repeater runat="server" ID="choiceList" DataSource='<%# BindItem.choice %>' ItemType="LexisNexisTestControl.IDMv2.Choice">
                    <ItemTemplate>
                        <div class="choice">
                            <input type="radio" id='<%# "choice_" + Item.choiceId %>' value='<%# Item.choiceId %>'
                                 name='question_<%# DataBinder.Eval(Container.NamingContainer.NamingContainer, "DataItem.questionId") %>' />
                            <label for=<%# "choice" + Item.choiceId %>><%# Item.text  %></label>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <input type="button" class="btnNextQuestion" value="Next Question" />
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Button ID="submitQuestions" OnClick="submitQuestions_Click" runat="server" Text="Submit Answers" CssClass="btnSubmitQuiz" />

    <script type="text/javascript">
        LNQuiz.Init();
    </script>
</div>
