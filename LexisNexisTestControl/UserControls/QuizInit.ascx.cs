using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;

namespace LexisNexisTestControl.UserControls
{
    public partial class QuizInit : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (this.IsPostBack)
            {
                //preserve the password value so you don't need to keep re-entering it; this is necessary since textbox2.textmode = password
                TextBox2.TextMode = TextBoxMode.SingleLine;
                TextBox2.BackColor = System.Drawing.Color.Gray;
                TextBox2.ForeColor = System.Drawing.Color.Gray;
                TextBox2.Text = this.TextBox2.Text;
            }
            else
            {
                //Prefill the form on first load
                string txID = System.Guid.NewGuid().ToString();  // generate a unique transaction ID; keep same TransactionID value for QuizScoreRequest

                TextBox1.Text = ConfigurationManager.AppSettings["IDMaccount"].ToString() + "/" + ConfigurationManager.AppSettings["IDMuserid"].ToString();
                TextBox2.TextMode = TextBoxMode.Password;
                TextBox2.Attributes.Add("value", ConfigurationManager.AppSettings["IDMpassword"].ToString());
                TextBox3.Text = txID;
                TextBox4.Text = ConfigurationManager.AppSettings["IDMworkflow"].ToString();


                TextBox5.Text = "en_US";
                TextBox6.Text = "Customer_Reference";
                TextBox7.Text = "159809033"; //Test Subject Harriet Bbittersweet-fictitious identity
                TextBox8.Text = "HARRIET";
                TextBox9.Text = "BBITTERSWEET";
                TextBox10.Text = "356 GRAND FORKS RD";
                TextBox11.Text = "MESQ";
                TextBox12.Text = "TX";
                TextBox13.Text = "75187";
                TextBox14.Text = "521284963";  //SSN
                TextBox15.Text = "1977";
                TextBox16.Text = "12";
                TextBox17.Text = "23";

            }

        }


        public void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }

        public void Button1_Click(object sender, EventArgs e)
        {
            //When form is submitted...
            //Bind to settings in web.config; set values for username and password from web.config, too
            IDMv2.IdentityProofingWSClient myIDMv2Client = new IDMv2.IdentityProofingWSClient();
            myIDMv2Client.ClientCredentials.UserName.UserName = TextBox1.Text;
            myIDMv2Client.ClientCredentials.UserName.Password = TextBox2.Text;
            //Note that the nonce in the header is optional


            //New profing request
            IDMv2.IdentityProofingRequest myIDRequest = new IDMv2.IdentityProofingRequest();


            IDMv2.InputSubject[] vInputSubject = new IDMv2.InputSubject[1];
            IDMv2.InputSubject vThisSubject = new IDMv2.InputSubject();

            IDMv2.InputPerson[] vInputPerson = new IDMv2.InputPerson[1];

            IDMv2.InputPerson vPerson = new IDMv2.InputPerson();
            vPerson.LexID = TextBox7.Text;  //assiging the lexID from the web form; if LexID is provided all other inputs are ignored
            vPerson.ssn = TextBox14.Text;

            IDMv2.InputName vSubjectName = new IDMv2.InputName();
            vSubjectName.first = TextBox8.Text;
            vSubjectName.last = TextBox9.Text;
            vPerson.name = vSubjectName;

            IDMv2.Address vAddress = new IDMv2.Address();
            vAddress.addressType = IDMv2.AddressPurposeType.PRIMARY_RESIDENCE;
            vAddress.city = TextBox11.Text;
            vAddress.streetAddress1 = TextBox10.Text;
            vAddress.state = TextBox12.Text;
            vAddress.zip5 = TextBox13.Text;

            IDMv2.InputAddress vInputAddress = new IDMv2.InputAddress();

            vInputAddress.addressPurpose = IDMv2.AddressPurposeType.PRIMARY_RESIDENCE;
            vInputAddress.addressLine1 = TextBox10.Text;
            vInputAddress.city = TextBox11.Text;
            vInputAddress.stateCode = TextBox12.Text;
            vInputAddress.zip5 = TextBox13.Text;

            IDMv2.InputAddress[] vPrimaryAddress = new IDMv2.InputAddress[1];
            vPrimaryAddress[0] = vInputAddress; //assign primary address to the array at index 0

            vPerson.address = vPrimaryAddress;  //Assign the InputAddress to the Input Subject (person)

            IDMv2.Date vDOB = new IDMv2.Date();


            vInputPerson[0] = vPerson;
            vThisSubject.Item = vPerson;
            vInputSubject[0] = vThisSubject;
            myIDRequest.Items = vInputSubject;

            //Required parameters
            myIDRequest.locale = TextBox5.Text;
            myIDRequest.customerReference = TextBox6.Text;
            myIDRequest.transactionID = TextBox3.Text;
            myIDRequest.workFlow = TextBox4.Text;

            //invoke the WebSvc
            IDMv2.IdentityProofingResponse myIDResponse = new IDMv2.IdentityProofingResponse();
            XmlSerializer xml = new XmlSerializer(typeof(IDMv2.IdentityProofingResponse),"http://ns.lexisnexis.com/identity-proofing/1.0");
            var sampleFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "App_Data", "sampleResponse.xml");
            using (Stream stream = new FileStream(sampleFilePath, FileMode.Open))
            {
                 myIDResponse = (IDMv2.IdentityProofingResponse)xml.Deserialize(stream);
            }
            //myIDResponse = myIDMv2Client.invokeIdentityService(myIDRequest);

            IDMv2.ProductResponse[] vProductResponse = new IDMv2.ProductResponse[1];
            vProductResponse = myIDResponse.productResponse;


            //Enable the panel box to show the behind-the-scenes
            Panel1.Visible = true;

            //For reference, show the input XML
            XmlSerializer ser2 = new XmlSerializer(myIDRequest.GetType());
            System.Text.StringBuilder sb2 = new System.Text.StringBuilder();
            System.IO.StringWriter writer2 = new System.IO.StringWriter(sb2);
            ser2.Serialize(writer2, myIDRequest); 	// Convert to an XML string 
            string xmlString2 = sb2.ToString();

            TextBox myTb3 = new TextBox();
            PlaceHolder1.Controls.Add(myTb3);
            myTb3.Width = 800;
            myTb3.Height = 200;
            myTb3.TextMode = TextBoxMode.MultiLine;
            myTb3.Text = xmlString2;


            //Handle the productResponse
            int rowsCount = myIDResponse.productResponse.Length;

            for (int i = 0; i < rowsCount; i++)
            {

                string[] vProdType = myIDResponse.productResponse[i].GetType().ToString().Split('.');

                TextBox myTb = new TextBox();
                PlaceHolder1.Controls.Add(myTb);
                myTb.Width = 800;
                string vThisProdType = vProdType[vProdType.Length - 1];
                myTb.Text = i.ToString() + ":" + vThisProdType;  //show the product response type

                //Take the resulting object and convert it to an XML string for display
                XmlSerializer ser = new XmlSerializer(vProductResponse[i].GetType());
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                System.IO.StringWriter writer = new System.IO.StringWriter(sb);
                ser.Serialize(writer, vProductResponse[i]); 	// Convert to an XML string 
                string xmlString = sb.ToString();

                TextBox myTb2 = new TextBox();
                PlaceHolder1.Controls.Add(myTb2);
                myTb2.Width = 800;
                myTb2.Height = 300;
                myTb2.TextMode = TextBoxMode.MultiLine;
                myTb2.Text = xmlString;

            }


            // Build the UI controls based on product response
            for (int j = 0; j < rowsCount; j++)
            {

                string[] vCheckProdType = myIDResponse.productResponse[j].GetType().ToString().Split('.');
                string vThisCheckProdType = vCheckProdType[vCheckProdType.Length - 1];

                if (vThisCheckProdType == "InstantVerifyResponse")
                {
                    //If InstantVerifyResponse is returned show the appropriate results
                    IDMv2.InstantVerifyResponse vIVResponse = new IDMv2.InstantVerifyResponse();

                    vIVResponse = vProductResponse[j] as IDMv2.InstantVerifyResponse;

                    PanelIVResults.Visible = true;
                    TableRow labelRow = new TableRow();
                    TableCell labelcell = new TableCell();

                    TableIVResults.Rows.Add(labelRow);
                    labelRow.Cells.Add(labelcell);
                    //labelcell.Text = vThisCheckProdType + "<br>LexID:" + vIVResponse.LexID.ToString() + "<br>Status:" + vIVResponse.status.ToString() + "<br>SubProduct:" + vIVResponse.verificationSubProductResponse[0].subProduct.ToString() + "<hr>";
                    labelcell.Text = vThisCheckProdType + "<br>Status:" + vIVResponse.status.ToString() + "<br>SubProduct:" + vIVResponse.verificationSubProductResponse[0].subProduct.ToString() + "<hr>";


                    string subProdResults = "<br>";
                    int rowsCountSubProd = vIVResponse.verificationSubProductResponse.Length;
                    for (int k = 0; k < rowsCountSubProd; k++)
                    {
                        int rowsCountCheckGroup = vIVResponse.verificationSubProductResponse[k].checkGroup.Length;
                        for (int l = 0; l < rowsCountCheckGroup; l++)
                        {
                            int rowsCountCheck = vIVResponse.verificationSubProductResponse[k].checkGroup[l].check.Length;
                            for (int m = 0; m < rowsCountCheck; m++)
                            {
                                subProdResults = vIVResponse.verificationSubProductResponse[k].checkGroup[l].check[m].checkCode.ToString() + ":" + vIVResponse.verificationSubProductResponse[k].checkGroup[l].check[m].status.ToString();
                                TableRow newRow = new TableRow();
                                TableCell newcell = new TableCell();
                                TableIVResults.Rows.Add(newRow);
                                newRow.Cells.Add(newcell);
                                newcell.Text = subProdResults;

                            }
                        }

                    }

                }

                else if (vThisCheckProdType == "InstantAuthenticateResponse")
                {
                    //Show the quiz questions for Instant Authenticate Response
                    IDMv2.InstantAuthenticateResponse vIAResponse = new IDMv2.InstantAuthenticateResponse();

                    vIAResponse = vProductResponse[j] as IDMv2.InstantAuthenticateResponse;

                    PanelIAResults.Visible = true;
                    TableRow labelRow = new TableRow();
                    TableCell labelcell = new TableCell();

                    TableIAResults.Rows.Add(labelRow);
                    labelRow.Cells.Add(labelcell);
                    string vQuizID = vIAResponse.questions.quizId;
                    //labelcell.Text = "<hr>" + vThisCheckProdType + "LexID:" + vIAResponse.LexID.ToString() + "<br>Status:" + vIAResponse.status.ToString() + "<br>QuizID:" + vIAResponse.questions.quizId +  "<hr>";
                    labelcell.Text = "<hr>" + vThisCheckProdType + "<br>Status:" + vIAResponse.status.ToString() + "<br>QuizID:" + vIAResponse.questions.quizId + "<hr>";


                    //Add a row for each quiz question
                    int vNumQs = vIAResponse.questions.numQuestions;

                    for (int rowQ = 0; rowQ < vNumQs; rowQ++)
                    {

                        string vQText = vIAResponse.questions.question[rowQ].text;
                        TableRow labelQText = new TableRow();
                        TableCell labelQcell = new TableCell();
                        TableIAResults.Rows.Add(labelQText);
                        labelQText.Cells.Add(labelQcell);
                        labelQcell.Text = "<hr>" + vQText;

                        string vQuestionID = vIAResponse.questions.question[rowQ].questionId;
                        int vNumChoices = vIAResponse.questions.question[rowQ].choice.Length;


                        //Set up the answer choices
                        TableRow rowChoices = new TableRow();
                        TableCell cellChoices = new TableCell();

                        //Build radio button list for the answerchoices
                        RadioButtonList RBListName = new RadioButtonList();
                        RBListName.ID = "QuizResponse" + rowQ.ToString();
                        RBListName.ClientIDMode = ClientIDMode.Static;

                        for (int numChoice = 0; numChoice < vNumChoices; numChoice++)
                        {

                            ListItem vChoice = new ListItem();
                            vChoice.Text = vIAResponse.questions.question[rowQ].choice[numChoice].text;
                            vChoice.Value = vQuizID + ":" + vQuestionID + ":" + vIAResponse.questions.question[rowQ].choice[numChoice].choiceId;
                            RBListName.Items.Add(vChoice);

                        }

                        TableIAResults.Rows.Add(rowChoices);
                        rowChoices.Cells.Add(cellChoices);
                        cellChoices.Controls.Add(RBListName);

                        HFnumQnumR.Value = vNumQs.ToString() + ":" + vNumChoices.ToString();

                    }

                    //Add a horizontal line after the last row
                    TableRow lastRow = new TableRow();
                    TableCell lastCell = new TableCell();
                    TableIAResults.Rows.Add(lastRow);
                    lastRow.Cells.Add(lastCell);
                    lastCell.Text = "<hr>";


                    but_SubmitAnswers.Visible = true;


                }


            }

        }

        public void but_SubmitAnswers_Click(object sender, EventArgs e)
        {
            //Submit the quiz for scoring
            string seeMe = "No.Questions:No.Responses = " + HFnumQnumR.Value.ToString();
            Label1.Text = seeMe;

            //Check the number of questions and responses; you should not permit the user to proceed until all questions are answered
            if (HFnumQnumR.Value != null)
            {

                //Show results
                PanelScoreResults.Visible = true;
                PanelIVResults.Visible = false;
                PanelIAResults.Visible = false;
                but_SubmitAnswers.Visible = false;

                TableRow labelRow = new TableRow();
                TableCell labelcell = new TableCell();

                TableScoreResults.Rows.Add(labelRow);
                labelRow.Cells.Add(labelcell);
                labelcell.Text = "<b>Scoring Results</b>";


                //Handle the form inputs
                string[] vQuizParms = HFnumQnumR.Value.ToString().ToString().Split(':');
                int vNumQs = Convert.ToInt32(vQuizParms[0]);
                int vNumRs = Convert.ToInt32(vQuizParms[1]);

                //Bind to settings in web.config; set values for username and password from web.config, too
                IDMv2.IdentityProofingWSClient myIDMv2Client = new IDMv2.IdentityProofingWSClient();
                myIDMv2Client.ClientCredentials.UserName.UserName = TextBox1.Text;
                myIDMv2Client.ClientCredentials.UserName.Password = TextBox2.Text;
                //Note that the nonce in the header is optional

                //New profing request
                IDMv2.IdentityProofingRequest myIDRequest = new IDMv2.IdentityProofingRequest();

                //QuizScoring Object
                IDMv2.IdentityProofingRequest vIDProofingRequest = new IDMv2.IdentityProofingRequest();
                IDMv2.IdentityScoreRequest[] vScoreRequest = new IDMv2.IdentityScoreRequest[1];
                IDMv2.IdentityScoreRequest vThisScoreRequest = new IDMv2.IdentityScoreRequest();

                vIDProofingRequest.Items = vScoreRequest;
                vIDProofingRequest.locale = TextBox5.Text;
                vIDProofingRequest.customerReference = TextBox6.Text;
                vIDProofingRequest.transactionID = TextBox3.Text;
                vIDProofingRequest.workFlow = TextBox4.Text;

                IDMv2.IdentityQuizAnswer[] vArrAnswer = new IDMv2.IdentityQuizAnswer[vNumQs];

                //Assign the results to the scoring request
                vThisScoreRequest.answer = vArrAnswer;
                vScoreRequest[0] = vThisScoreRequest;


                //Loop through each Group and find the selected values
                for (int i = 0; i < vNumQs; i++)
                {
                    string vGroupName = "ctl00$MainContent$QuizResponse" + i.ToString();

                    if (Request.Form[vGroupName] != null)
                    {
                        string[] vQuizAnswers = Request.Form[vGroupName].ToString().ToString().Split(':');
                        vThisScoreRequest.quizId = vQuizAnswers[0].ToString();

                        IDMv2.IdentityQuizAnswer vThisAnswer = new IDMv2.IdentityQuizAnswer();

                        vThisAnswer.questionId = vQuizAnswers[1].ToString();
                        vThisAnswer.choiceId = vQuizAnswers[2].ToString();
                        vArrAnswer[i] = vThisAnswer;

                        TableRow fooRow = new TableRow();
                        TableCell foocell = new TableCell();

                        TableScoreResults.Rows.Add(fooRow);
                        fooRow.Cells.Add(foocell);
                        foocell.Text = i.ToString() + " :Selected Answers [Quiz:Question:Response]:" + Request.Form[vGroupName].ToString();

                    }
                }



                //Create a new response object; call the service again to request the score
                IDMv2.IdentityProofingResponse myIDProofingResponse = new IDMv2.IdentityProofingResponse();
                myIDProofingResponse = myIDMv2Client.invokeIdentityService(vIDProofingRequest);

                IDMv2.ProductResponse[] vScoreReqResponse = new IDMv2.ProductResponse[1];
                vScoreReqResponse = myIDProofingResponse.productResponse;
                int respCount = myIDProofingResponse.productResponse.Length;

                //Check the type of each response
                for (int k = 0; k < respCount; k++)
                {

                    string[] vCheckProdType = vScoreReqResponse[k].GetType().ToString().Split('.');
                    string vThisResponse = vCheckProdType[2];

                    TableRow foo3Row = new TableRow();
                    TableCell foo3cell = new TableCell();
                    TableScoreResults.Rows.Add(foo3Row);
                    foo3Row.Cells.Add(foo3cell);
                    foo3cell.Text = vThisResponse;

                    if (vThisResponse == "InstantAuthenticateResponse")
                    {
                        //Show the Score Results
                        IDMv2.InstantAuthenticateResponse vScoringResponse = new IDMv2.InstantAuthenticateResponse();
                        vScoringResponse = vScoreReqResponse[k] as IDMv2.InstantAuthenticateResponse;
                        string vQuizScore = vScoringResponse.quizScore.score.ToString();
                        string vQuizStatus = vScoringResponse.status.ToString();

                        TableRow foo2Row = new TableRow();
                        TableCell foo2cell = new TableCell();
                        TableScoreResults.Rows.Add(foo2Row);
                        foo2Row.Cells.Add(foo2cell);
                        foo2cell.Text = "<b>STATUS: " + vQuizStatus.ToString() + " | SCORE: " + vQuizScore.ToString() + "</b>";

                    }


                }


            }

        }
    }
}