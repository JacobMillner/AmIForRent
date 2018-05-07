using ClosedXML.Excel;
using Dapper;
using ExcelLibrary.SpreadSheet;
using HouseStatusScraper.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HouseStatusScraper
{
	class Report
	{
		public static void ExecuteAndSaveReport(string filePath)
		{
			// create a new DataTable and set the columns
			DataTable newDataTable = new DataTable();
			newDataTable.Columns.Add("Website Name");
			newDataTable.Columns.Add("Status");
			newDataTable.Columns.Add("Date");

			// query our SiteData Table
			SQLiteConnection database = DatabaseUtils.ConnectToDatabase();
			List<SiteData> scrapes = database.Query<SiteData>("SELECT * FROM SiteData").ToList();

			if (scrapes != null)
			{
				foreach (SiteData sData in scrapes)
				{
					DataRow newRow = newDataTable.NewRow();
					newRow["Website Name"] = sData.WebsiteName;
					newRow["Status"] = sData.Status;
					newRow["Date"] = sData.Date;
					newRow.Table.Rows.Add(newRow);
				}
			}
			else
			{
				// TODO Throw error
			}
			string fileName = "ScrapeReport.xlsx";
			if (CreateXLS(newDataTable, filePath + "\\" + fileName))
			{
				MessageBox.Show("File '" + fileName + "' saved to location: " + filePath);
			}
			else
			{
				MessageBox.Show("Error Saving Report!");
			}
		}

		public static bool CreateXLS(DataTable data, string filePath)
		{
			bool sucsess = true;
			try
			{
				XLWorkbook wb = new XLWorkbook();
				DataTable dt = data;
				wb.Worksheets.Add(dt, "WorkSheet");
				wb.SaveAs(filePath);				
			}
			catch (Exception ex)
			{
				sucsess = false;
			}
			return sucsess;
		}
	}
}
