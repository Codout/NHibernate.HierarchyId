#region Usings

using NHibernate.Linq.Functions;
using NHibernate.Util;

#endregion

namespace NHibernate.HierarchyId.Linq;

public sealed class HierarchyHqlGeneratorRegistry : DefaultLinqToHqlGeneratorsRegistry
{
    public HierarchyHqlGeneratorRegistry()
    {
        RegisterGenerator(ReflectHelper.GetMethodDefinition(() => default(string).SqlToString()),
                          new ToStringGenerator());

        RegisterGenerator(
            ReflectHelper.GetMethodDefinition(() => default(string).IsDescendantOf(default(string))),
            new IsDescendantOfGenerator());

        RegisterGenerator(
            ReflectHelper.GetMethodDefinition(() => default(string).GetAncestor(default(int))),
            new GetAncestorGenerator());

        RegisterGenerator(
            ReflectHelper.GetMethodDefinition(() => default(string).GetDescendant(null,null)),
            new GetDescendantGenerator());

        RegisterGenerator(
            ReflectHelper.GetMethodDefinition(() => default(string).GetLevel()),
            new GetLevelGenerator());

        RegisterGenerator(
            ReflectHelper.GetMethodDefinition(() => default(string).GetReparentedValue(null,null)),
            new GetReparentedValueGenerator());

        RegisterGenerator(
            ReflectHelper.GetMethodDefinition(() => default(string).ToHierarchyId()),
            new HidParseGenerator());           
    }
}