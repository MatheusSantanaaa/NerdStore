using MediatR;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.DomainObjects.DTO;
using NerdStore.Core.Extensions;
using NerdStore.Core.Messages;
using NerdStore.Core.Messages.CommonMessages.IntegrationEvents;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Domain.Interfaces;
using NerdStore.Vendas.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Commands
{
    public class PedidoCommandHandler :
        IRequestHandler<AdicionarItemPedidoCommand, bool>,
        IRequestHandler<AtualizarItemPedidoCommand, bool>,
        IRequestHandler<RemoverItemPedidoCommand, bool>,
        IRequestHandler<AplicarVoucherPedidoCommand, bool>,
        IRequestHandler<IniciarPedidoCommand, bool>,
        IRequestHandler<FinalizarPedidoCommand, bool>,
        IRequestHandler<CancelarProcessamentoPedidoEstornarEstoqueCommand, bool>,
        IRequestHandler<CancelarProcessamentoPedidoCommand, bool>
    {

        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMediatorHandler _mediator;

        public PedidoCommandHandler(IPedidoRepository pedidoRepository, IMediatorHandler mediator)
        {
            _pedidoRepository = pedidoRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(AdicionarItemPedidoCommand message, CancellationToken cancellationToken)
        {
            if (!ValidarComando(message)) return false;

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(message.ClienteId);
            var pedidoItem = new PedidoItem(message.ProdutoId, message.Nome, message.Quantidade, message.ValorUnitario);

            if(pedido is null)
            {
                pedido = Pedido.PedidoFactory.NovoPedidoRasCunho(message.ClienteId);
                pedido.AdicionarItem(pedidoItem);

                 pedido.Teste(pedido);
                _pedidoRepository.Adicionar(pedido);
                pedido.AdicionarEvento(new PedidoRascunhoIniciadoEvent(message.ClienteId, message.ProdutoId));
            }
            else
            {
                var pedidoItemExistente = pedido.PedidoItemExistente(pedidoItem);
                pedido.AdicionarItem(pedidoItem);

                if (pedidoItemExistente)
                {
                    _pedidoRepository.AtualizarItem(pedido.PedidoItems.FirstOrDefault(x => x.ProdutoId.Equals(pedidoItem.ProdutoId)));
                }
                else
                {
                    _pedidoRepository.AdicionarItem(pedidoItem);
                }

                pedido.AdicionarEvento(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));
            }

            pedido.AdicionarEvento(new PedidoItemAdicionadoEvent(pedido.ClienteId, pedido.Id, message.ProdutoId, message.Nome, message.ValorUnitario, message.Quantidade));


            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(AtualizarItemPedidoCommand message, CancellationToken cancellationToken)
        {
            if (!ValidarComando(message)) return false;

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(message.ClienteId);

            if(pedido == null)
            {
                await _mediator.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado"));
                return false;
            }

            var pedidoItem = await _pedidoRepository.ObterItemPorPedido(pedido.Id, message.ProdutoId);

            if (!pedido.PedidoItemExistente(pedidoItem))
            {
                await _mediator.PublicarNotificacao(new DomainNotification("pedido", "Item do pedido não encontrado!"));
                return false;
            }


            pedido.AdicionarEvento(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));

            pedido.AtualizarUnidades(pedidoItem, message.Quantidade);

            _pedidoRepository.AtualizarItem(pedidoItem);
            _pedidoRepository.Atualizar(pedido);

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(RemoverItemPedidoCommand request, CancellationToken cancellationToken)
        {
            if (!ValidarComando(request)) return false;

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(request.ClienteId);
            
            if(pedido == null)
            {
                await _mediator.PublicarNotificacao(new DomainNotification("pedido", "Item do pedido não encontrado!"));
                return false;
            }

            var pedidoItem = await _pedidoRepository.ObterItemPorPedido(pedido.Id, request.ProdutoId);

            if(pedidoItem != null && !pedido.PedidoItemExistente(pedidoItem))
            {
                await _mediator.PublicarNotificacao(new DomainNotification("pedido", "Item do pedido não encontrado"));
                return false;
            }

            pedido.RemoverItem(pedidoItem);
            pedido.AdicionarEvento(new PedidoAtualizadoEvent(request.ClienteId, pedido.Id, pedido.ValorTotal));
            pedido.AdicionarEvento(new PedidoProdutoRemovidoItemEvent(request.ClienteId, pedido.Id, request.ProdutoId));

            _pedidoRepository.RemoverItem(pedidoItem);
            _pedidoRepository.Atualizar(pedido);

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(AplicarVoucherPedidoCommand request, CancellationToken cancellationToken)
        {
            if (!ValidarComando(request)) return false;

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(request.ClienteId);

            if(pedido == null)
            {
                await _mediator.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado!"));
                return false;
            }

            var voucher = await _pedidoRepository.ObterVoucherPorCodigo(request.CodigoVoucher);

            if(voucher == null)
            {
                await _mediator.PublicarNotificacao(new DomainNotification("pedido", "Voucher não encontrado"));
                return false;
            }

            var voucherValidacaoValidation = pedido.AplicarVoucher(voucher);

            if (!voucherValidacaoValidation.IsValid)
            {
                foreach(var error in voucherValidacaoValidation.Errors)
                {
                    await _mediator.PublicarNotificacao(new DomainNotification(error.ErrorCode, error.ErrorMessage));
                }

                return false;
            }

            pedido.AdicionarEvento(new VoucherAplicadoPedidoEvent(request.ClienteId, pedido.Id, voucher.Id));
            pedido.AdicionarEvento(new PedidoAtualizadoEvent(request.ClienteId, pedido.Id, pedido.ValorTotal));
           
             return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(IniciarPedidoCommand request, CancellationToken cancellationToken)
        {
            if (!ValidarComando(request)) return false;

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(request.ClienteId);
            pedido.IniciarPedido();

            var itensList = new List<Item>();
            pedido.PedidoItems.ForEach(i => itensList.Add(new Item { Id = i.ProdutoId, Quantidade = i.Quantidade }));
            var listaProdutosPedido = new ListaProdutosPedido { PedidoId = pedido.Id, Itens = itensList };

            pedido.AdicionarEvento(new PedidoIniciadoEvent(pedido.Id, pedido.ClienteId, listaProdutosPedido, pedido.ValorTotal, request.NomeCartao, request.NumeroCartao, request.ExpiracaoCartao, request.CvvCartao));

            _pedidoRepository.Atualizar(pedido);

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(FinalizarPedidoCommand request, CancellationToken cancellationToken)
        {
            var pedido = await _pedidoRepository.ObterPorId(request.PedidoId);

            if(pedido == null)
            {
                await _mediator.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado"));
                return false;
            }

            pedido.FinalizarPedido();

            pedido.AdicionarEvento(new PedidoFinalizadoEvent(request.PedidoId));

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(CancelarProcessamentoPedidoEstornarEstoqueCommand request, CancellationToken cancellationToken)
        {
            var pedido = await _pedidoRepository.ObterPorId(request.PedidoId);

            if(pedido == null)
            {
                await _mediator.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado"));
                return false;
            }

            var itemList = new List<Item>();
            pedido.PedidoItems.ForEach(i => itemList.Add(new Item { Id = i.ProdutoId, Quantidade = i.Quantidade }));
            var listaProdutosPedido = new ListaProdutosPedido { PedidoId = pedido.Id, Itens = itemList };

            pedido.AdicionarEvento(new PedidoProcessamentoCanceladoEvent(pedido.Id, pedido.ClienteId, listaProdutosPedido));
            pedido.TornarRascunho();

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(CancelarProcessamentoPedidoCommand request, CancellationToken cancellationToken)
        {
            var pedido = await _pedidoRepository.ObterPorId(request.PedidoId);

            if(pedido == null)
            {
                await _mediator.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado"));
                return false;
            }

            pedido.TornarRascunho();

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        private bool ValidarComando(Command message)
        {
            if(message.IsValid()) return true;

            foreach(var erro in message.ValidationResult.Errors)
            {
                // lançar evento de erro
                _mediator.PublicarNotificacao(new DomainNotification(message.MessageType, erro.ErrorMessage));
            }

            return false;
        }
    }
}
