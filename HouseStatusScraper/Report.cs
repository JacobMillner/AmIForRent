using Dapper;
using HouseStatusScraper.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseStatusScraper
{
	class Report
	{
		public void ExecuteAndSaveReport(string filePath)
		{
			// create a new DataTable and set the columns
			DataTable newData = new DataTable();
			newData.Columns.Add("Website Name");
			newData.Columns.Add("Status");
			newData.Columns.Add("Date");
			newData.Columns.Add("HTML");

			// query our SiteData Table
			SQLiteConnection database = DatabaseUtils.ConnectToDatabase();
			List<SiteData> stats = database.Query<SiteData>("SELECT * FROM SiteData").ToList();

			if (stats != null)
			{

			}
			else
			{
				// TODO Throw error
			}


			CreateCSV(newData, filePath);
		}

		private void CreateCSV(DataTable dataTable, string filePath, string delimiter = ",")
		{
			if (!Directory.Exists(Path.GetDirectoryName(filePath)))
				throw new DirectoryNotFoundException($"Destination folder not found: {filePath}");

			DataColumn[] columns = dataTable.Columns.Cast<DataColumn>().ToArray();
			List<string> lines = new List<string>();
			lines.Add(string.Join(delimiter, columns.Select(c => c.ColumnName)));
			lines.AddRange(dataTable.Rows.Cast<DataRow>().Select(row => string.Join(delimiter, columns.Select(c => row[c]))));
			File.WriteAllLines(filePath, lines);
		}
	}
}
