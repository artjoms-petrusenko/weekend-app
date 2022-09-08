using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weekend_app.Models
{
    public class CountryResponseObject
    {
        public bool Status { get; set; }
        public string Code { get; set; }
        public string FailMessage { get; set; }
        public List<Country>? CountryList { get; set; }

    }
}
