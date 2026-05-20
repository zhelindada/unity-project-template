using Dada.Foundations;

namespace Dada.Cores;
public abstract class ManagedManager<T> : Singleton<T> where T : ManagedManager<T>, new()
{
}
