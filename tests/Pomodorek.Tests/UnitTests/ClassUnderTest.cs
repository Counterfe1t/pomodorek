namespace Pomodorek.Tests.UnitTests;

public static class ClassUnderTest
{
    public static T Is<T>(params object[] knownSubstitutions) where T : class
    {
        var args = CreateParameters<T>(knownSubstitutions);
        var instance = Activator.CreateInstance(typeof(T), args) as T;

        return instance ?? throw new Exception($"The instance of type {nameof(T)} should not be null");
    }

    private static object[] CreateParameters<T>(params object[] known) where T : class
    {
        var mockedParams = new List<object>();
        var ctor = typeof(T).GetConstructors().FirstOrDefault();

        if (ctor is null)
            return [];

        var arrayOfParams = ctor
            .GetParameters()
            .Select(p => p.ParameterType)
            .ToArray();

        var knownParams = known
            .Select((item) => new
            {
                Instance = item,
                Interfaces = item.GetType().GetInterfaces(),
            })
            .ToArray();

        foreach (var param in arrayOfParams)
        {
            var paramMocked = knownParams.FirstOrDefault(knownParam => knownParam.Interfaces.Contains(param));

            if (paramMocked is not null)
                mockedParams.Add(paramMocked.Instance);
            else
            {
                var mockedParam = typeof(Mock<>).MakeGenericType(param);
                var mock = Activator.CreateInstance(mockedParam) as Mock;

                if (mock is not null)
                    mockedParams.Add(mock.Object);
            }
        }

        return [.. mockedParams];
    }
}