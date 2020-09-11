using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Donor
{
    public class Constituents
    {
        private List<Donation> transactions;
        private ContactInformation contactInformation;
        private BillingAddress billingAddress;
        private string accountNumber;
        private string typeOfConstituent;


        public Constituents(string _accountNumber, string _name, string _lastName, string _firstName, string _address, string _city, string _state, string _zipCode, string _phoneNumber, string _email, string _type)
        {
            this.accountNumber = _accountNumber;
            this.typeOfConstituent = _type;
            this.contactInformation = new ContactInformation(_name, _lastName, _firstName, _email, _phoneNumber);
            this.billingAddress = new BillingAddress( _address, _city, _state, _zipCode);
            this.transactions = new List<Donation>();
        }

        public void AddTransaction(Transaction donation)
        {
            transactions.Add(new Donation(donation.DonationDate, donation.Campaign, donation.MiniCampaign, donation.Fund, donation.TransactionType, donation.TransactionMethod, donation.DonationAmount));
        }

        public string GetAccountNumber()
        {
            return accountNumber;
        }

        public string GetName()
        {
            return contactInformation.LastName + ", " + contactInformation.FirstName;
        }

        public string GetTypeOfConstituent()
        {
            return typeOfConstituent;
        }

        public string GetAddress()
        {
            return billingAddress.CityAddress;
        }

        public string GetState()
        {
            return billingAddress.State;
        }

        public string GetCity()
        {
            return billingAddress.State;
        }

        public string GetZipCode()
        {
            return billingAddress.ZipCode;
        }

        public string GetLastName()
        {
            return contactInformation.LastName;
        }

        public string GetFirstName()
        {
            return contactInformation.FirstName;
        }

        public string GetEmail()
        {
            return contactInformation.Email;
        }

        public string GetPhoneNumber()
        {
            return contactInformation.PhoneNumber;
        }

    }



    public struct ContactInformation
    {
        public ContactInformation(string _name, string _lastName, string _firstName, string _email, string _phoneNumber)
        {
            Name = _name;
            LastName = _lastName;
            FirstName = _firstName;
            Email = _email;
            PhoneNumber = _phoneNumber;
        }

        public string Name { get; set; }
        public string LastName { set; get; }
        public string FirstName { set; get; }
        public string Email { set; get; }
        public string PhoneNumber { set; get; }
    }

    public struct BillingAddress
    {
        public BillingAddress(string _address, string _city, string _state, string _zipCode)
        {
            CityAddress = _address;
            City = _city;
            State = _state;
            ZipCode = _zipCode;
        }

        public string CityAddress { set; get; }
        public string City { set; get; }
        public string State { set; get; }
        public string ZipCode { set; get; }
    }

    public struct Donation
    {
        public Donation(string _donationDate, string _campaign, string _miniCampaign, string _fund, string _type, string _method, string _donationAmount)
        {
            DonationDate = _donationDate;
            DonationAmount = _donationAmount;
            Campaign = _campaign;
            MiniCampaign = _miniCampaign;
            Fund = _fund;
            TransactionType = _type;
            TransactionMethod = _method;

        }

        public string DonationDate { set; get; }
        public string DonationAmount { set; get; }
        public string Campaign { set; get; }
        public string MiniCampaign { set; get; }
        public string Fund { set; get; }
        public string TransactionType { set; get; }
        public string TransactionMethod { set; get; }
    }
}
