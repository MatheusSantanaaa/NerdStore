using NerdStore.Core.Messages.CommonMessages.IntegrationEvents;

namespace NerdStore.Pagamentos.Business.Events
{
    public class PagamentoRecusadoEvent : IntegrationEvents
    {
        public Guid PedidoId { get; set; }
        public Guid ClienteId { get; set; }
        public Guid PagamentoId { get; set; }
        public Guid TransacaoId { get; set; }
        public decimal Total { get; set; }

        public PagamentoRecusadoEvent(Guid pedidoId, Guid clienteId, Guid pagamentoId, Guid transacaoId, decimal total)
        {
            PedidoId = pedidoId;
            ClienteId = clienteId;
            PagamentoId = pagamentoId;
            TransacaoId = transacaoId;
            Total = total;
        }
    }

}