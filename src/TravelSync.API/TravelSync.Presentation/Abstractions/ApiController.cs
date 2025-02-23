using Microsoft.AspNetCore.Mvc;
using TravelSync.Application.Abstractions.Dispatching;

namespace TravelSync.Presentation.Abstractions;

[ApiController]
public abstract class ApiController(IDispatcher dispatcher) : ControllerBase
{
    private readonly IDispatcher _dispatcher = dispatcher;

    protected IDispatcher Dispatcher => this._dispatcher;
}