using FluentValidation;
using NerdStore.Vendas.Application.Commands;

namespace NerdStore.Vendas.Application.Validations
{
    public class RemoverItemPedidoValidation : AbstractValidator<RemoverItemPedidoCommand>
    {
        public RemoverItemPedidoValidation()
        {
            RuleFor(c => c.ClienteId)
               .NotEqual(Guid.Empty)
               .WithMessage("Id do cliente é inválido");

            RuleFor(c => c.ProdutoId)
              .NotEqual(Guid.Empty)
              .WithMessage("Id do produto é inválido");
        }
    }
}
