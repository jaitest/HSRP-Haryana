namespace HSRPDataEntryNew
{
    partial class Search
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
            this.btnCollection = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTodayCollection = new System.Windows.Forms.Label();
            this.lblEntryCount = new System.Windows.Forms.Label();
            this.lblShrinkingServer = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label54 = new System.Windows.Forms.Label();
            this.txtCashReceiptNo = new System.Windows.Forms.TextBox();
            this.label60 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCollection
            // 
            this.btnCollection.BackColor = System.Drawing.Color.Tomato;
            this.btnCollection.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCollection.Location = new System.Drawing.Point(713, 12);
            this.btnCollection.Name = "btnCollection";
            this.btnCollection.Size = new System.Drawing.Size(157, 36);
            this.btnCollection.TabIndex = 0;
            this.btnCollection.Text = "Today\'s Summary";
            this.btnCollection.UseVisualStyleBackColor = false;
            this.btnCollection.Click += new System.EventHandler(this.btnCollection_Click);
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
            this.panel1.Location = new System.Drawing.Point(20, 54);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(979, 170);
            this.panel1.TabIndex = 5;
            // 
            // lblTodayCollection
            // 
            this.lblTodayCollection.AutoSize = true;
            this.lblTodayCollection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTodayCollection.Location = new System.Drawing.Point(401, 120);
            this.lblTodayCollection.Name = "lblTodayCollection";
            this.lblTodayCollection.Size = new System.Drawing.Size(0, 13);
            this.lblTodayCollection.TabIndex = 5;
            // 
            // lblEntryCount
            // 
            this.lblEntryCount.AutoSize = true;
            this.lblEntryCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEntryCount.Location = new System.Drawing.Point(401, 79);
            this.lblEntryCount.Name = "lblEntryCount";
            this.lblEntryCount.Size = new System.Drawing.Size(0, 13);
            this.lblEntryCount.TabIndex = 4;
            // 
            // lblShrinkingServer
            // 
            this.lblShrinkingServer.AutoSize = true;
            this.lblShrinkingServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShrinkingServer.Location = new System.Drawing.Point(401, 43);
            this.lblShrinkingServer.Name = "lblShrinkingServer";
            this.lblShrinkingServer.Size = new System.Drawing.Size(0, 13);
            this.lblShrinkingServer.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(91, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Todays Collection :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(91, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Todays Entry Count : ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(91, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(264, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Total Records Pending to Sync From Server :";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.LimeGreen;
            this.button2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button2.Location = new System.Drawing.Point(561, 297);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(97, 31);
            this.button2.TabIndex = 106;
            this.button2.Text = "Search";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label54.ForeColor = System.Drawing.Color.Red;
            this.label54.Location = new System.Drawing.Point(253, 306);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(17, 22);
            this.label54.TabIndex = 105;
            this.label54.Text = "*";
            // 
            // txtCashReceiptNo
            // 
            this.txtCashReceiptNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCashReceiptNo.Location = new System.Drawing.Point(274, 308);
            this.txtCashReceiptNo.MaxLength = 30;
            this.txtCashReceiptNo.Name = "txtCashReceiptNo";
            this.txtCashReceiptNo.Size = new System.Drawing.Size(187, 20);
            this.txtCashReceiptNo.TabIndex = 103;
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label60.Location = new System.Drawing.Point(57, 310);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(179, 18);
            this.label60.TabIndex = 104;
            this.label60.Text = "Vehicle Registration No :";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 337);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(1000, 142);
            this.dataGridView1.TabIndex = 107;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.YellowGreen;
            this.button1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button1.Location = new System.Drawing.Point(679, 297);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(191, 31);
            this.button1.TabIndex = 108;
            this.button1.Text = "Update SMS Response";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Search
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1011, 524);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label54);
            this.Controls.Add(this.txtCashReceiptNo);
            this.Controls.Add(this.label60);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnCollection);
            this.Name = "Search";
            this.Text = "Search";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCollection;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTodayCollection;
        private System.Windows.Forms.Label lblEntryCount;
        private System.Windows.Forms.Label lblShrinkingServer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.TextBox txtCashReceiptNo;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
    }
}