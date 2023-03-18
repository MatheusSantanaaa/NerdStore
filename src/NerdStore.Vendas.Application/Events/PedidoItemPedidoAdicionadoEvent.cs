using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Application.Events
{
    public class PedidoItemPedidoAdicionadoEvent : Event
    {
        public Guid ClienteId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public Guid PedidoId { get; private set; }
        public decimal ValorUnitario { get; private set; }
        public int Quantidade { get; private set; }

        public PedidoItemPedidoAdicionadoEvent(Guid clienteId, Guid produtoId, Guid pedidoId, decimal valorUnitario, int quantidade)
        {
            AggregateId = pedidoId;
            ClienteId = clienteId;
            ProdutoId = produtoId;
            PedidoId = pedidoId;
            ValorUnitario = valorUnitario;
            Quantidade = quantidade;
        }
    }
}
