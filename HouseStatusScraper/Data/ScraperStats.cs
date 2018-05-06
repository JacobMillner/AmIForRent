using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseStatusScraper.Data
{
	public class ScraperStats
	{
		public DateTime LastScrapeDate { get; set; }
		public int TotalScrapes { get; set; }
	}
}
