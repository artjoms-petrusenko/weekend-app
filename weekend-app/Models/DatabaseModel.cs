using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weekend_app.Models
{
    public class DatabaseModel
    {
        public long Id { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string Weekends { get; set; }
    }
}
