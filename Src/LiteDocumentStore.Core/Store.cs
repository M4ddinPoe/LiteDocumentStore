namespace LiteDocumentStore.Core;

public sealed class Store
{
    private readonly string root;

    public Store(string root)
    {
        this.root = root;
    }

    public async Task<Collection<T>> GetCollectionAsync<T>(string name)
        where T : class
    {
        var collection = new Collection<T>(this.root, name);
        
        if (!File.Exists(collection.StoreFilePath))
        {
            var storeWriter = new StreamWriter(collection.StoreFilePath);
            var changeWriter = new StreamWriter(collection.StoreFilePath);
            
            await Task.WhenAll(
                storeWriter.WriteAsync(string.Empty),
                changeWriter.WriteAsync(string.Empty));
            
            storeWriter.Close();
            changeWriter.Close();

            await storeWriter.DisposeAsync();
            await changeWriter.DisposeAsync();

        }

        return collection;
    }
}