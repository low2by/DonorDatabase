using Donors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }

    public struct Individual
    {
        public string FirstName;
        public string LastName;
        public string Email;
        public string Phone;
    }

    public struct Orginization
    {
        public string country;
        public string address;
        public string city;
        public string state;
        public string zipCode;
    }
}
