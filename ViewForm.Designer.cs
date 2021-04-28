
namespace tink_oblig
{
    partial class ViewForm
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
            this.BoundListLayPannel = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.summ_cpn_lbl = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.total_money_lbl = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BoundListLayPannel
            // 
            this.BoundListLayPannel.AutoScroll = true;
            this.BoundListLayPannel.ColumnCount = 1;
            this.BoundListLayPannel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.BoundListLayPannel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BoundListLayPannel.Location = new System.Drawing.Point(0, 47);
            this.BoundListLayPannel.Name = "BoundListLayPannel";
            this.BoundListLayPannel.Padding = new System.Windows.Forms.Padding(5);
            this.BoundListLayPannel.RowCount = 1;
            this.BoundListLayPannel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.BoundListLayPannel.Size = new System.Drawing.Size(395, 316);
            this.BoundListLayPannel.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(395, 47);
            this.panel2.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.summ_cpn_lbl);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.total_money_lbl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 363);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(395, 40);
            this.panel1.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(201, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 30);
            this.label2.TabIndex = 10;
            this.label2.Text = "Купонный доход";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // summ_cpn_lbl
            // 
            this.summ_cpn_lbl.AutoEllipsis = true;
            this.summ_cpn_lbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.summ_cpn_lbl.Location = new System.Drawing.Point(276, 8);
            this.summ_cpn_lbl.Name = "summ_cpn_lbl";
            this.summ_cpn_lbl.Size = new System.Drawing.Size(107, 23);
            this.summ_cpn_lbl.TabIndex = 9;
            this.summ_cpn_lbl.Text = "summ total";
            this.summ_cpn_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(5, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 35);
            this.label1.TabIndex = 8;
            this.label1.Text = "Всего на сумму";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // total_money_lbl
            // 
            this.total_money_lbl.AutoEllipsis = true;
            this.total_money_lbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.total_money_lbl.Location = new System.Drawing.Point(72, 8);
            this.total_money_lbl.Name = "total_money_lbl";
            this.total_money_lbl.Size = new System.Drawing.Size(107, 23);
            this.total_money_lbl.TabIndex = 7;
            this.total_money_lbl.Text = "summ total";
            this.total_money_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 403);
            this.ControlBox = false;
            this.Controls.Add(this.BoundListLayPannel);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ViewForm";
            this.Text = "ViewForm";
            this.Load += new System.EventHandler(this.ViewForm_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel BoundListLayPannel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label total_money_lbl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label summ_cpn_lbl;
        private System.Windows.Forms.Label label1;
    }
}