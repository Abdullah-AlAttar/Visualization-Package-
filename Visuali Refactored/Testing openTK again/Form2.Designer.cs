namespace Testing_openTK_again
{
    partial class Form2
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
            this.glControl1 = new OpenTK.GLControl();
            this.colorMap1 = new ColorMapControl.ColorMap();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.floodBox = new System.Windows.Forms.CheckBox();
            this.isoValueText = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.zoomUpbtn = new System.Windows.Forms.Button();
            this.zoomDownBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(40, 200);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(541, 347);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = false;
            this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
            this.glControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.glControl1_KeyDown);
            this.glControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseDown);
            this.glControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseMove);
            this.glControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseUp);
            this.glControl1.Resize += new System.EventHandler(this.glControl1_Resize);
            // 
            // colorMap1
            // 
            this.colorMap1.Location = new System.Drawing.Point(701, 37);
            this.colorMap1.Name = "colorMap1";
            this.colorMap1.Size = new System.Drawing.Size(401, 547);
            this.colorMap1.TabIndex = 1;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(324, 111);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 12;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(38, 65);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Load File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(132, 68);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(512, 20);
            this.textBox1.TabIndex = 9;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(147, 111);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 15;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(58, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Coloring Type";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1109, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "label1";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(487, 111);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 20;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(906, 248);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(305, 21);
            this.comboBox3.TabIndex = 21;
            // 
            // floodBox
            // 
            this.floodBox.AutoSize = true;
            this.floodBox.Location = new System.Drawing.Point(710, 283);
            this.floodBox.Name = "floodBox";
            this.floodBox.Size = new System.Drawing.Size(52, 17);
            this.floodBox.TabIndex = 22;
            this.floodBox.Text = "Flood";
            this.floodBox.UseVisualStyleBackColor = true;
            this.floodBox.CheckedChanged += new System.EventHandler(this.floodBox_CheckedChanged);
            // 
            // isoValueText
            // 
            this.isoValueText.Location = new System.Drawing.Point(487, 155);
            this.isoValueText.Name = "isoValueText";
            this.isoValueText.Size = new System.Drawing.Size(100, 20);
            this.isoValueText.TabIndex = 23;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(370, 152);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 25;
            this.button3.Text = "Simulate";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // zoomUpbtn
            // 
            this.zoomUpbtn.Location = new System.Drawing.Point(606, 248);
            this.zoomUpbtn.Name = "zoomUpbtn";
            this.zoomUpbtn.Size = new System.Drawing.Size(75, 23);
            this.zoomUpbtn.TabIndex = 26;
            this.zoomUpbtn.Text = "zoom up";
            this.zoomUpbtn.UseVisualStyleBackColor = true;
            this.zoomUpbtn.Click += new System.EventHandler(this.zoomUpbtn_Click);
            // 
            // zoomDownBtn
            // 
            this.zoomDownBtn.Location = new System.Drawing.Point(606, 294);
            this.zoomDownBtn.Name = "zoomDownBtn";
            this.zoomDownBtn.Size = new System.Drawing.Size(75, 23);
            this.zoomDownBtn.TabIndex = 27;
            this.zoomDownBtn.Text = "Zoom Down";
            this.zoomDownBtn.UseVisualStyleBackColor = true;
            this.zoomDownBtn.Click += new System.EventHandler(this.zoomDownBtn_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1792, 633);
            this.Controls.Add(this.zoomDownBtn);
            this.Controls.Add(this.zoomUpbtn);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.isoValueText);
            this.Controls.Add(this.floodBox);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.colorMap1);
            this.Controls.Add(this.glControl1);
            this.Name = "Form2";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form2_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl glControl1;
        private ColorMapControl.ColorMap colorMap1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        public System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.CheckBox floodBox;
        private System.Windows.Forms.TextBox isoValueText;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button zoomUpbtn;
        private System.Windows.Forms.Button zoomDownBtn;
    }
}

