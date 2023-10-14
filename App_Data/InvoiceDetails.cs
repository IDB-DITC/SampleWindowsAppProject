using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleWindowsApp.App_Data
{
    internal class ItemDetails
    {
        public string ItemName { get; set; }
        public decimal UnitPrice { get; set; }
        public uint Quantity { get; set; }
        public decimal ItemTotal => UnitPrice * Quantity;
    }
}
