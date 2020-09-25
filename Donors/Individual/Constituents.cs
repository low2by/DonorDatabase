using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Donor
{
    public class Constituents
    {
        private List<Transaction> transactions;
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
            this.transactions = new List<Transaction>();
        }

        public Constituents(string _accountNumber, string _name, string _address, string _city, string _state, string _zipCode, string _phoneNumber, string _email, string _type)
        {
            this.accountNumber = _accountNumber;
            this.typeOfConstituent = _type;

            this.contactInformation = new ContactInformation(_name, _email, _phoneNumber);
            this.billingAddress = new BillingAddress(_address, _city, _state, _zipCode);
            this.transactions = new List<Transaction>();
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

            if (streetName.Equals("S".ToLower()) || streetName.Equals("S.".ToLower()) || streetName.Equals("SO.".ToLower()) || streetName.Equals("SO".ToLower()))
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
                streetName.Equals("AVENUE.".ToLower()) || streetName.Equals("AVE".ToLower()))
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

            if (streetName.Equals("BLVD".ToLower()) || streetName.Equals("BOUL".ToLower()) || streetName.Equals("BOULEVARD".ToLower()) || streetName.Equals("BOULV".ToLower()))
                streetName = "BLVD.".ToLower();

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
            city = city.ToLower().Trim();
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
                else
                {
                    yield return addressItem;
                }
                
            }

        }

        public void AddTransaction(Transaction incomingTran)
        {
            transactions.Add(incomingTran);
        }

        public List<Transaction> GetTransactions()
        {
            return transactions;
        }

        public string GetAccountNumber()
        {
            return accountNumber;
        }

        public string GetName()
        {
            return contactInformation.LastName() + ", " + contactInformation.FirstName();
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
            return contactInformation.LastName();
        }

        public string GetFirstName()
        {
            return contactInformation.FirstName();
        }

        public string GetEmail()
        {
            return contactInformation.Email();
        }

        public string GetPhoneNumber()
        {
            return contactInformation.PhoneNumber();
        }

    }



    public struct ContactInformation
    {
        string name;
        string lastName;
        string firstName;
        string email;
        string phoneNumber;
        public ContactInformation(string _name, string _lastName, string _firstName, string _email, string _phoneNumber)
        {
            name = _name;
            lastName = _lastName;
            firstName = _firstName;
            email = _email;
            phoneNumber = _phoneNumber;
        }

        public ContactInformation(string _name, string _email, string _phoneNumber)
        {
            name = _name;
            email = _email;
            phoneNumber = _phoneNumber;

            lastName = "";
            firstName = "";
        }

        public string Name()
        {
            return name;
        }
        public string LastName() 
        {
            return lastName;
        }
        public string FirstName()
        {
            return firstName;
        }
        public string Email()
        {
            return email;
        }
        public string PhoneNumber(){
            return phoneNumber; 
        }
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

        public static Dictionary<string, IEnumerable<Constituents>> HaveSameLastName<T>(this IEnumerable<Constituents> constituents)
        {
            Dictionary<string, IEnumerable<Constituents>> matchingLastnames = new Dictionary<string, IEnumerable<Constituents>>();
            foreach (Constituents person in constituents)
            {
                List<Constituents> listOfMathcingLastName = new List<Constituents>();

                foreach (Constituents listConstituents in constituents)
                {
                    if (person.GetAccountNumber().Equals(listConstituents.GetAccountNumber()))
                        continue;

                    if (person.GetLastName().Contains(listConstituents.GetLastName()))
                        listOfMathcingLastName.Add(listConstituents);
                }

                matchingLastnames.Add(person.GetAccountNumber(), listOfMathcingLastName);

            }

            return matchingLastnames;
        }

        public static Dictionary<string, IEnumerable<Constituents>> HaveSameAddress(this IEnumerable<Constituents> constituents)
        {
            Dictionary<string, IEnumerable<Constituents>> matchingLastnames = new Dictionary<string, IEnumerable<Constituents>>();
            foreach (Constituents person in constituents)
            {
                List<Constituents> listOfMathcingLastName = new List<Constituents>();

                foreach (Constituents listConstituents in constituents)
                {
                    if (person.GetAccountNumber().Equals(listConstituents.GetAccountNumber()))
                        continue;

                    if (person.GetAddress().Equals(listConstituents.GetAddress()))
                        listOfMathcingLastName.Add(listConstituents);
                }

                matchingLastnames.Add(person.GetAccountNumber(), listOfMathcingLastName);

            }

            return matchingLastnames;
        }

        public static Dictionary<string, Constituents> GetConstituentDictionary(this IEnumerable<Constituents> constituents)
        {
            Dictionary<string, Constituents> constituentsDictionary = new Dictionary<string, Constituents>();

            foreach (Constituents person in constituents)
            {
                constituentsDictionary.Add(person.GetAccountNumber(), person);
            }

            return constituentsDictionary;
        }

        public static Dictionary<string, Constituents> AddTransaction(this IEnumerable<Constituents> constituents, IEnumerable<Transaction>  donation)
        {
            Dictionary<string, Constituents> cons = constituents.GetConstituentDictionary();
            List<Transaction> listTran;

            foreach(Transaction trans in donation)
            {
                if (cons.ContainsKey(trans.GetAccountNumber()))
                {
                    listTran = cons[trans.GetAccountNumber()].GetTransactions();

                    foreach(Transaction consTran in listTran)
                    {
                        if (!consTran.TransactionsMatch(trans))
                            cons[trans.GetAccountNumber()].AddTransaction(trans);
                    }
                }
            }

            return cons;
        }
    }
}
