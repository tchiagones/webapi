using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DP.ProductionOrderWebApi.Models;
using System.Threading;
using DP.CCM.Modules.ProductionControl.Interface.Service;

namespace DP.ProductionOrderWebApi.Controllers
{
    public class MemoryController : ApiController
    {
        private readonly IProductionControlService _productionControlService;

        public MemoryController()
        {
            _productionControlService = new UnityBootstrapper().Resolve<IProductionControlService>();
        }

        [HttpGet]
        public bool UpdateAll(int id)
        {
            //try
            //{
            //    var thread = new Thread(() => Update(id));
            //    thread.Start();

            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}
            return true;
        }

        internal void Update(int id)
        {
            //ProductionOrderFactory.UpdateProductionOrderConsolidated(id);
            //EntryControlFactory.UpdateEntryControl(id);
        }
    }
}
