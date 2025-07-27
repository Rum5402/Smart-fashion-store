namespace Fashion.Contract.Interface
{
    public interface IStoreContextService
    {
        int? GetCurrentStoreId();
        string? GetCurrentStoreDomain();
    }
} 