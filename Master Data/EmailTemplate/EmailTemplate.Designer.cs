
namespace SMTAttendance
{
    partial class EmailTemplate
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
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.userdetail = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbSubject = new System.Windows.Forms.TextBox();
            this.saveBtn = new MaterialSkin.Controls.MaterialButton();
            this.tbCc = new System.Windows.Forms.TextBox();
            this.tbTo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePickerSendIn = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.label5.Location = new System.Drawing.Point(38, 89);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 19);
            this.label5.TabIndex = 240;
            this.label5.Text = "To:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.label1.Location = new System.Drawing.Point(36, 150);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 19);
            this.label1.TabIndex = 241;
            this.label1.Text = "Cc:";
            // 
            // tbMessage
            // 
            this.tbMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMessage.Location = new System.Drawing.Point(133, 269);
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(395, 204);
            this.tbMessage.TabIndex = 255;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.label3.Location = new System.Drawing.Point(38, 269);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 19);
            this.label3.TabIndex = 243;
            this.label3.Text = "Message";
            // 
            // userdetail
            // 
            this.userdetail.AutoSize = true;
            this.userdetail.BackColor = System.Drawing.Color.Transparent;
            this.userdetail.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.userdetail.Location = new System.Drawing.Point(29, 553);
            this.userdetail.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.userdetail.Name = "userdetail";
            this.userdetail.Size = new System.Drawing.Size(77, 19);
            this.userdetail.TabIndex = 244;
            this.userdetail.Text = "username";
            this.userdetail.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.label7.Location = new System.Drawing.Point(38, 203);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 19);
            this.label7.TabIndex = 242;
            this.label7.Text = "Subject";
            // 
            // tbSubject
            // 
            this.tbSubject.Location = new System.Drawing.Point(133, 203);
            this.tbSubject.Name = "tbSubject";
            this.tbSubject.Size = new System.Drawing.Size(395, 26);
            this.tbSubject.TabIndex = 254;
            // 
            // saveBtn
            // 
            this.saveBtn.AutoSize = false;
            this.saveBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.saveBtn.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.saveBtn.Depth = 0;
            this.saveBtn.HighEmphasis = true;
            this.saveBtn.Icon = null;
            this.saveBtn.Location = new System.Drawing.Point(242, 553);
            this.saveBtn.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.saveBtn.MouseState = MaterialSkin.MouseState.HOVER;
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.NoAccentTextColor = System.Drawing.Color.Empty;
            this.saveBtn.Size = new System.Drawing.Size(149, 44);
            this.saveBtn.TabIndex = 2256;
            this.saveBtn.Text = "Save";
            this.saveBtn.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.saveBtn.UseAccentColor = false;
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // tbCc
            // 
            this.tbCc.Location = new System.Drawing.Point(131, 143);
            this.tbCc.Name = "tbCc";
            this.tbCc.Size = new System.Drawing.Size(397, 26);
            this.tbCc.TabIndex = 253;
            // 
            // tbTo
            // 
            this.tbTo.Location = new System.Drawing.Point(131, 89);
            this.tbTo.Name = "tbTo";
            this.tbTo.Size = new System.Drawing.Size(397, 26);
            this.tbTo.TabIndex = 252;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.label2.Location = new System.Drawing.Point(38, 497);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 19);
            this.label2.TabIndex = 2257;
            this.label2.Text = "Send Time ";
            // 
            // dateTimePickerSendIn
            // 
            this.dateTimePickerSendIn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerSendIn.CustomFormat = "HH:mm";
            this.dateTimePickerSendIn.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerSendIn.Location = new System.Drawing.Point(133, 497);
            this.dateTimePickerSendIn.Name = "dateTimePickerSendIn";
            this.dateTimePickerSendIn.ShowUpDown = true;
            this.dateTimePickerSendIn.Size = new System.Drawing.Size(108, 26);
            this.dateTimePickerSendIn.TabIndex = 2258;
            this.dateTimePickerSendIn.Value = new System.DateTime(2022, 7, 7, 17, 0, 0, 0);
            // 
            // EmailTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 622);
            this.Controls.Add(this.dateTimePickerSendIn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbTo);
            this.Controls.Add(this.tbCc);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.tbSubject);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbMessage);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.userdetail);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Font = new System.Drawing.Font("Open Sans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EmailTemplate";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Sizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Report Email Template";
            this.Load += new System.EventHandler(this.EmailTemplate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label userdetail;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbSubject;
        private MaterialSkin.Controls.MaterialButton saveBtn;
        private System.Windows.Forms.TextBox tbCc;
        private System.Windows.Forms.TextBox tbTo;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.DateTimePicker dateTimePickerSendIn;
    }
}