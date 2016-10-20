using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModel
{
    // Status của Order
    public class OrderStatus
    {
        public const string DELIVERING = "DELIVERING";
        public const string PROCESSING = "PROCESSING";
        public const string CANCELED = "CANCELED";
        public const string COMPLETED = "COMPLETED";
    }
}
