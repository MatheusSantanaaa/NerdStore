using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Application.Commands
{
    public class FinalizarPedidoCommand : Command
    {
        public Guid PedidoId { get; set; }
        public Guid ClienteId { get; set; }

        public FinalizarPedidoCommand(Guid pedidoId, Guid clienteId)
        {
            AggregateId = pedidoId;
            PedidoId = pedidoId;
            ClienteId = clienteId;
        }
    }

    public class CancelarProcessamentoPedidoCommand : Command
    {

        public Guid PedidoId { get; set; }
        public Guid ClienteId { get; set; }
        public CancelarProcessamentoPedidoCommand(Guid pedidoId, Guid clienteId)
        {
            AggregateId = pedidoId;
            PedidoId = pedidoId;
            ClienteId = clienteId;
        }
    }
}
