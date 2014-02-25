using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LexisNexisTestControl.Models
{
    public class QuizUser
    {
        public long LexId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string SSN { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        /// <summary>
        /// Test Subject Harriet Bbittersweet-fictitious identity
        /// </summary>
        /// <returns></returns>
        public static QuizUser GetTestUser(){
            return new QuizUser()
            {
                LexId = 159809033, 
                FirstName = "HARRIET",
                LastName = "BBITTERSWEET",
                Address = "356 GRAND FORKS RD",
                City = "MESQ",
                State = "TX",
                Zip = "75187",
                SSN = "521284963",
                Year = 1977,
                Month  = 12,
                Day = 23
            };
        }
    }
}