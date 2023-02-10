using System;
using System.Windows.Forms;
using System.Drawing;

namespace Conferencia.Relatorios
{
    public partial class frmRelatorios : Form
    {
        private BindingSource dt = new BindingSource();

        public frmRelatorios(BindingSource dt)
        {
            InitializeComponent();
            this.dt = dt;
        }

        private void Relatorios_Load(object sender, EventArgs e)
        {
            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet", dt));
            this.reportViewer1.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
            ConfigurarMargens();
        }

        private void ConfigurarMargens()
        {
            System.Drawing.Printing.PageSettings pg = new System.Drawing.Printing.PageSettings();
            pg.Margins.Top = 3;
            pg.Margins.Bottom = 3;
            pg.Margins.Left = 3;
            pg.Margins.Right = 3;
            pg.Landscape = false;
            reportViewer1.SetPageSettings(pg);
            reportViewer1.RefreshReport();
        }
    }
}