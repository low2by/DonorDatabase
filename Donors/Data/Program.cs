using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace Donor
{
    class Program
    {
        //Individual constituent;
        private static BloomerangColumnHeaderConstituents headerConstituents;
        private static BloomerangColumnHeaderTransaction headerTransaction;
        private static CharityproudHeaderConstituents headerCharityproud;
        private static List<Constituents> constituents;
        private static List<Transaction> transactions;

        static void Main(string[] args)
        {
            headerConstituents = new BloomerangColumnHeaderConstituents();
            headerTransaction = new BloomerangColumnHeaderTransaction();
            headerCharityproud = new CharityproudHeaderConstituents();
            constituents = new List<Constituents>();
            transactions = new List<Transaction>();
            GetExcelFile();

            //foreach(Constituents name in constituents)
            //{
            //    Console.WriteLine("Name:\t"+name.GetName()+"\nType:\t"+name.GetTypeOfConstituent()+"\nAccount:\t"+name.GetAccountNumber() 
            //        + "\nStreet:\t"+name.GetAddress() +"\nCity:\t"+name.GetCity()+"\nState:\t"+name.GetState() + "\nZip Code:\t" + name.GetZipCode()
            //        + "\nEmail:\t"+ name.GetEmail()+ "\nPhone Number:\t"+name.GetPhoneNumber()+"\n\r\n\r");


            //}

            //foreach (Transaction name in transactions)
            //{
            //    Console.WriteLine("Name:\t" + name.GetName + "\nAccount:\t"+name.GetAccountNumber+ "\nDate:\t" + name.DonationDate + "\nCampaign:\t" + name.Campaign
            //        + "\nMini-Campaign:\t" + name.MiniCampaign + "\nFund:\t" + name.Fund + "\nType:\t" + name.TransactionType + "\nMethod:\t" + name.TransactionMethod
            //        + "\nAmount:\t" + name.DonationAmount + "\n\r\n\r");
            //}

            //constituents.Add(new Constituents("1", "eman low", "low", "eman", "2500 So. State St.", "SLC", "UT", "84108", "8015581375", "lemmanuel@yahoo.com", "individual"));
            //constituents.Add(new Constituents("2", "jane low", "low", "jane", "2500 So. State St.", "SLC", "UT", "84108", "8015581372", "lemmanuel14@yahoo.com", "individual"));
            //constituents.Add(new Constituents("3", "jane lok", "lok", "jane", "2500 So. State St.", "SLC", "UT", "84108", "8015581372", "lemmanuel14@yahoo.com", "individual"));
            //constituents.Add(new Constituents("4", "jane loa", "loa", "jane", "2500 So. State St.", "SLC", "UT", "84108", "8015581372", "lemmanuel14@yahoo.com", "individual"));



            //Dictionary<int, IEnumerable<Constituents>> listofMatchingAddress = constituents.HaveSameAddress();

            //constituents.AddTransaction(transactions);

            //foreach (Constituents person in constituents)
            //{
            //    Console.WriteLine(person.GetName() + " has " + listofMatchingAddress[person.GetAccountNumber()].Count() + " with the same last name:");
            //}

            Dictionary<string, Constituents> consTrans = constituents.AddTransaction(transactions);

            foreach (KeyValuePair<string, Constituents> person in consTrans)
            {
                //person.Value
                //    Console.WriteLine("Name:\t"+name.GetName()+"\nType:\t"+name.GetTypeOfConstituent()+"\nAccount:\t"+name.GetAccountNumber() 
                //        + "\nStreet:\t"+name.GetAddress() +"\nCity:\t"+name.GetCity()+"\nState:\t"+name.GetState() + "\nZip Code:\t" + name.GetZipCode()
                //        + "\nEmail:\t"+ name.GetEmail()+ "\nPhone Number:\t"+name.GetPhoneNumber()+"\n\r\n\r");
                //    Console.WriteLine("Name:\t" + name.GetName + "\nAccount:\t"+name.GetAccountNumber+ "\nDate:\t" + name.DonationDate + "\nCampaign:\t" + name.Campaign
                //        + "\nMini-Campaign:\t" + name.MiniCampaign + "\nFund:\t" + name.Fund + "\nType:\t" + name.TransactionType + "\nMethod:\t" + name.TransactionMethod
                //        + "\nAmount:\t" + name.DonationAmount + "\n\r\n\r");
            }

            Console.Read();
        }

        public static void GetExcelFile()
        {

            //Create COM Objects. Create a COM object for everything that is referenced
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\elotubai10\Desktop\all_transactions_constituent_bloomerang.xlsx");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            //this is for testing. delete leter
            rowCount = 5;


            //iterate over the rows and columns and print to the console as it appears in the file
            //excel is not zero based!!
            for (int i = 1; i <= rowCount; i++)
            {
                if (i == 1)
                {
                    GetHeader(ref colCount, ref xlRange, ref i);
                    continue;
                }

                if (headerTransaction.AmountColNum == 0)
                {
                    //Console.WriteLine("Setting the constituents\n\r\n\r");
                    SetIndividualConstituentsFields(ref i, ref xlRange);
                }
                else
                {
                    if (i < 3)
                        continue;
                    //Console.WriteLine("Row Count: " + i);
                    //Console.WriteLine("Setting the transaction\n\r\n\r");
                    SetTransactions(ref i, ref xlRange);
                }

                if(headerCharityproud.EmailColNum != 0 && headerCharityproud.FundColNum != 0)
                {
                    SetCharityConstituentsTransaction(ref i, ref xlRange);
                }

            }
            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //rule of thumb for releasing com objects:
            //  never use two dots, all COM objects must be referenced and released individually
            //  ex: [somthing].[something].[something] is bad

            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);

            //close and release
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);

            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
        }

        public void WriteSample()
        {

        }

        private static void SetIndividualConstituentsFields(ref int i, ref Excel.Range xlRange)
        {
            constituents.Add(new Constituents(GetFieldValue(ref i, ref xlRange, headerConstituents.AccountNumColNum),
                GetFieldValue(ref i, ref xlRange, headerConstituents.NameColNum), GetFieldValue(ref i, ref xlRange, headerConstituents.LastNameColNum), GetFieldValue(ref i, ref xlRange, headerConstituents.FirstNameColNum),
                GetFieldValue(ref i, ref xlRange, headerConstituents.CityAddressColNum), GetFieldValue(ref i, ref xlRange, headerConstituents.CityColNum), GetFieldValue(ref i, ref xlRange, headerConstituents.StateColNum), GetFieldValue(ref i, ref xlRange, headerConstituents.ZipCodeColNum),
                GetFieldValue(ref i, ref xlRange, headerConstituents.PhoneColNum), GetFieldValue(ref i, ref xlRange, headerConstituents.EmailColNum),
                GetFieldValue(ref i, ref xlRange, headerConstituents.TypeColNum)));

        }

        private static void SetCharityConstituentsTransaction(ref int i, ref Excel.Range xlRange)
        {
            constituents.Add(new Constituents(GetFieldValue(ref i, ref xlRange, headerCharityproud.AccountNumberColNum),
               GetFieldValue(ref i, ref xlRange, headerCharityproud.NameColNum),
               GetFieldValue(ref i, ref xlRange, headerCharityproud.CityColNum), GetFieldValue(ref i, ref xlRange, headerCharityproud.CityColNum), GetFieldValue(ref i, ref xlRange, headerCharityproud.StateColNum), GetFieldValue(ref i, ref xlRange, headerCharityproud.ZipCodeColNum),
               GetFieldValue(ref i, ref xlRange, headerCharityproud.PhoneColNum), GetFieldValue(ref i, ref xlRange, headerCharityproud.EmailColNum),
               GetFieldValue(ref i, ref xlRange, headerCharityproud.TypeColNum)));

            transactions.Add(new Transaction(GetFieldValue(ref i, ref xlRange, headerCharityproud.AccountNumberColNum), GetFieldValue(ref i, ref xlRange, headerCharityproud.NameColNum),
                GetFieldValue(ref i, ref xlRange, headerCharityproud.DateColNum), GetFieldValue(ref i, ref xlRange, headerCharityproud.CampaignColNum), GetFieldValue(ref i, ref xlRange, headerCharityproud.MiniCampaignColNum),
                 GetFieldValue(ref i, ref xlRange, headerCharityproud.FundColNum), GetFieldValue(ref i, ref xlRange, headerCharityproud.TypeColNum), GetFieldValue(ref i, ref xlRange, headerCharityproud.MethodColNum),
                  GetFieldValue(ref i, ref xlRange, headerCharityproud.AmountColNum)));
        }

        private static void SetTransactions(ref int i, ref Excel.Range xlRange)
        {
            //do this so we can begin the row at 3
            transactions.Add(new Transaction(GetFieldValue(ref i, ref xlRange, headerTransaction.AccountNumberColNum), GetFieldValue(ref i, ref xlRange, headerTransaction.NameColNum),
                GetFieldValue(ref i, ref xlRange, headerTransaction.DateColNum), GetFieldValue(ref i, ref xlRange, headerTransaction.CampaignColNum), GetFieldValue(ref i, ref xlRange, headerTransaction.MiniCampaignColNum),
                 GetFieldValue(ref i, ref xlRange, headerTransaction.FundColNum), GetFieldValue(ref i, ref xlRange, headerTransaction.TypeColNum), GetFieldValue(ref i, ref xlRange, headerTransaction.MethodColNum),
                  GetFieldValue(ref i, ref xlRange, headerTransaction.AmountColNum)));
        }

        private static string GetFieldValue(ref int i, ref Excel.Range xlRange, int _colNum)
        {
            string name = "";
            int colNum = _colNum;
            DateTime date;
            string[] dateFormat;

            if (xlRange.Cells[i, colNum] != null && xlRange.Cells[i, colNum].Value2 != null)
            {
                //name = xlRange.Cells[i, colNum].Value2.ToString();
                if (colNum == headerTransaction.DateColNum)
                {
                    double.TryParse(xlRange.Cells[i, colNum].Value2.ToString(), out double tmp);
                    date = DateTime.FromOADate(tmp);
                    dateFormat = date.GetDateTimeFormats();
                    name = dateFormat[0];
                }
                else
                {
                    name = xlRange.Cells[i, colNum].Value2.ToString();

                }

            }

            return name;
        }



        private static void GetHeader(ref int colCount, ref Excel.Range xlRange, ref int i)
        {
            string headerName;

            for (int j = 1; j <= colCount; j++)
            {
                //new line
                if (i == 1)
                {
                    headerName = xlRange.Cells[i, j].Value2.ToString();
                    headerName = headerName.Trim().ToLower();
                    AssignHeaderCol(ref headerName, ref j);
                    //Console.Write("\r\n");

                }



                //if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                //    Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");
            }
        }

        private static void AssignHeaderCol(ref string headerName, ref int j)
        {
            if (headerName.Equals("name"))
            {
                headerConstituents.NameColNum = j;
            }

            if (headerName.Contains("last") && headerName.Contains("name"))
            {
                headerConstituents.LastNameColNum = j;
            }

            if (headerName.Contains("first") && headerName.Contains("name"))
            {
                headerConstituents.FirstNameColNum = j;
            }

            if (headerName.Contains("account") && headerName.Contains("number"))
            {
                headerConstituents.AccountNumColNum = j;
            }

            if (headerName.Contains("primary") && headerName.Contains("street"))
            {
                headerConstituents.CityAddressColNum = j;
            }

            if (headerName.Contains("primary") && headerName.Contains("city"))
            {
                headerConstituents.CityColNum = j;
            }

            if (headerName.Contains("primary") && headerName.Contains("state"))
            {
                headerConstituents.StateColNum = j;
            }

            if (headerName.Contains("primary") && headerName.Contains("zip") && headerName.Contains("code"))
            {
                headerConstituents.ZipCodeColNum = j;
            }

            if (headerName.Contains("primary") && headerName.Contains("email") && headerName.Contains("address"))
            {
                headerConstituents.EmailColNum = j;
            }

            if (headerName.Equals("type"))
            {
                headerConstituents.TypeColNum = j;
            }

            if (headerName.Contains("primary") && headerName.Contains("phone") && headerName.Contains("number"))
            {
                headerConstituents.PhoneColNum = j;
            }

            //for transactions
            if (headerName.Equals("name"))
            {
                headerTransaction.NameColNum = j;
            }

            if (headerName.Contains("date"))
            {
                headerTransaction.DateColNum = j;
            }

            //for transactions
            if (headerName.Contains("campaign") && !headerName.Contains("mini"))
            {
                headerTransaction.CampaignColNum = j;
            }

            if (headerName.Contains("mini") && headerName.Contains("-campaign"))
            {
                headerTransaction.MiniCampaignColNum = j;
            }

            if (headerName.Contains("fund"))
            {
                headerTransaction.FundColNum = j;
            }

            if (headerName.Contains("type"))
            {
                headerTransaction.TypeColNum = j;
            }

            if (headerName.Contains("method"))
            {
                headerTransaction.MethodColNum = j;
            }

            if (headerName.Contains("amount"))
            {
                headerTransaction.AmountColNum = j;
            }

            if (headerName.Contains("account") && headerName.Contains("number"))
            {
                headerTransaction.AccountNumberColNum = j;
            }

            //for charity proud
            if(headerName.Contains("consituent") && headerName.Contains("number"))
            {
                headerCharityproud.NameColNum = j;
            }

            if (headerName.Contains("address") && headerName.Contains("line") && headerName.Contains("1"))
            {
                headerCharityproud.AddressLine1 = j;
            }

            if (headerName.Contains("address") && headerName.Contains("line") && headerName.Contains("2"))
            {
                headerCharityproud.AddressLine2 = j;
            }

            if (headerName.Contains("city"))
            {
                headerCharityproud.CityColNum = j;
            }

            if (headerName.Contains("state"))
            {
                headerCharityproud.StateColNum = j;
            }

            if (headerName.Contains("zip"))
            {
                headerCharityproud.ZipCodeColNum = j;
            }

            if (headerName.Contains("phone"))
            {
                headerCharityproud.PhoneColNum = j;
            }

            if (headerName.Contains("email"))
            {
                headerCharityproud.EmailColNum = j;
            }

            if (headerName.Contains("date"))
            {
                headerCharityproud.DateColNum = j;
            }

            if (headerName.Contains("campaign"))
            {
                headerCharityproud.CampaignColNum = j;
            }

            if (headerName.Contains("mini-campaign"))
            {
                headerCharityproud.MiniCampaignColNum = j;
            }

            if (headerName.Contains("fund") && headerName.Contains("type"))
            {
                headerCharityproud.FundColNum = j;
            }

            if (headerName.Contains("transaction") && headerName.Contains("type"))
            {
                headerCharityproud.TypeColNum = j;
            }

            if (headerName.Contains("gift") && headerName.Contains("type"))
            {
                headerCharityproud.MethodColNum = j;
            }

            if (headerName.Contains("amount"))
            {
                headerCharityproud.AmountColNum = j;
            }

            if (headerName.Contains("constituent") && headerName.Contains("id"))
            {
                headerCharityproud.AccountNumberColNum = j;
            }

        }

        public static void CreateIndividualConstituents(Excel.Range xlRange)
        {
            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            //this is for testing. delete leter
            int _rowCount = 3;
            //colCount = 1;


            for (int i = 1; i <= _rowCount; i++)
            {
                for (int j = 1; j <= colCount; j++)
                {
                    //new line
                    if (j == 1)
                    {
                        //Console.Write("\r\n");
                    }


                    //write the value to the console
                    Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");
                    //if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                    //    Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");
                }
            }
        }
    }

    public class BloomerangColumnHeaderConstituents
    {
        public int NameColNum { get; set; }
        public int FirstNameColNum { get; set; }
        public int LastNameColNum { get; set; }
        public int AccountNumColNum { get; set; }
        public int CityAddressColNum { get; set; }
        public int CityColNum { get; set; }
        public int StateColNum { get; set; }
        public int ZipCodeColNum { get; set; }
        public int EmailColNum { get; set; }
        public int TypeColNum { get; set; }
        public int PhoneColNum { get; set; }

    }

    public class BloomerangColumnHeaderTransaction
    {
        public int NameColNum { get; set; }
        public int DateColNum { get; set; }
        public int CampaignColNum { get; set; }
        public int MiniCampaignColNum { get; set; }
        public int FundColNum { get; set; }
        public int TypeColNum { get; set; }
        public int MethodColNum { get; set; }
        public int AmountColNum { get; set; }
        public int AccountNumberColNum { get; set; }

    }

    public class CharityproudHeaderConstituents
    {
        public int AccountNumberColNum { get; set; }
        public int NameColNum { get; set; }
        public int AddressLine1 { get; set; }
        public int AddressLine2 { get; set; }
        public int CityColNum { get; set; }
        public int StateColNum { get; set; }
        public int ZipCodeColNum { get; set; }
        public int PhoneColNum { get; set; }
        public int EmailColNum { get; set; }
        public int DateColNum { get; set; }
        public int CampaignColNum { get; set; }
        public int MiniCampaignColNum { get; set; }
        public int FundColNum { get; set; }
        public int TypeColNum { get; set; }
        public int AmountColNum { get; set; }
        public int MethodColNum { get; set; }

    }
}
