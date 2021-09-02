
namespace tink_oblig
{
    partial class ManagerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManagerForm));
            this.control_panel = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.history_cmb = new System.Windows.Forms.ComboBox();
            this.refresh_btn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.account_switcher_cmb = new System.Windows.Forms.ComboBox();
            this.view_panel = new System.Windows.Forms.Panel();
            this.control_panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // control_panel
            // 
            this.control_panel.Controls.Add(this.pictureBox1);
            this.control_panel.Controls.Add(this.label3);
            this.control_panel.Controls.Add(this.history_cmb);
            this.control_panel.Controls.Add(this.refresh_btn);
            this.control_panel.Controls.Add(this.label2);
            this.control_panel.Controls.Add(this.account_switcher_cmb);
            this.control_panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.control_panel.Location = new System.Drawing.Point(0, 0);
            this.control_panel.Name = "control_panel";
            this.control_panel.Size = new System.Drawing.Size(406, 55);
            this.control_panel.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Image = global::tink_oblig.Properties.Resources.icons8_повтор;
            this.pictureBox1.Location = new System.Drawing.Point(361, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            this.pictureBox1.WaitOnLoad = true;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(193, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 19);
            this.label3.TabIndex = 7;
            this.label3.Text = "Включение истории";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // history_cmb
            // 
            this.history_cmb.FormattingEnabled = true;
            this.history_cmb.Location = new System.Drawing.Point(193, 6);
            this.history_cmb.Name = "history_cmb";
            this.history_cmb.Size = new System.Drawing.Size(139, 23);
            this.history_cmb.TabIndex = 6;
            this.history_cmb.SelectionChangeCommitted += new System.EventHandler(this.history_cmb_SelectionChangeCommitted);
            // 
            // refresh_btn
            // 
            this.refresh_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.refresh_btn.BackColor = System.Drawing.Color.Transparent;
            this.refresh_btn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("refresh_btn.BackgroundImage")));
            this.refresh_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.refresh_btn.Location = new System.Drawing.Point(357, 9);
            this.refresh_btn.Name = "refresh_btn";
            this.refresh_btn.Size = new System.Drawing.Size(40, 40);
            this.refresh_btn.TabIndex = 5;
            this.refresh_btn.UseVisualStyleBackColor = false;
            this.refresh_btn.Click += new System.EventHandler(this.refresh_btn_Click);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(12, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "Тип аккаунта  (ID)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // account_switcher_cmb
            // 
            this.account_switcher_cmb.FormattingEnabled = true;
            this.account_switcher_cmb.Location = new System.Drawing.Point(12, 6);
            this.account_switcher_cmb.Name = "account_switcher_cmb";
            this.account_switcher_cmb.Size = new System.Drawing.Size(175, 23);
            this.account_switcher_cmb.TabIndex = 0;
            this.account_switcher_cmb.SelectionChangeCommitted += new System.EventHandler(this.account_switcher_cmb_SelectionChangeCommitted);
            // 
            // view_panel
            // 
            this.view_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.view_panel.Location = new System.Drawing.Point(0, 55);
            this.view_panel.MinimumSize = new System.Drawing.Size(406, 366);
            this.view_panel.Name = "view_panel";
            this.view_panel.Size = new System.Drawing.Size(406, 542);
            this.view_panel.TabIndex = 1;
            // 
            // ManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 597);
            this.Controls.Add(this.view_panel);
            this.Controls.Add(this.control_panel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(422, 636);
            this.Name = "ManagerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Облигатор";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ManagerForm_FormClosed);
            this.Shown += new System.EventHandler(this.ManagerForm_Shown);
            this.control_panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel control_panel;
        private System.Windows.Forms.Panel view_panel;
        private System.Windows.Forms.ComboBox account_switcher_cmb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button refresh_btn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox history_cmb;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}