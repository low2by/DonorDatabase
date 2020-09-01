using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace Donor
{
    class Program
    {
        //Individual constituent;
        static void Main(string[] args)
        {
            GetExcelFile();

            Console.Read();
        }

        public static void GetExcelFile()
        {

            //Create COM Objects. Create a COM object for everything that is referenced
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\elotubai10\Desktop\All_Individuals_Organizations_correct_information_bloomerang.xlsx");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            //this is for testing. delete leter
            rowCount = 3;
            //colCount = 1;

            //iterate over the rows and columns and print to the console as it appears in the file
            //excel is not zero based!!
            for (int i = 1; i <= rowCount; i++)
            {
                for (int j = 1; j <= colCount; j++)
                {
                    //new line
                    //if (j == 1)
                    //    Console.Write("\r\n");

                    //write the value to the console
                    string cellString = xlRange.Cells[i, j].Value2.ToString();
                    cellString = cellString.Replace("\n", "").Replace("\r", "");
                    Console.Write(cellString + "\t");
                    //if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                    //    Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");
                }
                Console.Write("\r\n\r\n");
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
                        Console.Write("\r\n");

                    //write the value to the console
                    Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");
                    //if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                    //    Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");
                }
            }
        }

        public static void BloomerangData()
        {

        }

        public static void CharityProudData()
        {

        }
    }

    public class BloomerangColumnHeaderConstituents
    {
        private int nameColNum = 1;
        private int firstNameColNum = 2;
        private int lastNameColNum = 3;
        private int accountNumColNum = 4;
        private int cityAddressColNum = 5;
        private int cityColNum = 6;
        private int stateColNum = 7;
        private int zipCodeColNum = 8;
        private int emailColNum = 9;
        private int typeColNum = 10;

        public int NameColNum { get => nameColNum; }
        public int FirstNameColNum { get => firstNameColNum; }
        public int LastNameColNum { get => lastNameColNum; }
        public int AccountNumColNum { get => accountNumColNum; }
        public int CityAddressColNum { get => cityAddressColNum;  }
        public int CityColNum { get => cityColNum; }
        public int StateColNum { get => stateColNum; }
        public int ZipCodeColNum { get => zipCodeColNum; }
        public int EmailColNum { get => emailColNum; }
        public int TypeColNum { get => typeColNum; }

    }

    public class BloomerangColumnHeaderTransaction
    {
        private int nameColNum = 1;
        private int dateColNum = 2;
        private int campaignColNum = 3;
        private int miniCampaignColNum = 4;
        private int fundColNum = 5;
        private int typeColNum = 6;
        private int methodColNum = 7;
        private int amountColNum = 8;
        private int accountNumberColNum = 9;

        public int NameColNum { get => nameColNum; }
        public int DateColNum { get => dateColNum; }
        public int CampaignColNum { get => campaignColNum; }
        public int MiniCampaignColNum { get => miniCampaignColNum; }
        public int FundColNum { get => fundColNum; }
        public int TypeColNum { get => typeColNum; }
        public int MethodColNum { get => methodColNum; }
        public int AmountColNum { get => amountColNum; }
        public int AccountNumberColNum { get => accountNumberColNum; }

    }
}
