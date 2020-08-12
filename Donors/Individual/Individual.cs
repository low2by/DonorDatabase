using System;
using System.Collections.Generic;

namespace Individual
{
    public class Individual
    {
        //public ContactInformation contactInfo;
        //public BillingAddress billingAddress;
        public string AccountNumber;


        public Individual(string _accountNumber, string _amount, string _lastName, string _firstName, string _donationName, string _email, string _country, string _address, string _city, string _state, string _zipCode)
        {
            //AccountNumber = _accountNumber;
            //contactInfo = new ContactInformation(_lastName, _firstName, _donationName, _email);
            //billingAddress = new BillingAddress(_country, _address, _city, _state, _zipCode);

        }

        public Individual(string _accountNumber, string _amount, string _lastName, string _firstName, string _donationName, string _email)
        {
            //AccountNumber = _accountNumber;
            //contactInfo = new ContactInformation( _lastName, _firstName, _donationName, _email);
        }
    }

}
