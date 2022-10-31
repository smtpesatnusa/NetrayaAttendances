
namespace SMTAttendance
{
    partial class AddShifts
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
            this.usernameLbl = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbShift = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.dateTimePickercf2Out = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickercf2In = new System.Windows.Forms.DateTimePicker();
            this.checkBoxCF2 = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimePickerMealOut = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickercf1Out = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerMealIn = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickercf1In = new System.Windows.Forms.DateTimePicker();
            this.checkBoxCF1 = new System.Windows.Forms.CheckBox();
            this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.saveBtn = new MaterialSkin.Controls.MaterialButton();
            this.SuspendLayout();
            // 
            // usernameLbl
            // 
            this.usernameLbl.AutoSize = true;
            this.usernameLbl.BackColor = System.Drawing.Color.Transparent;
            this.usernameLbl.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.usernameLbl.Location = new System.Drawing.Point(68, 494);
            this.usernameLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.usernameLbl.Name = "usernameLbl";
            this.usernameLbl.Size = new System.Drawing.Size(77, 19);
            this.usernameLbl.TabIndex = 220;
            this.usernameLbl.Text = "username";
            this.usernameLbl.Visible = false;
            // 
            // tbName
            // 
            this.tbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbName.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.tbName.Location = new System.Drawing.Point(233, 119);
            this.tbName.Margin = new System.Windows.Forms.Padding(4);
            this.tbName.MaxLength = 500;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(277, 26);
            this.tbName.TabIndex = 215;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.label2.Location = new System.Drawing.Point(59, 119);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 19);
            this.label2.TabIndex = 217;
            this.label2.Text = "Name";
            // 
            // cmbShift
            // 
            this.cmbShift.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbShift.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbShift.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbShift.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbShift.FormattingEnabled = true;
            this.cmbShift.IntegralHeight = false;
            this.cmbShift.Location = new System.Drawing.Point(233, 171);
            this.cmbShift.Margin = new System.Windows.Forms.Padding(4);
            this.cmbShift.Name = "cmbShift";
            this.cmbShift.Size = new System.Drawing.Size(277, 27);
            this.cmbShift.TabIndex = 216;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.label9.Location = new System.Drawing.Point(59, 171);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 19);
            this.label9.TabIndex = 232;
            this.label9.Text = "Category";
            // 
            // dateTimePickercf2Out
            // 
            this.dateTimePickercf2Out.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickercf2Out.CustomFormat = "HH:mm tt";
            this.dateTimePickercf2Out.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickercf2Out.Location = new System.Drawing.Point(396, 396);
            this.dateTimePickercf2Out.Name = "dateTimePickercf2Out";
            this.dateTimePickercf2Out.ShowUpDown = true;
            this.dateTimePickercf2Out.Size = new System.Drawing.Size(114, 26);
            this.dateTimePickercf2Out.TabIndex = 228;
            this.dateTimePickercf2Out.Value = new System.DateTime(2022, 7, 7, 15, 15, 0, 0);
            this.dateTimePickercf2Out.Visible = false;
            // 
            // dateTimePickercf2In
            // 
            this.dateTimePickercf2In.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickercf2In.CustomFormat = "HH:mm tt";
            this.dateTimePickercf2In.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickercf2In.Location = new System.Drawing.Point(233, 396);
            this.dateTimePickercf2In.Name = "dateTimePickercf2In";
            this.dateTimePickercf2In.ShowUpDown = true;
            this.dateTimePickercf2In.Size = new System.Drawing.Size(114, 26);
            this.dateTimePickercf2In.TabIndex = 227;
            this.dateTimePickercf2In.Value = new System.DateTime(2022, 7, 7, 15, 0, 0, 0);
            this.dateTimePickercf2In.Visible = false;
            this.dateTimePickercf2In.ValueChanged += new System.EventHandler(this.dateTimePickercf2In_ValueChanged);
            // 
            // checkBoxCF2
            // 
            this.checkBoxCF2.AutoSize = true;
            this.checkBoxCF2.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxCF2.Location = new System.Drawing.Point(61, 396);
            this.checkBoxCF2.Name = "checkBoxCF2";
            this.checkBoxCF2.Size = new System.Drawing.Size(119, 23);
            this.checkBoxCF2.TabIndex = 226;
            this.checkBoxCF2.Text = "Coffe Break 2";
            this.checkBoxCF2.UseVisualStyleBackColor = false;
            this.checkBoxCF2.CheckedChanged += new System.EventHandler(this.checkBoxCF2_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.label6.Location = new System.Drawing.Point(358, 286);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 19);
            this.label6.TabIndex = 246;
            this.label6.Text = "To";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.label4.Location = new System.Drawing.Point(356, 343);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 19);
            this.label4.TabIndex = 245;
            this.label4.Text = "To";
            this.label4.Visible = false;
            // 
            // dateTimePickerMealOut
            // 
            this.dateTimePickerMealOut.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerMealOut.CustomFormat = "HH:mm tt";
            this.dateTimePickerMealOut.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerMealOut.Location = new System.Drawing.Point(396, 282);
            this.dateTimePickerMealOut.Name = "dateTimePickerMealOut";
            this.dateTimePickerMealOut.ShowUpDown = true;
            this.dateTimePickerMealOut.Size = new System.Drawing.Size(114, 26);
            this.dateTimePickerMealOut.TabIndex = 222;
            this.dateTimePickerMealOut.Value = new System.DateTime(2022, 7, 7, 13, 0, 0, 0);
            // 
            // dateTimePickercf1Out
            // 
            this.dateTimePickercf1Out.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickercf1Out.CustomFormat = "HH:mm tt";
            this.dateTimePickercf1Out.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickercf1Out.Location = new System.Drawing.Point(396, 339);
            this.dateTimePickercf1Out.Name = "dateTimePickercf1Out";
            this.dateTimePickercf1Out.ShowUpDown = true;
            this.dateTimePickercf1Out.Size = new System.Drawing.Size(114, 26);
            this.dateTimePickercf1Out.TabIndex = 225;
            this.dateTimePickercf1Out.Value = new System.DateTime(2022, 7, 7, 10, 15, 0, 0);
            this.dateTimePickercf1Out.Visible = false;
            // 
            // dateTimePickerMealIn
            // 
            this.dateTimePickerMealIn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerMealIn.CustomFormat = "HH:mm tt";
            this.dateTimePickerMealIn.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerMealIn.Location = new System.Drawing.Point(233, 282);
            this.dateTimePickerMealIn.Name = "dateTimePickerMealIn";
            this.dateTimePickerMealIn.ShowUpDown = true;
            this.dateTimePickerMealIn.Size = new System.Drawing.Size(114, 26);
            this.dateTimePickerMealIn.TabIndex = 221;
            this.dateTimePickerMealIn.Value = new System.DateTime(2022, 7, 7, 12, 0, 0, 0);
            this.dateTimePickerMealIn.ValueChanged += new System.EventHandler(this.dateTimePickerLunchIn_ValueChanged);
            // 
            // dateTimePickercf1In
            // 
            this.dateTimePickercf1In.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickercf1In.CustomFormat = "HH:mm tt";
            this.dateTimePickercf1In.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickercf1In.Location = new System.Drawing.Point(233, 339);
            this.dateTimePickercf1In.Name = "dateTimePickercf1In";
            this.dateTimePickercf1In.ShowUpDown = true;
            this.dateTimePickercf1In.Size = new System.Drawing.Size(114, 26);
            this.dateTimePickercf1In.TabIndex = 224;
            this.dateTimePickercf1In.Value = new System.DateTime(2022, 7, 7, 10, 0, 0, 0);
            this.dateTimePickercf1In.Visible = false;
            this.dateTimePickercf1In.ValueChanged += new System.EventHandler(this.dateTimePickercf1In_ValueChanged);
            // 
            // checkBoxCF1
            // 
            this.checkBoxCF1.AutoSize = true;
            this.checkBoxCF1.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxCF1.Location = new System.Drawing.Point(63, 339);
            this.checkBoxCF1.Name = "checkBoxCF1";
            this.checkBoxCF1.Size = new System.Drawing.Size(119, 23);
            this.checkBoxCF1.TabIndex = 223;
            this.checkBoxCF1.Text = "Coffe Break 1";
            this.checkBoxCF1.UseVisualStyleBackColor = false;
            this.checkBoxCF1.CheckedChanged += new System.EventHandler(this.checkBoxCF1_CheckedChanged);
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerTo.CustomFormat = "HH:mm tt";
            this.dateTimePickerTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerTo.Location = new System.Drawing.Point(396, 228);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.ShowUpDown = true;
            this.dateTimePickerTo.Size = new System.Drawing.Size(114, 26);
            this.dateTimePickerTo.TabIndex = 218;
            this.dateTimePickerTo.Value = new System.DateTime(2022, 7, 7, 17, 0, 0, 0);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.label3.Location = new System.Drawing.Point(354, 232);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 19);
            this.label3.TabIndex = 238;
            this.label3.Text = "To";
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerFrom.CustomFormat = "HH:mm tt";
            this.dateTimePickerFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerFrom.Location = new System.Drawing.Point(233, 228);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.ShowUpDown = true;
            this.dateTimePickerFrom.Size = new System.Drawing.Size(114, 26);
            this.dateTimePickerFrom.TabIndex = 217;
            this.dateTimePickerFrom.Value = new System.DateTime(2022, 7, 7, 8, 0, 0, 0);
            this.dateTimePickerFrom.ValueChanged += new System.EventHandler(this.dateTimePickerFrom_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.label1.Location = new System.Drawing.Point(59, 228);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 19);
            this.label1.TabIndex = 236;
            this.label1.Text = "Work Hour";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.label5.Location = new System.Drawing.Point(356, 400);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 19);
            this.label5.TabIndex = 250;
            this.label5.Text = "To";
            this.label5.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.label7.Location = new System.Drawing.Point(59, 286);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 19);
            this.label7.TabIndex = 251;
            this.label7.Text = "Meal";
            // 
            // saveBtn
            // 
            this.saveBtn.AutoSize = false;
            this.saveBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.saveBtn.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.saveBtn.Depth = 0;
            this.saveBtn.HighEmphasis = true;
            this.saveBtn.Icon = null;
            this.saveBtn.Location = new System.Drawing.Point(304, 494);
            this.saveBtn.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.saveBtn.MouseState = MaterialSkin.MouseState.HOVER;
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.NoAccentTextColor = System.Drawing.Color.Empty;
            this.saveBtn.Size = new System.Drawing.Size(120, 44);
            this.saveBtn.TabIndex = 252;
            this.saveBtn.Text = "Save";
            this.saveBtn.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.saveBtn.UseAccentColor = false;
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // AddShift
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 575);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dateTimePickercf2Out);
            this.Controls.Add(this.dateTimePickercf2In);
            this.Controls.Add(this.checkBoxCF2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dateTimePickerMealOut);
            this.Controls.Add(this.dateTimePickercf1Out);
            this.Controls.Add(this.dateTimePickerMealIn);
            this.Controls.Add(this.dateTimePickercf1In);
            this.Controls.Add(this.checkBoxCF1);
            this.Controls.Add(this.dateTimePickerTo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dateTimePickerFrom);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbShift);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.usernameLbl);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Open Sans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddShift";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Sizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Shift";
            this.Load += new System.EventHandler(this.AddShift_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.Label usernameLbl;
        public System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbShift;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dateTimePickercf2Out;
        private System.Windows.Forms.DateTimePicker dateTimePickercf2In;
        private System.Windows.Forms.CheckBox checkBoxCF2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimePickerMealOut;
        private System.Windows.Forms.DateTimePicker dateTimePickercf1Out;
        private System.Windows.Forms.DateTimePicker dateTimePickerMealIn;
        private System.Windows.Forms.DateTimePicker dateTimePickercf1In;
        private System.Windows.Forms.CheckBox checkBoxCF1;
        private System.Windows.Forms.DateTimePicker dateTimePickerTo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private MaterialSkin.Controls.MaterialButton saveBtn;
    }
}