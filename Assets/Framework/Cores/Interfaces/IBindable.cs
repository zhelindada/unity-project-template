namespace Dada.Cores;

public interface IBindable<T>
{
    protected T BindedValue { get; set; }

    public void Bind(T dataToBind);
    public T GetBind();
    public void Unbind();
}
