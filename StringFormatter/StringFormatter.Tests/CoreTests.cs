using System.Collections.Concurrent;

namespace StringFormatter.Tests
{
    [TestClass]
    public class CoreTests
    {
        [TestMethod]
        public void Test_Compare_With_Valid_String()
        {
            // Arrange
            var user = new User("����", "������");
            string expected = $"������, {user.FirstName} {user.LastName}!";

            // Act
            var actual = user.GetGreeting();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}