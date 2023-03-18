using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Application.Events
{
    public class PedidoProdutoRemovidoItemEvent : Event
    {
        public Guid? ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }
        public Guid ProdutoId { get; private set; }

        public PedidoProdutoRemovidoItemEvent(Guid? clienteId, Guid pedidoId, Guid produtoId)
        {
            AggregateId = pedidoId;
            ClienteId = clienteId;
            PedidoId = pedidoId;
            ProdutoId = produtoId;
        }
    }
}
