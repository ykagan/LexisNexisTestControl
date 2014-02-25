<%@ Page Title="" Language="C#" MasterPageFile="~/Lexis.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="LexisNexisTestControl.WebForm1" %>
<%@ Register Src="~/UserControls/QuizIntro.ascx" TagPrefix="uc1" TagName="QuizIntro" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:QuizIntro runat="server" ID="QuizIntro" />
</asp:Content>
