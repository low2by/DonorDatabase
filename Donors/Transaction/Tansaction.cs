using System;

namespace Donor
{
    public class Transaction
    {

        public Transaction(string _donationDate, string _campaign, string _miniCampaign, string _fund,  string _type, string _method, float _donationAmount)
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
}
