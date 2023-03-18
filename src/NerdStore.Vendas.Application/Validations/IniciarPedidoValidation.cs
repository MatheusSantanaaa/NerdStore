using FluentValidation;
using NerdStore.Vendas.Application.Commands;

namespace NerdStore.Vendas.Application.Validations
{
    public class IniciarPedidoValidation : AbstractValidator<IniciarPedidoCommand>
    {
        public IniciarPedidoValidation()
        {
            RuleFor(c => c.ClienteId)
               .NotEqual(Guid.Empty)
               .WithMessage("Id do cliente é inválido");

            RuleFor(c => c.PedidoId)
               .NotEqual(Guid.Empty)
               .WithMessage("Id do pedido é inválido");

            RuleFor(c => c.NomeCartao)
               .NotEmpty()
               .WithMessage("O nome do cartão não foi informado");
            
            RuleFor(c => c.NumeroCartao)
               .NotEmpty()
                //.CreditCard()
               .WithMessage("O número do cartão não foi informado");

            RuleFor(c => c.ExpiracaoCartao)
               .NotEmpty()
               .WithMessage("Data expiração não foi informada");

            RuleFor(c => c.CvvCartao)
               .Length(3, 4)
               .WithMessage("O CVV não foi preenchido corretamente");


        }
    }
}
