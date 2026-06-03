using System;
using R3;

namespace Dada.Core.UI
{
    public abstract class UIView<TViewModel> : UIView, IUIViewDataReceiver
        where TViewModel : class
    {
        public TViewModel ViewModel { get; private set; }

        private readonly CompositeDisposable _bindings = new();

        void IUIViewDataReceiver.ReceiveData(object data)
        {
            if (data is TViewModel vm)
                Bind(vm);
        }

        public void Bind(TViewModel viewModel)
        {
            _bindings.Clear();
            ViewModel = viewModel;
            OnBind(viewModel);
        }

        protected void AddBinding(IDisposable disposable)
        {
            _bindings.Add(disposable);
        }

        protected abstract void OnBind(TViewModel viewModel);

        public void Unbind()
        {
            _bindings.Clear();
            ViewModel = null;
        }

        protected override void OnDestroy()
        {
            _bindings.Clear();
            Unbind();
            base.OnDestroy();
        }
    }
}
