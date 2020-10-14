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

            List<Constituents> constituents = new List<Constituents>();
            List<Transaction> transactions = new List<Transaction>();
            //GetExcelFile();

            foreach (string file in Directory.EnumerateFiles(@"C:\Users\elotubai10\Desktop\donordatabase\", "*.xlsx"))
            {
                BloomerangColumnHeaderConstituents headerConstituents = new BloomerangColumnHeaderConstituents();
                BloomerangColumnHeaderTransaction headerTransaction = new BloomerangColumnHeaderTransaction();
                CharityproudHeaderConstituents headerCharityproud = new CharityproudHeaderConstituents();
                string filepath = file;
                GetExcelFile(ref filepath, ref constituents, ref transactions, ref  headerConstituents, ref  headerTransaction, ref  headerCharityproud);
            }

            Dictionary<string, Constituents> woodbury = constituents.AddTransaction(transactions);

            WriteExcelFile(ref woodbury);


            Console.Read();
        }

        public static void GetWoodbury(ref Dictionary<string, Constituents> woodbury)
        {

        }

        public static void GetExcelFile(ref string filepath, ref List<Constituents> constituents, ref List<Transaction> transaction,
            ref BloomerangColumnHeaderConstituents headerConstituents, ref BloomerangColumnHeaderTransaction headerTransaction, ref CharityproudHeaderConstituents headerCharityproud)
        {

            
            //Create COM Objects. Create a COM object for everything that is referenced
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(filepath);
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            //this is for testing. delete leter
            rowCount = 22;


            //iterate over the rows and columns and print to the console as it appears in the file
            //excel is not zero based!!
            for (int i = 1; i <= rowCount; i++)
            {
                if (i == 1)
                {
                    GetHeader(ref colCount, ref xlRange, ref i, ref headerConstituents, ref headerTransaction, ref headerCharityproud);
                    continue;
                }

                if (headerTransaction.AmountColNum == 0)
                {
                    //Console.WriteLine("Setting the constituents\n\r\n\r");
                    SetIndividualConstituentsFields(ref constituents, ref i, ref xlRange, ref headerConstituents, ref headerTransaction);
                }
                
                if(headerTransaction.AmountColNum != 0 && headerCharityproud.AddressLine1 == 0)
                {
                    if (i < 3)
                        continue;
                    //Console.WriteLine("Row Count: " + i);
                    //Console.WriteLine("Setting the transaction\n\r\n\r");
                    SetTransactions(transaction, ref i, ref xlRange, headerTransaction);
                }
                    
                

                if(headerCharityproud.AddressLine1 != 0 && headerCharityproud.AddressLine2 != 0)
                {
                    SetCharityConstituentsTransaction(constituents, transaction, ref i, ref xlRange, headerCharityproud, headerTransaction);
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

        public static void WriteExcelFile(ref Dictionary<string, Constituents> constitunets)
        {
            Excel.Application excelApp = new Excel.Application();
            if (excelApp != null)
            {
                Excel.Workbook excelWorkbook = excelApp.Workbooks.Add();
                Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelWorkbook.Sheets.Add();

                excelWorksheet.Cells[1, 1] = "Value1";
                excelWorksheet.Cells[2, 1] = "Value2";
                excelWorksheet.Cells[3, 1] = "Value3";
                excelWorksheet.Cells[4, 1] = "Value4test";

                for(int i = 0; i < constitunets.Count; i++)
                {
                    for(int k = 0; k < constitunets.Values.Count + constitunets.TryGetValue(constitunets.ge))
                }

                excelApp.DisplayAlerts = false;
                excelApp.ActiveWorkbook.SaveAs(@"C:\Users\elotubai10\Desktop\donordatabase\abc1.xls", Excel.XlFileFormat.xlWorkbookNormal);
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

        private static void SetIndividualConstituentsFields(ref List<Constituents> constituents, ref int i, ref Excel.Range xlRange, ref BloomerangColumnHeaderConstituents headerConstituents, ref BloomerangColumnHeaderTransaction headerTransaction)
        {
            constituents.Add(new Constituents(GetFieldValue(ref i, ref xlRange, headerConstituents.AccountNumColNum, ref headerTransaction),
                GetFieldValue(ref i, ref xlRange, headerConstituents.NameColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerConstituents.LastNameColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerConstituents.FirstNameColNum, ref headerTransaction),
                GetFieldValue(ref i, ref xlRange, headerConstituents.CityAddressColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerConstituents.CityColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerConstituents.StateColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerConstituents.ZipCodeColNum, ref headerTransaction),
                GetFieldValue(ref i, ref xlRange, headerConstituents.PhoneColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerConstituents.EmailColNum, ref headerTransaction),
                GetFieldValue(ref i, ref xlRange, headerConstituents.TypeColNum, ref headerTransaction)));

        }

        private static void SetCharityConstituentsTransaction(List<Constituents> constituents, List<Transaction> transactions,ref int i, ref Excel.Range xlRange, CharityproudHeaderConstituents headerCharityproud, BloomerangColumnHeaderTransaction headerTransaction)
        {
            constituents.Add(new Constituents(GetFieldValue(ref i, ref xlRange, headerCharityproud.AccountNumberColNum, ref headerTransaction),
               GetFieldValue(ref i, ref xlRange, headerCharityproud.NameColNum, ref headerTransaction),
               GetFieldValue(ref i, ref xlRange, headerCharityproud.CityColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerCharityproud.CityColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerCharityproud.StateColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerCharityproud.ZipCodeColNum, ref headerTransaction),
               GetFieldValue(ref i, ref xlRange, headerCharityproud.PhoneColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerCharityproud.EmailColNum, ref headerTransaction),
               GetFieldValue(ref i, ref xlRange, headerCharityproud.TypeColNum, ref headerTransaction)));

            transactions.Add(new Transaction(GetFieldValue(ref i, ref xlRange, headerCharityproud.AccountNumberColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerCharityproud.NameColNum, ref headerTransaction),
                GetFieldValue(ref i, ref xlRange, headerCharityproud.DateColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerCharityproud.CampaignColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerCharityproud.MiniCampaignColNum, ref headerTransaction),
                 GetFieldValue(ref i, ref xlRange, headerCharityproud.FundColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerCharityproud.TypeColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerCharityproud.MethodColNum, ref headerTransaction),
                  GetFieldValue(ref i, ref xlRange, headerCharityproud.AmountColNum, ref headerTransaction)));
        }

        private static void SetTransactions(List<Transaction> transaction, ref int i, ref Excel.Range xlRange, BloomerangColumnHeaderTransaction headerTransaction)
        {
            //do this so we can begin the row at 3
            transaction.Add(new Transaction(GetFieldValue(ref i, ref xlRange, headerTransaction.AccountNumberColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerTransaction.NameColNum, ref headerTransaction),
                GetFieldValue(ref i, ref xlRange, headerTransaction.DateColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerTransaction.CampaignColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerTransaction.MiniCampaignColNum, ref headerTransaction),
                 GetFieldValue(ref i, ref xlRange, headerTransaction.FundColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerTransaction.TypeColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerTransaction.MethodColNum, ref headerTransaction),
                  GetFieldValue(ref i, ref xlRange, headerTransaction.AmountColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerTransaction.MarketValueColNum, ref headerTransaction), GetFieldValue(ref i, ref xlRange, headerTransaction.InKindDescrColNum, ref headerTransaction)));
        }

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



        private static void GetHeader(ref int colCount, ref Excel.Range xlRange, ref int i, ref BloomerangColumnHeaderConstituents headerConstituents, ref BloomerangColumnHeaderTransaction headerTransaction, ref CharityproudHeaderConstituents headerCharityproud)
        {
            string headerName;

            for (int j = 1; j <= colCount; j++)
            {
                //new line
                if (i == 1)
                {
                    headerName = xlRange.Cells[i, j].Value2.ToString();
                    headerName = headerName.Trim().ToLower();
                    AssignHeaderCol(ref headerName, ref j, ref headerConstituents, ref headerTransaction, ref headerCharityproud);
                    //Console.Write("\r\n");

                }



                //if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                //    Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");
            }
        }

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
        public int MarketValueColNum { get; set; }
        public int InKindDescrColNum { get; set; }
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
