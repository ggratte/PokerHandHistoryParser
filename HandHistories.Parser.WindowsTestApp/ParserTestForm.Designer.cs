namespace HandHistories.Parser.WindowsTestApp
{
    partial class ParserTestForm
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
            this.richTextBoxHandText = new System.Windows.Forms.RichTextBox();
            this.buttonParse = new System.Windows.Forms.Button();
            this.listBoxSite = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox_validateHands = new System.Windows.Forms.CheckBox();
            this.checkBox_serialize = new System.Windows.Forms.CheckBox();
            this.listBoxHands = new System.Windows.Forms.ListBox();
            this.richTextBoxParsedHand = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxHandText
            // 
            this.richTextBoxHandText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxHandText.Location = new System.Drawing.Point(4, 29);
            this.richTextBoxHandText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.richTextBoxHandText.Name = "richTextBoxHandText";
            this.tableLayoutPanel1.SetRowSpan(this.richTextBoxHandText, 4);
            this.richTextBoxHandText.Size = new System.Drawing.Size(483, 800);
            this.richTextBoxHandText.TabIndex = 0;
            this.richTextBoxHandText.Text = "";
            // 
            // buttonParse
            // 
            this.buttonParse.Location = new System.Drawing.Point(495, 768);
            this.buttonParse.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonParse.Name = "buttonParse";
            this.buttonParse.Size = new System.Drawing.Size(187, 60);
            this.buttonParse.TabIndex = 1;
            this.buttonParse.Text = "Parse";
            this.buttonParse.UseVisualStyleBackColor = true;
            this.buttonParse.Click += new System.EventHandler(this.buttonParse_Click);
            // 
            // listBoxSite
            // 
            this.listBoxSite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxSite.FormattingEnabled = true;
            this.listBoxSite.ItemHeight = 16;
            this.listBoxSite.Location = new System.Drawing.Point(495, 29);
            this.listBoxSite.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listBoxSite.Name = "listBoxSite";
            this.listBoxSite.Size = new System.Drawing.Size(192, 675);
            this.listBoxSite.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.richTextBoxHandText, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.listBoxSite, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.checkBox_validateHands, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonParse, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.checkBox_serialize, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.listBoxHands, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.richTextBoxParsedHand, 3, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 69F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1382, 833);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(573, 4);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Site";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(176, 4);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Hand History Text";
            // 
            // checkBox_validateHands
            // 
            this.checkBox_validateHands.AutoSize = true;
            this.checkBox_validateHands.Location = new System.Drawing.Point(495, 712);
            this.checkBox_validateHands.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox_validateHands.Name = "checkBox_validateHands";
            this.checkBox_validateHands.Size = new System.Drawing.Size(124, 20);
            this.checkBox_validateHands.TabIndex = 5;
            this.checkBox_validateHands.Text = "Validate hands";
            this.checkBox_validateHands.UseVisualStyleBackColor = true;
            // 
            // checkBox_serialize
            // 
            this.checkBox_serialize.AutoSize = true;
            this.checkBox_serialize.Location = new System.Drawing.Point(495, 740);
            this.checkBox_serialize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox_serialize.Name = "checkBox_serialize";
            this.checkBox_serialize.Size = new System.Drawing.Size(141, 20);
            this.checkBox_serialize.TabIndex = 5;
            this.checkBox_serialize.Text = "Serialize to JSON";
            this.checkBox_serialize.UseVisualStyleBackColor = true;
            // 
            // listBoxHands
            // 
            this.listBoxHands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxHands.FormattingEnabled = true;
            this.listBoxHands.ItemHeight = 16;
            this.listBoxHands.Location = new System.Drawing.Point(694, 28);
            this.listBoxHands.Name = "listBoxHands";
            this.listBoxHands.Size = new System.Drawing.Size(194, 677);
            this.listBoxHands.TabIndex = 6;
            this.listBoxHands.SelectedIndexChanged += new System.EventHandler(this.listBoxHands_SelectedIndexChanged);
            this.listBoxHands.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.listBoxHands_Format);
            // 
            // richTextBoxParsedHand
            // 
            this.richTextBoxParsedHand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxParsedHand.Location = new System.Drawing.Point(894, 28);
            this.richTextBoxParsedHand.Name = "richTextBoxParsedHand";
            this.tableLayoutPanel1.SetRowSpan(this.richTextBoxParsedHand, 4);
            this.richTextBoxParsedHand.Size = new System.Drawing.Size(485, 802);
            this.richTextBoxParsedHand.TabIndex = 7;
            this.richTextBoxParsedHand.Text = "";
            // 
            // ParserTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1382, 833);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ParserTestForm";
            this.Text = "Hand Parser Test App";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxHandText;
        private System.Windows.Forms.Button buttonParse;
        private System.Windows.Forms.ListBox listBoxSite;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox_validateHands;
        private System.Windows.Forms.CheckBox checkBox_serialize;
        private System.Windows.Forms.ListBox listBoxHands;
        private System.Windows.Forms.RichTextBox richTextBoxParsedHand;
    }
}

