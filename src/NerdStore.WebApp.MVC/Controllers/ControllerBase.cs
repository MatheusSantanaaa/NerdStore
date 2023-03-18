using MediatR;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommonMessages.Notifications;

namespace NerdStore.WebApp.MVC.Controllers
{
    public abstract class ControllerBase : Controller
    {
        private readonly DomainNotificationHandler _notifications;
        private IMediatorHandler _mediator;

        protected Guid ClienteId = Guid.Parse("34AB5FD0-7BAE-4F6D-BAC7-9B4D1AA86D29");

        protected ControllerBase(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _mediator = mediator;
        }

        protected bool OperacaoValida()
        {
            return !_notifications.TemNotificacao();
        }

        protected IEnumerable<string> ObterMensagensErro()
        {
            return _notifications.ObterNotificacoes().Select(x => x.Value).ToList();
        }

        protected void NotificarErro(string codigo, string mensagem)
        {
            _mediator.PublicarNotificacao(new DomainNotification(codigo, mensagem));
        }
    }
}
