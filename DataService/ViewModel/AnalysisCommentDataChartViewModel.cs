using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModel
{

    public class AnalysisDataForCharViewModel
    {
        public DateTime Time { get; set; }
        public List<AnalysisCommentDataChartViewModel> ListData { get; set; }
        public AnalysisDataForCharViewModel()
        {
            ListData = new List<AnalysisCommentDataChartViewModel>();
        }
    }

    public class AnalysisCommentDataChartViewModel
    {
        public int IntentId { get; set; }
        public string IntentName { get; set; }
        public int CommentNumber { get; set; }
    }
}
