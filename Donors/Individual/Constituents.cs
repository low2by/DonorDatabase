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
        private string createdDate;


        public Constituents(string _accountNumber, string _name, string _lastName, string _firstName, string _address, string _city, string _state, string _zipCode, string _phoneNumber, string _email, string _type, string _createdDate)
        {
            this.accountNumber = _accountNumber;
            this.typeOfConstituent = _type;
            this.createdDate = _createdDate;
            this.contactInformation = new ContactInformation(_name, _lastName, _firstName, _email, _phoneNumber);
            this.billingAddress = new BillingAddress(_address, _city, _state, _zipCode);
            this.transactions = new List<Transaction>();
        }

        public Constituents(string _accountNumber, string _name, string _address1, string _address2, string _city, string _state, string _zipCode, string _phoneNumber, string _email)
        {
            this.accountNumber = _accountNumber;
            this.typeOfConstituent = " ";
            createdDate = " ";
            this.contactInformation = new ContactInformation(_name, _email, _phoneNumber);
            this.billingAddress = new BillingAddress(_address1, _address2, _city, _state, _zipCode);
            this.transactions = new List<Transaction>();
        }

        public double TotalDonation()
        {
            double total = 0.0;

            foreach (Transaction trans in GetTransactions())
            {
                total += trans.Amount;
            }

            return total;
        }


        public void AddTransaction(Transaction incomingTran)
        {
            transactions.Add(incomingTran);
        }

        public List<Transaction> GetTransactions()
        {
            return transactions;
        }

        public bool HasTransactions()
        {
            return transactions.Count == 0;
        }

        public string GetAccountNumber()
        {
            return accountNumber;
        }

        public string GetCreatedDate()
        {
            return createdDate;
        }

        public bool HasCreatedDate()
        {
            return createdDate.Trim().Count() == 0;
        }

        public bool HasAccountNumber()
        {
            return accountNumber.Trim().Count() == 0;
        }

        public string GetName()
        {
            return contactInformation.Name();
        }

        public bool HasName()
        {
            return contactInformation.Name().Count() == 0;
        }

        public string GetTypeOfConstituent()
        {
            return typeOfConstituent;
        }

        public bool HasTypeOfConstituent()
        {
            return typeOfConstituent.Trim().Count() == 0;
        }

        public string GetAddress()
        {
            return billingAddress.CityAddress;
        }

        public bool HasAddress()
        {
            return billingAddress.CityAddress.Trim().Count() == 0;
        }

        public string GetState()
        {
            return billingAddress.State;
        }

        public bool HasState()
        {
            return billingAddress.State.Trim().Count() == 0;
        }

        public string GetCity()
        {
            return billingAddress.City;
        }

        public bool HasCity()
        {
            return billingAddress.City.Trim().Count() == 0;
        }

        public string GetZipCode()
        {
            //some zip code have 9 number seperated by a dash
            //just get the first five number that represent the destination post office or delivery area
            string[] zipCode = billingAddress.ZipCode.Split('-');
            //return billingAddress.ZipCode;
            return zipCode[0];
        }

        public bool HasZipCode()
        {
            return billingAddress.ZipCode.Trim().Count() == 0;
        }

        public string GetLastName()
        {
            return contactInformation.LastName();
        }

        public bool HasLastName()
        {
            return contactInformation.LastName().Trim().Count() == 0;
        }

        public string GetFirstName()
        {
            return contactInformation.FirstName();
        }

        public bool HasFirstName()
        {
            return contactInformation.FirstName().Trim().Count() == 0;
        }

        public string GetEmail()
        {
            return contactInformation.Email();
        }

        public bool HasEmail()
        {
            return contactInformation.Email().Trim().Count() == 0;
        }

        public string GetPhoneNumber()
        {
            return contactInformation.PhoneNumber();
        }

        public bool HasPhoneNumber()
        {
            return contactInformation.PhoneNumber().Trim().Count() == 0;
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

        public bool IsMatchingContactInformation(ref ContactInformation left, ref ContactInformation right)
        {
            return left.Name().Trim().Equals(right.Name().Trim())
                && left.Email().Trim().Equals(right.Email().Trim())
                && left.PhoneNumber().Trim().Equals(right.PhoneNumber().Trim());
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
        public string PhoneNumber()
        {
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

        public BillingAddress(string _address1, string _address2, string _city, string _state, string _zipCode)
        {
            CityAddress = _address1 + " " + _address2;
            City = _city;
            State = _state;
            ZipCode = _zipCode;
        }

        public bool IsMatchingBillingAddress(ref BillingAddress left, ref BillingAddress right)
        {
            return left.City.Trim().Equals(right.City.Trim())
                && left.GetFormatedAddress().Trim().Equals(right.GetFormatedAddress().Trim())
                && left.State.Trim().Equals(right.State.Trim())
                && left.ZipCode.Trim().Equals(right.ZipCode.Trim());
        }

        /// <summary>
        /// Returns an address that has been formatted to have correcte appreviations, have the same spelling for standard address
        /// </summary>
        /// <returns></returns>
        public string GetFormatedAddress()
        {
            string addy = FormatAddress(CityAddress);
            return addy.Replace("\n", "").Replace("\r", "");
        }

        private static string FormatAddress(string address)
        {
            address = address.ToLower();
            string result = "";
            foreach (string addyItem in GetEachAddressItem(address))
            {

                result += AbbreviatedStreetNames(addyItem) + " ";
            }

            result = result.Trim();

            return FirstCharUpperCase(ref result);
        }

        private static string FirstCharUpperCase(ref string address)
        {
            string eachString = "";
            string[] addyArr = address.Split(' ');
            foreach (string strAddy in addyArr)
            {
                if (strAddy.Trim().Length > 0)
                    eachString += " " + char.ToUpper(strAddy[0]) + strAddy.Substring(1);
            }

            return eachString.Trim();
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

        private static IEnumerable<string> GetEachAddressItem(string address)
        {
            string[] arr = address.Split(' ');
            foreach (string addressItem in arr)
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

            foreach (char item in arr)
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

        public static Dictionary<string, Constituents> RemoveDublicates(this List<Constituents> constituents)
        {
            Dictionary<string, Constituents> consCharityBloom = new Dictionary<string, Constituents>();

            foreach (Constituents cons in constituents)
            {

            }

            return consCharityBloom;
        }

        public static Dictionary<string, Constituents> GetConstituentDictionary(this List<Constituents> constituents)
        {
            return constituents.ToDictionary(c => c.GetAccountNumber());
        }

        public static Dictionary<string, Constituents> GetConstituentDictionaryCharityproud(this List<Constituents> constituents)
        {
            Dictionary<string, Constituents> consDictionary = new Dictionary<string, Constituents>();

            foreach (Constituents con in constituents)
            {
                if (consDictionary.ContainsKey(con.GetAccountNumber()))
                    continue;

                consDictionary.Add(con.GetAccountNumber(), con);
            }

            return consDictionary;
        }

        /// <summary>
        /// Adds a transaction corresponding to its constituents
        /// </summary>
        /// <param name="constituents"></param>
        /// <param name="donation"></param>
        /// <returns></returns>
        public static Dictionary<string, Constituents> AddTransaction(this Dictionary<string, Constituents> constituents, ref List<Transaction> donation)
        {
            //Dictionary<string, Constituents> cons = constituents.GetConstituentDictionary();

            foreach (Transaction transactions in donation)
            {
                if (constituents.ContainsKey(transactions.GetAccountNumber()))
                {
                    constituents[transactions.GetAccountNumber()].AddTransaction(transactions);
                }
            }

            return constituents;
        }

        /// <summary>
        /// Combines the data from charity proud and Bloomarang
        /// </summary>
        /// <param name="constituents"></param>
        /// <returns></returns>
        public static Dictionary<string, Constituents> CombineCharityBloomarang(this Dictionary<string, Constituents> constituents)
        {
            //how to combine the data
            //not the names: people could have the same name
            Dictionary<string, Constituents> charityBloomarang = new Dictionary<string, Constituents>();

            return charityBloomarang;

        }

        /// <summary>
        /// This method removes dublicates from the bloomerang data. Dublicates are accounts with the same name but one of the accounts is missing
        /// an attribute
        /// </summary>
        /// <param name="constituents"></param>
        /// <returns></returns>
        public static Dictionary<string, Constituents> RemoveDublicates(this Dictionary<string, Constituents> constituents)
        {

            Dictionary<string, Constituents> nonDubCons = new Dictionary<string, Constituents>();
            Dictionary<string, Constituents> removedDubCons = new Dictionary<string, Constituents>();

            //make a list. this list will help us remove constituents that we belive are dublicates
            //List<Constituents> cons = new List<Constituents>(constituents.Values.ToList());

            //two foreach loops
            //the first loop gets the cons
            //in the second foreach loop, we get a consCompare. this will be from the constituents dictionary list
            //we compare the cons and consCompare if the names match (last and first name)
            //if the names match, we compare the attributes and see which attribute they do not have
            //check which one has the most attributes and add the missing attribute to it
            //add the new cons with the most attributes and the new added one and put it in the nonDubCons list
            //if the con is not in the removed list add the con 
            foreach (KeyValuePair<string, Constituents> cons in constituents)
            {
                foreach (KeyValuePair<string, Constituents> consCompare in constituents)
                {
                    if (cons.Value.GetName().Equals(consCompare.Value.GetName()))
                    {

                    }
                }
            }

            return nonDubCons;
        }

        /// <summary>
        /// This method 
        /// </summary>
        /// <param name="constituents"></param>
        /// <param name="transactions"></param>
        /// <returns></returns>
        public static Dictionary<string, Constituents> GetTopConstituents(this Dictionary<string, Constituents> constituents, ref List<Transaction> transactions)
        {
            Dictionary<string, Constituents> listCons = new Dictionary<string, Constituents>();

            foreach (Transaction trans in transactions)
            {
                if (constituents.ContainsKey(trans.GetAccountNumber()))
                {
                    listCons.Add(constituents[trans.GetAccountNumber()].GetAccountNumber(), constituents[trans.GetAccountNumber()]);
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
            Dictionary<string, Constituents> conList = new Dictionary<string, Constituents>();
            List<Transaction> transList = new List<Transaction>();

            string[] dateArray;

            foreach (Constituents cons in constituents)
            {
                if (cons.GetName().ToLower().Contains(lookUp) || cons.GetLastName().ToLower().Contains(lookUp)
                    || cons.GetFirstName().ToLower().Contains(lookUp) || cons.GetEmail().ToLower().Contains(lookUp))
                {

                    conList.Add(cons.GetAccountNumber(), cons);
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
        public static Dictionary<string, Constituents> DescendingOrderDonations(this Dictionary<string, Constituents> constituents, ref List<Transaction> transaction)
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

        /// <summary>
        /// This method orders the constituents from highest to lowest donations
        /// </summary>
        /// <param name="constituents"></param>
        /// <param name="transaction"></param>
        /// <returns>Returns a Dictionary of constituents with only one transaction</returns>
        public static Dictionary<string, Constituents> DescendingOrderTotalDonations(this Dictionary<string, Constituents> constituents, ref List<Transaction> transaction)
        {
            Dictionary<string, Constituents> cons = constituents.AddTransaction(ref transaction);
            List<Constituents> consList = cons.Values.ToList();
            List<Constituents> orderedAmount = consList.OrderByDescending(a => a.TotalDonation()).ToList();
            return orderedAmount.ToDictionary(c => c.GetAccountNumber());
        }

        public static Dictionary<string, Constituents> MatchingNames(this Dictionary<string, Constituents> constituents)
        {
            //constituents with the same name
            Dictionary<string, Constituents> matchnames = new Dictionary<string, Constituents>();
            foreach (KeyValuePair<string, Constituents> cons in constituents)
            {
                //not everyone will have a first and last name
                if (cons.Value.GetLastName().Trim().Count() == 0 && cons.Value.GetFirstName().Trim().Count() == 0 || cons.Value.GetTypeOfConstituent().ToLower().Equals("orginization"))
                    continue;

                if (cons.Value.GetTypeOfConstituent().Trim().ToLower().Equals("organization"))
                    continue;

                if (cons.Value.GetName().Trim().ToLower().Contains("anonymous"))
                    continue;


                foreach (KeyValuePair<string, Constituents> consCompare in constituents)
                {

                    if (consCompare.Value.GetLastName().Trim().Count() == 0 && consCompare.Value.GetFirstName().Trim().Count() == 0 || consCompare.Value.GetTypeOfConstituent().Equals("Orginization"))
                        continue;

                    if (consCompare.Value.GetTypeOfConstituent().Trim().ToLower().Equals("organization"))
                        continue;

                    if (consCompare.Value.GetName().Trim().ToLower().Contains("anonymous"))
                        continue;

                    //check that we do not have the consCompare in the matchnames
                    //then compare that the last and first name match
                    if (cons.Key != consCompare.Key && cons.Value.GetFirstName().Equals(consCompare.Value.GetFirstName()) && cons.Value.GetLastName().Equals(consCompare.Value.GetLastName()))
                    {
                        //add this constituent
                        if (!matchnames.ContainsKey(cons.Key) && !matchnames.ContainsKey(consCompare.Key))
                        {
                            //if the consCompare doesnt have an address or that the address is different, continue
                            if (consCompare.Value.GetAddress().Trim().Count() == 0 || consCompare.Value.GetAddress().Equals(cons.Value.GetAddress()))
                            {
                                matchnames.Add(consCompare.Key, consCompare.Value);
                                matchnames.Add(cons.Key, cons.Value);
                            }

                        }

                    }
                }
            }

            return matchnames;
        }


        //public static Dictionary<string, Constituents> Update(this Dictionary<string, Constituents> constituents, ref Dictionary<string, Constituents> matchingNames, out Dictionary<string, Constituents> removeDubCons)
        //{
        //    //go through the matching names dictionary
        //    //check the date 
        //}

    }


}
