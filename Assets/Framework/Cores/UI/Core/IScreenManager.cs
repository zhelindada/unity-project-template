using System;
using Cysharp.Threading.Tasks;

namespace Dada.Cores
{
    /// <summary>
    /// 屏幕管理器公共接口——UITKManager 和 UGUIManager 都实现此接口。
    /// 一套接口、两种实现。VContainer 注入 IScreenManager，无需关心底层 UI 框架。
    /// </summary>
    public interface IScreenManager
    {
        /// <summary>
        /// 全局状态变更 (screenId, oldState, newState)
        /// </summary>
        event Action<string, string, string> OnScreenStateChanged;

        /// <summary>
        /// 屏幕显示 (screenId)
        /// </summary>
        event Action<string> OnScreenShown;

        /// <summary>
        /// 屏幕隐藏 (screenId)
        /// </summary>
        event Action<string> OnScreenHidden;

        /// <summary>
        /// 注册已创建的屏幕实例
        /// </summary>
        void RegisterScreen(IScreen screen);

        /// <summary>
        /// 获取已注册的屏幕
        /// </summary>
        T GetScreen<T>(string screenId) where T : class, IScreen;

        /// <summary>
        /// 异步显示屏幕
        /// </summary>
        UniTask ShowScreenAsync(string screenId, string initialState = null);

        /// <summary>
        /// 异步销毁屏幕
        /// </summary>
        UniTask DestroyScreenAsync(string screenId);

        /// <summary>
        /// 隐藏所有屏幕
        /// </summary>
        UniTask HideAllAsync();
    }
}
