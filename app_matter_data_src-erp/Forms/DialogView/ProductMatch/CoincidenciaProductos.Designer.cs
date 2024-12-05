namespace app_matter_data_src_erp.Forms.DialogView
{
    partial class CoincidenciaProductos
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSalir = new FontAwesome.Sharp.IconButton();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pageNumberLabel = new System.Windows.Forms.Label();
            this.nextPageButton = new FontAwesome.Sharp.IconButton();
            this.previousPageButton = new FontAwesome.Sharp.IconButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnContinuar = new System.Windows.Forms.Button();
            this.dataTable = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label6 = new System.Windows.Forms.Label();
            this.lblEstado = new System.Windows.Forms.Label();
            this.lblNumeroCompra = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.SteelBlue;
            this.panel1.Controls.Add(this.btnSalir);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(-6, -7);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(615, 64);
            this.panel1.TabIndex = 3;
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
            this.btnSalir.Location = new System.Drawing.Point(560, 18);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(36, 26);
            this.btnSalir.TabIndex = 2;
            this.btnSalir.UseVisualStyleBackColor = false;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(183, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(247, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Coincidencia de productos";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.btnContinuar);
            this.panel2.Controls.Add(this.dataTable);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.lblEstado);
            this.panel2.Controls.Add(this.lblNumeroCompra);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(12, 74);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(578, 412);
            this.panel2.TabIndex = 4;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.pageNumberLabel);
            this.panel4.Controls.Add(this.nextPageButton);
            this.panel4.Controls.Add(this.previousPageButton);
            this.panel4.Controls.Add(this.pictureBox1);
            this.panel4.Location = new System.Drawing.Point(20, 328);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(168, 64);
            this.panel4.TabIndex = 13;
            // 
            // pageNumberLabel
            // 
            this.pageNumberLabel.AutoSize = true;
            this.pageNumberLabel.BackColor = System.Drawing.Color.SteelBlue;
            this.pageNumberLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pageNumberLabel.ForeColor = System.Drawing.Color.White;
            this.pageNumberLabel.Location = new System.Drawing.Point(79, 21);
            this.pageNumberLabel.Name = "pageNumberLabel";
            this.pageNumberLabel.Size = new System.Drawing.Size(17, 18);
            this.pageNumberLabel.TabIndex = 13;
            this.pageNumberLabel.Text = "0";
            // 
            // nextPageButton
            // 
            this.nextPageButton.BackColor = System.Drawing.Color.White;
            this.nextPageButton.BackgroundImage = global::app_matter_data_src_erp.Properties.Resources.iconCircleButton;
            this.nextPageButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nextPageButton.FlatAppearance.BorderSize = 0;
            this.nextPageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nextPageButton.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.nextPageButton.IconChar = FontAwesome.Sharp.IconChar.CaretRight;
            this.nextPageButton.IconColor = System.Drawing.Color.Black;
            this.nextPageButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.nextPageButton.IconSize = 25;
            this.nextPageButton.Location = new System.Drawing.Point(118, 13);
            this.nextPageButton.Name = "nextPageButton";
            this.nextPageButton.Size = new System.Drawing.Size(36, 36);
            this.nextPageButton.TabIndex = 11;
            this.nextPageButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.nextPageButton.UseVisualStyleBackColor = false;
            this.nextPageButton.Click += new System.EventHandler(this.nextPageButton_Click);
            // 
            // previousPageButton
            // 
            this.previousPageButton.BackColor = System.Drawing.Color.White;
            this.previousPageButton.BackgroundImage = global::app_matter_data_src_erp.Properties.Resources.iconCircleButton;
            this.previousPageButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.previousPageButton.FlatAppearance.BorderSize = 0;
            this.previousPageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.previousPageButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.previousPageButton.IconChar = FontAwesome.Sharp.IconChar.CaretLeft;
            this.previousPageButton.IconColor = System.Drawing.Color.Black;
            this.previousPageButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.previousPageButton.IconSize = 25;
            this.previousPageButton.Location = new System.Drawing.Point(16, 13);
            this.previousPageButton.Name = "previousPageButton";
            this.previousPageButton.Size = new System.Drawing.Size(36, 36);
            this.previousPageButton.TabIndex = 9;
            this.previousPageButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.previousPageButton.UseVisualStyleBackColor = false;
            this.previousPageButton.Click += new System.EventHandler(this.previousPageButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::app_matter_data_src_erp.Properties.Resources.iconCircle;
            this.pictureBox1.Location = new System.Drawing.Point(69, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(36, 36);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // btnContinuar
            // 
            this.btnContinuar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnContinuar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnContinuar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnContinuar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContinuar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnContinuar.ForeColor = System.Drawing.Color.White;
            this.btnContinuar.Location = new System.Drawing.Point(337, 331);
            this.btnContinuar.Name = "btnContinuar";
            this.btnContinuar.Size = new System.Drawing.Size(220, 49);
            this.btnContinuar.TabIndex = 11;
            this.btnContinuar.Text = "Guardar cambios";
            this.btnContinuar.UseVisualStyleBackColor = false;
            this.btnContinuar.Click += new System.EventHandler(this.btnContinuar_Click);
            // 
            // dataTable
            // 
            this.dataTable.AllowUserToAddRows = false;
            this.dataTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dataTable.BackgroundColor = System.Drawing.Color.White;
            this.dataTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataTable.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataTable.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.dataTable.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataTable.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataTable.EnableHeadersVisualStyles = false;
            this.dataTable.Location = new System.Drawing.Point(20, 73);
            this.dataTable.MultiSelect = false;
            this.dataTable.Name = "dataTable";
            this.dataTable.ReadOnly = true;
            this.dataTable.RowHeadersVisible = false;
            this.dataTable.RowHeadersWidth = 51;
            this.dataTable.RowTemplate.Height = 24;
            this.dataTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataTable.Size = new System.Drawing.Size(537, 235);
            this.dataTable.TabIndex = 10;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Código\n Producto ERP";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 125;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Producto\nERP";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 125;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Producto\nSRC";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 125;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label6.Location = new System.Drawing.Point(214, 37);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(343, 16);
            this.label6.TabIndex = 9;
            this.label6.Text = "*El producto ERP y el producto SRC deben ser el mismo";
            // 
            // lblEstado
            // 
            this.lblEstado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEstado.AutoSize = true;
            this.lblEstado.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstado.Location = new System.Drawing.Point(416, 13);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(141, 18);
            this.lblEstado.TabIndex = 8;
            this.lblEstado.Text = "No hay coincidencia";
            // 
            // lblNumeroCompra
            // 
            this.lblNumeroCompra.AutoSize = true;
            this.lblNumeroCompra.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumeroCompra.Location = new System.Drawing.Point(17, 35);
            this.lblNumeroCompra.Name = "lblNumeroCompra";
            this.lblNumeroCompra.Size = new System.Drawing.Size(100, 18);
            this.lblNumeroCompra.TabIndex = 7;
            this.lblNumeroCompra.Text = "DG5T 279913";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(355, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 18);
            this.label3.TabIndex = 6;
            this.label3.Text = "Estado:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(14, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(158, 18);
            this.label2.TabIndex = 5;
            this.label2.Text = "Número de compra:";
            // 
            // CoincidenciaProductos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(602, 498);
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "CoincidenciaProductos";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FontAwesome.Sharp.IconButton btnSalir;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataTable;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.Label lblNumeroCompra;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.Button btnContinuar;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label pageNumberLabel;
        private FontAwesome.Sharp.IconButton nextPageButton;
        private FontAwesome.Sharp.IconButton previousPageButton;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}