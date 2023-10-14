using SampleWindowsApp.App_Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SampleWindowsApp
{
    public partial class InvoiceListForm : Form
    {

        Repository repository = new Repository();
        public InvoiceListForm()
        {
            InitializeComponent();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var id = InvoiceGrid.SelectedRows[0].Cells[0].Value.ToString();


            if(int.Parse(id) > 0)
            {
                InvoiceEntryForm form = new InvoiceEntryForm();

                form.InvoiceId = int.Parse(id);


                form.ShowDialog(this);
            }

           
        }

        private void InvoiceListForm_Load(object sender, EventArgs e)
        {
            DataLoad();
        }



        void DataLoad()
        {

           //this.InvoiceGrid.DataSource = repository.GetInvoices();
           this.invoiceMasterBindingSource.DataSource = repository.GetInvoices();

        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            DataLoad();
        }
    }
}
