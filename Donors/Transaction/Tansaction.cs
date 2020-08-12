using System;

namespace Transaction
{
    public class Transaction
    {

        public Transaction(string _donationDate, float _donationAmount, string _campaign, string _miniCampaign, string _fund, string _transactionType, string _transactionMethod)
        {
            DonationDate = _donationDate;
            DonationAmount = _donationAmount;
            Campaign = _campaign;
            MiniCampaign = _miniCampaign;
            Fund = _fund;
            TransactionType = _transactionType;
            TransactionMethod = _transactionMethod;

        }

        public string DonationDate { get; }
        public float DonationAmount { get; }
        public string Campaign { get; }
        public string MiniCampaign { get; }
        public string Fund { get; }
        public string TransactionType { get; private set; }
        public string TransactionMethod { get; private set; }
    }
}
