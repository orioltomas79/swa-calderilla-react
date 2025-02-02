namespace Calderilla.DataAccess.Tests
{
    public class BlobRepoShould
    {
        [Fact]
        public void GetMessage_ReturnsExpectedMessage()
        {
            // Arrange
            var blobRepo = new BlobRepo();

            // Act
            var result = blobRepo.GetMessage();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Hello from Blob repo!", result);
        }

        [Fact]
        public void GetOperations_ReturnsExpectedOperations()
        {
            // Arrange
            var blobRepo = new BlobRepo();

            // Act
            var result = blobRepo.GetOperations(1, 2025);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }
    }
}