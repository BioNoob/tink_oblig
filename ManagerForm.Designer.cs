
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
            this.view_panel = new System.Windows.Forms.Panel();
            this.account_switcher_cmb = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.change_history_btn = new System.Windows.Forms.Button();
            this.control_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // control_panel
            // 
            this.control_panel.Controls.Add(this.change_history_btn);
            this.control_panel.Controls.Add(this.label3);
            this.control_panel.Controls.Add(this.label2);
            this.control_panel.Controls.Add(this.label1);
            this.control_panel.Controls.Add(this.account_switcher_cmb);
            this.control_panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.control_panel.Location = new System.Drawing.Point(0, 0);
            this.control_panel.Name = "control_panel";
            this.control_panel.Size = new System.Drawing.Size(391, 70);
            this.control_panel.TabIndex = 0;
            // 
            // view_panel
            // 
            this.view_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.view_panel.Location = new System.Drawing.Point(0, 70);
            this.view_panel.Name = "view_panel";
            this.view_panel.Size = new System.Drawing.Size(391, 527);
            this.view_panel.TabIndex = 1;
            // 
            // account_switcher_cmb
            // 
            this.account_switcher_cmb.FormattingEnabled = true;
            this.account_switcher_cmb.Location = new System.Drawing.Point(12, 21);
            this.account_switcher_cmb.Name = "account_switcher_cmb";
            this.account_switcher_cmb.Size = new System.Drawing.Size(269, 23);
            this.account_switcher_cmb.TabIndex = 0;
            this.account_switcher_cmb.SelectionChangeCommitted += new System.EventHandler(this.account_switcher_cmb_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Выбранный аккаунт";
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(12, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "Тип";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(155, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 19);
            this.label3.TabIndex = 3;
            this.label3.Text = "Id";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // change_history_btn
            // 
            this.change_history_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.change_history_btn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("change_history_btn.BackgroundImage")));
            this.change_history_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.change_history_btn.Location = new System.Drawing.Point(324, 11);
            this.change_history_btn.Name = "change_history_btn";
            this.change_history_btn.Size = new System.Drawing.Size(55, 55);
            this.change_history_btn.TabIndex = 4;
            this.change_history_btn.UseVisualStyleBackColor = true;
            // 
            // ManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 597);
            this.Controls.Add(this.view_panel);
            this.Controls.Add(this.control_panel);
            this.Name = "ManagerForm";
            this.Text = "ManagerForm";
            this.control_panel.ResumeLayout(false);
            this.control_panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel control_panel;
        private System.Windows.Forms.Panel view_panel;
        private System.Windows.Forms.ComboBox account_switcher_cmb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button change_history_btn;
    }
}