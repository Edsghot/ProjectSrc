﻿using System;
using System.Windows.Forms;

namespace app_matter_data_src_erp.Forms.DialogView
{
    public partial class Sucursal : Form
    {

        private readonly MainComprasSrc mainForm;
        public Sucursal(MainComprasSrc mainForm)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.mainForm = mainForm;
        }
        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            mainForm.ShowToast("Datos de la sucursal añadidos con éxito.", "success");
            this.Close();
        }
    }
}
