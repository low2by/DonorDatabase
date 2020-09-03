using System;
using System.Collections.Generic;

namespace Donor
{
    public class Individual
    {
        private List<Transaction> transactions;
        private ContactInformation contactInformation;
        private BillingAddress billingAddress;
        private string accountNumber;


        public Individual(string _accountNumber, string _name, string _lastName, string _firstName, string _address, string _city, string _state, string _zipCode, string _phoneNumber, string _email, string _type)
        {
            this.accountNumber = _accountNumber;
            this.contactInformation = new ContactInformation(_name, _lastName, _firstName, _email, _phoneNumber);
            this.billingAddress = new BillingAddress( _address, _city, _state, _zipCode);
            this.transactions = new List<Transaction>();
        }

        public Individual(string _accountNumber, string _name, string _date, string campaign, string _fund, string _type, string _method, string _amount)
        {
            this.accountNumber = _accountNumber;
            this.transactions = new List<Transaction>();
        }

        public void AddNewTransaction(string _donationDate, string _campaign, string _miniCampaign, string _fund, string _type, string _method, float _donationAmount)
        {
            Transaction newTransaction = new Transaction(_donationDate, _campaign, _miniCampaign, _fund, _type, _method, _donationAmount);
            this.transactions.Add(newTransaction);
        }

    }

    public struct Transaction
    {
        public Transaction(string _donationDate, string _campaign, string _miniCampaign, string _fund, string _type, string _method, float _donationAmount)
        {
            DonationDate = _donationDate;
            DonationAmount = _donationAmount;
            Campaign = _campaign;
            MiniCampaign = _miniCampaign;
            Fund = _fund;
            TransactionType = _type;
            TransactionMethod = _method;

        }

        public string DonationDate { set;  get; }
        public float DonationAmount { set;  get; }
        public string Campaign { set;  get; }
        public string MiniCampaign { set; get; }
        public string Fund { set; get; }
        public string TransactionType { set; get; }
        public string TransactionMethod { set; get; }
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
}
