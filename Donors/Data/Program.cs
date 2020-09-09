﻿using System;
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
        private static BloomerangColumnHeaderConstituents header;
        private static List<Constituents> constituents;
        static void Main(string[] args)
        {
            header = new BloomerangColumnHeaderConstituents();
            constituents = new List<Constituents>();
            GetExcelFile();

            foreach(Constituents name in constituents)
            {
                Console.WriteLine("This is the person: "+name.GetName()+"\t"+"The Constituent is a/an: "+name.GetTypeOfConstituent());
            }

            Console.Read();
        }

        public static void GetExcelFile()
        {

            //Create COM Objects. Create a COM object for everything that is referenced
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\elotubai10\Desktop\all_Individuals_Organizations_proper_information.xlsx");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            //this is for testing. delete leter
            rowCount = 10;
            //colCount = 1;

            string cellString;


            //iterate over the rows and columns and print to the console as it appears in the file
            //excel is not zero based!!
            for (int i = 1; i <= rowCount; i++)
            {
                if (i == 1)
                {
                    GetHeader(ref colCount, ref xlRange, ref i);
                    continue;
                }

                SetIndividualConstituentsFields(ref i, ref xlRange);

                for (int j = 1; j <= colCount; j++)
                {
                    //if (i == 1)
                    //    GetHeader(ref colCount, ref xlRange, ref i);

                    //SetIndividualConstituentsFields(ref i, ref xlRange);
                    //write the value to the console
                    if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                    {
                        cellString = xlRange.Cells[i, j].Value2.ToString();
                        cellString = cellString.Replace("\n", "").Replace("\r", "");
                        //indivConstituents.Add(new Individual())
                    }
                    else
                    {
                        cellString = "";

                    }

                    Console.Write(cellString + "\t");



                }
                Console.Write("\r\n\r\n");
                //Console.Write(header.FirstNameColNum.ToString() + "\r\n\r\n"); //for test, delete later
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

        private static void SetIndividualConstituentsFields(ref int i, ref Excel.Range xlRange)
        {
            constituents.Add(new Constituents(GetFieldValue(ref i, ref xlRange, header.AccountNumColNum), 
                GetFieldValue(ref i, ref xlRange, header.NameColNum), GetFieldValue(ref i, ref xlRange, header.LastNameColNum), GetFieldValue(ref i, ref xlRange, header.FirstNameColNum), 
                GetFieldValue(ref i, ref xlRange, header.CityAddressColNum), GetFieldValue(ref i, ref xlRange, header.CityColNum), GetFieldValue(ref i, ref xlRange, header.StateColNum), GetFieldValue(ref i, ref xlRange, header.ZipCodeColNum),
                GetFieldValue(ref i, ref xlRange, header.PhoneColNum), GetFieldValue(ref i, ref xlRange, header.EmailColNum),
                GetFieldValue(ref i, ref xlRange, header.TypeColNum)));

           
        }

        private static string GetFieldValue(ref int i, ref Excel.Range xlRange, int _colNum)
        {
            string name;
            int colNum = _colNum;

            if (xlRange.Cells[i, colNum] != null && xlRange.Cells[i, colNum].Value2 != null)
            {
                name = xlRange.Cells[i, colNum].Value2.ToString();
                name = name.Replace("\n", "").Replace("\r", "");
                //indivConstituents.Add(new Individual())
            }
            else
            {
                name = "";

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
                header.NameColNum = j;
            }

            if (headerName.Contains("last") && headerName.Contains("name"))
            {
                header.LastNameColNum = j;
            }

            if (headerName.Contains("first") && headerName.Contains("name"))
            {
                header.FirstNameColNum = j;
            }

            if (headerName.Contains("account") && headerName.Contains("number"))
            {
                header.AccountNumColNum = j;
            }

            if (headerName.Contains("primary") && headerName.Contains("street"))
            {
                header.CityAddressColNum = j;
            }

            if (headerName.Contains("primary") && headerName.Contains("city"))
            {
                header.CityColNum = j;
            }

            if (headerName.Contains("primary") && headerName.Contains("state"))
            {
                header.StateColNum = j;
            }

            if (headerName.Contains("primary") && headerName.Contains("zip") && headerName.Contains("code"))
            {
                header.ZipCodeColNum = j;
            }

            if (headerName.Contains("primary") && headerName.Contains("email") && headerName.Contains("address"))
            {
                header.EmailColNum = j;
            }

            if (headerName.Equals("type"))
            {
                header.TypeColNum = j;
            }

            if (headerName.Contains("primary") && headerName.Contains("phone") && headerName.Contains("number"))
            {
                header.PhoneColNum = j;
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

        public static void BloomerangData()
        {

        }

        public static void CharityProudData()
        {

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
}
