using System.Reflection;

namespace TravelSync.AppHost;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
