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

        static void Main(string[] args)
        {

            Dictionary<string, Constituents> constituents = new Dictionary<string, Constituents>();
            List<Transaction> transactions = new List<Transaction>();

            foreach (string file in Directory.EnumerateFiles(@"C:\Users\elotubai10\OneDrive - Granite School District\donordatabase\", "*.xlsx"))
            {
                BloomerangColumnHeaderConstituents headerConstituents = new BloomerangColumnHeaderConstituents();
                BloomerangColumnHeaderTransaction headerTransaction = new BloomerangColumnHeaderTransaction();
                CharityproudHeaderConstituents headerCharityproud = new CharityproudHeaderConstituents();
                string filepath = file;
                if (filepath.Contains("~$"))
                    continue;
                GetExcelFile(ref filepath, ref constituents, ref transactions, ref headerConstituents, ref headerTransaction, ref headerCharityproud);
            }

            Dictionary<string, Constituents> consWithTransactions = constituents.AddTransaction(ref transactions);
            Dictionary<string, Constituents> removeCons = new Dictionary<string, Constituents>();
            Dictionary<string, Transaction> removeTrans = new Dictionary<string, Transaction>();
            Dictionary<string, Transaction> addTrans = new Dictionary<string, Transaction>();
            Dictionary<string, Constituents> consWithTransactions_removedDub = consWithTransactions.RemoveDublicates(ref removeCons, ref removeTrans, ref addTrans);

            //Dictionary<string, Constituents> combinedCharityBloomarang = constituents.CombineCharityBloomarang();
            WriteExcelFile(ref consWithTransactions, "all constituents with their trasactions");
            WriteExcelFile(ref consWithTransactions_removedDub, "all constituents with their trasaction no dublicates");
            WriteExcelFileDonors(ref removeCons, "the duplicates that were removed");
            WriteExcelFileTrans(ref removeTrans, "the transactions that were removed");
            WriteExcelFileTrans(ref addTrans, "the transactions that were added");

            Console.WriteLine("All Done *Zara's voice*");
            Console.Read();
        }

        public static void GetExcelFile(ref string filepath, ref Dictionary<string, Constituents> constituents, ref List<Transaction> transaction,
            ref BloomerangColumnHeaderConstituents headerConstituents, ref BloomerangColumnHeaderTransaction headerTransaction, ref CharityproudHeaderConstituents headerCharityproud)
        {

            Console.WriteLine("Getting data from file: " + filepath);
            //Create COM Objects. Create a COM object for everything that is referenced
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(filepath);
            Excel.Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            //this is for testing. delete leter
            //rowCount = 200;


            //iterate over the rows and columns and print to the console as it appears in the file
            //excel is not zero based!!
            for (int i = 1; i <= rowCount; i++)
            {
                if (i == 1)
                {
                    GetHeader(ref colCount, ref xlRange, ref i, ref headerConstituents, ref headerTransaction, ref headerCharityproud);

                    if (headerTransaction.AmountColNum == 0)
                        Console.WriteLine("Setting the Constituents : " + rowCount + " constituents...");
                    else
                        Console.WriteLine("Setting the Transactions : " + rowCount + " Transactions...");
                    continue;
                }

                if (headerTransaction.AmountColNum == 0)
                {
                    //Console.WriteLine("Setting the constituents\n\r\n\r");
                    SetIndividualConstituentsFields(ref constituents, ref i, ref xlRange, ref headerConstituents, ref headerTransaction);
                    Console.WriteLine("Constituent: \t" + i);
                }

                if (headerTransaction.AmountColNum != 0 && headerCharityproud.AddressLine1 == 0)
                {
                    if (i < 3)
                        continue;
                    //Console.WriteLine("Row Count: " + i);
                    //Console.WriteLine("Setting the transaction\n\r\n\r");
                    SetTransactions(transaction, ref i, ref xlRange, headerTransaction);
                    Console.WriteLine("Transaction: \t" + i);
                }



                if (headerCharityproud.AddressLine1 != 0 && headerCharityproud.AddressLine2 != 0)
                {
                    SetCharityConstituentsTransaction(ref constituents, ref transaction, ref i, ref xlRange, ref headerCharityproud, ref headerTransaction);
                    Console.WriteLine("Constituent with Transaction: \t" + i);
                }

                //Console.WriteLine("At row : " + i");

            }

            //Console.WriteLine("Finished Setting the Constituents and Transation: ");
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

        public static void WriteExcelFile(ref Dictionary<string, Constituents> constitunets, string filename)
        {
            Console.WriteLine("Writing to File");
            Excel.Application excelApp = new Excel.Application();
            if (excelApp != null)
            {
                Excel.Workbook excelWorkbook = excelApp.Workbooks.Add();
                Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelWorkbook.Sheets.Add();
                Excel.Range xlRange = excelWorksheet.UsedRange;

                xlRange.Cells[1, 1] = "Account Number";
                xlRange.Cells[1, 2] = "Name";
                xlRange.Cells[1, 3] = "Last Name";
                xlRange.Cells[1, 4] = "First Name";
                xlRange.Cells[1, 5] = "Primary Street";
                xlRange.Cells[1, 6] = "Primary City";
                xlRange.Cells[1, 7] = "Primary State";
                xlRange.Cells[1, 8] = "Primary ZIP Code";
                xlRange.Cells[1, 9] = "Primary Phone Number";
                xlRange.Cells[1, 10] = "Primary Email Address";
                xlRange.Cells[1, 11] = "Type";
                xlRange.Cells[1, 12] = "Date";
                xlRange.Cells[1, 13] = "Campaign";
                xlRange.Cells[1, 14] = "Mini-Campaign";
                xlRange.Cells[1, 15] = "Fund";
                xlRange.Cells[1, 16] = "Type";
                xlRange.Cells[1, 17] = "Method";
                xlRange.Cells[1, 18] = "Amount";
                xlRange.Cells[1, 19] = "In Kind Market Value";
                xlRange.Cells[1, 20] = "In Kind Description";
                xlRange.Cells[1, 21] = "Total Donation";

                int row = 2;
                int consNum = 1, transNum = 0;
                bool addRow;
                foreach (KeyValuePair<string, Constituents> cons in constitunets)
                {
                    xlRange.Cells[row, 1] = cons.Value.GetAccountNumber();
                    xlRange.Cells[row, 2] = cons.Value.GetName();
                    xlRange.Cells[row, 3] = cons.Value.GetLastName();
                    xlRange.Cells[row, 4] = cons.Value.GetFirstName();
                    xlRange.Cells[row, 5] = cons.Value.GetAddress();
                    xlRange.Cells[row, 6] = cons.Value.GetCity();
                    xlRange.Cells[row, 7] = cons.Value.GetState();
                    xlRange.Cells[row, 8] = cons.Value.GetZipCode();
                    xlRange.Cells[row, 9] = cons.Value.GetPhoneNumber();
                    xlRange.Cells[row, 10] = cons.Value.GetEmail();
                    xlRange.Cells[row, 11] = cons.Value.GetTypeOfConstituent();
                    xlRange.Cells[row, 21] = cons.Value.TotalDonation();

                    Console.WriteLine("Constituent: " + consNum + " of " + constitunets.Count);

                    addRow = true;
                    foreach (Transaction trans in cons.Value.GetTransactions())
                    {
                        transNum += 1;
                        xlRange.Cells[row, 12] = trans.DonationDate;
                        xlRange.Cells[row, 13] = trans.Campaign;
                        xlRange.Cells[row, 14] = trans.MiniCampaign;
                        xlRange.Cells[row, 15] = trans.Fund;
                        xlRange.Cells[row, 16] = trans.TransactionType;
                        xlRange.Cells[row, 17] = trans.TransactionMethod;
                        xlRange.Cells[row, 18] = trans.DonationAmount;
                        xlRange.Cells[row, 19] = trans.InKindMarketValue;
                        xlRange.Cells[row, 20] = trans.InKindDescr;
                        row += 1;
                        addRow = false;

                        //Console.WriteLine("Constituent: " + transNum + " of " + cons.Value.GetTransactions().Count);
                    }

                    transNum = 0;
                    consNum += 1;
                    if (addRow)
                        row += 1;

                }

                excelApp.DisplayAlerts = false;
                excelApp.ActiveWorkbook.SaveAs(@"C:\Users\elotubai10\OneDrive - Granite School District\donordatabaseresult\" + filename + ".xls", Excel.XlFileFormat.xlWorkbookNormal);
                excelApp.DisplayAlerts = true;

                excelWorkbook.Close();
                excelApp.Quit();

                Marshal.FinalReleaseComObject(excelWorksheet);
                Marshal.FinalReleaseComObject(excelWorkbook);
                Marshal.FinalReleaseComObject(excelApp);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public static void WriteExcelFileDonors(ref Dictionary<string, Constituents> constitunets, string filename)
        {
            Console.WriteLine("Writing to File");
            Excel.Application excelApp = new Excel.Application();
            if (excelApp != null)
            {
                Excel.Workbook excelWorkbook = excelApp.Workbooks.Add();
                Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelWorkbook.Sheets.Add();
                Excel.Range xlRange = excelWorksheet.UsedRange;

                xlRange.Cells[1, 1] = "Account Number";
                xlRange.Cells[1, 2] = "Name";
                xlRange.Cells[1, 3] = "Last Name";
                xlRange.Cells[1, 4] = "First Name";
                xlRange.Cells[1, 5] = "Primary Street";
                xlRange.Cells[1, 6] = "Primary City";
                xlRange.Cells[1, 7] = "Primary State";
                xlRange.Cells[1, 8] = "Primary ZIP Code";
                xlRange.Cells[1, 9] = "Primary Phone Number";
                xlRange.Cells[1, 10] = "Primary Email Address";
                xlRange.Cells[1, 11] = "Type";

                int row = 2;
                int consNum = 1;
                //, transNum = 0;
                //bool addRow;
                foreach (KeyValuePair<string, Constituents> cons in constitunets)
                {
                    xlRange.Cells[row, 1] = cons.Value.GetAccountNumber();
                    xlRange.Cells[row, 2] = cons.Value.GetName();
                    xlRange.Cells[row, 3] = cons.Value.GetLastName();
                    xlRange.Cells[row, 4] = cons.Value.GetFirstName();
                    xlRange.Cells[row, 5] = cons.Value.GetAddress();
                    xlRange.Cells[row, 6] = cons.Value.GetCity();
                    xlRange.Cells[row, 7] = cons.Value.GetState();
                    xlRange.Cells[row, 8] = cons.Value.GetZipCode();
                    xlRange.Cells[row, 9] = cons.Value.GetPhoneNumber();
                    xlRange.Cells[row, 10] = cons.Value.GetEmail();
                    xlRange.Cells[row, 11] = cons.Value.GetTypeOfConstituent();
                    xlRange.Cells[row, 21] = cons.Value.TotalDonation();

                    Console.WriteLine("Constituent: " + consNum + " of " + constitunets.Count);

                    consNum += 1;
                    row += 1;

                }

                excelApp.DisplayAlerts = false;
                excelApp.ActiveWorkbook.SaveAs(@"C:\Users\elotubai10\OneDrive - Granite School District\donordatabaseresult\" + filename + ".xls", Excel.XlFileFormat.xlWorkbookNormal);
                excelApp.DisplayAlerts = true;

                excelWorkbook.Close();
                excelApp.Quit();

                Marshal.FinalReleaseComObject(excelWorksheet);
                Marshal.FinalReleaseComObject(excelWorkbook);
                Marshal.FinalReleaseComObject(excelApp);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public static void WriteExcelFileTrans(ref Dictionary<string, Transaction> transactions, string filename)
        {
            Console.WriteLine("Writing to File");
            Excel.Application excelApp = new Excel.Application();
            if (excelApp != null)
            {
                Excel.Workbook excelWorkbook = excelApp.Workbooks.Add();
                Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelWorkbook.Sheets.Add();
                Excel.Range xlRange = excelWorksheet.UsedRange;

                xlRange.Cells[1, 1] = "Account Number";
                xlRange.Cells[1, 2] = "Date";
                xlRange.Cells[1, 3] = "Campaign";
                xlRange.Cells[1, 4] = "Mini-Campaign";
                xlRange.Cells[1, 5] = "Fund";
                xlRange.Cells[1, 6] = "Type";
                xlRange.Cells[1, 7] = "Method";
                xlRange.Cells[1, 8] = "Amount";
                xlRange.Cells[1, 9] = "In Kind Market Value";
                xlRange.Cells[1, 10] = "In Kind Description";

                int row = 2;
                int transNum = 0;
                //bool addRow;
                foreach (KeyValuePair<string, Transaction> trans in transactions)
                {
                    xlRange.Cells[row, 1] = trans.Value.GetAccountNumber();
                    xlRange.Cells[row, 2] = trans.Value.DonationDate;
                    xlRange.Cells[row, 3] = trans.Value.Campaign;
                    xlRange.Cells[row, 4] = trans.Value.MiniCampaign;
                    xlRange.Cells[row, 5] = trans.Value.Fund;
                    xlRange.Cells[row, 6] = trans.Value.TransactionType;
                    xlRange.Cells[row, 7] = trans.Value.TransactionMethod;
                    xlRange.Cells[row, 8] = trans.Value.DonationAmount;
                    xlRange.Cells[row, 9] = trans.Value.InKindMarketValue;
                    xlRange.Cells[row, 10] = trans.Value.InKindDescr;
                    Console.WriteLine("Transaction: " + transNum + " of " + transactions.Count);
                    transNum++;
                    row += 1;

                }

                excelApp.DisplayAlerts = false;
                excelApp.ActiveWorkbook.SaveAs(@"C:\Users\elotubai10\OneDrive - Granite School District\donordatabaseresult\" + filename + ".xls", Excel.XlFileFormat.xlWorkbookNormal);
                excelApp.DisplayAlerts = true;

                excelWorkbook.Close();
                excelApp.Quit();

                Marshal.FinalReleaseComObject(excelWorksheet);
                Marshal.FinalReleaseComObject(excelWorkbook);
                Marshal.FinalReleaseComObject(excelApp);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// This is used to set the constituents
        /// </summary>
        /// <param name="constituents"></param>
        /// <param name="i"></param>
        /// <param name="xlRange"></param>
        /// <param name="headerConstituents"></param>
        /// <param name="headerTransaction"></param>
        private static void SetIndividualConstituentsFields(ref Dictionary<string, Constituents> constituents, ref int i, ref Excel.Range xlRange, ref BloomerangColumnHeaderConstituents headerConstituents, ref BloomerangColumnHeaderTransaction headerTransaction)
        {
            //changing the implementation of adding constituents. us a dictionary to add the constituents so we can check if we already have a constituent.
            if (!constituents.ContainsKey(GetFieldValue(ref i, ref xlRange, headerConstituents.AccountNumColNum, ref headerTransaction)))
            {
                constituents.Add(GetFieldValue(ref i, ref xlRange, headerConstituents.AccountNumColNum, ref headerTransaction), new Constituents(GetFieldValue(ref i, ref xlRange, headerConstituents.AccountNumColNum, ref headerTransaction),
                                GetFieldValue(ref i, ref xlRange, headerConstituents.NameColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerConstituents.LastNameColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerConstituents.FirstNameColNum, ref headerTransaction),
                                GetFieldValue(ref i, ref xlRange, headerConstituents.CityAddressColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerConstituents.CityColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerConstituents.StateColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerConstituents.ZipCodeColNum, ref headerTransaction),
                                GetFieldValue(ref i, ref xlRange, headerConstituents.PhoneColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerConstituents.EmailColNum, ref headerTransaction),
                                GetFieldValue(ref i, ref xlRange, headerConstituents.TypeColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerConstituents.CreatedDate, ref headerTransaction)));
            }


        }

        /// <summary>
        /// This is used to set the transaction and the transaction of the chairity proud database for donors
        /// </summary>
        /// <param name="constituents"></param>
        /// <param name="transactions"></param>
        /// <param name="i"></param>
        /// <param name="xlRange"></param>
        /// <param name="headerCharityproud"></param>
        /// <param name="headerTransaction"></param>
        private static void SetCharityConstituentsTransaction(ref Dictionary<string, Constituents> constituents, ref List<Transaction> transactions, ref int i, ref Excel.Range xlRange, ref CharityproudHeaderConstituents headerCharityproud, ref BloomerangColumnHeaderTransaction headerTransaction)
        {
            // Constituents(string _accountNumber, string _name, string _address1, string _address2, string _city, string _state, string _zipCode, string _phoneNumber, string _email, string _type)

            if (!constituents.ContainsKey(GetFieldValue(ref i, ref xlRange, headerCharityproud.AccountNumberColNum, ref headerTransaction)))
            {
                constituents.Add(GetFieldValue(ref i, ref xlRange, headerCharityproud.AccountNumberColNum, ref headerTransaction), new Constituents(GetFieldValue(ref i, ref xlRange, headerCharityproud.AccountNumberColNum, ref headerTransaction),
                                GetFieldValue(ref i, ref xlRange, headerCharityproud.NameColNum, ref headerTransaction),
                                GetFieldValue(ref i, ref xlRange, headerCharityproud.AddressLine1, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerCharityproud.AddressLine2, ref headerTransaction),
                                GetFieldValue(ref i, ref xlRange, headerCharityproud.CityColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerCharityproud.StateColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerCharityproud.ZipCodeColNum, ref headerTransaction),
                                GetFieldValue(ref i, ref xlRange, headerCharityproud.PhoneColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerCharityproud.EmailColNum, ref headerTransaction)));

            }

            transactions.Add(new Transaction(GetFieldValue(ref i, ref xlRange, headerCharityproud.AccountNumberColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerCharityproud.NameColNum, ref headerTransaction),
                GetFieldValue(ref i, ref xlRange, headerCharityproud.DateColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerCharityproud.CampaignColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerCharityproud.MiniCampaignColNum, ref headerTransaction),
                 GetFieldValue(ref i, ref xlRange, headerCharityproud.FundColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerCharityproud.TypeColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerCharityproud.MethodColNum, ref headerTransaction),
                  GetFieldValue(ref i, ref xlRange, headerCharityproud.AmountColNum, ref headerTransaction)));
        }

        /// <summary>
        /// This is used to set the transactions from the bloomarang database
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="i"></param>
        /// <param name="xlRange"></param>
        /// <param name="headerTransaction"></param>
        private static void SetTransactions(List<Transaction> transaction, ref int i, ref Excel.Range xlRange, BloomerangColumnHeaderTransaction headerTransaction)
        {
            //do this so we can begin the row at 3
            transaction.Add(new Transaction(GetFieldValue(ref i, ref xlRange, headerTransaction.AccountNumberColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerTransaction.NameColNum, ref headerTransaction),
                GetFieldValue(ref i, ref xlRange, headerTransaction.DateColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerTransaction.CampaignColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerTransaction.MiniCampaignColNum, ref headerTransaction),
                 GetFieldValue(ref i, ref xlRange, headerTransaction.FundColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerTransaction.TypeColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerTransaction.MethodColNum, ref headerTransaction),
                  GetFieldValue(ref i, ref xlRange, headerTransaction.AmountColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerTransaction.MarketValueColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerTransaction.InKindDescrColNum, ref headerTransaction)));
        }

        /// <summary>
        /// This method gets the content of each field. The name, address, transaction ect... using the index of the headers
        /// </summary>
        /// <param name="i"></param>
        /// <param name="xlRange"></param>
        /// <param name="_colNum"></param>
        /// <param name="headerTransaction"></param>
        /// <returns>The content of the item in the row and col</returns>
        private static string GetFieldValue(ref int i, ref Excel.Range xlRange, int _colNum, ref BloomerangColumnHeaderTransaction headerTransaction)
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
                    //the date format when parsed to a double comes out as a whole number representing the days since the time was started for computers
                    double.TryParse(xlRange.Cells[i, colNum].Value2.ToString(), out double tmp);

                    //we get the date as a double and format it to a m/d/y
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

        private static void GetHeader(ref int colCount, ref Excel.Range xlRange, ref int i, ref BloomerangColumnHeaderConstituents headerConstituents, ref BloomerangColumnHeaderTransaction headerTransaction, ref CharityproudHeaderConstituents headerCharityproud)
        {
            string headerName;

            //Console.WriteLine("Getting the Header");
            for (int j = 1; j <= colCount; j++)
            {
                //new line
                if (i == 1)
                {
                    headerName = xlRange.Cells[i, j].Value2.ToString();
                    headerName = headerName.Trim().ToLower();
                    AssignHeaderCol(ref headerName, ref j, ref headerConstituents, ref headerTransaction, ref headerCharityproud);

                }

            }
        }

        /// <summary>
        /// This method assigns the column header number/index from the excel file and assigns it to either the constituents index or the transaction index.
        /// </summary>
        /// <param name="headerName"></param>
        /// <param name="j"></param>
        /// <param name="headerConstituents"></param>
        /// <param name="headerTransaction"></param>
        /// <param name="headerCharityproud"></param>
        private static void AssignHeaderCol(ref string headerName, ref int j, ref BloomerangColumnHeaderConstituents headerConstituents, ref BloomerangColumnHeaderTransaction headerTransaction, ref CharityproudHeaderConstituents headerCharityproud)
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

            if (headerName.Contains("created") && headerName.Contains("date"))
            {
                headerConstituents.CreatedDate = j;
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

            if (headerName.Contains("in") && headerName.Contains("kind") && headerName.Contains("fair") && headerName.Contains("market") && headerName.Contains("value"))
            {
                headerTransaction.MarketValueColNum = j;
            }

            if (headerName.Contains("in") && headerName.Contains("kind") && headerName.Contains("description"))
            {
                headerTransaction.InKindDescrColNum = j;
            }

            //for charity proud
            if (headerName.Contains("constituent") && headerName.Contains("name"))
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

    }

    public class BloomerangColumnHeaderConstituents
    {
        /// <summary>
        /// The name of a constituent. It can have a first and/or a last name 
        /// </summary>
        public int NameColNum { get; set; }

        /// <summary>
        /// The first name of the constituent
        /// </summary>
        public int FirstNameColNum { get; set; }

        /// <summary>
        /// The last name of the constitunet
        /// </summary>
        public int LastNameColNum { get; set; }

        /// <summary>
        /// The account number of the constituent
        /// </summary>
        public int AccountNumColNum { get; set; }

        /// <summary>
        /// The street address of the constituent resident
        /// </summary>
        public int CityAddressColNum { get; set; }

        /// <summary>
        /// The city of the constituent resident
        /// </summary>
        public int CityColNum { get; set; }

        /// <summary>
        /// The state of the constituent resident
        /// </summary>
        public int StateColNum { get; set; }

        /// <summary>
        /// The sip code of the consituents resident
        /// </summary>
        public int ZipCodeColNum { get; set; }

        /// <summary>
        /// The email of the constituent
        /// </summary>
        public int EmailColNum { get; set; }

        /// <summary>
        /// The type of constitunet it is. An orginization or an individual constituents
        /// </summary>
        public int TypeColNum { get; set; }

        /// <summary>
        /// The constitunets phone number
        /// </summary>
        public int PhoneColNum { get; set; }

        /// <summary>
        /// The date the constituent was created
        /// </summary>
        public int CreatedDate { get; set; }

    }

    public class BloomerangColumnHeaderTransaction
    {
        /// <summary>
        /// The name of a constituent. It can have a first and/or a last name 
        /// </summary>
        public int NameColNum { get; set; }

        /// <summary>
        /// The date is the day a transaction was given
        /// </summary>
        public int DateColNum { get; set; }

        /// <summary>
        /// The campaign that the transaction happened in
        /// </summary>
        public int CampaignColNum { get; set; }

        /// <summary>
        /// The mini-campaign with the campaing that the transaction happened in
        /// </summary>
        public int MiniCampaignColNum { get; set; }

        /// <summary>
        /// The fund (fundraiser) where the transaction happened
        /// </summary>
        public int FundColNum { get; set; }

        /// <summary>
        /// The type of transaction. Reoccuring or just a one time donation
        /// </summary>
        public int TypeColNum { get; set; }

        /// <summary>
        /// How the transaction was given. credit card, check, in-kind, cash, EFT ect...
        /// </summary>
        public int MethodColNum { get; set; }

        /// <summary>
        /// The amount that was given in a transaction
        /// </summary>
        public int AmountColNum { get; set; }

        /// <summary>
        /// the amount that is represented by the in-kind donation
        /// </summary>
        public int MarketValueColNum { get; set; }

        /// <summary>
        /// Describe what was givin in the in-kind transaction 
        /// </summary>
        public int InKindDescrColNum { get; set; }

        /// <summary>
        /// The account number for the constituent 
        /// </summary>
        public int AccountNumberColNum { get; set; }

    }

    public class CharityproudHeaderConstituents
    {
        /// <summary>
        /// The constituents acount number
        /// </summary>
        public int AccountNumberColNum { get; set; }

        /// <summary>
        /// The constituents name. It has first and/or last name
        /// </summary>
        public int NameColNum { get; set; }

        /// <summary>
        /// The address for the constituent
        /// </summary>
        public int AddressLine1 { get; set; }

        /// <summary>
        /// the apt number, suit, campus building ect... that comes with the address
        /// </summary>
        public int AddressLine2 { get; set; }

        /// <summary>
        /// The city that the constituent's resident
        /// </summary>
        public int CityColNum { get; set; }

        /// <summary>
        /// The state that the constituent's resident
        /// </summary>
        public int StateColNum { get; set; }

        /// <summary>
        /// The zip code that the constituent's resident
        /// </summary>
        public int ZipCodeColNum { get; set; }

        /// <summary>
        /// The constituent's phone number
        /// </summary>
        public int PhoneColNum { get; set; }

        /// <summary>
        /// The constituent's phone email
        /// </summary>
        public int EmailColNum { get; set; }

        /// <summary>
        /// The date the transaction was recieved
        /// </summary>
        public int DateColNum { get; set; }

        /// <summary>
        /// The campaign that the transaction happened in
        /// </summary>
        public int CampaignColNum { get; set; }

        /// <summary>
        /// The mini-campaign with the campaing that the transaction happened in
        /// </summary>
        public int MiniCampaignColNum { get; set; }

        /// <summary>
        /// The fund (fundraiser) where the transaction happened
        /// </summary>
        public int FundColNum { get; set; }

        /// <summary>
        /// The type of transaction. Reoccuring or just a one time donation
        /// </summary>
        public int TypeColNum { get; set; }

        /// <summary>
        /// The amount that was given in a transaction
        /// </summary>
        public int AmountColNum { get; set; }

        /// <summary>
        /// How the transaction was given. credit card, check, in-kind, cash, EFT ect...
        /// </summary>
        public int MethodColNum { get; set; }

    }
}
