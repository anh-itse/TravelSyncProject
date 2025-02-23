using Mono.Cecil;
using NetArchTest.Rules;

namespace TravelSync.Architecture.Tests.Rules;

public abstract class BaseArchitectureRule : ICustomRule
{
    public abstract bool MeetsRule(TypeDefinition type);
}
