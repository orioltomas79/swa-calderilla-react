namespace Calderilla.Services.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var message = Service1.GetMessage("user");
            Assert.Equal("Welcome to Calderilla Service user!", message);
        }
    }
}