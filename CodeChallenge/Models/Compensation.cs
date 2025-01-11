using System;

using System.ComponentModel.DataAnnotations;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        [Key] // force to be primary key
        public String Employee { get; set; }
        public int Salary { get; set; }
        public String effectiveDate { get; set; }
    }
}


