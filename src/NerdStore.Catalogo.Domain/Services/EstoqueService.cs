using NerdStore.Catalogo.Domain.Events;
using NerdStore.Catalogo.Domain.Interfaces;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.DomainObjects.DTO;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Domain.Services
{
    public class EstoqueService : IEstoqueService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMediatorHandler _mediator;

        public EstoqueService(IProdutoRepository produtoRepository, IMediatorHandler mediator)
        {
            _produtoRepository = produtoRepository;
            _mediator = mediator;
        }

        public async Task<bool> DebitarEstoque(Guid produtoId, int quantidade)
        {
            if (!await DebitarItemEstoque(produtoId, quantidade)) return false;

            return await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> DebitarListaProdutosPedido(ListaProdutosPedido lista)
        {
            foreach (var item in lista.Itens)
            {
                if (!await DebitarEstoque(item.Id, item.Quantidade)) return false;
            }

            return await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> DebitarItemEstoque(Guid produtoId, int quantidade)
        {
            var produto = await _produtoRepository.GetById(produtoId);

            if (produto is null) return false;

            if (!produto.PossuiEstoque(quantidade))
            {
                await _mediator.PublicarNotificacao(new DomainNotification("Estoque", $"Produto - {produto.Nome} sem estoque"));
                return false;
            }

            produto.DebitarEstoque(quantidade);

            // TODO: Parametrizar a quantidade de estoque baixo
            if (produto.QuantidadeEstoque < 10)
            {
                await _mediator.PublicarDomainEvent(new ProdutoEstoqueAbaixoEvent(produto.Id, produto.QuantidadeEstoque));
            }

            _produtoRepository.Update(produto);
            return true;
        }

        public async Task<bool> ReporEstoque(Guid produtoId, int quantidade)
        {
            var sucesso = await ReporItemEstoque(produtoId, quantidade);

            if (!sucesso) return false;
            return await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> ReporListaProdutosPedido(ListaProdutosPedido lista)
        {
            foreach (var item in lista.Itens)
            {
                if (!await ReporEstoque(item.Id, item.Quantidade)) return false;
            }

            return await _produtoRepository.UnitOfWork.Commit();
        }
        public async Task<bool> ReporItemEstoque(Guid produtoId, int quantidade)
        {
            var produto = await _produtoRepository.GetById(produtoId);
            if (produto is null) return false;

            produto.ReporEstoque(quantidade);

            _produtoRepository.Update(produto);

            return false;
        }
        public void Dispose()
        {
            _produtoRepository.Dispose();
        }

    }
}
