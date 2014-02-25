<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuizInit.ascx.cs" Inherits="LexisNexisTestControl.UserControls.QuizInit" %>

<div>

    <p>
        <strong>Sample App:  Generate a Quiz </strong>
        <br />
        This sample application is provided "as-is"; it is provided to provide an example application which demonstrates a specific example of connecting to and consuming the LN Identity Proofing service using .NET/C#.  It is designed to use a workflow which incorporate an IV and IA response with a single quiz (no follow-up quiz). 
        <br />
        No additional support for this code sample is provided or available.
    <br />
        Default values for account/username and password are set in web.config
    <br />
        Works with Version2 of the WSDL at https://staging.identitymanagement.lexisnexis.com/identity-proofing/services/identityProofingServiceWS/v2?WSDL
    </p>

    <!--- Show the primary input parameters required by the security protocol, the app, and the subject --->
    <table>
        <tr>
            <td><strong>TRANSACTION INPUTS</strong></td>
            <td><i>(Enter values below)</i></td>
        </tr>
        <tr>
            <td>ACCT/USERNAME:</td>
            <td>
                <asp:TextBox ID="TextBox1" runat="server" Width="240"></asp:TextBox></td>
        </tr>
        <tr>
            <td>PASSWORD:</td>
            <td>
                <asp:TextBox ID="TextBox2" runat="server" Width="240"></asp:TextBox></td>
        </tr>
        <tr>
            <td>TRANSACTION_ID (client generated):</td>
            <td>
                <asp:TextBox ID="TextBox3" runat="server" Width="240"></asp:TextBox></td>
        </tr>
        <tr>
            <td>WORKFLOW NAME:</td>
            <td>
                <asp:TextBox ID="TextBox4" runat="server" Width="240"></asp:TextBox></td>
        </tr>
        <tr>
            <td>LANGUAGE:</td>
            <td>
                <asp:TextBox ID="TextBox5" runat="server" Width="240"></asp:TextBox></td>
        </tr>
        <tr>
            <td>CUSTOMER REFERENCE:</td>
            <td>
                <asp:TextBox ID="TextBox6" runat="server" Width="240"></asp:TextBox></td>
        </tr>
        <tr>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td colspan="2"><strong>SUBJECT INPUTS</strong></td>
        </tr>
        <tr>
            <td>LEXID:</td>
            <td>
                <asp:TextBox ID="TextBox7" runat="server" Width="240"></asp:TextBox></td>
        </tr>
        <tr>
            <td>FIRSTNAME:</td>
            <td>
                <asp:TextBox ID="TextBox8" runat="server" Width="240"></asp:TextBox></td>
        </tr>
        <tr>
            <td>LASTNAME:</td>
            <td>
                <asp:TextBox ID="TextBox9" runat="server" Width="240"></asp:TextBox></td>
        </tr>
        <tr>
            <td>ADDRESS:</td>
            <td>
                <asp:TextBox ID="TextBox10" runat="server" Width="240"></asp:TextBox></td>
        </tr>
        <tr>
            <td>CITY:</td>
            <td>
                <asp:TextBox ID="TextBox11" runat="server" Width="240"></asp:TextBox></td>
        </tr>
        <tr>
            <td>STATE:</td>
            <td>
                <asp:TextBox ID="TextBox12" runat="server" Width="240"></asp:TextBox></td>
        </tr>
        <tr>
            <td>ZIPCODE:</td>
            <td>
                <asp:TextBox ID="TextBox13" runat="server" Width="240"></asp:TextBox></td>
        </tr>
        <tr>
            <td>SSN:</td>
            <td>
                <asp:TextBox ID="TextBox14" runat="server" Width="240"></asp:TextBox></td>
        </tr>
        <tr>
            <td>DOB YEAR:</td>
            <td>
                <asp:TextBox ID="TextBox15" runat="server" Width="240"></asp:TextBox></td>
        </tr>
        <tr>
            <td>DOB MONTH:</td>
            <td>
                <asp:TextBox ID="TextBox16" runat="server" Width="240"></asp:TextBox></td>
        </tr>
        <tr>
            <td>DOB DAY:</td>
            <td>
                <asp:TextBox ID="TextBox17" runat="server" Width="240"></asp:TextBox></td>
        </tr>
        <tr>
            <td colspan="2">
                <hr />
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="Button1" runat="server" Text="SUBMIT" OnClick="Button1_Click" />
                &nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Start Over" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <hr />
            </td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField ID="HFnumQnumR" runat="server" />
            </td>
        </tr>
    </table>


    <asp:Label ID="Label1" runat="server" Font-Bold="True"></asp:Label>
    <asp:PlaceHolder ID="phTest" runat="server"></asp:PlaceHolder>

    <asp:Panel ID="PanelIVResults" runat="server" Visible="false">
        <asp:Table ID="TableIVResults" runat="server"></asp:Table>
        <br />
    </asp:Panel>

    <asp:Panel ID="PanelIAResults" runat="server" Visible="false">
        <asp:PlaceHolder ID="phIAResults" runat="server"></asp:PlaceHolder>
        <asp:Table ID="TableIAResults" runat="server"></asp:Table>
        <asp:Button ID="but_SubmitAnswers" runat="server" Text="Submit Answers"
            Visible="false" OnClick="but_SubmitAnswers_Click" />
        <br />
    </asp:Panel>

    <asp:Panel ID="PanelScoreResults" runat="server" Visible="false">
        <asp:Table ID="TableScoreResults" runat="server"></asp:Table>
        <br />
    </asp:Panel>


    <asp:Panel ID="Panel1" runat="server" Visible="false">
        <table>
            <tr>
                <td colspan="2">INPUT XML</td>
            </tr>
            <tr>
                <td>
                    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                </td>
            </tr>
        </table>
    </asp:Panel>


</div>
