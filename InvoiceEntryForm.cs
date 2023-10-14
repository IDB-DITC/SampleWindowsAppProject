using SampleWindowsApp.App_Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SampleWindowsApp
{
    public partial class InvoiceEntryForm : Form
    {


        Repository repository = new Repository();
        public int InvoiceId { get; set; } = 0;

        public InvoiceEntryForm()
        {
            InitializeComponent();



        }
        private void InvoiceEntry_Load(object sender, EventArgs e)
        {
            ResetForm();
            LoadInitData();
        }


        void LoadInitData()
        {
            if (InvoiceId > 0)
            {
                var invoice = repository.GetInvoice(InvoiceId);

                txtId.Text = invoice.InvoiceId.ToString();
                InvoiceDate.Value = invoice.Date;
                txtPhoneNo.Text = invoice.PhoneNo;
                txtName.Text = invoice.CustomerName;
                txtAddress.Text = invoice.CustomerAddress;


                itemDetailsBindingSource.DataSource = invoice.ItemList;

                

            }
        }


        void LoadUpdateData()
        {

        }


        void ResetForm()
        {
            txtId.Text = null;
            InvoiceDate.Value = DateTime.Now;
            txtName.Text = null;
            txtAddress.Text = null;
            txtPhoneNo.Text = null;
            gridItem.Rows.Clear();
            InvoiceDate.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                InvoiceMaster invoice = new InvoiceMaster();

                if (txtId.Text.Length > 0)
                    invoice.InvoiceId = Convert.ToInt32(txtId.Text);

                invoice.Date = InvoiceDate.Value;
                invoice.PhoneNo = txtPhoneNo.Text;
                invoice.CustomerName = txtName.Text;
                invoice.CustomerAddress = txtAddress.Text;





                foreach (DataGridViewRow item in gridItem.Rows)
                {

                    if (item.IsNewRow) continue;

                    ItemDetails itemDetails = new ItemDetails();

                    itemDetails.ItemName = item.Cells[0].Value.ToString();
                    itemDetails.UnitPrice = Convert.ToDecimal(item.Cells[1].Value);
                    itemDetails.Quantity = Convert.ToUInt32(item.Cells[2].Value);
                    invoice.ItemList.Add(itemDetails);
                }

                if (txtId.Text.Length > 0)
                {
                    
                    int rw = repository.UpdateInvoice(invoice);


                    if (rw > 0)
                    {
                        MessageBox.Show("Data updated successfully");
                    }
                }
                else
                {
                    int rw = repository.SaveInvoice(invoice);


                    if (rw > 0)
                    {
                        MessageBox.Show("Data saved successfully");
                    }
                }
                

                ResetForm();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtId.Text.Length > 0)
            {

                var dialog = MessageBox.Show("Delete record", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if(dialog == DialogResult.OK)
                {
                    int rw = repository.DeleteInvoice(txtId.Text);


                    if (rw > 0)
                    {
                        MessageBox.Show("Data deleted successfully");
                    }
                }




            }
        }
    }
}
