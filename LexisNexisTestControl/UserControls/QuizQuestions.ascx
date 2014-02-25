<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuizQuestions.ascx.cs" Inherits="LexisNexisTestControl.UserControls.QuizQuestions"  %>


<div id="questionContainer" runat="server">
    <p>Please answer the following questions</p>

    <asp:Repeater runat="server" ID="questionList">
        <ItemTemplate>
            <div class="question">
                <p class="body" id="divBody"><%# Eval("text") %></p>
                <asp:Repeater runat="server" ID="choiceList" DataSource='<%# Eval("choice") %>'>
                    <ItemTemplate>
                        <div class="choice">
                            <input type="radio"  value='<%#Eval("choiceId") %>' name='question_<%# DataBinder.Eval(Container.Parent.Parent, "DataItem.questionId") %>'>
                    
                            </input>
                            <label for='<%#Eval("choiceId") %>'><%#Eval("text")%></label>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Button ID="submitQuestions" OnClick="submitQuestions_Click" runat="server" Text="Submit Answers" />
</div>
