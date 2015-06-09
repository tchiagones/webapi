using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DP.CCM.Entity.Models.ValueObject;
using DP.CCM.Entity.Models.Filters;
using DP.ProductionOrderWebApiAgent.Helpers;
using DP.ProductionOrderWebApiAgent.Common;

namespace DP.ProductionOrderWebApiAgent
{
    public class ProductionOrderAgent : BaseAgent
    {
        public ProductionOrderAgent()
            : base("ProductionOrder")
        {
        }

        public ProductionOrderResult<ProductionOrderItem> ListProductionOrder(ProductionOrderFilter filter)
        {
            return Post<ProductionOrderFilter, ProductionOrderResult<ProductionOrderItem>>("ListOrders", filter);
        }

        public ProductionOrderResult<ProductionOrderConsolidatedItem> ListConsolidatedOrders(ProductionOrderConsolidatedFilter filter)
        {
            return Post<ProductionOrderConsolidatedFilter, ProductionOrderResult<ProductionOrderConsolidatedItem>>("ListConsolidatedOrders", filter);
        }


        public IEnumerable<ProductionOrderPieChartItem> ListOrdersPieChart(ProductionOrderFilter filter)
        {
            return Post<ProductionOrderFilter, IEnumerable<ProductionOrderPieChartItem>>("ListOrdersPieChart", filter);
        }

        public IEnumerable<ProductionOrderLineChartItem> ListOrdersLineChart(ProductionOrderFilter filter)
        {
            return Post<ProductionOrderFilter, IEnumerable<ProductionOrderLineChartItem>>("ListOrdersLineChart", filter);
        }

        public IEnumerable<ProductionOrderConsolidateToExcelItem> ListToExcelConsolidateOrders(ProductionOrderConsolidatedFilter filter)
        {
            return Post<ProductionOrderConsolidatedFilter, IEnumerable<ProductionOrderConsolidateToExcelItem>>("ListToExcelConsolidateOrders", filter);
        }

        public IEnumerable<ProductionOrderPieChartItem> ListPieChartConsolidateOrders(ProductionOrderConsolidatedFilter filter)
        {
            return Post<ProductionOrderConsolidatedFilter, IEnumerable<ProductionOrderPieChartItem>>("ListPieChartConsolidateOrders", filter);
        }



    }
}
