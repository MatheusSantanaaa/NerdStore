using NerdStore.Vendas.Application.Queries.ViewModels;
using NerdStore.Vendas.Domain.Enum;
using NerdStore.Vendas.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Queries
{
    public interface IPedidosQueries
    {
        Task<CarrinhoViewModel> ObterCarrinhoCliente(Guid clienteId);
        Task<IEnumerable<PedidoViewModel>> ObterPedidosCliente(Guid clienteId);
    }

    public class PedidoQueries : IPedidosQueries
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoQueries(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<CarrinhoViewModel> ObterCarrinhoCliente(Guid clienteId)
        {
            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(clienteId);

            if (pedido is null) return null;

            var carrinho = new CarrinhoViewModel()
            {
                ClienteId = pedido.ClienteId,
                ValorTotal = pedido.ValorTotal,
                PedidoId = pedido.Id,
                ValorDesconto = pedido.Desconto,
                SubTotal = pedido.Desconto + pedido.ValorTotal
              
            };

            if(pedido.VoucherId != null)
            {
                carrinho.VoucherCodigo = pedido.Voucher.Codigo.ToString();
            }

            foreach(var item in pedido.PedidoItems)
            {
                carrinho.Items.Add(new CarrinhoItemViewModel
                {
                    ProdutoId = item.ProdutoId,
                    ProdutoNome = item.ProdutoNome,
                    Quantidade = item.Quantidade,
                    ValorUnitario = item.ValorUnitario,
                    ValorTotal = item.ValorUnitario * item.Quantidade
                });
            }
            return carrinho;
        }

        public async Task<IEnumerable<PedidoViewModel>> ObterPedidosCliente(Guid clienteId)
        {
            var pedidos = await _pedidoRepository.ObterListaPorClienteId(clienteId);

            pedidos = pedidos.Where(p => p.PedidoStatus == PedidoStatus.Pago || p.PedidoStatus == PedidoStatus.Cancelado)
                .OrderByDescending(p => p.Codigo);

            if (!pedidos.Any()) return null;

            var pedidosView = new List<PedidoViewModel>();

            foreach(var item in pedidos)
            {
                pedidosView.Add(new PedidoViewModel
                {
                    ValorTotal = item.ValorTotal,
                    PedidoStatus = (int)item.PedidoStatus,
                    Codigo = item.Codigo,
                    DataCadastro = item.DataCadastro
                });
            }

            return pedidosView;
        }
    }
}
