using NerdStore.Core.Messages;
using NerdStore.Vendas.Application.Validations;

namespace NerdStore.Vendas.Application.Commands
{
    public class RemoverItemPedidoCommand : Command
    {
        public Guid ClienteId { get; private set; }
        public Guid ProdutoId { get; private set; }

        public RemoverItemPedidoCommand(Guid clienteId, Guid produtoId)
        {
            ClienteId = clienteId;
            ProdutoId = produtoId;
        }
        public override bool IsValid()
        {
            ValidationResult = new RemoverItemPedidoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
