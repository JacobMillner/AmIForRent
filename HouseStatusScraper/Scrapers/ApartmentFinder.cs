using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using HouseStatusScraper.Data;
using HouseStatusScraper.Enums;
using IronWebScraper;

namespace HouseStatusScraper.Scrapers
{
	class ApartmentFinder : WebScraper
	{
		public IProgress<int> Progress { get; set; }
		public HouseStatus Status { get; set; }
		private string PageHTML { get; set; }
		private string WebsiteName { get; set; }

		public override void Init()
		{
			this.LoggingLevel = WebScraper.LogLevel.All;
			this.Request("https://www.apartmentfinder.com/Off-Campus-Housing/Ohio/Apartments-Near-University-Of-Dayton-76wt4ey/q/?sr=7", Parse);
			WebsiteName = "apartmentfinder.com";
		}

		public override void Parse(Response response)
		{
			string html = response.Html;
			PageHTML = html;
			// split out our lines so we can show progress
			string[] lines = html.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

			int totalLines = lines.Count();
			int i = 0;
			foreach (string line in lines)
			{
				// send progress to the progress bar
				if (Progress != null)
					Progress.Report((i + 1) * 100 / totalLines);

				// check for specific texts
				if (line.Contains("20 Plumwood"))
				{
					Status = HouseStatus.ForRent;
					if (Progress != null)
						Progress.Report(totalLines * 100 / totalLines);
					break;
				}
				i++;
			}

			// craigslist special case
			if (Status == HouseStatus.None)
			{
				Status = HouseStatus.NotListed;
			}

			if (Status != HouseStatus.None)
			{
				// log it in the db
				WriteLogToDatabase();
			}
			else
			{
				// TODO log an error
			}
		}

		private void WriteLogToDatabase()
		{
			// open our db connection
			SQLiteConnection database = DatabaseUtils.ConnectToDatabase();

			// insert our new stats record
			SiteData newSiteData = new SiteData();
			newSiteData.WebsiteName = WebsiteName;
			newSiteData.Status = Status.ToString();
			newSiteData.Date = DateTime.Now;
			newSiteData.HTML = PageHTML;
			database.Execute("INSERT INTO SiteData (WebsiteName, Status, Date, HTML) VALUES (@WebsiteName, @Status, @Date, @HTML)", newSiteData);

			// close our db connection
			database.Close();
		}
	}
}

