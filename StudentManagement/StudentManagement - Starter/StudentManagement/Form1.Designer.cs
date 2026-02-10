namespace StudentManagement
{
    partial class Form1
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
            tableLayoutPanel1 = new TableLayoutPanel();
            listBox1 = new ListBox();
            Load = new Button();
            groupBox1 = new GroupBox();
            txtWorkterm = new TextBox();
            txtProgram = new TextBox();
            txtAge = new TextBox();
            txtLname = new TextBox();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            txtFname = new TextBox();
            label2 = new Label();
            label1 = new Label();
            Save = new Button();
            tableLayoutPanel2 = new TableLayoutPanel();
            btnAdd = new Button();
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(listBox1, 2, 0);
            tableLayoutPanel1.Controls.Add(Load, 0, 1);
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(Save, 1, 1);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 2, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RightToLeft = RightToLeft.No;
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 66.6666641F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Size = new Size(941, 504);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // listBox1
            // 
            listBox1.Dock = DockStyle.Fill;
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(473, 3);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(465, 330);
            listBox1.TabIndex = 0;
            // 
            // Load
            // 
            Load.Location = new Point(3, 339);
            Load.Name = "Load";
            Load.RightToLeft = RightToLeft.Yes;
            Load.Size = new Size(229, 78);
            Load.TabIndex = 1;
            Load.Text = "Load Student Data";
            Load.UseVisualStyleBackColor = true;
            Load.Click += button1_Click;
            // 
            // groupBox1
            // 
            tableLayoutPanel1.SetColumnSpan(groupBox1, 2);
            groupBox1.Controls.Add(txtWorkterm);
            groupBox1.Controls.Add(txtProgram);
            groupBox1.Controls.Add(txtAge);
            groupBox1.Controls.Add(txtLname);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(txtFname);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(430, 330);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "New Student ";
            // 
            // txtWorkterm
            // 
            txtWorkterm.Location = new Point(216, 242);
            txtWorkterm.Name = "txtWorkterm";
            txtWorkterm.Size = new Size(125, 27);
            txtWorkterm.TabIndex = 9;
            // 
            // txtProgram
            // 
            txtProgram.Location = new Point(216, 185);
            txtProgram.Name = "txtProgram";
            txtProgram.Size = new Size(125, 27);
            txtProgram.TabIndex = 8;
            // 
            // txtAge
            // 
            txtAge.Location = new Point(216, 140);
            txtAge.Name = "txtAge";
            txtAge.Size = new Size(125, 27);
            txtAge.TabIndex = 7;
            // 
            // txtLname
            // 
            txtLname.Location = new Point(216, 95);
            txtLname.Name = "txtLname";
            txtLname.Size = new Size(125, 27);
            txtLname.TabIndex = 6;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(14, 249);
            label5.Name = "label5";
            label5.Size = new Size(76, 20);
            label5.TabIndex = 4;
            label5.Text = "WorkTerm";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(14, 192);
            label4.Name = "label4";
            label4.Size = new Size(66, 20);
            label4.TabIndex = 3;
            label4.Text = "Program";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 143);
            label3.Name = "label3";
            label3.Size = new Size(36, 20);
            label3.TabIndex = 2;
            label3.Text = "Age";
            // 
            // txtFname
            // 
            txtFname.Location = new Point(216, 45);
            txtFname.Name = "txtFname";
            txtFname.Size = new Size(125, 27);
            txtFname.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 95);
            label2.Name = "label2";
            label2.Size = new Size(79, 20);
            label2.TabIndex = 1;
            label2.Text = "Last Name";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 48);
            label1.Name = "label1";
            label1.Size = new Size(80, 20);
            label1.TabIndex = 0;
            label1.Text = "First Name";
            // 
            // Save
            // 
            Save.Location = new Point(238, 339);
            Save.Name = "Save";
            Save.Size = new Size(229, 78);
            Save.TabIndex = 3;
            Save.Text = "Save Student Data";
            Save.UseVisualStyleBackColor = true;
            Save.Click += Save_Click;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(btnAdd, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(473, 339);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(465, 162);
            tableLayoutPanel2.TabIndex = 4;
            // 
            // btnAdd
            // 
            btnAdd.Dock = DockStyle.Fill;
            btnAdd.Location = new Point(3, 3);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(226, 75);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "Add New Student";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(941, 504);
            Controls.Add(tableLayoutPanel1);
            Name = "Form1";
            Text = "Form1";
            tableLayoutPanel1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private ListBox listBox1;
        private Button Load;
        private GroupBox groupBox1;
        private TextBox txtWorkterm;
        private TextBox txtProgram;
        private TextBox txtAge;
        private TextBox txtLname;
        private Label label5;
        private Label label4;
        private Label label3;
        private TextBox txtFname;
        private Label label2;
        private Label label1;
        private Button Save;
        private TableLayoutPanel tableLayoutPanel2;
        private Button btnAdd;
    }
}
