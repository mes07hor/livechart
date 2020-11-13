namespace livechart
{
    partial class FirstForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FirstForm));
            this.metroComboBox1 = new MetroFramework.Controls.MetroComboBox();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButton_facilitator = new System.Windows.Forms.RadioButton();
            this.radioButton_organizer = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // metroComboBox1
            // 
            this.metroComboBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.metroComboBox1.FormattingEnabled = true;
            this.metroComboBox1.ItemHeight = 23;
            this.metroComboBox1.Location = new System.Drawing.Point(133, 176);
            this.metroComboBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.metroComboBox1.Name = "metroComboBox1";
            this.metroComboBox1.Size = new System.Drawing.Size(180, 29);
            this.metroComboBox1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroComboBox1.TabIndex = 0;
            this.metroComboBox1.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroComboBox1.UseSelectable = true;
            this.metroComboBox1.UseStyleColors = true;
            this.metroComboBox1.SelectedIndexChanged += new System.EventHandler(this.metroComboBox1_SelectedIndexChanged);
            // 
            // metroButton1
            // 
            this.metroButton1.Location = new System.Drawing.Point(168, 254);
            this.metroButton1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(108, 52);
            this.metroButton1.Style = MetroFramework.MetroColorStyle.Yellow;
            this.metroButton1.TabIndex = 1;
            this.metroButton1.Text = "Open";
            this.metroButton1.UseSelectable = true;
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Dubai", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(100, 122);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 49);
            this.label1.TabIndex = 2;
            this.label1.Text = "Choose your Group";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Dubai", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(112, 19);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(228, 49);
            this.label2.TabIndex = 3;
            this.label2.Text = "Choose your Role";
            // 
            // radioButton_facilitator
            // 
            this.radioButton_facilitator.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton_facilitator.AutoSize = true;
            this.radioButton_facilitator.BackColor = System.Drawing.Color.Gainsboro;
            this.radioButton_facilitator.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.radioButton_facilitator.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumSlateBlue;
            this.radioButton_facilitator.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton_facilitator.Location = new System.Drawing.Point(109, 80);
            this.radioButton_facilitator.Name = "radioButton_facilitator";
            this.radioButton_facilitator.Size = new System.Drawing.Size(94, 31);
            this.radioButton_facilitator.TabIndex = 5;
            this.radioButton_facilitator.TabStop = true;
            this.radioButton_facilitator.Text = "Facilitator";
            this.radioButton_facilitator.UseVisualStyleBackColor = false;
            // 
            // radioButton_organizer
            // 
            this.radioButton_organizer.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton_organizer.AutoSize = true;
            this.radioButton_organizer.BackColor = System.Drawing.SystemColors.Control;
            this.radioButton_organizer.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton_organizer.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.radioButton_organizer.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumSlateBlue;
            this.radioButton_organizer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton_organizer.Location = new System.Drawing.Point(248, 80);
            this.radioButton_organizer.Name = "radioButton_organizer";
            this.radioButton_organizer.Size = new System.Drawing.Size(92, 31);
            this.radioButton_organizer.TabIndex = 6;
            this.radioButton_organizer.TabStop = true;
            this.radioButton_organizer.Text = "Organizer";
            this.radioButton_organizer.UseVisualStyleBackColor = false;
            // 
            // FirstForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ClientSize = new System.Drawing.Size(446, 385);
            this.Controls.Add(this.radioButton_organizer);
            this.Controls.Add(this.radioButton_facilitator);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.metroButton1);
            this.Controls.Add(this.metroComboBox1);
            this.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "FirstForm";
            this.Text = "FirstForm";
            this.Load += new System.EventHandler(this.FirstForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroComboBox metroComboBox1;
        private MetroFramework.Controls.MetroButton metroButton1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioButton_facilitator;
        private System.Windows.Forms.RadioButton radioButton_organizer;
    }
}