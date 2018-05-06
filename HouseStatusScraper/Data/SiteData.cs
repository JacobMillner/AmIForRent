using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseStatusScraper.Data
{
	public class SiteData
	{
		public string WebsiteName { get; set; }
		public string Status { get; set; }
		public DateTime Date { get; set; }
		public string HTML { get; set; }
	}
}
