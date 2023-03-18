using MediatR;
using NerdStore.Catalogo.Domain.Interfaces;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommonMessages.IntegrationEvents;

namespace NerdStore.Catalogo.Domain.Events
{
    public class ProdutoEventHandler : 
        INotificationHandler<ProdutoEstoqueAbaixoEvent>,
        INotificationHandler<PedidoIniciadoEvent>,
        INotificationHandler<PedidoProcessamentoCanceladoEvent>
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IEstoqueService _estoqueService;
        private readonly IMediatorHandler _mediator;

        public ProdutoEventHandler(IProdutoRepository produtoRepository, IEstoqueService estoqueService, IMediatorHandler mediator)
        {
            _produtoRepository = produtoRepository;
            _estoqueService = estoqueService;
            _mediator = mediator;
        }

        public async Task Handle(ProdutoEstoqueAbaixoEvent message, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.GetById(message.AggregateId);
            
            //Enviar um email para aquisicao de mais produtos
        }

        public async Task Handle(PedidoIniciadoEvent message, CancellationToken cancellationToken)
        {
            var result = await _estoqueService.DebitarListaProdutosPedido(message.ProdutosPedido);

            if (result)
            {
                await _mediator.PublicarEvento(new PedidoEstoqueConfirmadoEvent(message.PedidoId, message.ClienteId, message.Total, message.ProdutosPedido, message.NomeCartao, message.NumeroCartao, message.ExpiracaoCartao, message.CvvCartao));
            }
            else
            {
                await _mediator.PublicarEvento(new PedidoEstoqueRejeitadoEvent(message.PedidoId, message.ClienteId));
            }
        }

        public async Task Handle(PedidoProcessamentoCanceladoEvent notification, CancellationToken cancellationToken)
        {
            await _estoqueService.ReporListaProdutosPedido(notification.ProddutosPedido);
        }
    }
}
