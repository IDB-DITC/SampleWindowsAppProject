using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleWindowsApp.App_Data
{
    internal class InvoiceMaster
    {

        public int InvoiceId { get; set; }
        public DateTime Date { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string PhoneNo { get; set; }
        public List<ItemDetails> ItemList { get; set; } = new List<ItemDetails>();



    }
}
