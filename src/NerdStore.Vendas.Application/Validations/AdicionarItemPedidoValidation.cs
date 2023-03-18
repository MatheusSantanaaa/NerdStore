using FluentValidation;
using NerdStore.Vendas.Application.Commands;

namespace NerdStore.Vendas.Application.Validations
{
    public class AdicionarItemPedidoValidation : AbstractValidator<AdicionarItemPedidoCommand>
    {
        public AdicionarItemPedidoValidation()
        {
            RuleFor(c => c.ClienteId)
               .NotEqual(Guid.Empty)
               .WithMessage("Id do cliente é inválido");

            RuleFor(c => c.Quantidade)
               .GreaterThan(0)
               .WithMessage("A quantidade miníma de um item é 1");

            RuleFor(c => c.Quantidade)
               .LessThan(15)
               .WithMessage("A quantidade máxima de um item é 15");

            RuleFor(c => c.ValorUnitario)
               .GreaterThan(0)
               .WithMessage("O valor do item precisa ser maior do que 0");
        }
    }
}
