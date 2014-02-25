<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Quiz.ascx.cs" Inherits="LexisNexisTestControl.UserControls.Quiz" %>
<%@ Register Src="~/UserControls/QuizQuestions.ascx" TagPrefix="uc1" TagName="QuizQuestions" %>

<div class="alert ">
    <asp:Label ID="lblAlert" runat="server" />
</div>

<div >
    
    <p id="pnlIntro" runat="server" visible="false">Hello <asp:Label ID="lblName" runat="server" />! </p>

    <uc1:QuizQuestions runat="server" id="QuizQuestions" OnQuestionsSubmitted="questions_Submitted" />
    
    <div id="pnlResult" runat="server" visible="false">
        Scored result: <asp:Label ID="lblScore" runat="server" /> <br />
        Status result: <asp:Label ID="lblStatus" runat="server" />
    </div>
</div>
