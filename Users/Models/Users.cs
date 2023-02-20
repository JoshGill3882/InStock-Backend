using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWSServerlessBlogApi.Models
{
    public class Users
    {
        public string Email { get; set; }
        public string AccountStatus { get; set; }
        public int CreationDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}