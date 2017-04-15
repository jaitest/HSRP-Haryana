namespace HSRPDataEntryNew
{
    partial class MDIForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MDIForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cashReceiptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orderEmbossingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orderClosedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTodayCollection = new System.Windows.Forms.Label();
            this.lblEntryCount = new System.Windows.Forms.Label();
            this.lblShrinkingServer = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Emborss_Done = new System.Windows.Forms.Timer(this.components);
            this.Cashcollection_Done = new System.Windows.Forms.Timer(this.components);
            this.Closed_Done = new System.Windows.Forms.Timer(this.components);
            this.CashCollection = new System.Windows.Forms.WebBrowser();
            this.ClosingDone = new System.Windows.Forms.WebBrowser();
            this.EmborssingDone = new System.Windows.Forms.WebBrowser();
            this.PullTimer = new System.Windows.Forms.Timer(this.components);
            this.PushTimer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.LightSteelBlue;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.transactionToolStripMenuItem,
            this.reportToolStripMenuItem,
            this.helpMenu});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1354, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "MenuStrip";
            // 
            // fileMenu
            // 
            this.fileMenu.BackColor = System.Drawing.Color.CadetBlue;
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator3,
            this.exitToolStripMenuItem});
            this.fileMenu.ImageTransparentColor = System.Drawing.SystemColors.ActiveBorder;
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(37, 20);
            this.fileMenu.Text = "&File";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(89, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.BackColor = System.Drawing.Color.CadetBlue;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // transactionToolStripMenuItem
            // 
            this.transactionToolStripMenuItem.BackColor = System.Drawing.Color.CadetBlue;
            this.transactionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cashReceiptToolStripMenuItem,
            this.orderEmbossingToolStripMenuItem,
            this.orderClosedToolStripMenuItem});
            this.transactionToolStripMenuItem.Name = "transactionToolStripMenuItem";
            this.transactionToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.transactionToolStripMenuItem.Text = "Transaction ";
            // 
            // cashReceiptToolStripMenuItem
            // 
            this.cashReceiptToolStripMenuItem.BackColor = System.Drawing.Color.CadetBlue;
            this.cashReceiptToolStripMenuItem.Name = "cashReceiptToolStripMenuItem";
            this.cashReceiptToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.cashReceiptToolStripMenuItem.Text = "Cash Receipt";
            this.cashReceiptToolStripMenuItem.Click += new System.EventHandler(this.MDIForm_Load);
            // 
            // orderEmbossingToolStripMenuItem
            // 
            this.orderEmbossingToolStripMenuItem.Name = "orderEmbossingToolStripMenuItem";
            this.orderEmbossingToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.orderEmbossingToolStripMenuItem.Text = "Order Embossing";
            this.orderEmbossingToolStripMenuItem.Click += new System.EventHandler(this.orderEmbossingToolStripMenuItem_Click_1);
            // 
            // orderClosedToolStripMenuItem
            // 
            this.orderClosedToolStripMenuItem.Name = "orderClosedToolStripMenuItem";
            this.orderClosedToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.orderClosedToolStripMenuItem.Text = "Order Closed";
            this.orderClosedToolStripMenuItem.Click += new System.EventHandler(this.orderClosedToolStripMenuItem_Click);
            // 
            // reportToolStripMenuItem
            // 
            this.reportToolStripMenuItem.BackColor = System.Drawing.Color.CadetBlue;
            this.reportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backUpToolStripMenuItem});
            this.reportToolStripMenuItem.Name = "reportToolStripMenuItem";
            this.reportToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.reportToolStripMenuItem.Text = "Report";
            // 
            // backUpToolStripMenuItem
            // 
            this.backUpToolStripMenuItem.Name = "backUpToolStripMenuItem";
            this.backUpToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.backUpToolStripMenuItem.Text = "Back Up";
            this.backUpToolStripMenuItem.Click += new System.EventHandler(this.backUpToolStripMenuItem_Click);
            // 
            // helpMenu
            // 
            this.helpMenu.BackColor = System.Drawing.Color.CadetBlue;
            this.helpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.indexToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.toolStripSeparator8,
            this.aboutToolStripMenuItem});
            this.helpMenu.Name = "helpMenu";
            this.helpMenu.Size = new System.Drawing.Size(44, 20);
            this.helpMenu.Text = "&Help";
            // 
            // contentsToolStripMenuItem
            // 
            this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            this.contentsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F1)));
            this.contentsToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.contentsToolStripMenuItem.Text = "&Contents";
            // 
            // indexToolStripMenuItem
            // 
            this.indexToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("indexToolStripMenuItem.Image")));
            this.indexToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black;
            this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
            this.indexToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.indexToolStripMenuItem.Text = "&Index";
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("searchToolStripMenuItem.Image")));
            this.searchToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black;
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.searchToolStripMenuItem.Text = "&Search";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(165, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.aboutToolStripMenuItem.Text = "&About ... ...";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 711);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1354, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "StatusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel.Text = "Status";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.CausesValidation = false;
            this.panel1.Controls.Add(this.lblTodayCollection);
            this.panel1.Controls.Add(this.lblEntryCount);
            this.panel1.Controls.Add(this.lblShrinkingServer);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Enabled = false;
            this.panel1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.panel1.Location = new System.Drawing.Point(1108, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(257, 82);
            this.panel1.TabIndex = 4;
            this.panel1.Visible = false;
            // 
            // lblTodayCollection
            // 
            this.lblTodayCollection.AutoSize = true;
            this.lblTodayCollection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTodayCollection.Location = new System.Drawing.Point(144, 58);
            this.lblTodayCollection.Name = "lblTodayCollection";
            this.lblTodayCollection.Size = new System.Drawing.Size(0, 13);
            this.lblTodayCollection.TabIndex = 5;
            // 
            // lblEntryCount
            // 
            this.lblEntryCount.AutoSize = true;
            this.lblEntryCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEntryCount.Location = new System.Drawing.Point(145, 33);
            this.lblEntryCount.Name = "lblEntryCount";
            this.lblEntryCount.Size = new System.Drawing.Size(0, 13);
            this.lblEntryCount.TabIndex = 4;
            // 
            // lblShrinkingServer
            // 
            this.lblShrinkingServer.AutoSize = true;
            this.lblShrinkingServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShrinkingServer.Location = new System.Drawing.Point(145, 11);
            this.lblShrinkingServer.Name = "lblShrinkingServer";
            this.lblShrinkingServer.Size = new System.Drawing.Size(0, 13);
            this.lblShrinkingServer.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(11, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Todays Collection :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Todays Entry Count : ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = " Shrink From Server :";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 100000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Emborss_Done
            // 
            this.Emborss_Done.Interval = 150000;
            this.Emborss_Done.Tick += new System.EventHandler(this.Emborss_Done_Tick_1);
            // 
            // Cashcollection_Done
            // 
            this.Cashcollection_Done.Interval = 120000;
            this.Cashcollection_Done.Tick += new System.EventHandler(this.Cashcollection_Done_Tick_1);
            // 
            // Closed_Done
            // 
            this.Closed_Done.Interval = 180000;
            this.Closed_Done.Tick += new System.EventHandler(this.Closed_Done_Tick_1);
            // 
            // CashCollection
            // 
            this.CashCollection.Location = new System.Drawing.Point(1251, 684);
            this.CashCollection.MinimumSize = new System.Drawing.Size(20, 20);
            this.CashCollection.Name = "CashCollection";
            this.CashCollection.Size = new System.Drawing.Size(20, 20);
            this.CashCollection.TabIndex = 11;
            this.CashCollection.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.CashCollection_DocumentCompleted);
            // 
            // ClosingDone
            // 
            this.ClosingDone.Location = new System.Drawing.Point(1296, 684);
            this.ClosingDone.MinimumSize = new System.Drawing.Size(20, 20);
            this.ClosingDone.Name = "ClosingDone";
            this.ClosingDone.Size = new System.Drawing.Size(20, 20);
            this.ClosingDone.TabIndex = 10;
            this.ClosingDone.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.ClosingDone_DocumentCompleted);
            // 
            // EmborssingDone
            // 
            this.EmborssingDone.Location = new System.Drawing.Point(1337, 684);
            this.EmborssingDone.MinimumSize = new System.Drawing.Size(20, 20);
            this.EmborssingDone.Name = "EmborssingDone";
            this.EmborssingDone.Size = new System.Drawing.Size(20, 20);
            this.EmborssingDone.TabIndex = 9;
            this.EmborssingDone.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.EmborssingDone_DocumentCompleted);
            // 
            // PullTimer
            // 
            this.PullTimer.Interval = 600000;
            // 
            // PushTimer
            // 
            this.PushTimer.Enabled = true;
            this.PushTimer.Interval = 900000;
            this.PushTimer.Tick += new System.EventHandler(this.PushTimer_Tick_1);
            // 
            // MDIForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(1354, 733);
            this.Controls.Add(this.CashCollection);
            this.Controls.Add(this.ClosingDone);
            this.Controls.Add(this.EmborssingDone);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MDIForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HSRP3.0.9";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MDIForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpMenu;
        private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripMenuItem transactionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cashReceiptToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTodayCollection;
        private System.Windows.Forms.Label lblEntryCount;
        private System.Windows.Forms.Label lblShrinkingServer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem reportToolStripMenuItem;
        private System.Windows.Forms.Timer Emborss_Done;
        private System.Windows.Forms.Timer Cashcollection_Done;
        private System.Windows.Forms.Timer Closed_Done;
        private System.Windows.Forms.WebBrowser CashCollection;
        private System.Windows.Forms.WebBrowser ClosingDone;
        private System.Windows.Forms.WebBrowser EmborssingDone;
        private System.Windows.Forms.Timer PullTimer;
        private System.Windows.Forms.Timer PushTimer;
        private System.Windows.Forms.ToolStripMenuItem orderEmbossingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem orderClosedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backUpToolStripMenuItem;
    }
}



