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
            Assert.Equal("Hello from Blob repo!", result);
        }
    }
}