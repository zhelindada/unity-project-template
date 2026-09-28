namespace Dada.Cores;

public interface ISerializationService
{
    void Save<T>(string key, T value);
    T Load<T>(string key, T defaultValue = default);
    bool HasKey(string key);
    void Delete(string key);
    void DeleteAll();
    void Flush();
}
