using System;
using System.Collections;
using UnityEngine;

namespace Dada.Cores
{
    /// <summary>
    /// 所有UI模块的抽象基类。提供统一的生命周期、动画支持和取消处理。
    ///
    /// 生命周期：OnPreShow → 动画 → OnShow → [活跃] → OnPreHide → 动画 → OnHide → 回收/销毁
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIModule : MonoBehaviour, ICancellable
    {
        [Header("Module")]
        [SerializeField] protected bool _closeOnEscape = true;

        private UIFader _fader;
        private Coroutine _animRoutine;

        /// <summary>所属层级，由UIManager在打开时设定。</summary>
        public UILayerType LayerType { get; internal set; }

        /// <summary>当前是否处于打开状态。</summary>
        public bool IsOpen { get; private set; }

        /// <summary>是否正在播放动画。</summary>
        public bool IsAnimating { get; private set; }

        /// <summary>直接操作该CanvasGroup控制透明度和交互。</summary>
        public CanvasGroup CanvasGroup => Fader.CanvasGroup;

        /// <summary>Fader组件（懒加载）。</summary>
        public UIFader Fader
        {
            get
            {
                if (_fader == null)
                    _fader = GetComponent<UIFader>() ?? gameObject.AddComponent<UIFader>();
                return _fader;
            }
        }

        // ==================== UIManager调用的内部方法 ====================

        /// <summary>由UIManager调用：打开模块。</summary>
        internal void DoOpen(object data = null)
        {
            if (IsOpen) return;
            IsOpen = true;
            gameObject.SetActive(true);
            Fader.HideImmediate();
            SetData(data);
            OnPreShow();
            IsAnimating = true;
            _animRoutine = StartCoroutine(OpenRoutine());
        }

        /// <summary>由UIManager调用：关闭模块。</summary>
        internal void DoClose()
        {
            if (!IsOpen || IsAnimating) return;
            IsOpen = false;
            OnPreHide();
            IsAnimating = true;
            _animRoutine = StartCoroutine(CloseRoutine());
        }

        /// <summary>立刻关闭，跳过动画。</summary>
        internal void DoCloseImmediate()
        {
            if (!IsOpen) return;
            IsOpen = false;
            if (_animRoutine != null)
            {
                StopCoroutine(_animRoutine);
                _animRoutine = null;
            }
            IsAnimating = false;
            Fader.HideImmediate();
            gameObject.SetActive(false);
            OnHide();
        }

        private IEnumerator OpenRoutine()
        {
            PlayShowAnimation();
            yield return new WaitUntil(() => !IsAnimating);
            OnShow();
        }

        private IEnumerator CloseRoutine()
        {
            PlayHideAnimation();
            yield return new WaitUntil(() => !IsAnimating);
            Fader.HideImmediate();
            gameObject.SetActive(false);
            OnHide();
        }

        // ==================== 动画 ====================

        /// <summary>播放入场动画，完成后设置IsAnimating=false。</summary>
        protected virtual void PlayShowAnimation()
        {
            Fader.FadeIn(() => IsAnimating = false);
        }

        /// <summary>播放退场动画，完成后设置IsAnimating=false。</summary>
        protected virtual void PlayHideAnimation()
        {
            Fader.FadeOut(() => IsAnimating = false);
        }

        // ==================== 生命周期钩子（子类重写） ====================

        /// <summary>打开前调用（GameObject已激活，alpha=0）。用于设置初始状态。</summary>
        protected virtual void OnPreShow() { }

        /// <summary>入场动画播放完毕后调用。用于注册事件、开始刷新协程等。</summary>
        protected virtual void OnShow() { }

        /// <summary>关闭前调用。用于保存状态。</summary>
        protected virtual void OnPreHide() { }

        /// <summary>退场动画播放完毕且GameObject已隐藏后调用。用于注销事件。</summary>
        protected virtual void OnHide() { }

        /// <summary>设置数据上下文。子类重写以接收和解析传入的数据对象。</summary>
        public virtual void SetData(object data) { }

        // ==================== ICancellable ====================

        /// <summary>ESC/返回键响应。默认关闭自己。</summary>
        public virtual bool Cancel()
        {
            if (!_closeOnEscape || !IsOpen || IsAnimating) return false;
            if (UIManager.HasInstance)
                UIManager.Instance.Hide(this);
            return true;
        }

        // ==================== 工具方法 ====================

        /// <summary>关闭自身。快捷方法。</summary>
        public void Close()
        {
            if (UIManager.HasInstance)
                UIManager.Instance.Hide(this);
        }

        protected virtual void OnDestroy()
        {
            if (UIManager.HasInstance)
                UIManager.Instance.UnregisterModule(this);
        }
    }
}
