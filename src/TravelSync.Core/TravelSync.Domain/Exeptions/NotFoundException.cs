namespace TravelSync.Domain.Exeptions
{
    public abstract class NotFoundException(string message) : AppException("Not Found", message)
    {
    }
}
