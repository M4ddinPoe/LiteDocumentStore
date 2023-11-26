namespace LiteDocumentStore.Cli.Model;

public record Customer(
    string Number,
    string FirstName,
    string LastName,
    bool IsActive);
