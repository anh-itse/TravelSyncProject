namespace TravelSync.Domain.Exeptions;

public abstract class BadRequestException(string message) : DomainException("Bad Request", message)
{
}
