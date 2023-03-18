using NerdStore.Core.DomainObjects.DTO;
using NerdStore.Core.Messages.CommonMessages.DomainEvents;

namespace NerdStore.Core.Messages.CommonMessages.IntegrationEvents
{
    public class PedidoIniciadoEvent : IntegrationEvents
    {
        public Guid PedidoId { get; private set; }
        public Guid ClienteId { get; private set; }
        public ListaProdutosPedido ProdutosPedido { get; private set; }
        public decimal Total { get; private set; }
        public string NomeCartao { get; private set; }
        public string NumeroCartao { get; private set; }
        public string ExpiracaoCartao { get; private set; }
        public string CvvCartao { get; private set; }

        public PedidoIniciadoEvent(Guid pedidoId, Guid clienteId, ListaProdutosPedido produtosPedido, decimal total, string nomeCartao, string numeroCartao, string expiracaoCartao, string cvvCartao)
        {
            PedidoId = pedidoId;
            ClienteId = clienteId;
            ProdutosPedido = produtosPedido;
            Total = total;
            NomeCartao = nomeCartao;
            NumeroCartao = numeroCartao;
            ExpiracaoCartao = expiracaoCartao;
            CvvCartao = cvvCartao;
        }
    }
}
