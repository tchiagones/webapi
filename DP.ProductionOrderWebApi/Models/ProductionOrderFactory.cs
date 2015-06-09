using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DP.CCM.Entity.Enum;
using DP.CCM.Modules.ProductionControl.Interface.Service;
using DP.Framework.Factory;
using DP.CCM.Entity.Models.Filters;
using DP.CCM.Entity.Models.ValueObject;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace DP.ProductionOrderWebApi.Models
{
    internal static class ProductionOrderFactory
    {
        private static readonly object _mock = new object();
        private static readonly IProductionControlService _productionControlService;
        public static Dictionary<int, ReadOnlyCollection<ProductionOrderConsolidatedDatabaseRow>> MemoryProductionOrder;
        public static IEnumerable<ProductionOrderConsolidatedDatabaseRow> ViewModelAdapter;

        static ProductionOrderFactory()
        {
            var container = new UnityBootstrapper();
            _productionControlService = container.Resolve<IProductionControlService>();


            MemoryProductionOrder = new Dictionary<int, ReadOnlyCollection<ProductionOrderConsolidatedDatabaseRow>>();
        }

        public static void UpdateProductionOrderConsolidated(int clientId)
        {
            lock (_mock)
            {
                var result = new ReadOnlyCollection<ProductionOrderConsolidatedDatabaseRow>(
                                                                    _productionControlService.GetOrders(clientId)
                                                                               .Select(order =>
                                                                                   {
                                                                                       var currentStatus = _productionControlService.GetCurrentStatus(order);
                                                                                       return new ProductionOrderConsolidatedDatabaseRow
                                                                                       {
                                                                                           OrderId = order.Id,
                                                                                           ScheduleDate = order.ScheduleDate.HasValue ? order.ScheduleDate.Value : order.ScheduleDate,
                                                                                           OppeningDate = currentStatus.StartDate,
                                                                                           StatusId = currentStatus.OrderStatus.Id,
                                                                                           Status = currentStatus.OrderStatus.Description,
                                                                                           Status2 = String.Format("Data/Hora de {0}", currentStatus.OrderStatus.Description2),
                                                                                           //QuantityDelivered = _productionControlService.GetSentQuantityByOrder(order),
                                                                                           //QuantityNotDelivered = order.ProductionOrderFeedbacks.Where(a => a.FeedbackType != null && a.FeedbackType.Id == (int)EnumFeedbackType.Returned).Sum(o => o.Value),
                                                                                           //AmountProcess = order.ProductionOrderFeedbacks.Where(a => a.FeedbackType != null && a.FeedbackType.Id == (int)EnumFeedbackType.Processed).Sum(o => o.Value),


                                                                                           Communication = order.ServiceCommunication.CommunicationType.Description,
                                                                                           CommunicationTypeId = order.ServiceCommunication.CommunicationTypeId,

                                                                                           ChannelTypeId = order.ServiceCommunication.CommunicationType.ChannelTypeId,
                                                                                           StartDate = currentStatus.StartDate,

                                                                                           Service = order.ServiceCommunication.Service.Name,
                                                                                           ServiceId = order.ServiceCommunication.Service.Id,
                                                                                           ChannelType = order.ServiceCommunication.CommunicationType.ChannelType.Description,

                                                                                           OrderStatusId = currentStatus.OrderStatus.Id,
                                                                                           ControlRegistryId = order.ControlRegistryId
                                                                                       };
                                                                                   })
                    //.Where(a => a.StatusId == (int)EnumOrderStatus.Concluido)
                                                                                .ToList());

                var productionOrderIds = result.Select(o => Convert.ToInt32(o.OrderId));

                var quantities = _productionControlService.GetProductionOrderQuantities(productionOrderIds).ToList();

                quantities.ForEach(o =>
                {
                    var order = result.FirstOrDefault(resultItem => resultItem.OrderId == o.ProductionOrderId);

                    if (order != null)
                    {
                        order.AmountProcess = o.ProcessQuantity;
                        order.QuantityDelivered = o.SentQuantity;
                        order.QuantityNotDelivered = o.ReturnedQuantity;
                    }

                });



                lock (MemoryProductionOrder)
                {
                    if (MemoryProductionOrder.ContainsKey(clientId))
                    {
                        MemoryProductionOrder[clientId] = result;
                    }
                    else
                    {
                        MemoryProductionOrder.Add(clientId, result);
                    }
                }
            }
        }



        private static IEnumerable<ProductionOrderConsolidatedDatabaseRow> ApplyFilterConsolidatedOrdersProductionOrderConsolidated(ProductionOrderConsolidatedFilter filter)
        {
            if (!ValidateClientInMemory(filter.ClientId))
                UpdateProductionOrderConsolidated(filter.ClientId);

            var query = MemoryProductionOrder[filter.ClientId]
                        .Where(a => a.StatusId == (int)EnumOrderStatus.Concluido)
                        .AsEnumerable();

            if (filter.CanalId.HasValue)
            {
                query = query.Where(x => x.ChannelTypeId == filter.CanalId.Value);
            }

            if (filter.ServicoIds.Any())
            {
                query = query.Where(x => filter.ServicoIds.Contains(x.ServiceId));
            }

            if (filter.DataInicio.HasValue)
            {
                query = query.Where(x => x.StartDate >= filter.DataInicio);
            }

            if (filter.DataFim.HasValue)
            {
                query = query.Where(x => x.StartDate <= filter.DataFim);
            }
            if (!string.IsNullOrEmpty(filter.FiltroTexto))
            {
                query = query.Where(o => o.OppeningDateFormated.ToLower().Contains(filter.FiltroTexto.ToLower()) ||
                            o.AmountProcess.ToString().Contains(filter.FiltroTexto) ||
                            o.QuantityDelivered.ToString().Contains(filter.FiltroTexto) ||
                            o.QuantityNotDelivered.ToString().Contains(filter.FiltroTexto));
            }


            return query;
        }

        public static ProductionOrderResult<ProductionOrderConsolidatedItem> ListOrdersProductionOrderConsolidated(ProductionOrderConsolidatedFilter filter)
        {
            var resultQuery = ApplyFilterConsolidatedOrdersProductionOrderConsolidated(filter)
                    .GroupBy(a => a.OppeningDate.Value.Year + a.OppeningDate.Value.Month).Select(b =>
                                new ProductionOrderConsolidatedItem
                                {
                                    OppeningDate = b.First().OppeningDate.Value,
                                    AmountProcess = b.Sum(c => c.AmountProcess),
                                    QuantityDelivery = b.Sum(c => c.QuantityDelivered),
                                    QuantityNotDelivery = b.Sum(c => c.QuantityNotDelivered)
                                });

            if (filter.AscOrderDirection)
            {
                resultQuery = resultQuery.OrderBy(Utils.Utility.GetFunc<ProductionOrderConsolidatedItem>(filter.OrderFieldPropertyName));
            }
            else
            {
                resultQuery = resultQuery.OrderByDescending(Utils.Utility.GetFunc<ProductionOrderConsolidatedItem>(filter.OrderFieldPropertyName));
            }

            return new ProductionOrderResult<ProductionOrderConsolidatedItem>(resultQuery.Skip(filter.DisplayStart).Take(filter.DisplayLength), resultQuery.Count());
        }

        public static IEnumerable<ProductionOrderPieChartItem> LoadPieChartProductionOrderConsolidated(ProductionOrderConsolidatedFilter filter)
        {
            var resultado = ApplyFilterConsolidatedOrdersProductionOrderConsolidated(filter)
                                    .GroupBy(o => o.Service)
                                    .Select(o => o.Select(q => new { q.Service, q.ServiceId, Quantidade_Processada = q.AmountProcess }))
                                    .Distinct();
            var result = new List<ProductionOrderPieChartItem>();

            resultado.ToList().ForEach(item =>
                result.Add(new ProductionOrderPieChartItem
                {
                    ColumnName = item.FirstOrDefault().Service,
                    Value = item.Sum(o => o.Quantidade_Processada)
                }));
            return result;
        }

        public static IEnumerable<ProductionOrderConsolidateToExcelItem> ListToExcelConsolidateOrders(ProductionOrderConsolidatedFilter filter)
        {
            var resultQuery = ApplyFilterConsolidatedOrdersProductionOrderConsolidated(filter)
                .Select(x => new ProductionOrderConsolidateToExcelItem
                {
                    AmountProcessd = x.AmountProcess,
                    OppeningDate = x.OppeningDate,
                    QuantityDelivered = x.QuantityDelivered,
                    QuantityNotDelivered = x.QuantityNotDelivered,
                    Service = x.Service,
                    ServiceId = x.ServiceId
                });
            return resultQuery;
        }

        public static bool ValidateClientInMemory(int clientId)
        {
            return MemoryProductionOrder.ContainsKey(clientId);
        }





        private static IEnumerable<ProductionOrderConsolidatedDatabaseRow> ApplyFilterProductionOrder(ProductionOrderFilter filter)
        {
            if (!ValidateClientInMemory(filter.ClientId))
                UpdateProductionOrderConsolidated(filter.ClientId);

            var query = MemoryProductionOrder[filter.ClientId].AsEnumerable();
            if (filter.CanalId.HasValue)
            {
                query = query.Where(x => x.ChannelTypeId == filter.CanalId.Value);
            }

            if (filter.ServicoIds.Any())
            {
                query = query.Where(x => filter.ServicoIds.Contains(x.ServiceId));
            }

            if (filter.DataInicio.HasValue)
            {
                query = query.Where(x => x.StartDate >= filter.DataInicio);
            }

            if (filter.DataFim.HasValue)
            {
                query = query.Where(x => x.StartDate <= filter.DataFim.Value.AddDays(1).AddSeconds(-1));
            }

            if (filter.EntryControlId > 0)
            {
                query = query.Where(x => x.ControlRegistryId == filter.EntryControlId);
            }

            if (filter.OrderStatusId > 0)
            {
                query = query.Where(x => x.OrderStatusId == filter.OrderStatusId);
            }

            if (!string.IsNullOrEmpty(filter.FiltroTexto) && filter.FiltroTexto.Length > 3)
            {
                query = query.Where(
                    o => o.OrderId.ToString("00000").Contains(filter.FiltroTexto) ||
                    o.Communication.ToLower().Contains(filter.FiltroTexto.ToLower()) ||
                    o.Service.ToLower().Contains(filter.FiltroTexto.ToLower()) ||
                    o.ChannelType.ToLower().Contains(filter.FiltroTexto.ToLower()) ||
                    o.StartDate.ToString().Contains(filter.FiltroTexto) ||
                    o.QuantityDelivered.ToString().Contains(filter.FiltroTexto) ||
                    o.QuantityNotDelivered.ToString().Contains(filter.FiltroTexto) ||
                    o.AmountProcess.ToString().Contains(filter.FiltroTexto)
                );
            }

            return query;
        }

        public static ProductionOrderResult<ProductionOrderItem> ListOrders(ProductionOrderFilter filter)
        {
            var resultQuery = ApplyFilterProductionOrder(filter)
                                .Select(order =>
                                    new ProductionOrderItem
                                    {
                                        OrderId = order.OrderId,
                                        ScheduleDate = order.ScheduleDate,
                                        OppeningDate = order.OppeningDate,
                                        StatusId = order.OrderStatusId,
                                        Status = order.Status,
                                        Status2 = order.Status2,
                                        QuantityDelivered = order.QuantityDelivered,
                                        QuantityNotDelivered = order.QuantityNotDelivered,
                                        AmountProcess = order.AmountProcess,
                                        Communication = order.Communication,
                                        CommunicationId = order.CommunicationTypeId,
                                        Service = order.Service,
                                        ServiceId = order.ServiceId,
                                        Channel = order.ChannelType
                                    });


            if (filter.AscOrderDirection)
            {
                resultQuery = resultQuery.OrderBy(Utils.Utility.GetFunc<ProductionOrderItem>(filter.OrderFieldPropertyName)).ToList();
            }
            else
            {
                resultQuery = resultQuery.OrderByDescending(Utils.Utility.GetFunc<ProductionOrderItem>(filter.OrderFieldPropertyName)).ToList();
            }

            return new ProductionOrderResult<ProductionOrderItem>(resultQuery.Skip(filter.DisplayStart).Take(filter.DisplayLength), resultQuery.Count());
        }

        public static IEnumerable<ProductionOrderPieChartItem> ListOrdersPieChart(ProductionOrderFilter filter)
        {
            return ApplyFilterProductionOrder(filter)
                    .GroupBy(o => o.Service)
                    .Select(o => new ProductionOrderPieChartItem
                                    {
                                        ColumnName = o.Key,
                                        Value = o.Sum(f => f.AmountProcess)
                                    })
                    .ToList();
        }

        public static IEnumerable<ProductionOrderLineChartItem> ListOrdersLineChart(ProductionOrderFilter filter)
        {
            var resultQuery = ApplyFilterProductionOrder(filter)
                .Select(x => new ProductionOrderLineChartItem
                            {
                                AmountProcessd = x.AmountProcess,
                                OppeningDate = x.OppeningDate,
                                QuantityDelivered = x.QuantityDelivered,
                                QuantityNotDelivered = x.QuantityNotDelivered,
                                Service = x.Service,
                                ServiceId = x.ServiceId
                            });

            return resultQuery;
        }
    }
}