using FluentValidation;
using NerdStore.Vendas.Application.Commands;

namespace NerdStore.Vendas.Application.Validations
{
    public class AplicarVoucherPedidoValidation : AbstractValidator<AplicarVoucherPedidoCommand>
    {
        public AplicarVoucherPedidoValidation()
        {
            RuleFor(c => c.ClienteId)
               .NotEqual(Guid.Empty)
               .WithMessage("Id do cliente é inválido");

            RuleFor(c => c.CodigoVoucher)
               .NotEmpty()
              .WithMessage("O código do voucher não pode ser vazio");
        }
    }
}
