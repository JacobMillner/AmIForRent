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

			setCurLoadingTextSafe("sibcycline.com...");
			SibcyCline scraperSibcycline = new SibcyCline();
			scraperSibcycline.Progress = progress;
			scraperSibcycline.Start();

			setCurLoadingTextSafe("movoto.com...");
			Movoto scraperMovoto = new Movoto();
			scraperMovoto.Progress = progress;
			scraperMovoto.Start();

			setCurLoadingTextSafe("trulia.com...");
			Trulia scraperTrulia = new Trulia();
			scraperTrulia.Progress = progress;
			scraperTrulia.Start();

			setCurLoadingTextSafe("remax.com...");
			Remax scraperRemax = new Remax();
			scraperRemax.Progress = progress;
			scraperRemax.Start();

			// this site doesnt work
			//setCurLoadingTextSafe("irongaterentals.com...");
			//IronGateRealtors scraperIron = new IronGateRealtors();
			//scraperIron.Progress = progress;
			//scraperIron.Start();

			// this site doesnt work
			//setCurLoadingTextSafe("forsalebyowner.com...");
			//ForSaleByOwner scraperFSBO = new ForSaleByOwner();
			//scraperFSBO.Progress = progress;
			//scraperFSBO.Start();

			setCurLoadingTextSafe("hotpads.com...");
			Hotpads scraperHotpads = new Hotpads();
			scraperHotpads.Progress = progress;
			scraperHotpads.Start();

			setCurLoadingTextSafe("craigslist.com...");
			Craigslist scraperCraigslist = new Craigslist();
			scraperCraigslist.Progress = progress;
			scraperCraigslist.Start();

			setCurLoadingTextSafe("apartmentfinder.com...");
			ApartmentFinder scraperApartmentFinder = new ApartmentFinder();
			scraperApartmentFinder.Progress = progress;
			scraperApartmentFinder.Start();

			setCurLoadingTextSafe("apartmenthomeliving.com...");
			ApartmentHomeLiving scraperApartmentLiving = new ApartmentHomeLiving();
			scraperApartmentLiving.Progress = progress;
			scraperApartmentLiving.Start();

			setCurLoadingTextSafe("apartments.com...");
			Apartments scraperApartments = new Apartments();
			scraperApartments.Progress = progress;
			scraperApartments.Start();

			// update our scraper stats and change the labels
			DatabaseUtils.UpdateScraperStats();
			updateScraperStatsLabels();

			// reset the progress bar
			if (progress != null)
				progress.Report(0);
			setCurLoadingTextSafe("---");
			MessageBox.Show(@"Scrape Complete!" + "\n"
				+ "----------------------" + "\n"
				+ "hotpads.com: " + scraperHotpads.Status.ToString() + "\n"
				+ "craigslist.com: " + scraperRedfin.Status.ToString() + "\n"
				+ "sibcycline.com: " + scraperSibcycline.Status.ToString() + "\n"
				+ "movoto.com: " + scraperMovoto.Status.ToString() + "\n"
				+ "trulia.com: " + scraperTrulia.Status.ToString() + "\n"
				+ "remax.com: " + scraperRemax.Status.ToString() + "\n"
				+ "hotpads.com: " + scraperHotpads.Status.ToString() + "\n"
				+ "craigslist.com: " + scraperCraigslist.Status.ToString() + "\n"
				+ "apartmentfinder.com: " + scraperApartmentFinder.Status.ToString() + "\n"
				+ "apartmenthomeliving.com: " + scraperApartmentLiving.Status.ToString() + "\n"
				+ "apartments.com: " + scraperApartments.Status.ToString() + "\n"
				);
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

		private void btnReport_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			fbd.Description = "Select Report Export Folder";

			if (fbd.ShowDialog() == DialogResult.OK)
			{
				string sSelectedPath = fbd.SelectedPath;
				Report.ExecuteAndSaveReport(fbd.SelectedPath);
			}
		}
	}
}
