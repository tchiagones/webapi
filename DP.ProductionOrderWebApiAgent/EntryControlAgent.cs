using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DP.ProductionOrderWebApiAgent.Helpers;
using DP.CCM.Entity.Models.ValueObject;
using DP.CCM.Entity.Models.Filters;
using DP.ProductionOrderWebApiAgent.Common;

namespace DP.ProductionOrderWebApiAgent
{
    public class EntryControlAgent : BaseAgent
    {

        public EntryControlAgent()
            : base("EntryControl")
        {
        }

        public ProductionOrderResult<EntryControlItem> ListEntryControl(EntryControlFilter filter)
        {
            return Post<EntryControlFilter, ProductionOrderResult<EntryControlItem>>("LoadEntryControl", filter);
        }
    }
}
