using Dapper;
using HouseStatusScraper.Data;
using HouseStatusScraper.Scrapers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace HouseStatusScraper
{
	public partial class Form1 : Form
	{
		private string address { get; set; }
		private DateTime lastScrapeDate { get; set; }
		private int numOfScrapes { get; set; }

		public Form1()
		{
			InitializeComponent();
			// grab the user settings
			XmlSerializer mySerializer = new XmlSerializer(typeof(UserSettings));
			FileStream userSettingsFileStream = new FileStream("UserSettings.xml", FileMode.Open);
			UserSettings settings = (UserSettings)mySerializer.Deserialize(userSettingsFileStream);
			userSettingsFileStream.Close();
			address = settings.Address;

			// set the last scrape label
			SQLiteConnection database = DatabaseUtils.ConnectToDatabase();
			ScraperStats stats = database.Query<ScraperStats>("SELECT * FROM ScraperStats ORDER BY LastScrapeDate").FirstOrDefault();
			if (stats != null)
			{
				// if we have stats - update the labels
				numOfScrapes = stats.TotalScrapes;
				lastScrapeDate = stats.LastScrapeDate;
				LastRunDate.Text = "Last Scrape Date: " + lastScrapeDate.ToShortDateString();
				TotalScraptes.Text = "Total Scrapes: " + numOfScrapes.ToString();
			}
			database.Close();
		}

		private async void btnScrape_Click(object sender, EventArgs e)
		{
			// check if already scraped today

			// setup our progress bar
			progressBar1.Maximum = 100;
			progressBar1.Step = 1;

			var progress = new Progress<int>(v =>
			{
				// This lambda is executed in context of UI thread,
				// so it can safely update form controls
				progressBar1.Value = v;
			});

			// start scrapin'
			await Task.Run(() => ScrapeIt(progress));
		}

		public void ScrapeIt(IProgress<int> progress)
		{
			setCurLoadingTextSafe("Realtor.com...");
			Realtor scraper = new Realtor();
			scraper.Progress = progress;
			scraper.Start();

            setCurLoadingTextSafe("redfin.com...");
            Redfin scraperRedfin = new Redfin();
            scraperRedfin.Progress = progress;
            scraperRedfin.Start();

            // update our scraper stats and change the labels
            DatabaseUtils.UpdateScraperStats();
			updateScraperStatsLabels();
		}

		/// <summary>
		/// Changes the currently loading label regardless of which thread you are in
		/// </summary>
		/// <param name="txt">Value you want to show</param>
		private void setCurLoadingTextSafe(string txt)
		{
			if (CurLoading.InvokeRequired)
			{ CurLoading.Invoke(new Action(() => CurLoading.Text = txt)); return; }
			CurLoading.Text = txt;
		}

		/// <summary>
		/// Updates the Scraper Stats labels
		/// </summary>
		private void updateScraperStatsLabels()
		{
			SQLiteConnection database = DatabaseUtils.ConnectToDatabase();
			ScraperStats stats = database.Query<ScraperStats>("SELECT * FROM ScraperStats ORDER BY LastScrapeDate").FirstOrDefault();
			if (stats != null)
			{
				// if we have stats - update the labels
				numOfScrapes = stats.TotalScrapes;
				lastScrapeDate = stats.LastScrapeDate;

				if (LastRunDate.InvokeRequired)
				{ LastRunDate.Invoke(new Action(() => LastRunDate.Text = "Last Scrape Date: " + lastScrapeDate.ToShortDateString())); }
				LastRunDate.Text = "Last Scrape Date: " + lastScrapeDate.ToShortDateString();

				if (TotalScraptes.InvokeRequired)
				{ TotalScraptes.Invoke(new Action(() => TotalScraptes.Text = "Total Scrapes: " + numOfScrapes.ToString())); }
				TotalScraptes.Text = "Total Scrapes: " + numOfScrapes.ToString();
			}

			database.Close();
			return;
		}
	}
}
