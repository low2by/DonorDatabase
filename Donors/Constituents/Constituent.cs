using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donors
{
    /// <summary>
    /// This is a constituent repr
    /// </summary>
    public class Constituent
    {
        public string amount;
        public string FirstName;
        public string LastName;
        public string Email;
        public string Phone;
        public ContactInfo info = new ContactInfo();
        public BillingAddress addy = new BillingAddress();

        public Constituent(ContactInfo _info, BillingAddress _addy, string amount)
        {

        }
    }

    public struct ContactInfo
    {
        public string FirstName;
        public string LastName;
        public string Email;
        public string Phone;
    }

    public struct BillingAddress
    {
        public string country;
        public string address;
        public string city;
        public string state;
        public string zipCode;
    }
}
