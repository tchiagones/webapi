using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DP.CCM.Entity.Models.ValueObject;
using DP.CCM.Entity.Models.Filters;
using DP.ProductionOrderWebApi.Models;

namespace DP.ProductionOrderWebApi.Controllers
{
    public class EntryControlController : ApiController
    {
        [HttpPost]
        // GET api/ProductionOrderConsoldate/LoadConsolidatedOrders
        public ProductionOrderResult<EntryControlItem> LoadEntryControl([FromBody]EntryControlFilter filter)
        {
            return EntryControlFactory.ListEntryControl(filter);
        }

        [HttpGet]
        // GET api/productionorderconsoldate/5
        public bool UpdateEntryControl(int id)
        {
            try
            {
                EntryControlFactory.UpdateEntryControl(id);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
