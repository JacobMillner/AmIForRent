using IronWebScraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseStatusScraper.Scrapers
{
	class Zillow : WebScraper
	{
		public IProgress<int> Progress { get; set; }
		public override void Init()
		{
			this.LoggingLevel = WebScraper.LogLevel.All;
			HttpIdentity foo = new HttpIdentity();
			foo.HttpRequestHeaders = new Dictionary<string, string>()
			{
				{ "authority", "www.zillow.com" },
				{"method","GET"},
				{"path","/homedetails/20-Plumwood-Rd-Dayton-OH-45409/35082326_zpid/?fullpage=true"},
				{"scheme", "https"},
				{"accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8"},
				{"accept-encoding", "gzip, deflate, br"},
				{"accept-language", "en-US,en;q=0.9"},
				{"cache-control", "max-age=0"},
				{"referer", "https://www.zillow.com/homedetails/20-Plumwood-Rd-Dayton-OH-45409/35082326_zpid/"},
				{"upgrade-insecure-requests","1"},
				{"user-agent","Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.139 Safari/537.36"},
			};
			this.Request("https://www.zillow.com/homedetails/20-Plumwood-Rd-Dayton-OH-45409/35082326_zpid/", Parse, identity: foo);
		}

		public override void Parse(Response response)
		{
			for (int j = 0; j < 100000; j++)
			{
				//Caluculate(j);

				// Use progress to notify UI thread that progress has
				// changed
				if (Progress != null)
					Progress.Report((j + 1) * 100 / 100000);
			}

			foreach (var title_link in response.Css("estimates"))
			{
				string strTitle = title_link.TextContentClean;
				Scrape(new ScrapedData() { { "Title", strTitle } });
			}

			if (response.CssExists("div.prev-post > a[href]"))
			{
				var next_page = response.Css("div.prev-post > a[href]")[0].Attributes["href"];
				this.Request(next_page, Parse);
			}
		}
	}
}
