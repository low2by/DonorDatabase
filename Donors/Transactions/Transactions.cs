using System;
using System.Collections.Generic;

namespace Donor
{

    public class Transaction
    {
        private string accountNumber;
        private string name;
        public Transaction(string _accountNumber, string _name, string _donationDate, string _campaign, string _miniCampaign, string _fund, string _type, string _method, string _donationAmount)
        {
            name = _name;
            accountNumber = _accountNumber;
            DonationDate = _donationDate;
            DonationAmount = _donationAmount;
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
        public string DonationDate { set; get; }
        public string DonationAmount { set; get; }
        public string Campaign { set; get; }
        public string MiniCampaign { set; get; }
        public string Fund { set; get; }
        public string TransactionType { set; get; }
        public string TransactionMethod { set; get; }
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
                donation.DonationAmount.Trim().Equals(incomingDonation.DonationAmount.Trim())
                )
            {
                return true;
            }
            return false;
        }

        public static Dictionary<string, Transaction> GetTransactionDictionary(this IEnumerable<Transaction> transaction)
        {
            Dictionary<string, Transaction> transactionDictionary = new Dictionary<string, Transaction>();

            foreach (Transaction trans in transaction)
            {
                transactionDictionary.Add(trans.GetAccountNumber(), trans);
            }

            return transactionDictionary;
        }
    }
}
