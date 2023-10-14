using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SampleWindowsApp.App_Data
{
    internal class Repository
    {



        string conString = "server =.; database = SampleStore; trusted_connection= true; ";

        public Repository()
        {

        }



        public List<InvoiceMaster> GetInvoices()
        {


            List<InvoiceMaster> invoices = new List<InvoiceMaster>();

            using (SqlConnection con = new SqlConnection(conString))
            {
                var cmd = con.CreateCommand();


                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "SP_GetInvoices";

                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                DataSet ds = new DataSet();
                con.Open();
                sda.Fill(ds);


                if (ds.Tables.Count > 0)
                {



                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        InvoiceMaster invoice = new InvoiceMaster();
                        invoice.InvoiceId = Convert.ToInt32(dr["InvoiceId"]);
                        invoice.Date = Convert.ToDateTime(dr["InvoiceDate"]);
                        invoice.CustomerName = dr["CustomerName"].ToString();
                        invoice.PhoneNo = dr["PhoneNo"].ToString();
                        invoice.CustomerAddress = dr["CustomerAddress"]?.ToString();
                        invoices.Add(invoice);
                    }

                }

            }

            return invoices;
        }

        public InvoiceMaster GetInvoice(int InvoiceId)
        {

            InvoiceMaster invoice = new InvoiceMaster();
            using (SqlConnection con = new SqlConnection(conString))
            {
                var cmd = con.CreateCommand();


                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "SP_GetInvoices";

                cmd.Parameters.Add(new SqlParameter("@invoiceId", InvoiceId));

                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                DataSet ds = new DataSet();

                sda.Fill(ds);


                if (ds.Tables.Count > 0)
                {

                    var dr = ds.Tables[0].Rows[0];





                    invoice.InvoiceId = Convert.ToInt32(dr["InvoiceId"]);
                    invoice.Date = Convert.ToDateTime(dr["InvoiceDate"]);
                    invoice.CustomerName = dr["CustomerName"].ToString();
                    invoice.PhoneNo = dr["PhoneNo"].ToString();
                    invoice.CustomerAddress = dr["CustomerAddress"]?.ToString();

                    foreach (DataRow row in ds.Tables[1].Rows)
                    {

                        ItemDetails item = new ItemDetails();


                        item.ItemName = row["ItemName"].ToString();
                        item.UnitPrice = Convert.ToDecimal(row["UnitPrice"]);
                        item.Quantity = Convert.ToUInt32(row["Quantity"]);

                        invoice.ItemList.Add(item);
                    }



                }

            }
            return invoice;
        }



        public int SaveInvoice(InvoiceMaster Invoice)
        {
            int rowNo = 0;
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                var tran = con.BeginTransaction();
                var cmd = con.CreateCommand();

                cmd.Transaction = tran;



                try
                {


                    cmd.CommandText = "select isnull(max(invoiceid), 0) + 1 as InvoiceId from InvoiceMaster ";


                    string Invoiceid = cmd.ExecuteScalar()?.ToString();



                    cmd.CommandText = $"INSERT INTO [dbo].[InvoiceMaster]([InvoiceId],[InvoiceDate],[CustomerName],[CustomerAddress],[PhoneNo]) VALUES (  {Invoiceid}, '{Invoice.Date.ToShortDateString()}', '{Invoice.CustomerName}', '{Invoice.CustomerAddress}', '{Invoice.PhoneNo}'   )";


                    rowNo = cmd.ExecuteNonQuery();


                    if (rowNo > 0)
                    {

                        foreach (var item in Invoice.ItemList)
                        {
                            cmd.CommandText = $"INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId] ,[ItemName] ,[UnitPrice] ,[Quantity])  VALUES ({Invoiceid} ,'{item.ItemName}' , '{item.UnitPrice}' , '{item.Quantity}')";


                            int r1 = cmd.ExecuteNonQuery();
                        }

                    }

                    tran.Commit();
                }
                catch (SqlException e)
                {

                    tran.Rollback();
                    MessageBox.Show(e.Message);
                    return 0;
                }
            }
            return rowNo;
        }


        public int UpdateInvoice(InvoiceMaster Invoice)
        {
            int rowNo = 0;
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                var tran = con.BeginTransaction();
                var cmd = con.CreateCommand();

                cmd.Transaction = tran;



                try
                {




                    cmd.CommandText = $"UPDATE [dbo].[InvoiceMaster]   SET [InvoiceDate] =  '{Invoice.Date.ToShortDateString()}', [CustomerName] = '{Invoice.CustomerName}',[CustomerAddress] = '{Invoice.CustomerAddress}',[PhoneNo] = '{Invoice.PhoneNo}' where InvoiceId = {Invoice.InvoiceId}";

                    rowNo = cmd.ExecuteNonQuery();


                    if (rowNo > 0)
                    {
                        cmd.CommandText = $"delete from [dbo].[InvoiceDetails] where InvoiceId = {Invoice.InvoiceId}";


                        if (cmd.ExecuteNonQuery() >= 0)
                        {
                            foreach (var item in Invoice.ItemList)
                            {
                                cmd.CommandText = $"INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId] ,[ItemName] ,[UnitPrice] ,[Quantity])  VALUES ({Invoice.InvoiceId} ,'{item.ItemName}' , '{item.UnitPrice}' , '{item.Quantity}')";


                                cmd.ExecuteNonQuery();
                            }
                        }

                           

                    }

                    tran.Commit();
                }
                catch (SqlException e)
                {

                    tran.Rollback();
                    MessageBox.Show(e.Message);
                    return 0;
                }
            }
            return rowNo;
        }

        public int DeleteInvoice(string InvoiceId)
        {
            int rowNo = 0;
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                var tran = con.BeginTransaction();
                var cmd = con.CreateCommand();

                cmd.Transaction = tran;




                try
                {

                    cmd.CommandText = $"delete from [dbo].[InvoiceDetails]   where InvoiceId = {InvoiceId}";

                    rowNo = cmd.ExecuteNonQuery();


                    cmd.CommandText = $"delete from [dbo].[InvoiceMaster]   where InvoiceId = {InvoiceId}";

                    rowNo = cmd.ExecuteNonQuery();

                    tran.Commit();

                }
                catch (SqlException e)
                {
                    tran.Rollback();
                    MessageBox.Show(e.Message);
                    return 0;
                }
            }
            return rowNo;
        }
    }
}
