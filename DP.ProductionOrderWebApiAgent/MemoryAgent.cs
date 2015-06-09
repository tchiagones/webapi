using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DP.ProductionOrderWebApiAgent.Helpers;
using DP.ProductionOrderWebApiAgent.Common;

namespace DP.ProductionOrderWebApiAgent
{
    public class MemoryAgent : BaseAgent
    {
        public MemoryAgent()
            : base("Memory")
        {
        }

        public void UpdateModel(int clientId)
        {
            var uri = CreateUrl("UpdateAll", clientId);
            Log(() => { Proxy.Get<bool>(uri); });
        }


    }
}
