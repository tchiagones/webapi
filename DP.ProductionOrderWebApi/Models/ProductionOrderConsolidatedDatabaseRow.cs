using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace DP.ProductionOrderWebApi.Models
{
    public class ProductionOrderConsolidatedDatabaseRow
    {
        public int? StatusId { get; set; }
        public string Status { get; set; }
        public string Status2 { get; set; }
        public decimal OrderId { get; set; }
        /// <summary> Identificador da comunicação </summary>
        public int CommunicationTypeId { get; set; }
        /// <summary> Nome da comunicação </summary>
        public string Communication { get; set; }
        /// <summary> Nome do serviço </summary>
        public string Service { get; set; }
        /// <summary> Identificador do serviço </summary>
        public int ServiceId { get; set; }
        /// <summary> Nome do tipo de canal de comunicação </summary>
        public string ChannelType { get; set; }
        /// <summary> Data de criação da ordem de produção </summary>
        public DateTime CreationDate { get; set; }
        public string CreationDateFormated
        {
            get { return CreationDate.ToString("dd/MM/yyyy HH:mm:ss"); }
        }
        public DateTime? ScheduleDate { get; set; }
        public string Data_Agendamento_Formatada
        {
            get
            {
                if (ScheduleDate.HasValue)
                    return ScheduleDate.Value.ToString("dd/MM/yyyy HH:mm:ss");
                else
                    return string.Empty;
            }
        }
        public DateTime? OppeningDate { get; set; }
        public string OppeningDateFormated
        {
            get
            {
                return OppeningDate.HasValue ? OppeningDate.Value.ToString("dd/MM/yyyy HH:mm:ss") : " - ";
            }
        }

        /// <summary> Total de processados </summary>
        public int AmountProcess { get; set; }
        /// <summary> Total de entregues com corfirmação </summary>
        public int QuantityDelivered { get; set; }
        /// <summary> Total de entregues sem confirmação </summary>
        public int QuantityNotDelivered { get; set; }

        public int ChannelTypeId { get; set; }

        public DateTime? StartDate { get; set; }
        
        public int OrderStatusId { get; set; }

        public int? ControlRegistryId { get; set; }
    }
}