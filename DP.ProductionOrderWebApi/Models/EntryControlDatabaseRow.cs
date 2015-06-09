using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DP.ProductionOrderWebApi.Models
{
    public class EntryControlDatabaseRow
    {
        public int Id { get; set; }
        public int StatusId { get; set; }

        public int? SLA { get; set; }
        public string StatusDescription { get; set; }

        public int ServiceId { get; set; }
        public string ServiceDescription { get; set; }

        public DateTime EntryDate { get; set; }


    }
}