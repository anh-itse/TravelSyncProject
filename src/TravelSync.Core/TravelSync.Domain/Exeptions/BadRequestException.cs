namespace TravelSync.Domain.Exeptions;

public abstract class BadRequestException(string message) : AppException("Bad Request", message)
{
}
