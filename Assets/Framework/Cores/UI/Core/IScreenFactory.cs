namespace Dada.Cores
{
    /// <summary>
    /// 屏幕工厂接口——通过 screenId 创建屏幕，调用方无需知道具体类型。
    /// VContainer 注入后，工厂内部通过 IObjectResolver 解析已注册的屏幕类型。
    ///
    /// 使用:
    ///   [Inject] IScreenFactory _factory;
    ///   var screen = _factory.Create{CharacterInfoPanel}("ContextInfoPanel");
    ///   // 或者不知道具体类型时:
    ///   IScreen screen = _factory.Create("ContextInfoPanel");
    /// </summary>
    public interface IScreenFactory
    {
        /// <summary>
        /// 按 screenId 创建屏幕实例（泛型版本，返回具体类型）。
        /// </summary>
        T Create<T>(string screenId) where T : class, IScreen;

        /// <summary>
        /// 按 screenId 创建屏幕实例（非泛型版本，返回 IScreen）。
        /// </summary>
        IScreen Create(string screenId);
    }
}
