using FakeItEasy;

namespace TestProject1;

public interface ITest
{
    void Thing();
}

public class Test : ITest
{
    public void Thing()
    {
        throw new NotImplementedException();
    }
}

public class Derp
{
    private readonly ITest _test;

    public Derp(ITest test)
    {
        _test = test;
    }
    
    public void DoWork()
    {
        //_test.Thing();
    }
}

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var fakeThing = A.Fake<ITest>();
        var derp = new Derp(fakeThing);
        
        derp.DoWork();
        A.CallTo(() => fakeThing.Thing()).MustHaveHappened();
    }
}