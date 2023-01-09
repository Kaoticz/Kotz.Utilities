using System.Reflection;

namespace Kotz.Tests.Extensions;

public sealed class AssemblyExtTest
{
    private static readonly Assembly _assembly = Assembly.GetExecutingAssembly();

    [Theory]
    [InlineData(true, typeof(ConcreteA))]
    [InlineData(true, typeof(ConcreteB))]
    [InlineData(true, typeof(ConcreteC))]
    [InlineData(true, typeof(ConcreteD))]
    [InlineData(true, typeof(ConcreteE))]
    [InlineData(false, typeof(IInterfaceA))]
    [InlineData(false, typeof(AbstractA))]
    [InlineData(false, typeof(AbstractB))]
    internal void GetConcreteTypesTest(bool contains, Type type)
    {
        var concreteTypes = _assembly.GetConcreteTypes();

        if (contains)
            Assert.Contains(type, concreteTypes);
        else
            Assert.DoesNotContain(type, concreteTypes);
    }

    [Theory]
    [InlineData(false, typeof(ConcreteA))]
    [InlineData(false, typeof(ConcreteB))]
    [InlineData(false, typeof(ConcreteC))]
    [InlineData(false, typeof(ConcreteD))]
    [InlineData(false, typeof(ConcreteE))]
    [InlineData(true, typeof(IInterfaceA))]
    [InlineData(true, typeof(AbstractA))]
    [InlineData(true, typeof(AbstractB))]
    internal void GetAbstractTypesTest(bool contains, Type type)
    {
        var concreteTypes = _assembly.GetAbstractTypes();

        if (contains)
            Assert.Contains(type, concreteTypes);
        else
            Assert.DoesNotContain(type, concreteTypes);
    }

    [Theory]
    [InlineData(typeof(ConcreteA), typeof(ConcreteA))]
    [InlineData(typeof(ConcreteB), typeof(ConcreteB), typeof(ConcreteC), typeof(ConcreteE))]
    [InlineData(typeof(ConcreteC), typeof(ConcreteC), typeof(ConcreteE))]
    [InlineData(typeof(ConcreteD), typeof(ConcreteD))]
    [InlineData(typeof(ConcreteE), typeof(ConcreteE))]
    [InlineData(typeof(IInterfaceA), typeof(ConcreteD))]
    [InlineData(typeof(AbstractA), typeof(ConcreteB), typeof(ConcreteC), typeof(ConcreteD), typeof(ConcreteE))]
    [InlineData(typeof(AbstractB), typeof(ConcreteE))]
    internal void GetConcreteTypesOfTest(Type type, params Type[] answers)
    {
        var concreteTypes = _assembly.GetConcreteTypesOf(type)
            .ToArray();

        Assert.Equal(answers.Length, concreteTypes.Length);

        foreach (var answer in answers)
            Assert.Contains(answer, concreteTypes);
    }

    [Theory]
    [InlineData(typeof(ConcreteA))]
    [InlineData(typeof(ConcreteB), typeof(AbstractB))]
    [InlineData(typeof(ConcreteC), typeof(AbstractB))]
    [InlineData(typeof(ConcreteD))]
    [InlineData(typeof(ConcreteE))]
    [InlineData(typeof(IInterfaceA), typeof(IInterfaceA))]
    [InlineData(typeof(AbstractA), typeof(AbstractA), typeof(AbstractB))]
    [InlineData(typeof(AbstractB), typeof(AbstractB))]
    internal void GetAbstractTypesOfTest(Type type, params Type[] answers)
    {
        var concreteTypes = _assembly.GetAbstractTypesOf(type)
            .ToArray();

        Assert.Equal(answers.Length, concreteTypes.Length);

        foreach (var answer in answers)
            Assert.Contains(answer, concreteTypes);
    }
}

internal interface IInterfaceA { }
internal abstract class AbstractA { }
internal class ConcreteA { }
internal class ConcreteB : AbstractA { }
internal class ConcreteC : ConcreteB { }
internal class ConcreteD : AbstractA, IInterfaceA { }
internal abstract class AbstractB : ConcreteC { }
internal class ConcreteE : AbstractB { }