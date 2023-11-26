namespace LiteDocumentStore.Core;

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

public sealed class Collection<T>
    where T : class
{
    // todo: configuration
    private const int BufferSize = 65536; 
    
    private readonly string root;
    private readonly string name;

    internal Collection(string root, string name)
    {
        this.root = root;
        this.name = name;

        this.StoreFilePath = Path.Combine(root, $"{name}.lds");
        this.ChangesFilePath = Path.Combine(root, $"{name}.chg.lds");
    }
    
    internal string StoreFilePath { get; }
    
    internal string ChangesFilePath { get; }

    public async Task InsertAsync(Guid id, T item)
    {
        string json = JsonSerializer.Serialize(item);
        string hash = CalculateSHA1(json);
        var storeData = $"{id} {hash} 1";
        var changesData = $"{hash} {json}";

        StreamWriter storeWriter = new StreamWriter(StoreFilePath, true, Encoding.UTF8, BufferSize);
        StreamWriter changesWriter = new StreamWriter(ChangesFilePath, true, Encoding.UTF8, BufferSize);
        
        await Task.WhenAll(
            storeWriter.WriteLineAsync(storeData),
            changesWriter.WriteLineAsync(changesData));
        
        storeWriter.Close();
        changesData.Clone();

        await storeWriter.DisposeAsync();
        await changesWriter.DisposeAsync();
    }

    public async Task Update(Guid id, T item)
    {
        
    }
    
    private static string CalculateSHA1(string input)
    {
        using (SHA1 sha1 = SHA1.Create())
        {
            // Convert the input string to a byte array
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            // Compute the hash
            byte[] hashBytes = sha1.ComputeHash(inputBytes);

            // Convert the hash bytes to a hexadecimal string
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}