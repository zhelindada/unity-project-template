using System;
using System.Threading;
using MessagePipe;
using R3;

namespace Dada.Core.UI
{
    public static class UIMessageExtensions
    {
        public static IDisposable Subscribe<TMessage>(
            this ISubscriber<TMessage> subscriber,
            UIView view,
            Action<TMessage> handler)
        {
            var d = subscriber.Subscribe(handler);
            return d.AddTo(view);
        }

        public static IDisposable Subscribe<TMessage>(
            this ISubscriber<TMessage> subscriber,
            UIView view,
            Action<TMessage> handler,
            Action<TMessage> onError,
            Action onCompleted)
        {
            var d = subscriber.Subscribe(handler, onError, onCompleted);
            return d.AddTo(view);
        }

        public static void Publish<TMessage>(
            this IPublisher<TMessage> publisher,
            TMessage message)
        {
            publisher.Publish(message);
        }

        public static Observable<TMessage> AsObservable<TMessage>(
            this ISubscriber<TMessage> subscriber,
            UIView view)
        {
            return Observable.Create<TMessage>(observer =>
            {
                var d = subscriber.Subscribe(
                    x => observer.OnNext(x),
                    ex => observer.OnErrorResume(ex),
                    () => observer.OnCompleted());
                return d.AddTo(view);
            });
        }
    }
}
