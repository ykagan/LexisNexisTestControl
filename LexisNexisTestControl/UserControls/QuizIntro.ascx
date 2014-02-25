<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuizIntro.ascx.cs" Inherits="LexisNexisTestControl.UserControls.QuizIntro" %>
<%@ Register Src="~/UserControls/Quiz.ascx" TagPrefix="uc1" TagName="Quiz" %>

<div id="pnlIntro" runat="server">
    Please click here to verify your identity.
    <asp:Button runat="server" OnClick="BeginQuiz_Click" text="Start Quiz"/>
</div>
<uc1:Quiz runat="server" id="Quiz" />