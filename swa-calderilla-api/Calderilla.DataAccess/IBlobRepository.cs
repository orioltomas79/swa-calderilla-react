﻿namespace Calderilla.DataAccess
{
    public interface IBlobRepository
    {
        Task<List<T>> ReadListAsync<T>(string blobName);

        Task WriteListAsync<T>(string blobName, List<T> list);

        Task<bool> ExistsAsync(string blobName);

        Task DeleteAsync(string blobName);
    }
}
