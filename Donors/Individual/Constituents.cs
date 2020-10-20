using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
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

        public bool IsMatchingAddress(ref Constituents left, ref Constituents right)
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
                if(strAddy.Trim().Length > 0)
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
            return contactInformation.Name();
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
            return billingAddress.City;
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
        /// <summary>
        /// Determines if a char array has both ints and characters
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
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

        public static Dictionary<string, Dictionary<string, Constituents>> HaveSameLastName<T>(this IEnumerable<Constituents> constituents)
        {
            //the return variable
            Dictionary<string, Dictionary<string, Constituents>> matchingLastName = new Dictionary<string, Dictionary<string, Constituents>>();

            //go through every constituent and check the address agaisnt the other constituents
            foreach (Constituents currentCon in constituents)
            {
                //if the address of the currentCon is empty, continue
                if (currentCon.GetLastName().Trim().Length == 0)
                    continue;

                //create the dictionary for the constituents to be added in
                Dictionary<string, Constituents> sameAddy = new Dictionary<string, Constituents>();

                //add the currentCon  onto the sameAddy dictionary
                sameAddy.Add(currentCon.GetAccountNumber(), currentCon);

                //the remaining constituents that we compare the current constituent with. 
                foreach (Constituents compareWithCon in constituents)
                {
                    //if the currentCon is the compareWithCon continue
                    if (currentCon.GetAccountNumber().Equals(compareWithCon.GetAccountNumber()))
                        continue;

                    //if the compareWithCon doesnt have an address, continue
                    if (compareWithCon.GetLastName().Trim().Length == 0)
                        continue;

                    //if the compareWithCon has the same address, add it to the matchingAddress dictionary
                    if (currentCon.GetLastName().Equals(compareWithCon.GetLastName()))
                        sameAddy.Add(currentCon.GetAccountNumber(), currentCon);
                }

                //add the currentCon into the matchingAddress dictionary
                matchingLastName.Add(currentCon.GetAccountNumber(), sameAddy);

            }

            return matchingLastName;
        }

        public static Dictionary<string, Dictionary<string, Constituents>> HaveSameAddress(this List<Constituents> constituents)
        {
            //the return variable
            Dictionary<string, Dictionary<string, Constituents>> matchingAddress = new Dictionary<string, Dictionary<string, Constituents>>();

            //go through every constituent and check the address agaisnt the other constituents
            foreach (Constituents currentCon in constituents)
            {
                //if the address of the currentCon is empty, continue
                if (currentCon.GetAddress().Trim().Length == 0)
                    continue;

                //create the dictionary for the constituents to be added in
                Dictionary<string, Constituents> sameAddy = new Dictionary<string, Constituents>();

                //add the currentCon  onto the sameAddy dictionary
                sameAddy.Add(currentCon.GetAccountNumber(), currentCon);

                //the remaining constituents that we compare the current constituent with. 
                foreach (Constituents compareWithCon in constituents)
                {
                    //if the currentCon is the compareWithCon continue
                    if (currentCon.GetAccountNumber().Equals(compareWithCon.GetAccountNumber()))
                        continue;

                    //if the compareWithCon doesnt have an address, continue
                    if (compareWithCon.GetAddress().Trim().Length == 0)
                        continue;

                    //if the compareWithCon has the same address, add it to the matchingAddress dictionary
                    if (currentCon.GetAddress().Equals(compareWithCon.GetAddress()))
                        sameAddy.Add(currentCon.GetAccountNumber(), currentCon);
                }

                //add the currentCon into the matchingAddress dictionary
                matchingAddress.Add(currentCon.GetAccountNumber(), sameAddy);

            }

            return matchingAddress;
        }

        public static Dictionary<string, Constituents> GetConstituentDictionary(this List<Constituents> constituents)
        {
            return constituents.ToDictionary(c => c.GetAccountNumber());
        }

        /// <summary>
        /// Adds a transaction corresponding to its constituents
        /// </summary>
        /// <param name="constituents"></param>
        /// <param name="donation"></param>
        /// <returns></returns>
        public static Dictionary<string, Constituents> AddTransaction(this List<Constituents> constituents, ref List<Transaction>  donation)
        {
            Dictionary<string, Constituents> cons = constituents.GetConstituentDictionary();

            foreach(Transaction transactions in donation)
            {
                if (cons.ContainsKey(transactions.GetAccountNumber()))
                {
                    cons[transactions.GetAccountNumber()].AddTransaction(transactions);
                }
            }

            return cons;
        }

        /// <summary>
        /// This method 
        /// </summary>
        /// <param name="constituents"></param>
        /// <param name="transactions"></param>
        /// <returns></returns>
        public static Dictionary<string, Constituents> GetTopConstituents(this List<Constituents> constituents, ref List<Transaction> transactions)
        {
            List<Constituents> listCons = new List<Constituents>();
            Dictionary<string, Constituents> cons = constituents.GetConstituentDictionary();

            foreach (Transaction trans in transactions)
            {
                if (cons.ContainsKey(trans.GetAccountNumber()))
                {
                    listCons.Add(cons[trans.GetAccountNumber()]);
                }
            }
            
            return listCons.AddTransaction(ref transactions);
        }

        /// <summary>
        /// Finds any contituents with the specified lookUp string. Returns a constituents that contains the lookUp string in the constituents name and email with a specified date
        /// </summary>
        /// <param name="constituents">List of constituents</param>
        /// <param name="transaction">List of Transactions</param>
        /// <param name="lookUp">Used to look up the constituents that contain the lookUp string</param>
        /// <param name="date">Used to return a specified date of the transactions</param>
        /// <returns>Returns a dictionary of constituents that contain a specified date and string</returns>
        public static Dictionary<string, Constituents> GetConstituentsWithNameDate(this List<Constituents> constituents, ref List<Transaction> transaction, ref string lookUp, ref int date)
        {
            //Console.WriteLine("Getting Constitunets with WoodBury names and email");
            List<Constituents> conList = new List<Constituents>();
            List<Transaction> transList = new List<Transaction>();

            string[] dateArray;

            foreach (Constituents cons in constituents)
            {
                if (cons.GetName().ToLower().Contains(lookUp) || cons.GetLastName().ToLower().Contains(lookUp)
                    || cons.GetFirstName().ToLower().Contains(lookUp) || cons.GetEmail().ToLower().Contains(lookUp))
                {

                    conList.Add(cons);
                }
            }

            foreach (Transaction trans in transaction)
            {
                dateArray = trans.DonationDate.Split('/');

                if (int.TryParse(dateArray[2], out int year) && year > date)
                {
                    transList.Add(trans);
                }
            }

            return conList.AddTransaction(ref transList);


        }

        /// <summary>
        /// This method orders the constituents from highest to lowest donations
        /// </summary>
        /// <param name="constituents"></param>
        /// <param name="transaction"></param>
        /// <returns>Returns a Dictionary of constituents with only one transaction</returns>
        public static Dictionary<string, Constituents> DescendingOrderDonations(this List<Constituents> constituents, ref List<Transaction> transaction)
        {
            List<Transaction> orderedAmount = transaction.OrderByDescending(a => a.Amount).ToList();
            Dictionary<string, Transaction> orderedAmountDic = new Dictionary<string, Transaction>();

            foreach (Transaction trans in orderedAmount)
            {
                if (!orderedAmountDic.ContainsKey(trans.GetAccountNumber()))
                {
                    orderedAmountDic.Add(trans.GetAccountNumber(), trans);
                }
            }

            List<Transaction> orderedAmountList = new List<Transaction>(orderedAmountDic.Values.ToList());

            return constituents.GetTopConstituents(ref orderedAmountList);
        }
    }
}
