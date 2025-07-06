using System.Text.Json;
using Azure.Storage.Blobs;

namespace Calderilla.DataAccess
{
    public class BlobRepository : IBlobRepository
    {
        private const string ContainerName = "swa-calderilla";

        public BlobRepository()
        {

        }

        public async Task<List<T>> ReadListAsync<T>(string blobName)
        {
            var blobClient = await GetBlobClientAsync(blobName).ConfigureAwait(false);

            if (!await blobClient.ExistsAsync()) return [];

            await using var memoryStream = new MemoryStream();
            await blobClient.DownloadToAsync(memoryStream).ConfigureAwait(false);
            memoryStream.Position = 0;

            return await JsonSerializer.DeserializeAsync<List<T>>(memoryStream).ConfigureAwait(false) ?? [];
        }

        public async Task WriteListAsync<T>(string blobName, List<T> list)
        {
            var blobClient = await GetBlobClientAsync(blobName).ConfigureAwait(false);

            await using var memoryStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(memoryStream, list).ConfigureAwait(false);
            memoryStream.Position = 0;

            await blobClient.UploadAsync(memoryStream, overwrite: true).ConfigureAwait(false);
        }

        private static async Task<BlobClient> GetBlobClientAsync(string blobName)
        {
            var connectionStr = Environment.GetEnvironmentVariable("StorageAccountConnectionString");
            if (string.IsNullOrEmpty(connectionStr))
            {
                throw new InvalidOperationException("Storage account connection string is not set.");
            }

            var blobContainerClient = new BlobContainerClient(connectionStr, ContainerName);
            await blobContainerClient.CreateIfNotExistsAsync().ConfigureAwait(false);

            return blobContainerClient.GetBlobClient(blobName);
        }

        public async Task<bool> ExistsAsync(string blobName)
        {
            var blobClient = await GetBlobClientAsync(blobName).ConfigureAwait(false);
            return await blobClient.ExistsAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(string blobName)
        {
            var blobClient = await GetBlobClientAsync(blobName).ConfigureAwait(false);
            await blobClient.DeleteIfExistsAsync().ConfigureAwait(false);
        }
    }
}
