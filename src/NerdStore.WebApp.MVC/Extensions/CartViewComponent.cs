using Microsoft.AspNetCore.Mvc;
using NerdStore.Vendas.Application.Queries;

namespace NerdStore.WebApp.MVC.Extensions
{
    public class CartViewComponent : ViewComponent
    {
        private readonly IPedidosQueries _pedidosQueries;

        // TODO: Obter cliente logado
        protected Guid ClienteId = Guid.Parse("34AB5FD0-7BAE-4F6D-BAC7-9B4D1AA86D29");

        public CartViewComponent(IPedidosQueries pedidosQueries)
        {
            _pedidosQueries = pedidosQueries;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var carrinho = await _pedidosQueries.ObterCarrinhoCliente(ClienteId);
            var itens = carrinho?.Items.Count() ?? 0;

            return View(itens);
        }
    }
}
