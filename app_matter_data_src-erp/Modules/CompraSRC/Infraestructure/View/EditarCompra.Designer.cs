namespace app_matter_data_src_erp.Forms
{
    partial class EditarCompra
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSalir = new FontAwesome.Sharp.IconButton();
            this.lblCode = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblCoincidencia = new System.Windows.Forms.Label();
            this.btnContinuar = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cbMes = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.SteelBlue;
            this.panel1.Controls.Add(this.btnSalir);
            this.panel1.Controls.Add(this.lblCode);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(-6, -6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(620, 64);
            this.panel1.TabIndex = 1;
            // 
            // btnSalir
            // 
            this.btnSalir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSalir.BackColor = System.Drawing.Color.SteelBlue;
            this.btnSalir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSalir.FlatAppearance.BorderSize = 0;
            this.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalir.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnSalir.IconChar = FontAwesome.Sharp.IconChar.X;
            this.btnSalir.IconColor = System.Drawing.SystemColors.HighlightText;
            this.btnSalir.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnSalir.IconSize = 22;
            this.btnSalir.Location = new System.Drawing.Point(548, 18);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(38, 35);
            this.btnSalir.TabIndex = 5;
            this.btnSalir.UseVisualStyleBackColor = false;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCode.ForeColor = System.Drawing.Color.White;
            this.lblCode.Location = new System.Drawing.Point(327, 22);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(93, 22);
            this.lblCode.TabIndex = 1;
            this.lblCode.Text = "0000 000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(173, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Editar - Compra ";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblCoincidencia);
            this.panel2.Controls.Add(this.btnContinuar);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(31, 75);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(537, 289);
            this.panel2.TabIndex = 2;
            // 
            // lblCoincidencia
            // 
            this.lblCoincidencia.AutoSize = true;
            this.lblCoincidencia.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCoincidencia.ForeColor = System.Drawing.Color.Chocolate;
            this.lblCoincidencia.Location = new System.Drawing.Point(16, 176);
            this.lblCoincidencia.Name = "lblCoincidencia";
            this.lblCoincidencia.Size = new System.Drawing.Size(234, 16);
            this.lblCoincidencia.TabIndex = 26;
            this.lblCoincidencia.Text = "Ver concidencia de productos >>";
            this.lblCoincidencia.Click += new System.EventHandler(this.lblCoincidencia_Click);
            // 
            // btnContinuar
            // 
            this.btnContinuar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnContinuar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnContinuar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContinuar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnContinuar.ForeColor = System.Drawing.Color.White;
            this.btnContinuar.Location = new System.Drawing.Point(146, 213);
            this.btnContinuar.Name = "btnContinuar";
            this.btnContinuar.Size = new System.Drawing.Size(237, 57);
            this.btnContinuar.TabIndex = 25;
            this.btnContinuar.Text = "Continuar";
            this.btnContinuar.UseVisualStyleBackColor = false;
            this.btnContinuar.Click += new System.EventHandler(this.btnContinuar_Click);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.White;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.comboBox2);
            this.panel5.ForeColor = System.Drawing.Color.Black;
            this.panel5.Location = new System.Drawing.Point(277, 119);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(237, 40);
            this.panel5.TabIndex = 24;
            // 
            // comboBox2
            // 
            this.comboBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.comboBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Almacen1",
            "Almacen2"});
            this.comboBox2.Location = new System.Drawing.Point(16, 6);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(202, 30);
            this.comboBox2.TabIndex = 7;
            this.comboBox2.Text = "Almacen";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.White;
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.comboBox3);
            this.panel6.ForeColor = System.Drawing.Color.Black;
            this.panel6.Location = new System.Drawing.Point(16, 119);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(237, 40);
            this.panel6.TabIndex = 23;
            // 
            // comboBox3
            // 
            this.comboBox3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.comboBox3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "Sucursal1",
            "Sucursal2"});
            this.comboBox3.Location = new System.Drawing.Point(16, 6);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(202, 30);
            this.comboBox3.TabIndex = 7;
            this.comboBox3.Text = "Sucursal";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(13, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 16);
            this.label3.TabIndex = 22;
            this.label3.Text = "Datos:";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.comboBox1);
            this.panel3.ForeColor = System.Drawing.Color.Black;
            this.panel3.Location = new System.Drawing.Point(277, 37);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(237, 40);
            this.panel3.TabIndex = 21;
            // 
            // comboBox1
            // 
            this.comboBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "2020",
            "2021",
            "2022",
            "2023",
            "2024"});
            this.comboBox1.Location = new System.Drawing.Point(16, 6);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(202, 30);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.Text = "Año";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.cbMes);
            this.panel4.ForeColor = System.Drawing.Color.Black;
            this.panel4.Location = new System.Drawing.Point(16, 37);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(237, 40);
            this.panel4.TabIndex = 20;
            // 
            // cbMes
            // 
            this.cbMes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbMes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbMes.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMes.FormattingEnabled = true;
            this.cbMes.Items.AddRange(new object[] {
            "Enero",
            "Febrero",
            "Marzo",
            "Abril",
            "Mayo",
            "Junio",
            "Julio",
            "Agosto",
            "Septiembre",
            "Octubre",
            "Noviembre",
            "Diciembre"});
            this.cbMes.Location = new System.Drawing.Point(16, 6);
            this.cbMes.Name = "cbMes";
            this.cbMes.Size = new System.Drawing.Size(202, 30);
            this.cbMes.TabIndex = 7;
            this.cbMes.Text = "Mes";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 16);
            this.label2.TabIndex = 19;
            this.label2.Text = "Periodo:";
            // 
            // EditarCompra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(604, 382);
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "EditarCompra";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox cbMes;
        private System.Windows.Forms.Label label2;
        private FontAwesome.Sharp.IconButton btnSalir;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label lblCoincidencia;
        private System.Windows.Forms.Button btnContinuar;
    }
}