using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

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

        public bool IsMatchingAddress(Constituents left, Constituents right)
        {
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;
            return FormatAddress(left.GetAddress()).Equals(FormatAddress(right.GetAddress())) 
                && FormatCity(left.GetCity()).Equals(FormatCity(right.GetCity())) 
                && left.GetState().Equals(right.GetState()) 
                && left.GetZipCode().Equals(right.GetZipCode());
        }

        public bool HaveSameLastName(Constituents left, Constituents right)
        {
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;
            return left.GetLastName().Contains(right.GetLastName());
        }

        private static string FormatAddress(string address)
        {
            address = address.ToLower();
            string result = "";
            foreach(string addyItem in GetEachAddressItem(address))
            {
                result += AbbreviatedStreetNames(addyItem) + " ";
            }

            result = result.Trim();

            return FirstCharUpperCase(ref result);
        }

        private static string AbbreviatedStreetNames(string streetName)
        {
            if (streetName.Equals("DRIVE".ToLower()) || 
                streetName.Equals("DRIV".ToLower()) || streetName.Equals("DRIV.".ToLower()) || 
                streetName.Equals("DRV".ToLower()) || streetName.Equals("DRV.".ToLower()) ||
                streetName.Equals("DR".ToLower()) || streetName.Equals("DR,".ToLower()))
                streetName = "DR.".ToLower();

            if (streetName.Equals("ST".ToLower()) || streetName.Equals("STREET".ToLower()) || 
                streetName.Equals("STRT".ToLower()) || streetName.Equals("STRT.".ToLower()) || 
                streetName.Equals("STR".ToLower()) || streetName.Equals("STR.".ToLower()))
                streetName = "ST.".ToLower();

            if (streetName.Equals("LN".ToLower()) || streetName.Equals("LANE".ToLower()))
                streetName = "LN.".ToLower();

            if (streetName.Equals("S".ToLower()) || streetName.Equals("S.".ToLower()) || streetName.Equals("SO.".ToLower()))
                streetName = "South".ToLower();

            if (streetName.Equals("N".ToLower()) || streetName.Equals("N.".ToLower()))
                streetName = "North".ToLower();

            if (streetName.Equals("E".ToLower()) || streetName.Equals("E.".ToLower()))
                streetName = "East".ToLower();

            if (streetName.Equals("W".ToLower()) || streetName.Equals("W.".ToLower()))
                streetName = "West".ToLower();

            if (streetName.Equals("CIR".ToLower()) || streetName.Equals("CIR.".ToLower()) || 
                streetName.Equals("CIRC".ToLower()) || streetName.Equals("CIRC.".ToLower()) || 
                streetName.Equals("CIRCL".ToLower()) || streetName.Equals("CIRCL.".ToLower()) || 
                streetName.Equals("CRCL".ToLower()) || streetName.Equals("CRCL.".ToLower()) || 
                streetName.Equals("CRCLE".ToLower()) || streetName.Equals("CRCLE.".ToLower()) ||
                streetName.Equals("CIRCLE".ToLower()))
            {
                streetName = "Cir.".ToLower();
            }

            if (streetName.Equals("RD".ToLower()) || streetName.Equals("ROAD".ToLower()))
                streetName = "RD.".ToLower();

            if (streetName.Equals("AV".ToLower()) || streetName.Equals("AV.".ToLower()) || 
                streetName.Equals("AVEN".ToLower()) || streetName.Equals("AVEN.".ToLower()) ||
                streetName.Equals("AVENU".ToLower()) || streetName.Equals("AVENU.".ToLower()) ||
                streetName.Equals("AVN".ToLower()) || streetName.Equals("AVN.".ToLower()) || 
                streetName.Equals("AVNUE".ToLower()) || streetName.Equals("AVNUE.".ToLower()) ||
                streetName.Equals("AVENUE.".ToLower()))
            {
                streetName = "AVE.".ToLower();
            }

            if (streetName.Equals("CT".ToLower()) || streetName.Equals("COURT".ToLower()))
                streetName = "CT.".ToLower();

            if (streetName.Equals("Place".ToLower()) || streetName.Equals("PL".ToLower()))
                streetName = "PL.".ToLower();

            if (streetName.Equals("PKWY".ToLower()) || streetName.Equals("PKWY.".ToLower()) ||
                streetName.Equals("PKWYS".ToLower()) || streetName.Equals("PKWYS.".ToLower()) ||
                streetName.Equals("PARKWAYS".ToLower()))
                streetName = "PKWY.".ToLower();

            if (streetName.Equals("COVE".ToLower()) || streetName.Equals("COVE".ToLower()))
                streetName = "CV.".ToLower();

            return streetName;
        }

        private static IEnumerable<string> Seperte(string address)
        {

            var words = new List<string> { string.Empty };
            for (var i = 0; i < address.Length; i++)
            {
                words[words.Count - 1] += address[i];
                if (i + 1 < address.Length && char.IsLetter(address[i]) != char.IsLetter(address[i + 1]))
                {
                    words.Add(string.Empty);
                }
            }
            return words;

        }
         
        private static string FormatCity(string city)
        {
            city = city.ToLower();
            if (city.Equals("slc"))
                city = city.Replace("slc", "salt lake city");
            return FirstCharUpperCase(ref city);
        }

        private static string FirstCharUpperCase(ref string address)
        {
            string eachString = "";
            string[] addyArr = address.Split(' ');
            foreach(string strAddy in addyArr)
            {
                eachString += " " + char.ToUpper(strAddy[0]) + strAddy.Substring(1);
            }

            return eachString.Trim();
        }

        private static IEnumerable<string> GetEachAddressItem(string address)
        {
            string[] arr = address.Split(' ');
            foreach(string addressItem in arr)
            {
                //If an address has the number and name stuck together, seperate them
                char[] itemChar = addressItem.ToCharArray();
                if (itemChar.HasBothIntChar())
                {
                    foreach (string seperateVale in Seperte(addressItem))
                    {
                        yield return seperateVale;
                    }
                }
                
                yield return addressItem;
            }

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
            string addy = FormatAddress(billingAddress.CityAddress);
            return addy.Replace("\n", "").Replace("\r", "");
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

    public static class MyExtensions
    {
        public static bool HasBothIntChar(this char[] arr)
        {
            bool intItem = false;
            bool charItem = false;

            foreach(char item in arr)
            {
                if (int.TryParse(item.ToString(), out int holder))
                    intItem = true;

                if (!int.TryParse(item.ToString(), out int holder2))
                    charItem = true;
            }

            return intItem && charItem;
        }
    }
}
