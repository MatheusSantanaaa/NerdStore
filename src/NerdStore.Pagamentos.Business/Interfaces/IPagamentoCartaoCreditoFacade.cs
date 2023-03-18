using NerdStore.Pagamentos.Business.Models;
using NerdStore.Pagamentos.Business.VO;

namespace NerdStore.Pagamentos.Business.Interfaces
{
    public interface IPagamentoCartaoCreditoFacade
    {
        Transacao RealizarPagamento(Pedido pedido, Pagamento pagamento);
    }
}
