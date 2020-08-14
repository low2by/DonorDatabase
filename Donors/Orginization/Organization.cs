using System;

namespace Donor
{
    public class Organization
    {
        public Organization(string _accountNumber, string _lastName, string _firstName, string _email, string _phoneNumber, string _country, string _address, string _city, string _state, string _zipCode)
        {
        }

        public Organization(string _accountNumber, string _lastName, string _firstName, string _email, string _phoneNumber)
        {
        }

        public Organization(string _accountNumber, string _country, string _address, string _city, string _state, string _zipCode)
        {
        }
    }
}
