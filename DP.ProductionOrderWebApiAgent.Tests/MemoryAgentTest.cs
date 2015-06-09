using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DP.ProductionOrderWebApiAgent.Tests
{
    [TestClass]
    public class MemoryAgentTest
    {
        [TestMethod]
        public void TestMethod1()
        {

            var memoryAgent = new MemoryAgent();
            memoryAgent.UpdateModel(9);

        }
    }
}
