namespace tink_oblig
{
    partial class LoginForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.login_btn = new System.Windows.Forms.Button();
            this.exit_btn = new System.Windows.Forms.Button();
            this.keypare_cmb = new System.Windows.Forms.ComboBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.id_save_chk = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // login_btn
            // 
            this.login_btn.Location = new System.Drawing.Point(12, 41);
            this.login_btn.Name = "login_btn";
            this.login_btn.Size = new System.Drawing.Size(67, 22);
            this.login_btn.TabIndex = 0;
            this.login_btn.Text = "Вход";
            this.login_btn.UseVisualStyleBackColor = true;
            this.login_btn.Click += new System.EventHandler(this.login_btn_Click);
            // 
            // exit_btn
            // 
            this.exit_btn.Location = new System.Drawing.Point(268, 41);
            this.exit_btn.Name = "exit_btn";
            this.exit_btn.Size = new System.Drawing.Size(67, 22);
            this.exit_btn.TabIndex = 1;
            this.exit_btn.Text = "Закрыть";
            this.exit_btn.UseVisualStyleBackColor = true;
            this.exit_btn.Click += new System.EventHandler(this.exit_btn_Click);
            // 
            // keypare_cmb
            // 
            this.keypare_cmb.FormattingEnabled = true;
            this.keypare_cmb.Location = new System.Drawing.Point(12, 12);
            this.keypare_cmb.Name = "keypare_cmb";
            this.keypare_cmb.Size = new System.Drawing.Size(323, 23);
            this.keypare_cmb.TabIndex = 1;
            this.keypare_cmb.TextUpdate += new System.EventHandler(this.keypare_cmb_TextUpdate);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoEllipsis = true;
            this.linkLabel1.Location = new System.Drawing.Point(12, 66);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(323, 14);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Как получить API код для входа";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel1.Click += new System.EventHandler(this.linkLabel1_Click);
            // 
            // id_save_chk
            // 
            this.id_save_chk.Location = new System.Drawing.Point(85, 41);
            this.id_save_chk.Name = "id_save_chk";
            this.id_save_chk.Size = new System.Drawing.Size(177, 24);
            this.id_save_chk.TabIndex = 3;
            this.id_save_chk.Text = "Сохранить ключ";
            this.id_save_chk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.id_save_chk.UseVisualStyleBackColor = true;
            this.id_save_chk.Click += new System.EventHandler(this.id_save_chk_Click);
            // 
            // LoginForm
            // 
            this.AcceptButton = this.login_btn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this.exit_btn;
            this.ClientSize = new System.Drawing.Size(347, 85);
            this.ControlBox = false;
            this.Controls.Add(this.id_save_chk);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.keypare_cmb);
            this.Controls.Add(this.exit_btn);
            this.Controls.Add(this.login_btn);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Вход";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button login_btn;
        private System.Windows.Forms.Button exit_btn;
        private System.Windows.Forms.ComboBox keypare_cmb;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.CheckBox id_save_chk;
    }
}

