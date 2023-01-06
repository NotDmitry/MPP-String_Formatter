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

        [TestMethod]
        public void Test_Escape_Sequence_Between()
        {
            // Arrange
            var user = new User("����", "������");
            string expected = $"������, {{FirstName}} {user.LastName}!";

            // Act
            var actual = Core.StringFormatter.Shared.Format("������, {{FirstName}} {LastName}!", user);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_Escape_Sequence_OnStart()
        {
            // Arrange
            var user = new User("����", "������");
            string expected = $"{{FirstName}} {user.LastName}!";

            // Act
            var actual = Core.StringFormatter.Shared.Format("{{FirstName}} {LastName}!", user);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_Valid_OnStart()
        {
            // Arrange
            var user = new User("����", "������");
            string expected = $"{user.FirstName} {user.LastName}!";

            // Act
            var actual = Core.StringFormatter.Shared.Format("{FirstName} {LastName}!", user);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_Escape_Sequence_End()
        {
            // Arrange
            var user = new User("����", "������");
            string expected = $"{user.FirstName} {{LastName}}";

            // Act
            var actual = Core.StringFormatter.Shared.Format("{FirstName} {{LastName}}", user);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_Valid_End()
        {
            // Arrange
            var user = new User("����", "������");
            string expected = $"{user.FirstName} {user.LastName}";

            // Act
            var actual = Core.StringFormatter.Shared.Format("{FirstName} {LastName}", user);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_Double_Escape()
        {
            // Arrange
            var user = new User("����", "������");
            string expected = $"������, {{{{LastName}}}}";

            // Act
            var actual = Core.StringFormatter.Shared.Format("������, {{{{LastName}}}}", user);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_Combine_Escape_Valid()
        {
            // Arrange
            var user = new User("����", "������");
            string expected = $"������, {user.FirstName} {{{user.LastName}}}";

            // Act
            var actual = Core.StringFormatter.Shared.Format("������, {FirstName} {{{LastName}}}", user);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}