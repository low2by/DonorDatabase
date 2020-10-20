using System;
using System.Collections.Generic;
using System.Linq;

namespace Donor
{

    public class Transaction
    {
        private string accountNumber;
        private string name;
        public Transaction(string _accountNumber, string _name, string _donationDate, string _campaign, string _miniCampaign, string _fund, string _type, string _method, string _donationAmount, string _InKindMarketValue, string _InKindDescr)
        {
            name = _name;
            accountNumber = _accountNumber;
            DonationDate = _donationDate;
            if(Double.TryParse(_donationAmount, out double donationAmount))
            {
                DonationAmount = donationAmount;
            }
            else
            {
                DonationAmount = 0;
            }
            Campaign = _campaign;
            MiniCampaign = _miniCampaign;
            Fund = _fund;
            TransactionType = _type;
            TransactionMethod = _method;
            if (Double.TryParse(_InKindMarketValue, out double inKindAmount))
            {
                InKindMarketValue = inKindAmount;
            }
            else
            {
                InKindMarketValue = 0;
            }
            InKindDescr = _InKindDescr;

        }

        public Transaction(string _accountNumber, string _name, string _donationDate, string _campaign, string _miniCampaign, string _fund, string _type, string _method, string _donationAmount)
        {
            name = _name;
            accountNumber = _accountNumber;
            DonationDate = _donationDate;
            if (Double.TryParse(_donationAmount, out double donationAmount))
            {
                DonationAmount = donationAmount;
            }
            else
            {
                DonationAmount = 0;
            }
            Campaign = _campaign;
            MiniCampaign = _miniCampaign;
            Fund = _fund;
            TransactionType = _type;
            TransactionMethod = _method;

        }

        //public int GetAccountNumber { get => accountNumber; private set { } }
        public string GetAccountNumber()
        {
            return accountNumber;
        }
        public string GetName { get => name.Trim(); private set { } }
        public string DonationDate { private set; get; }
        public double DonationAmount { set; get; }
        public string Campaign { private set; get; }
        public string MiniCampaign { private set; get; }
        public string Fund { private set; get; }
        public string TransactionType { private set; get; }
        public string TransactionMethod { private set; get; }
        public double InKindMarketValue { get; set; }
        public string InKindDescr { get; private set; }
        public double Amount
        {
            private set
            {
                if (DonationAmount == 0)
                    Amount = InKindMarketValue;
                Amount = DonationAmount;
            }
            get
            {
                if (DonationAmount == 0)
                    return InKindMarketValue;
                return DonationAmount;
            }
        }
    }

    public static class MyExtensions
    {
        public static bool TransactionsMatch(this Transaction donation, Transaction incomingDonation)
        {
            if (donation.DonationDate.Trim().Equals(incomingDonation.DonationDate.Trim()) && 
                donation.Campaign.Trim().Equals(incomingDonation.Campaign.Trim()) &&
                donation.MiniCampaign.Trim().Equals(incomingDonation.MiniCampaign.Trim()) &&
                donation.Fund.Trim().Equals(incomingDonation.Fund.Trim()) &&
                donation.TransactionType.Trim().Equals(incomingDonation.TransactionType.Trim()) &&
                donation.TransactionMethod.Trim().Equals(incomingDonation.TransactionMethod.Trim()) &&
                donation.Amount == incomingDonation.Amount
                )
            {
                return true;
            }
            return false;
        }

    }
}
