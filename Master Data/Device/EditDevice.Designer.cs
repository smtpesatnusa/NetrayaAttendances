
namespace SMTAttendance
{
    partial class EditDevice
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
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.cmbInout = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.saveBtn = new MaterialSkin.Controls.MaterialButton();
            this.isActiveCheckBox = new System.Windows.Forms.CheckBox();
            this.tbipAddress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbDepartment = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.usernameLbl = new System.Windows.Forms.Label();
            this.cmbWorkArea = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbInout
            // 
            this.cmbInout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbInout.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbInout.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbInout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInout.FormattingEnabled = true;
            this.cmbInout.Items.AddRange(new object[] {
            "In",
            "Out",
            "In/Out"});
            this.cmbInout.Location = new System.Drawing.Point(79, 397);
            this.cmbInout.Margin = new System.Windows.Forms.Padding(4);
            this.cmbInout.Name = "cmbInout";
            this.cmbInout.Size = new System.Drawing.Size(277, 27);
            this.cmbInout.TabIndex = 257;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.label1.Location = new System.Drawing.Point(75, 366);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 19);
            this.label1.TabIndex = 258;
            this.label1.Text = "In/Out";
            // 
            // saveBtn
            // 
            this.saveBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.saveBtn.AutoSize = false;
            this.saveBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.saveBtn.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.saveBtn.Depth = 0;
            this.saveBtn.HighEmphasis = true;
            this.saveBtn.Icon = null;
            this.saveBtn.Location = new System.Drawing.Point(155, 492);
            this.saveBtn.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.saveBtn.MouseState = MaterialSkin.MouseState.HOVER;
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.NoAccentTextColor = System.Drawing.Color.Empty;
            this.saveBtn.Size = new System.Drawing.Size(120, 44);
            this.saveBtn.TabIndex = 256;
            this.saveBtn.Text = "Save";
            this.saveBtn.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.saveBtn.UseAccentColor = false;
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // isActiveCheckBox
            // 
            this.isActiveCheckBox.AutoSize = true;
            this.isActiveCheckBox.Checked = true;
            this.isActiveCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.isActiveCheckBox.Location = new System.Drawing.Point(79, 433);
            this.isActiveCheckBox.Name = "isActiveCheckBox";
            this.isActiveCheckBox.Size = new System.Drawing.Size(86, 23);
            this.isActiveCheckBox.TabIndex = 251;
            this.isActiveCheckBox.Text = "Is Active";
            this.isActiveCheckBox.UseVisualStyleBackColor = true;
            // 
            // tbipAddress
            // 
            this.tbipAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbipAddress.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.tbipAddress.Location = new System.Drawing.Point(79, 253);
            this.tbipAddress.Margin = new System.Windows.Forms.Padding(4);
            this.tbipAddress.MaxLength = 30;
            this.tbipAddress.Name = "tbipAddress";
            this.tbipAddress.Size = new System.Drawing.Size(277, 26);
            this.tbipAddress.TabIndex = 250;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.label2.Location = new System.Drawing.Point(75, 221);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 19);
            this.label2.TabIndex = 254;
            this.label2.Text = "IP Address";
            // 
            // cmbDepartment
            // 
            this.cmbDepartment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDepartment.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbDepartment.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDepartment.FormattingEnabled = true;
            this.cmbDepartment.Location = new System.Drawing.Point(79, 178);
            this.cmbDepartment.Margin = new System.Windows.Forms.Padding(4);
            this.cmbDepartment.Name = "cmbDepartment";
            this.cmbDepartment.Size = new System.Drawing.Size(277, 27);
            this.cmbDepartment.TabIndex = 249;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.label7.Location = new System.Drawing.Point(75, 147);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 19);
            this.label7.TabIndex = 253;
            this.label7.Text = "Department";
            // 
            // usernameLbl
            // 
            this.usernameLbl.AutoSize = true;
            this.usernameLbl.BackColor = System.Drawing.Color.Transparent;
            this.usernameLbl.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.usernameLbl.Location = new System.Drawing.Point(24, 470);
            this.usernameLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.usernameLbl.Name = "usernameLbl";
            this.usernameLbl.Size = new System.Drawing.Size(77, 19);
            this.usernameLbl.TabIndex = 259;
            this.usernameLbl.Text = "username";
            this.usernameLbl.Visible = false;
            // 
            // cmbWorkArea
            // 
            this.cmbWorkArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbWorkArea.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbWorkArea.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbWorkArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWorkArea.FormattingEnabled = true;
            this.cmbWorkArea.Location = new System.Drawing.Point(79, 104);
            this.cmbWorkArea.Margin = new System.Windows.Forms.Padding(4);
            this.cmbWorkArea.Name = "cmbWorkArea";
            this.cmbWorkArea.Size = new System.Drawing.Size(277, 27);
            this.cmbWorkArea.TabIndex = 261;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.label3.Location = new System.Drawing.Point(75, 77);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 19);
            this.label3.TabIndex = 260;
            this.label3.Text = "Work Area";
            // 
            // tbPort
            // 
            this.tbPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPort.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.tbPort.Location = new System.Drawing.Point(79, 327);
            this.tbPort.Margin = new System.Windows.Forms.Padding(4);
            this.tbPort.MaxLength = 30;
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(277, 26);
            this.tbPort.TabIndex = 262;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.label4.Location = new System.Drawing.Point(75, 295);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 19);
            this.label4.TabIndex = 263;
            this.label4.Text = "Port";
            // 
            // EditDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 570);
            this.Controls.Add(this.tbPort);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbWorkArea);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.usernameLbl);
            this.Controls.Add(this.cmbInout);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.isActiveCheckBox);
            this.Controls.Add(this.tbipAddress);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbDepartment);
            this.Controls.Add(this.label7);
            this.Font = new System.Drawing.Font("Open Sans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditDevice";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Sizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Device List";
            this.Load += new System.EventHandler(this.Devicelist_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label label1;
        private MaterialSkin.Controls.MaterialButton saveBtn;
        private System.Windows.Forms.CheckBox isActiveCheckBox;
        public System.Windows.Forms.TextBox tbipAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.Label usernameLbl;
        public System.Windows.Forms.ComboBox cmbInout;
        public System.Windows.Forms.ComboBox cmbDepartment;
        private System.Windows.Forms.ComboBox cmbWorkArea;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Label label4;
    }
}