using Dapper;
using HouseStatusScraper.Data;
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
			string siteDataSQL = "CREATE TABLE SiteData (ID INTEGER PRIMARY KEY AUTOINCREMENT, WebsiteName VARCHAR, Status VARCHAR, Date DATETIME, HTML VARCHAR)";	
			SQLiteCommand siteDataCommand = new SQLiteCommand(siteDataSQL, m_dbConnection);
			siteDataCommand.ExecuteNonQuery();

			string scraperStatsSQL = "CREATE TABLE ScraperStats (ID INTEGER PRIMARY KEY AUTOINCREMENT, LastScrapeDate DATETIME, TotalScrapes INTEGER)";
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

		public static void UpdateScraperStats()
		{
			SQLiteConnection database = ConnectToDatabase();
			string currentDate = DateTime.Now.ToString();
			int totalScrapes = 1;

			ScraperStats stats = database.Query<ScraperStats>("SELECT * FROM ScraperStats ORDER BY LastScrapeDate").FirstOrDefault();
			if (stats != null)
			{
				//check the stats and remove the old entry
				totalScrapes = stats.TotalScrapes + 1;
				database.Execute("DELETE FROM ScraperStats");
			}

			// insert our new stats record
			ScraperStats newStats = new ScraperStats();
			newStats.LastScrapeDate = DateTime.Now;
			newStats.TotalScrapes = totalScrapes;
			database.Execute("INSERT INTO ScraperStats (LastScrapeDate, TotalScrapes) VALUES (@LastScrapeDate, @TotalScrapes)", newStats);

			database.Close();
		}
	}
}
