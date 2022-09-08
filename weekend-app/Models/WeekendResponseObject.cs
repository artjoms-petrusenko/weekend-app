using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weekend_app.Models
{
    public class WeekendResponseObject
    {
        public bool Status { get; set; }
        public string Code { get; set; }
        public string? FailMessage { get; set; }
        public List<Weekend>? WeekendList { get; set; }
    }
}
