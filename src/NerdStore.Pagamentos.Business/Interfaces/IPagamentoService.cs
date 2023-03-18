using NerdStore.Core.DomainObjects.DTO;
using NerdStore.Pagamentos.Business.Models;

namespace NerdStore.Pagamentos.Business.Interfaces
{
    public interface IPagamentoService
    {
        Task<Transacao> RealizarPagamentoPedido(PagamentoPedido pagamentoPedido);
    }
}
