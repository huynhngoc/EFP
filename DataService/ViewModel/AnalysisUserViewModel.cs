using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModel
{
    public class AnalysisUserViewModel
    {
        public string UserName { get; set; }
        public string UserFBId { get; set; }
        public bool IsCustomer { get; set; }
        public int TotalComment { get; set; }
        public int UnreadComment { get; set; }
        public List<StatusTotal> ListStatusNumber;
        public List<IntentTotal> ListIntentNumber;
        public AnalysisUserViewModel()
        {
            ListStatusNumber = new List<StatusTotal>();
            ListIntentNumber = new List<IntentTotal>();
        }
    }

    public class StatusTotal
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int StatusCount { get; set; }
    }

    public class IntentTotal
    {
        public string IntentName { get; set; }
        public int IntentCount { get; set; }
    }

}
