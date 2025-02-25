using Microsoft.AspNetCore.Mvc;
using TravelSync.Application.Abstractions.Dispatching;

namespace TravelSync.Presentation.Abstractions;

[ApiController]
public abstract class ApiController(IDispatcher dispatcher) : ControllerBase
{
    protected IDispatcher Dispatcher => dispatcher;
}