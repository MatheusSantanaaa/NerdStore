using NerdStore.Core.Messages.CommonMessages.DomainEvents;

namespace NerdStore.Catalogo.Domain.Events
{
    public class ProdutoEstoqueAbaixoEvent : DomainEvent
    {
        public int QuantidadeRestante { get; private set; }
        public ProdutoEstoqueAbaixoEvent(Guid aggregateId, int quantidadeRestante) : base(aggregateId)
        {
            QuantidadeRestante = quantidadeRestante;
        }
    }
}
