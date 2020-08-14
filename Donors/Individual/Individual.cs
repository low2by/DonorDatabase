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


        public Individual(string _accountNumber, string _lastName, string _firstName, string _address, string _city, string _state, string _zipCode, string _phoneNumber, string _email)
        {
            this.accountNumber = _accountNumber;
            this.contactInformation = new ContactInformation(_lastName, _firstName, _email, _phoneNumber);
            this.billingAddress = new BillingAddress( _address, _city, _state, _zipCode);
            this.transactions = new List<Transaction>();
        }

        public Individual(string _accountNumber, string _lastName, string _firstName, string _email, string _phoneNumber)
        {
            this.accountNumber = _accountNumber;
            this.contactInformation = new ContactInformation(_lastName, _firstName, _email, _phoneNumber);
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

        public string DonationDate { get; }
        public float DonationAmount { get; }
        public string Campaign { get; }
        public string MiniCampaign { get; }
        public string Fund { get; }
        public string TransactionType { get; }
        public string TransactionMethod { get; }
    } 

    public struct ContactInformation
    {
        public ContactInformation(string _lastName, string _firstName, string _email, string _phoneNumber)
        {
            LastName = _lastName;
            FirstName = _firstName;
            Email = _email;
            PhoneNumber = _phoneNumber;
        }

        public string LastName { get; }
        public string FirstName { get; }
        public string Email { get; }
        public string PhoneNumber { get; }
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

        public string CityAddress { get; }
        public string City { get; }
        public string State { get; }
        public string ZipCode { get; }
    }
}
