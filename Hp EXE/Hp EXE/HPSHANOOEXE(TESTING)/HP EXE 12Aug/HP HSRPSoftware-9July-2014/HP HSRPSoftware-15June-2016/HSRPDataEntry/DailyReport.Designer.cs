namespace HSRPDataEntryNew
{
    partial class DailyReport
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnexporttoexcel = new System.Windows.Forms.Button();
            this.btnpdfprint = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.todate = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.BtnSave = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.FromDate = new System.Windows.Forms.DateTimePicker();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.groupBox1.Controls.Add(this.btnexporttoexcel);
            this.groupBox1.Controls.Add(this.btnpdfprint);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.todate);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.dataGridView2);
            this.groupBox1.Controls.Add(this.BtnSave);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.FromDate);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(984, 488);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // btnexporttoexcel
            // 
            this.btnexporttoexcel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnexporttoexcel.Location = new System.Drawing.Point(674, 21);
            this.btnexporttoexcel.Name = "btnexporttoexcel";
            this.btnexporttoexcel.Size = new System.Drawing.Size(133, 27);
            this.btnexporttoexcel.TabIndex = 34;
            this.btnexporttoexcel.Text = "Report In Excel";
            this.btnexporttoexcel.UseVisualStyleBackColor = true;
            this.btnexporttoexcel.Click += new System.EventHandler(this.btnexporttoexcel_Click);
            // 
            // btnpdfprint
            // 
            this.btnpdfprint.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnpdfprint.Location = new System.Drawing.Point(535, 22);
            this.btnpdfprint.Name = "btnpdfprint";
            this.btnpdfprint.Size = new System.Drawing.Size(133, 28);
            this.btnpdfprint.TabIndex = 33;
            this.btnpdfprint.Text = "Report In PDF";
            this.btnpdfprint.UseVisualStyleBackColor = true;
            this.btnpdfprint.Click += new System.EventHandler(this.btnpdfprint_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 18);
            this.label1.TabIndex = 32;
            this.label1.Text = "To Date";
            // 
            // todate
            // 
            this.todate.Location = new System.Drawing.Point(95, 52);
            this.todate.Name = "todate";
            this.todate.Size = new System.Drawing.Size(220, 22);
            this.todate.TabIndex = 31;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Tomato;
            this.button1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(813, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(79, 28);
            this.button1.TabIndex = 29;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView2.Location = new System.Drawing.Point(3, 84);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(978, 401);
            this.dataGridView2.TabIndex = 28;
            // 
            // BtnSave
            // 
            this.BtnSave.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.BtnSave.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSave.Location = new System.Drawing.Point(371, 21);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(158, 29);
            this.BtnSave.TabIndex = 26;
            this.BtnSave.Text = "Go";
            this.BtnSave.UseVisualStyleBackColor = false;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 18);
            this.label4.TabIndex = 7;
            this.label4.Text = "From Date";
            // 
            // FromDate
            // 
            this.FromDate.Location = new System.Drawing.Point(95, 18);
            this.FromDate.Name = "FromDate";
            this.FromDate.Size = new System.Drawing.Size(220, 22);
            this.FromDate.TabIndex = 2;
            // 
            // DailyReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 496);
            this.Controls.Add(this.groupBox1);
            this.Name = "DailyReport";
            this.Text = "Daily Report";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker FromDate;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker todate;
        private System.Windows.Forms.Button btnpdfprint;
        private System.Windows.Forms.Button btnexporttoexcel;

    }
}