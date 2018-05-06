using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseStatusScraper
{
	public class DatabaseUtils
	{
		public static void SetupDatabase()
		{
			// create and then connect to the new sqlite db
			SQLiteConnection.CreateFile("Database.sqlite");
			SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=Database.sqlite;Version=3;");
			m_dbConnection.Open();

			// create the tables
			string siteDataSQL = "CREATE TABLE SiteData (WebsiteName VARCHAR, Status VARCHAR, Date DATETIME, HTML VARCHAR)";	
			SQLiteCommand siteDataCommand = new SQLiteCommand(siteDataSQL, m_dbConnection);
			siteDataCommand.ExecuteNonQuery();

			string scraperStatsSQL = "CREATE TABLE ScraperStats (LastScrapeDate DATETIME, TotalScrapes int)";
			SQLiteCommand scraperStatsCommand = new SQLiteCommand(scraperStatsSQL, m_dbConnection);
			scraperStatsCommand.ExecuteNonQuery();

			m_dbConnection.Close();
		}

		public static SQLiteConnection ConnectToDatabase()
		{
			SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=Database.sqlite;Version=3;");
			m_dbConnection.Open();

			return m_dbConnection;
		}
	}
}
