using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DP.CCM.Entity.Models.ValueObject;
using DP.CCM.Entity.Models.Filters;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Web.Mvc;
using DP.ProductionOrderWebApi.Models;
using DP.ProductionOrderWebApiAgent;

namespace DP.ProductionOrderWebApi.Tests
{
    [TestClass]
    public class ProductionOrderConsoldateControllerTest
    {
        private JsonHelper _proxy;

        public ProductionOrderConsoldateControllerTest()
        {
            _proxy = new JsonHelper();
        }

        [TestMethod]
        public void LoadConsolidatedOrders_DeveRetornarUmaListaValidaDeProductionOrderResult_Success()
        {
            var filter = new ProductionOrderConsolidatedFilter
            {
                OrderFieldPropertyName = GetExpressionText((ProductionOrderConsolidatedItem c) => c.QuantityDelivery),
                ServicoIds = new List<int> { 1, 2 },
                DisplayStart = 0,
                DisplayLength = 10,
                DataInicio = DateTime.Now.AddMonths(-4),
                DataFim = DateTime.Now.AddDays(2),
                ClientId = 9,
                AscOrderDirection = false
            };

            var result =
                _proxy.Post<ProductionOrderConsolidatedFilter, ProductionOrderResult<ProductionOrderConsolidatedItem>>("productionorderconsoldate/LoadConsolidatedOrders", filter);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 5);
        }
        [TestMethod]
        public void UpdateMemory_DeveRetornarTrue_Success()
        {
            var result = _proxy.Get<Boolean>("productionorderconsoldate/UpdateOrder/9");

            Assert.IsTrue(result);
        }
        public static string GetExpressionText<TEntity, T>(Expression<Func<TEntity, T>> expression)
        {
            return ExpressionHelper.GetExpressionText((LambdaExpression)expression);
        }

        [TestMethod]
        public void UpdateMemoryViaApiAgent()
        {
            new MemoryAgent().UpdateModel(9);
        }
    }
}
