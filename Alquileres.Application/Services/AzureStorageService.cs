using Alquileres.Application.Interfaces.Application;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace Alquileres.Application.Services;

public class AzureStorageService : IAzureStorageService
{
    private readonly IConfiguration _config;

    public AzureStorageService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<byte[]> DownloadFileByteArray(string blobName, string blobContainerName)
    {
        try
        {
            var blobServiceClient = new BlobServiceClient(_config.GetSection("AzureStorageConnection").Value);
            var containerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            if (await blobClient.ExistsAsync())
            {
                using (var ms = new MemoryStream())
                {
                    blobClient.DownloadTo(ms);
                    return ms.ToArray();
                }
            }

            return null;
        }
        catch (Azure.RequestFailedException ex)
        {
            throw;
        }
        catch
        {
            throw;
        }
    }

    public async Task UploadFile(string blobName, string blobContainerName, byte[] fileData)
    {
        try
        {
            var blobServiceClient = new BlobServiceClient(_config.GetSection("AzureStorageConnection").Value);
            var containerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);

            var resp = await containerClient
                .GetBlobClient(blobName)
                .UploadAsync(new BinaryData(fileData), true);
        }
        catch
        {
            throw;
        }
    }
}
