namespace TravelSync.Domain.Exeptions
{
    public abstract class NotFoundException(string message) : DomainException("Not Found", message)
    {
    }
}
