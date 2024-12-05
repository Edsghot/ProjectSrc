using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms
{
    partial class UCImportacionesCompra
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.dateInicio = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblDateFin = new System.Windows.Forms.Label();
            this.dateFin = new System.Windows.Forms.DateTimePicker();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblDateInicio = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panelContainerTable = new System.Windows.Forms.Panel();
            this.dataTable = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pageNumberLabel = new System.Windows.Forms.Label();
            this.nextPageButton = new FontAwesome.Sharp.IconButton();
            this.previousPageButton = new FontAwesome.Sharp.IconButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnImportar = new FontAwesome.Sharp.IconButton();
            this.iconButton2 = new FontAwesome.Sharp.IconButton();
            this.iconButton1 = new FontAwesome.Sharp.IconButton();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panelContainerTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(27, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(482, 50);
            this.label1.TabIndex = 0;
            this.label1.Text = "Importación de compras\nServicios de Recepción de Comprobantes (SRC)";
            // 
            // dateInicio
            // 
            this.dateInicio.CalendarForeColor = System.Drawing.Color.Black;
            this.dateInicio.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dateInicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateInicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateInicio.Location = new System.Drawing.Point(15, 39);
            this.dateInicio.Margin = new System.Windows.Forms.Padding(15);
            this.dateInicio.Name = "dateInicio";
            this.dateInicio.Size = new System.Drawing.Size(236, 49);
            this.dateInicio.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.dateFin);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.btnImportar);
            this.panel1.Controls.Add(this.iconButton2);
            this.panel1.Controls.Add(this.iconButton1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dateInicio);
            this.panel1.Location = new System.Drawing.Point(18, 89);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1284, 98);
            this.panel1.TabIndex = 6;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.White;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.comboBox1);
            this.panel5.Location = new System.Drawing.Point(542, 37);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(237, 40);
            this.panel5.TabIndex = 14;
            // 
            // comboBox1
            // 
            this.comboBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "No está listo",
            "Listo",
            "Error"});
            this.comboBox1.Location = new System.Drawing.Point(16, 6);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(206, 30);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.Text = "Estado importación";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Controls.Add(this.lblDateFin);
            this.panel4.Location = new System.Drawing.Point(283, 39);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(189, 36);
            this.panel4.TabIndex = 13;
            // 
            // lblDateFin
            // 
            this.lblDateFin.AutoSize = true;
            this.lblDateFin.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateFin.Location = new System.Drawing.Point(16, 8);
            this.lblDateFin.Name = "lblDateFin";
            this.lblDateFin.Size = new System.Drawing.Size(101, 20);
            this.lblDateFin.TabIndex = 0;
            this.lblDateFin.Text = "Fecha de fin";
            // 
            // dateFin
            // 
            this.dateFin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dateFin.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateFin.Location = new System.Drawing.Point(282, 37);
            this.dateFin.Margin = new System.Windows.Forms.Padding(15);
            this.dateFin.Name = "dateFin";
            this.dateFin.Size = new System.Drawing.Size(236, 49);
            this.dateFin.TabIndex = 12;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.lblDateInicio);
            this.panel3.Location = new System.Drawing.Point(16, 41);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(189, 36);
            this.panel3.TabIndex = 11;
            // 
            // lblDateInicio
            // 
            this.lblDateInicio.AutoSize = true;
            this.lblDateInicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateInicio.Location = new System.Drawing.Point(16, 8);
            this.lblDateInicio.Name = "lblDateInicio";
            this.lblDateInicio.Size = new System.Drawing.Size(122, 20);
            this.lblDateInicio.TabIndex = 0;
            this.lblDateInicio.Text = "Fecha de inicio";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(231, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Ingrese el rango de fecha de emisión:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(30, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(436, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "*Todos lo campos en “pendiente” se deben completar de forma manual.";
            // 
            // panelContainerTable
            // 
            this.panelContainerTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelContainerTable.Controls.Add(this.dataTable);
            this.panelContainerTable.Location = new System.Drawing.Point(18, 209);
            this.panelContainerTable.Name = "panelContainerTable";
            this.panelContainerTable.Size = new System.Drawing.Size(1284, 450);
            this.panelContainerTable.TabIndex = 8;
            // 
            // dataTable
            // 
            this.dataTable.AllowUserToAddRows = false;
            this.dataTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataTable.BackgroundColor = System.Drawing.SystemColors.InactiveBorder;
            this.dataTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataTable.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9,
            this.Column10,
            this.Column11});
            this.dataTable.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.InactiveBorder;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataTable.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataTable.Location = new System.Drawing.Point(15, 12);
            this.dataTable.Name = "dataTable";
            this.dataTable.ReadOnly = true;
            this.dataTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataTable.RowTemplate.Height = 24;
            this.dataTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataTable.Size = new System.Drawing.Size(1249, 432);
            this.dataTable.TabIndex = 9;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "N° de compra";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 125;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Fecha de emisión";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 125;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Sucursal";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 125;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Asiento\r\ntipo";
            this.Column4.MinimumWidth = 6;
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 125;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Observación";
            this.Column5.MinimumWidth = 6;
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 125;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Sub total";
            this.Column6.MinimumWidth = 6;
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 125;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "IGV";
            this.Column7.MinimumWidth = 6;
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Width = 125;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Total";
            this.Column8.MinimumWidth = 6;
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Width = 125;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Fecha de llegada";
            this.Column9.MinimumWidth = 6;
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            this.Column9.Width = 125;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "Coincidencia de productos";
            this.Column10.MinimumWidth = 6;
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            this.Column10.Width = 125;
            // 
            // Column11
            // 
            this.Column11.HeaderText = "Estado de importación";
            this.Column11.MinimumWidth = 6;
            this.Column11.Name = "Column11";
            this.Column11.ReadOnly = true;
            this.Column11.Width = 125;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pageNumberLabel);
            this.panel2.Controls.Add(this.nextPageButton);
            this.panel2.Controls.Add(this.previousPageButton);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Location = new System.Drawing.Point(18, 665);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(168, 64);
            this.panel2.TabIndex = 9;
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
            this.nextPageButton.BackColor = System.Drawing.SystemColors.Control;
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
            this.previousPageButton.BackColor = System.Drawing.SystemColors.Control;
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
            // btnImportar
            // 
            this.btnImportar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportar.BackColor = System.Drawing.Color.Navy;
            this.btnImportar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnImportar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportar.ForeColor = System.Drawing.Color.White;
            this.btnImportar.IconChar = FontAwesome.Sharp.IconChar.RightFromBracket;
            this.btnImportar.IconColor = System.Drawing.Color.White;
            this.btnImportar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnImportar.IconSize = 35;
            this.btnImportar.Location = new System.Drawing.Point(1130, 36);
            this.btnImportar.Name = "btnImportar";
            this.btnImportar.Size = new System.Drawing.Size(134, 45);
            this.btnImportar.TabIndex = 10;
            this.btnImportar.Text = "Importar";
            this.btnImportar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnImportar.UseVisualStyleBackColor = false;
            this.btnImportar.Click += new System.EventHandler(this.btnImportar_Click);
            // 
            // iconButton2
            // 
            this.iconButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButton2.BackColor = System.Drawing.Color.MediumBlue;
            this.iconButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.iconButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iconButton2.ForeColor = System.Drawing.Color.White;
            this.iconButton2.IconChar = FontAwesome.Sharp.IconChar.NfcDirectional;
            this.iconButton2.IconColor = System.Drawing.Color.White;
            this.iconButton2.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconButton2.IconSize = 35;
            this.iconButton2.Location = new System.Drawing.Point(969, 36);
            this.iconButton2.Name = "iconButton2";
            this.iconButton2.Size = new System.Drawing.Size(142, 45);
            this.iconButton2.TabIndex = 9;
            this.iconButton2.Text = "Escanear";
            this.iconButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.iconButton2.UseVisualStyleBackColor = false;
            // 
            // iconButton1
            // 
            this.iconButton1.BackColor = System.Drawing.Color.SteelBlue;
            this.iconButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.iconButton1.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.iconButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.iconButton1.IconChar = FontAwesome.Sharp.IconChar.ArrowsSpin;
            this.iconButton1.IconColor = System.Drawing.Color.White;
            this.iconButton1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconButton1.IconSize = 35;
            this.iconButton1.Location = new System.Drawing.Point(797, 36);
            this.iconButton1.Name = "iconButton1";
            this.iconButton1.Size = new System.Drawing.Size(50, 45);
            this.iconButton1.TabIndex = 8;
            this.iconButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.iconButton1.UseVisualStyleBackColor = false;
            // 
            // UCImportacionesCompra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelContainerTable);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UCImportacionesCompra";
            this.Size = new System.Drawing.Size(1324, 796);
            this.Load += new System.EventHandler(this.UCImportacionesCompra_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panelContainerTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataTable)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateInicio;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private FontAwesome.Sharp.IconButton iconButton1;
        private FontAwesome.Sharp.IconButton btnImportar;
        private FontAwesome.Sharp.IconButton iconButton2;
        private System.Windows.Forms.Label label3;
        private Panel panelContainerTable;
        private DataGridView dataTable;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column6;
        private DataGridViewTextBoxColumn Column7;
        private DataGridViewTextBoxColumn Column8;
        private DataGridViewTextBoxColumn Column9;
        private DataGridViewTextBoxColumn Column10;
        private DataGridViewTextBoxColumn Column11;
        private Panel panel2;
        private FontAwesome.Sharp.IconButton nextPageButton;
        private FontAwesome.Sharp.IconButton previousPageButton;
        private Label pageNumberLabel;
        private Panel panel3;
        private Label lblDateInicio;
        private Panel panel4;
        private Label lblDateFin;
        private DateTimePicker dateFin;
        private Panel panel5;
        private PictureBox pictureBox1;
    }
}
