using FluentValidation.Results;
using NerdStore.Core.Messages;
using NerdStore.Vendas.Application.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Commands
{
    public class AdicionarItemPedidoCommand : Command
    {
        public Guid Id { get; private set; }
        public Guid ClienteId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public string Nome { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        public AdicionarItemPedidoCommand(Guid clienteId, Guid produtoId, string nome, int quantidade, decimal valorUnitario)
        {
            ClienteId = clienteId;
            ProdutoId = produtoId;
            Nome = nome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }
        public override bool IsValid()
        {
            ValidationResult = new AdicionarItemPedidoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

}
