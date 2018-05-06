namespace HouseStatusScraper
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnScrape = new System.Windows.Forms.Button();
			this.LastRunDate = new System.Windows.Forms.Label();
			this.btnReport = new System.Windows.Forms.Button();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.CurLoading = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnScrape
			// 
			this.btnScrape.Location = new System.Drawing.Point(96, 71);
			this.btnScrape.Name = "btnScrape";
			this.btnScrape.Size = new System.Drawing.Size(107, 59);
			this.btnScrape.TabIndex = 0;
			this.btnScrape.Text = "Scrape";
			this.btnScrape.UseVisualStyleBackColor = true;
			this.btnScrape.Click += new System.EventHandler(this.btnScrape_Click);
			// 
			// LastRunDate
			// 
			this.LastRunDate.AutoSize = true;
			this.LastRunDate.Location = new System.Drawing.Point(12, 28);
			this.LastRunDate.Name = "LastRunDate";
			this.LastRunDate.Size = new System.Drawing.Size(125, 13);
			this.LastRunDate.TabIndex = 1;
			this.LastRunDate.Text = "Last Scrape Date: Never";
			// 
			// btnReport
			// 
			this.btnReport.Location = new System.Drawing.Point(96, 150);
			this.btnReport.Name = "btnReport";
			this.btnReport.Size = new System.Drawing.Size(107, 59);
			this.btnReport.TabIndex = 2;
			this.btnReport.Text = "View Report";
			this.btnReport.UseVisualStyleBackColor = true;
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(12, 237);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(279, 23);
			this.progressBar1.TabIndex = 3;
			// 
			// CurLoading
			// 
			this.CurLoading.AutoSize = true;
			this.CurLoading.Location = new System.Drawing.Point(131, 221);
			this.CurLoading.Name = "CurLoading";
			this.CurLoading.Size = new System.Drawing.Size(16, 13);
			this.CurLoading.TabIndex = 4;
			this.CurLoading.Text = "---";
			this.CurLoading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(303, 263);
			this.Controls.Add(this.CurLoading);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.btnReport);
			this.Controls.Add(this.LastRunDate);
			this.Controls.Add(this.btnScrape);
			this.Name = "Form1";
			this.Text = "Am I For Rent?";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnScrape;
		private System.Windows.Forms.Label LastRunDate;
		private System.Windows.Forms.Button btnReport;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label CurLoading;
	}
}

