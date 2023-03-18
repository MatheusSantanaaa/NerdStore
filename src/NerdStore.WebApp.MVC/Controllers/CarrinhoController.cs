using MediatR;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalogo.Application.Interfaces;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Queries;
using NerdStore.Vendas.Application.Queries.ViewModels;

namespace NerdStore.WebApp.MVC.Controllers
{
    public class CarrinhoController : ControllerBase
    {
        private readonly IProdutoAppService _produtoAppService;
        private readonly IPedidosQueries _pedidosQueries;
        private readonly IMediatorHandler _mediator;

        public CarrinhoController(INotificationHandler<DomainNotification> notifications,
                                  IProdutoAppService produtoAppService,
                                  IMediatorHandler mediator,
                                  IPedidosQueries pedidosQueries) : base(notifications, mediator)
        {
            _produtoAppService = produtoAppService;
            _mediator = mediator;
            _pedidosQueries = pedidosQueries;
        }

        [Route("meu-carrinho")]
        public async Task<IActionResult> Index()
        {
            return View(await _pedidosQueries.ObterCarrinhoCliente(ClienteId));
        }

        [HttpPost]
        [Route("meu-carrinho")]
        public async Task<IActionResult> AdicionarItem(Guid id, int quantidade)
        {
            var produto = await _produtoAppService.ObterPorId(id);

            if (produto is null) return BadRequest();

            if(produto.QuantidadeEstoque < quantidade)
            {
                TempData["Erros"] = "Produto com estoque insuficiente";
                return RedirectToAction("ProdutoDetalhe", controllerName: "Vitrine", new { id });
            }

            var command = new AdicionarItemPedidoCommand(ClienteId, produto.Id, produto.Nome, quantidade, produto.Valor);
            await _mediator.EnviarComando(command);

            //se tudo deu certo?
            if (OperacaoValida())
            {
                return RedirectToAction(actionName:"Index");
            }

            TempData["Erros"] = ObterMensagensErro();
            return RedirectToAction("ProdutoDetalhe", controllerName: "Vitrine", new { id });
        }

        [HttpPost]
        [Route("remover-item")]
        public async Task<IActionResult> RemoverItem(Guid id)
        {
            var produto = await _produtoAppService.ObterPorId(id);
            if (produto is null) return BadRequest();

            var command = new RemoverItemPedidoCommand(ClienteId, id);
            await _mediator.EnviarComando(command);

            if (OperacaoValida())
            {
                return RedirectToAction("Index");
            }

            return View("Index", await _pedidosQueries.ObterCarrinhoCliente(ClienteId));
        }


        [HttpPost]
        [Route("atualizar-item")]
        public async Task<IActionResult> AtualizarItem(Guid id, int quantidade)
        {
            var produto = await _produtoAppService.ObterPorId(id);
            if (produto is null) return BadRequest();

            var command = new AtualizarItemPedidoCommand(ClienteId, id, quantidade);
            await _mediator.EnviarComando(command);

            if (OperacaoValida())
            {
                return RedirectToAction("Index");
            }

            return View("Index", await _pedidosQueries.ObterCarrinhoCliente(ClienteId));
        }

        [HttpPost]
        [Route("aplicar-voucher")]
        public async Task<IActionResult> AplicarVoucher(string voucherCodigo)
        {
            var command = new AplicarVoucherPedidoCommand(ClienteId, voucherCodigo);
            await _mediator.EnviarComando(command);

            if (OperacaoValida())
            {
                return RedirectToAction("Index");
            }

            return View("Index", await _pedidosQueries.ObterCarrinhoCliente(ClienteId));
        }

        [Route("resumo-da-compra")]
        public async Task<IActionResult> ResumoDaCompra()
        {
            return View(await _pedidosQueries.ObterCarrinhoCliente(ClienteId));
        }

        [HttpPost]
        [Route("iniciar-pedido")]
        public async Task<IActionResult> IniciarPedido(CarrinhoViewModel carrinhoViewModel)
        {
            var carrinho = await _pedidosQueries.ObterCarrinhoCliente(ClienteId);

            var command = new IniciarPedidoCommand(carrinho.PedidoId, ClienteId, carrinho.ValorTotal, carrinhoViewModel.Pagamento.NomeCartao,
                 carrinhoViewModel.Pagamento.NumeroCartao, carrinhoViewModel.Pagamento.ExpiracaoCartao, carrinhoViewModel.Pagamento.CvvCartao);


            await _mediator.EnviarComando(command);

            if (OperacaoValida())
            {
                return RedirectToAction("Index", controllerName: "Pedido");
            }

            return View("ResumoDaCompra", await _pedidosQueries.ObterCarrinhoCliente(ClienteId));
        }
    }
}
