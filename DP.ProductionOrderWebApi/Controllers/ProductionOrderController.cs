using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DP.CCM.Entity.Models.Filters;
using DP.CCM.Entity.Models.ValueObject;
using DP.CCM.Entity.Enum;
using DP.CCM.Modules.ECM.Interface.Service;
using DP.CCM.Modules.ProductionControl.Interface.Service;
using DP.Framework.Factory;
using DP.ProductionOrderWebApi.Models;
using System.Collections.ObjectModel;

namespace DP.ProductionOrderWebApi.Controllers
{
    public class ProductionOrderController : ApiController
    {
        [HttpPost]
        // GET api/ProductionOrder/ListConsolidatedOrders
        public ProductionOrderResult<ProductionOrderConsolidatedItem> ListConsolidatedOrders([FromBody]ProductionOrderConsolidatedFilter filter)
        {
            return ProductionOrderFactory.ListOrdersProductionOrderConsolidated(filter);
        }

        [HttpPost]
        // POST api/ProductionOrder/ListPieChartConsolidateOrders
        public IEnumerable<ProductionOrderPieChartItem> ListPieChartConsolidateOrders([FromBody]ProductionOrderConsolidatedFilter filter)
        {
            return ProductionOrderFactory.LoadPieChartProductionOrderConsolidated(filter);
        }

        [HttpPost]
        // POST api/ProductionOrder/ListToExcelConsolidateOrders
        public IEnumerable<ProductionOrderConsolidateToExcelItem> ListToExcelConsolidateOrders([FromBody] ProductionOrderConsolidatedFilter filter)
        {
            return ProductionOrderFactory.ListToExcelConsolidateOrders(filter);
        }

        // POST api/ProductionOrder/ListOrders
        public ProductionOrderResult<ProductionOrderItem> ListOrders(ProductionOrderFilter filter)
        {
            return ProductionOrderFactory.ListOrders(filter);
        }

        // POST api/ProductionOrder/ListOrdersPieChart
        public IEnumerable<ProductionOrderPieChartItem> ListOrdersPieChart([FromBody]ProductionOrderFilter filter)
        {
            return ProductionOrderFactory.ListOrdersPieChart(filter);
        }

        // POST api/ProductionOrder/ListOrdersLineChart
        public IEnumerable<ProductionOrderLineChartItem> ListOrdersLineChart([FromBody]ProductionOrderFilter filter)
        {
            return ProductionOrderFactory.ListOrdersLineChart(filter);
        }

      
    }
}
