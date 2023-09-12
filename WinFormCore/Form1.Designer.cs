using System.Drawing;
using System.Windows.Forms;

namespace FormCore
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
            pictureBox1 = new PictureBox();
            panel1 = new Panel();
            groupBox3 = new GroupBox();
            radioButton4 = new RadioButton();
            radioButton3 = new RadioButton();
            radioButton2 = new RadioButton();
            radioButton1 = new RadioButton();
            groupBox2 = new GroupBox();
            checkBox3 = new CheckBox();
            checkBox2 = new CheckBox();
            chkTypeLoop = new CheckBox();
            checkBox4 = new CheckBox();
            checkBox1 = new CheckBox();
            chkDrawLine = new CheckBox();
            groupBox1 = new GroupBox();
            label8 = new Label();
            label3 = new Label();
            label2 = new Label();
            txtbuffer = new TextBox();
            txtRectHeight = new TextBox();
            txtRectWidth = new TextBox();
            txtfps = new TextBox();
            label10 = new Label();
            chk10W = new CheckBox();
            label9 = new Label();
            txtShowTime = new TextBox();
            label7 = new Label();
            txtDrawTime = new TextBox();
            label6 = new Label();
            txtsleepScend = new TextBox();
            label4 = new Label();
            txtTotalGeometry = new TextBox();
            label5 = new Label();
            txtLimit = new TextBox();
            label1 = new Label();
            chkLoop = new CheckBox();
            chkTiled = new CheckBox();
            button1 = new Button();
            mapborder = new Panel();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            mapborder.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(mapborder, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 83.3333359F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 262F));
            tableLayoutPanel1.Size = new Size(1923, 1181);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Padding = new Padding(5);
            pictureBox1.Size = new Size(1917, 979);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            tableLayoutPanel1.SetColumnSpan(panel1, 2);
            panel1.Controls.Add(groupBox3);
            panel1.Controls.Add(groupBox2);
            panel1.Controls.Add(groupBox1);
            panel1.Controls.Add(txtfps);
            panel1.Controls.Add(label10);
            panel1.Controls.Add(chk10W);
            panel1.Controls.Add(label9);
            panel1.Controls.Add(txtShowTime);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(txtDrawTime);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(txtsleepScend);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(txtTotalGeometry);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(txtLimit);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(chkLoop);
            panel1.Controls.Add(chkTiled);
            panel1.Controls.Add(button1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 4);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(1917, 188);
            panel1.TabIndex = 1;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(radioButton4);
            groupBox3.Controls.Add(radioButton3);
            groupBox3.Controls.Add(radioButton2);
            groupBox3.Controls.Add(radioButton1);
            groupBox3.Location = new Point(873, 51);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(571, 57);
            groupBox3.TabIndex = 5;
            groupBox3.TabStop = false;
            // 
            // radioButton4
            // 
            radioButton4.AutoSize = true;
            radioButton4.Location = new Point(280, 21);
            radioButton4.Name = "radioButton4";
            radioButton4.Size = new Size(75, 24);
            radioButton4.TabIndex = 0;
            radioButton4.TabStop = true;
            radioButton4.Text = "自适应";
            radioButton4.UseVisualStyleBackColor = true;
            radioButton4.CheckedChanged += radioButton4_CheckedChanged;
            // 
            // radioButton3
            // 
            radioButton3.AutoSize = true;
            radioButton3.Location = new Point(193, 21);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(60, 24);
            radioButton3.TabIndex = 0;
            radioButton3.TabStop = true;
            radioButton3.Text = "居中";
            radioButton3.UseVisualStyleBackColor = true;
            radioButton3.CheckedChanged += radioButton3_CheckedChanged;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(99, 21);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(60, 24);
            radioButton2.TabIndex = 0;
            radioButton2.TabStop = true;
            radioButton2.Text = "拉伸";
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.CheckedChanged += radioButton2_CheckedChanged;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(22, 21);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(60, 24);
            radioButton1.TabIndex = 0;
            radioButton1.TabStop = true;
            radioButton1.Text = "普通";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBox2.Controls.Add(checkBox3);
            groupBox2.Controls.Add(checkBox2);
            groupBox2.Controls.Add(chkTypeLoop);
            groupBox2.Controls.Add(checkBox4);
            groupBox2.Controls.Add(checkBox1);
            groupBox2.Controls.Add(chkDrawLine);
            groupBox2.Location = new Point(873, 0);
            groupBox2.Margin = new Padding(3, 4, 3, 4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(3, 4, 3, 4);
            groupBox2.Size = new Size(571, 57);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            // 
            // checkBox3
            // 
            checkBox3.AutoSize = true;
            checkBox3.Location = new Point(193, 22);
            checkBox3.Margin = new Padding(3, 4, 3, 4);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(76, 24);
            checkBox3.TabIndex = 3;
            checkBox3.Text = "画矩形";
            checkBox3.UseVisualStyleBackColor = true;
            checkBox3.CheckedChanged += checkBox3_CheckedChanged;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(99, 22);
            checkBox2.Margin = new Padding(3, 4, 3, 4);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(76, 24);
            checkBox2.TabIndex = 3;
            checkBox2.Text = "画线框";
            checkBox2.UseVisualStyleBackColor = true;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            // 
            // chkTypeLoop
            // 
            chkTypeLoop.AutoSize = true;
            chkTypeLoop.Location = new Point(489, 19);
            chkTypeLoop.Margin = new Padding(3, 4, 3, 4);
            chkTypeLoop.Name = "chkTypeLoop";
            chkTypeLoop.Size = new Size(61, 24);
            chkTypeLoop.TabIndex = 3;
            chkTypeLoop.Text = "循环";
            chkTypeLoop.UseVisualStyleBackColor = true;
            chkTypeLoop.CheckedChanged += chkTypeLoop_CheckedChanged;
            // 
            // checkBox4
            // 
            checkBox4.AutoSize = true;
            checkBox4.Location = new Point(383, 22);
            checkBox4.Margin = new Padding(3, 4, 3, 4);
            checkBox4.Name = "checkBox4";
            checkBox4.Size = new Size(76, 24);
            checkBox4.TabIndex = 3;
            checkBox4.Text = "画文字";
            checkBox4.UseVisualStyleBackColor = true;
            checkBox4.CheckedChanged += checkBox4_CheckedChanged;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(294, 22);
            checkBox1.Margin = new Padding(3, 4, 3, 4);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(61, 24);
            checkBox1.TabIndex = 3;
            checkBox1.Text = "画圆";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // chkDrawLine
            // 
            chkDrawLine.AutoSize = true;
            chkDrawLine.Location = new Point(19, 22);
            chkDrawLine.Margin = new Padding(3, 4, 3, 4);
            chkDrawLine.Name = "chkDrawLine";
            chkDrawLine.Size = new Size(61, 24);
            chkDrawLine.TabIndex = 3;
            chkDrawLine.Text = "画线";
            chkDrawLine.UseVisualStyleBackColor = true;
            chkDrawLine.CheckedChanged += chkDrawLine_CheckedChanged;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(txtbuffer);
            groupBox1.Controls.Add(txtRectHeight);
            groupBox1.Controls.Add(txtRectWidth);
            groupBox1.Location = new Point(1449, 4);
            groupBox1.Margin = new Padding(3, 4, 3, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 4, 3, 4);
            groupBox1.Size = new Size(458, 111);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "绘制矩形尺寸";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Microsoft YaHei UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point);
            label8.Location = new Point(26, 67);
            label8.Name = "label8";
            label8.Size = new Size(64, 24);
            label8.TabIndex = 0;
            label8.Text = "间距：";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft YaHei UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(213, 36);
            label3.Name = "label3";
            label3.Size = new Size(64, 24);
            label3.TabIndex = 0;
            label3.Text = "宽度：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft YaHei UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(26, 34);
            label2.Name = "label2";
            label2.Size = new Size(64, 24);
            label2.TabIndex = 0;
            label2.Text = "宽度：";
            // 
            // txtbuffer
            // 
            txtbuffer.Location = new Point(104, 69);
            txtbuffer.Margin = new Padding(3, 4, 3, 4);
            txtbuffer.Name = "txtbuffer";
            txtbuffer.Size = new Size(91, 27);
            txtbuffer.TabIndex = 2;
            txtbuffer.Text = "10";
            // 
            // txtRectHeight
            // 
            txtRectHeight.Location = new Point(291, 39);
            txtRectHeight.Margin = new Padding(3, 4, 3, 4);
            txtRectHeight.Name = "txtRectHeight";
            txtRectHeight.Size = new Size(91, 27);
            txtRectHeight.TabIndex = 2;
            txtRectHeight.Text = "10";
            // 
            // txtRectWidth
            // 
            txtRectWidth.Location = new Point(104, 36);
            txtRectWidth.Margin = new Padding(3, 4, 3, 4);
            txtRectWidth.Name = "txtRectWidth";
            txtRectWidth.Size = new Size(91, 27);
            txtRectWidth.TabIndex = 2;
            txtRectWidth.Text = "10";
            // 
            // txtfps
            // 
            txtfps.Location = new Point(660, 88);
            txtfps.Margin = new Padding(3, 4, 3, 4);
            txtfps.Name = "txtfps";
            txtfps.Size = new Size(73, 27);
            txtfps.TabIndex = 2;
            txtfps.Text = "100";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(750, 94);
            label10.Name = "label10";
            label10.Size = new Size(31, 20);
            label10.TabIndex = 1;
            label10.Text = "fps";
            // 
            // chk10W
            // 
            chk10W.AutoSize = true;
            chk10W.Location = new Point(119, 90);
            chk10W.Margin = new Padding(3, 4, 3, 4);
            chk10W.Name = "chk10W";
            chk10W.Size = new Size(61, 24);
            chk10W.TabIndex = 3;
            chk10W.Text = "适应";
            chk10W.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(599, 95);
            label9.Name = "label9";
            label9.Size = new Size(54, 20);
            label9.TabIndex = 1;
            label9.Text = "帧率：";
            // 
            // txtShowTime
            // 
            txtShowTime.Location = new Point(750, 51);
            txtShowTime.Margin = new Padding(3, 4, 3, 4);
            txtShowTime.Name = "txtShowTime";
            txtShowTime.Size = new Size(84, 27);
            txtShowTime.TabIndex = 2;
            txtShowTime.Text = "100";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(599, 56);
            label7.Name = "label7";
            label7.Size = new Size(144, 20);
            label7.TabIndex = 1;
            label7.Text = "整体耗时（毫秒）：";
            // 
            // txtDrawTime
            // 
            txtDrawTime.Location = new Point(750, 12);
            txtDrawTime.Margin = new Padding(3, 4, 3, 4);
            txtDrawTime.Name = "txtDrawTime";
            txtDrawTime.Size = new Size(84, 27);
            txtDrawTime.TabIndex = 2;
            txtDrawTime.Text = "100";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(599, 18);
            label6.Name = "label6";
            label6.Size = new Size(144, 20);
            label6.TabIndex = 1;
            label6.Text = "绘制耗时（毫秒）：";
            // 
            // txtsleepScend
            // 
            txtsleepScend.Location = new Point(510, 9);
            txtsleepScend.Margin = new Padding(3, 4, 3, 4);
            txtsleepScend.Name = "txtsleepScend";
            txtsleepScend.Size = new Size(70, 27);
            txtsleepScend.TabIndex = 2;
            txtsleepScend.Text = "100";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(360, 15);
            label4.Name = "label4";
            label4.Size = new Size(144, 20);
            label4.TabIndex = 1;
            label4.Text = "循环间隔（毫秒）：";
            // 
            // txtTotalGeometry
            // 
            txtTotalGeometry.Location = new Point(213, 45);
            txtTotalGeometry.Margin = new Padding(3, 4, 3, 4);
            txtTotalGeometry.Name = "txtTotalGeometry";
            txtTotalGeometry.Size = new Size(125, 27);
            txtTotalGeometry.TabIndex = 2;
            txtTotalGeometry.Text = "100000";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(93, 49);
            label5.Name = "label5";
            label5.Size = new Size(114, 20);
            label5.TabIndex = 1;
            label5.Text = "实际图形数量：";
            // 
            // txtLimit
            // 
            txtLimit.Location = new Point(213, 9);
            txtLimit.Margin = new Padding(3, 4, 3, 4);
            txtLimit.Name = "txtLimit";
            txtLimit.Size = new Size(125, 27);
            txtLimit.TabIndex = 2;
            txtLimit.Text = "100000";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(93, 12);
            label1.Name = "label1";
            label1.Size = new Size(114, 20);
            label1.TabIndex = 1;
            label1.Text = "最大图形数量：";
            // 
            // chkLoop
            // 
            chkLoop.AutoSize = true;
            chkLoop.Location = new Point(360, 51);
            chkLoop.Margin = new Padding(3, 4, 3, 4);
            chkLoop.Name = "chkLoop";
            chkLoop.Size = new Size(91, 24);
            chkLoop.TabIndex = 3;
            chkLoop.Text = "循环绘制";
            chkLoop.UseVisualStyleBackColor = true;
            chkLoop.CheckedChanged += chkTiled_CheckedChanged;
            // 
            // chkTiled
            // 
            chkTiled.AutoSize = true;
            chkTiled.Location = new Point(213, 88);
            chkTiled.Margin = new Padding(3, 4, 3, 4);
            chkTiled.Name = "chkTiled";
            chkTiled.Size = new Size(91, 24);
            chkTiled.TabIndex = 3;
            chkTiled.Text = "平铺覆盖";
            chkTiled.UseVisualStyleBackColor = true;
            chkTiled.CheckedChanged += chkTiled_CheckedChanged;
            // 
            // button1
            // 
            button1.Location = new Point(9, 9);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(77, 52);
            button1.TabIndex = 0;
            button1.Text = "执行";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // mapborder
            // 
            tableLayoutPanel1.SetColumnSpan(mapborder, 2);
            mapborder.Controls.Add(pictureBox1);
            mapborder.Dock = DockStyle.Fill;
            mapborder.Location = new Point(3, 199);
            mapborder.Name = "mapborder";
            mapborder.Size = new Size(1917, 979);
            mapborder.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1923, 1181);
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Form1";
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            mapborder.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private PictureBox pictureBox1;
        private Panel panel1;
        private CheckBox chkDrawLine;
        private TextBox txtLimit;
        private Label label1;
        private Button button1;
        private GroupBox groupBox1;
        private Label label3;
        private Label label2;
        private TextBox txtRectHeight;
        private TextBox txtRectWidth;
        private GroupBox groupBox2;
        private CheckBox checkBox3;
        private CheckBox checkBox2;
        private CheckBox chkTypeLoop;
        private CheckBox checkBox1;
        private CheckBox chkTiled;
        private TextBox txtsleepScend;
        private Label label4;
        private TextBox txtTotalGeometry;
        private Label label5;
        private TextBox txtDrawTime;
        private Label label6;
        private TextBox txtShowTime;
        private Label label7;
        private CheckBox checkBox4;
        private Label label8;
        private TextBox txtbuffer;
        private TextBox txtfps;
        private Label label9;
        private Label label10;
        private CheckBox chk10W;
        private GroupBox groupBox3;
        private RadioButton radioButton4;
        private RadioButton radioButton3;
        private RadioButton radioButton2;
        private RadioButton radioButton1;
        private CheckBox chkLoop;
        private Panel mapborder;
    }
}