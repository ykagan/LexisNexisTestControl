using LexisNexisTestControl.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace LexisNexisTestControl.Services
{
    public static class LNQuizService
    {
        /// <summary>
        /// Initialize a IDMv2 service session instance
        /// </summary>
        /// <returns></returns>
        public static IDMv2.IdentityProofingWSClient InitWSClient()
        {
            IDMv2.IdentityProofingWSClient myIDMv2Client = new IDMv2.IdentityProofingWSClient();
            myIDMv2Client.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["IDMaccount"].ToString() + "/" + ConfigurationManager.AppSettings["IDMuserid"].ToString();
            myIDMv2Client.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["IDMpassword"].ToString();
            //Note that the nonce in the header is optional
            return myIDMv2Client;
        }

        private static bool UseMockResponses
        {
            get
            {
                return Boolean.Parse(ConfigurationManager.AppSettings["UseMockResponses"]);
            }
        }

        /// <summary>
        /// Creates a IDMv2 request to be passed to the service
        /// </summary>
        /// <param name="item"></param>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public static IDMv2.IdentityProofingRequest GetRequest(object item, string transactionId)
        {
            //New profing request
            IDMv2.IdentityProofingRequest request = new IDMv2.IdentityProofingRequest()
            {
                Items = new object[] {item},
                locale = "en_US", //TODO: add to web.config
                customerReference = "my_ref", //TODO: do we need this?
                transactionID = transactionId,
                workFlow = ConfigurationManager.AppSettings["IDMworkflow"].ToString()
            };
            return request;
        }
        public static IDMv2.InstantAuthenticateResponse GetQuizQuestions(QuizUser user, string transactionId)
        {
            //Generate an InputSubject model to get quiz for
            IDMv2.InputSubject subject = GetSubject(user);

            var ws = InitWSClient();
            var request = GetRequest(subject, System.Guid.NewGuid().ToString());
            IDMv2.ProductResponse[] productResponses;
            //invoke service
            if (!UseMockResponses)
            {
                productResponses = ws.invokeIdentityService(request).productResponse;
            }
            else
            {

                #region OfflineResponse
                IDMv2.IdentityProofingResponse response;
                XmlSerializer xml = new XmlSerializer(typeof(IDMv2.IdentityProofingResponse), "http://ns.lexisnexis.com/identity-proofing/1.0");
                var sampleFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "App_Data", "sampleResponse.xml");
                using (Stream stream = new FileStream(sampleFilePath, FileMode.Open))
                {
                    response = (IDMv2.IdentityProofingResponse)xml.Deserialize(stream);
                }
                productResponses = response.productResponse;
                #endregion
            }

            if (productResponses == null || productResponses.Length == 0)
            {
                throw new Exception("No matching users found");
            }
            var authResponse = productResponses.FirstOrDefault() as IDMv2.InstantAuthenticateResponse;
            if(authResponse == null)
            {
                throw new Exception("Wrong product type or data");
            }
            return authResponse;
        }

        private static IDMv2.InputSubject GetSubject(QuizUser user)
        {
            IDMv2.InputSubject InputSubject;

            IDMv2.InputPerson vPerson = new IDMv2.InputPerson()
            {
                LexID = user.LexId.ToString(),//if LexID is provided all other inputs are ignored
                ssn = user.SSN,
                name = new IDMv2.InputName()
                {
                    first = user.FirstName,
                    last = user.LastName
                },
                address = new IDMv2.InputAddress[1]{
                    new IDMv2.InputAddress()//assign primary address to the array at index 0 
                    {
                        addressPurpose = IDMv2.AddressPurposeType.PRIMARY_RESIDENCE,
                        city = user.City,
                        addressLine1 = user.Address,
                        stateCode = user.State,
                        zip5 = user.Zip
                    }
                },
                dateOfBirth = new IDMv2.Date()
                {
                    Year = user.Year.ToString(),
                    Month = user.Month.ToString(),
                    Day = user.Day.ToString()
                },
            };

            InputSubject = new IDMv2.InputSubject(){
                    Item = vPerson
            };
            return InputSubject;
        }

        public class QuizResponse
        {
            public string Score { get; set; }
            public IDMv2.InstantAuthenticateProductStatus Status { get; set; }
        }
        public static QuizResponse GetProductResponse(IDMv2.IdentityScoreRequest request, string transactionId)
        {
            IDMv2.IdentityProofingWSClient client = InitWSClient();

            var proofingRequest = GetRequest(request, transactionId);
            IDMv2.InstantAuthenticateResponse response;
            if(!UseMockResponses)
            {
                response = client.invokeIdentityService(proofingRequest).productResponse.First() as IDMv2.InstantAuthenticateResponse;//TODO: can there be more than one response?          
            }
            else
            {
                #region Mock Response
                IDMv2.IdentityProofingResponse proofingResponse;
                XmlSerializer xml = new XmlSerializer(typeof(IDMv2.IdentityProofingResponse), "http://ns.lexisnexis.com/identity-proofing/1.0");
                var sampleFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "App_Data", "sampleResponsePass.xml");
                using (Stream stream = new FileStream(sampleFilePath, FileMode.Open))
                {
                    proofingResponse = (IDMv2.IdentityProofingResponse)xml.Deserialize(stream);
                }
                response = proofingResponse.productResponse.First() as IDMv2.InstantAuthenticateResponse;
                #endregion
            }
           
            if(response != null)
            {
                return new QuizResponse
                {
                    Score = response.quizScore != null ? response.quizScore.score :"0",
                    Status =  response.status
                    //TODO: alternative quiz option
                };
            }
            return null;
            
        }

    }
}