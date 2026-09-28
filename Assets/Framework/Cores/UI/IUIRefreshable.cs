namespace Dada.Cores
{
    /// <summary>
    /// 可刷新显示的UI。当底层数据变化时调用Refresh()更新视图。
    /// </summary>
    public interface IUIRefreshable
    {
        void Refresh();
    }
}
