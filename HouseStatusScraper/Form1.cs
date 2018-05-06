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

		public Form1()
		{
			InitializeComponent();
			// grab the user settings
			XmlSerializer mySerializer = new XmlSerializer(typeof(UserSettings));
			FileStream userSettingsFileStream = new FileStream("UserSettings.xml", FileMode.Open);
			UserSettings settings = (UserSettings)mySerializer.Deserialize(userSettingsFileStream);
			userSettingsFileStream.Close();

			address = settings.Address;
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

			DatabaseUtils.UpdateScraperStats();
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
	}
}
