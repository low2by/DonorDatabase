using System;

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

        public string GetAccountNumber { get => accountNumber; private set { } }
        public string GetName { get => name; private set { } }
        public string DonationDate { set; get; }
        public string DonationAmount { set; get; }
        public string Campaign { set; get; }
        public string MiniCampaign { set; get; }
        public string Fund { set; get; }
        public string TransactionType { set; get; }
        public string TransactionMethod { set; get; }
    }
}
