using NerdStore.Core.DomainObjects.DTO;

namespace NerdStore.Core.Messages.CommonMessages.IntegrationEvents
{
    public class PedidoProcessamentoCanceladoEvent : IntegrationEvents
    {
        public Guid PedidoId { get; set; }
        public Guid ClienteId { get; set; }
        public ListaProdutosPedido ProddutosPedido { get; set; }

        public PedidoProcessamentoCanceladoEvent(Guid pedidoId, Guid clienteId, ListaProdutosPedido proddutosPedido)
        {
            AggregateId = pedidoId;
            PedidoId = pedidoId;
            ClienteId = clienteId;
            ProddutosPedido = proddutosPedido;
        }
    }
}
