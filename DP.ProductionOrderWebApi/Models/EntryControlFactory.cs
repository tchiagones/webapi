using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;
using DP.CCM.Modules.EntryControl.Interface.Service;
using DP.CCM.Entity.Enum;
using DP.Framework.Utils;
using DP.CCM.Entity.Models.Filters;
using DP.CCM.Entity.Models.ValueObject;

namespace DP.ProductionOrderWebApi.Models
{
    internal static class EntryControlFactory
    {
        private static readonly IEntryControlFileService _entryControlService;
        public static Dictionary<int, ReadOnlyCollection<EntryControlDatabaseRow>> MemoryEntryControl;

        static EntryControlFactory()
        {
            var container = new UnityBootstrapper();
            _entryControlService = container.Resolve<IEntryControlFileService>();


            MemoryEntryControl = new Dictionary<int, ReadOnlyCollection<EntryControlDatabaseRow>>();
        }


        public static void UpdateEntryControl(int clientId)
        {

            var query = _entryControlService.Get(clientId);

            var returnn = query.Select(entry => new EntryControlDatabaseRow
            {
                StatusDescription = EnumHelper.GetEnumDescription((EnumControlRegistryStatus)entry.CalculatedStatusId),
                StatusId = entry.CalculatedStatusId,
                Id = entry.Id,
                EntryDate = entry.CreateDate,
                SLA = entry.SLA,
                ServiceDescription = entry.Service.Name,
                ServiceId = entry.ServiceId
            }).ToList();

            var result = new ReadOnlyCollection<EntryControlDatabaseRow>(returnn);

            lock (MemoryEntryControl)
            {
                if (MemoryEntryControl.ContainsKey(clientId))
                {
                    MemoryEntryControl[clientId] = result;
                }
                else
                {
                    MemoryEntryControl.Add(clientId, result);
                }
            }
        }

        public static IEnumerable<EntryControlDatabaseRow> GetEntryControlCollection(int clientId)
        {

            if (MemoryEntryControl.ContainsKey(clientId).Equals(false))
                UpdateEntryControl(clientId);

            return MemoryEntryControl[clientId].AsEnumerable();
        }

        private static IEnumerable<EntryControlDatabaseRow> ApplyFilterEntryControl(EntryControlFilter filter)
        {
            var query = GetEntryControlCollection(filter.ClientId);


            if (!string.IsNullOrEmpty(filter.FiltroTexto))
            {
                query = query.Where(o => string.IsNullOrWhiteSpace(filter.FiltroTexto).Equals(true) ? true :
                   o.Id.ToString("00000").Contains(filter.FiltroTexto) || //Id
                   o.ServiceDescription.Contains(filter.FiltroTexto) || //Service Name
                   (o.SLA.HasValue ? o.SLA.Value.ToString().Contains(filter.FiltroTexto) : false) || // SLA
                   (o.EntryDate.ToString("dd/MM/yyyy HH:mm:ss").Contains(filter.FiltroTexto)));


            }

            if (filter.ServicoIds.Any())
            {
                query = query.Where(x => filter.ServicoIds.Contains(x.ServiceId));
            }

            if (filter.DataInicio.HasValue)
            {
                query = query.Where(x => x.EntryDate >= filter.DataInicio);
            }

            if (filter.DataFim.HasValue)
            {
                query = query.Where(x => x.EntryDate <= filter.DataFim.Value.AddDays(1).AddSeconds(-1));
            }

            if (filter.ControlRegistryStatus.HasValue)
            {
                query = query.Where(o => o.StatusId == (int)filter.ControlRegistryStatus.Value);
            }


            return query;
        }

        public static ProductionOrderResult<EntryControlItem> ListEntryControl(EntryControlFilter filter)
        {
            var resultQuery = ApplyFilterEntryControl(filter).Select(o => new EntryControlItem()
            {
                EntryDate = o.EntryDate,
                //Id = o.Id.ToString("00000"),
                Id = o.Id,
                Service = o.ServiceDescription,
                SLA = o.SLA,
                Status = o.StatusDescription,
                StatusId = o.StatusId
            });

            if (filter.AscOrderDirection)
            {
                resultQuery = resultQuery.OrderBy(Utils.Utility.GetFunc<EntryControlItem>(filter.OrderFieldPropertyName));
            }
            else
            {
                resultQuery = resultQuery.OrderByDescending(Utils.Utility.GetFunc<EntryControlItem>(filter.OrderFieldPropertyName));
            }

            return new ProductionOrderResult<EntryControlItem>(resultQuery.Skip(filter.DisplayStart).Take(filter.DisplayLength), resultQuery.Count());
        }




    }




}