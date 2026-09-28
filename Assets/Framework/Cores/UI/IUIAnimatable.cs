using System;

namespace Dada.Cores
{
    /// <summary>
    /// 支持显示/隐藏动画的UI模块。由UIModule在生命周期中自动调用。
    /// </summary>
    public interface IUIAnimatable
    {
        /// <summary>入场动画完成后回调。</summary>
        void PlayShowAnimation(Action onComplete);

        /// <summary>退场动画完成后回调。</summary>
        void PlayHideAnimation(Action onComplete);
    }
}
