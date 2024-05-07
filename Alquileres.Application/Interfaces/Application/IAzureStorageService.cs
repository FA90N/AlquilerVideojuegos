namespace Alquileres.Application.Interfaces.Application;

public interface IAzureStorageService
{
    Task<byte[]> DownloadFileByteArray(string blobName, string blobContainerName);
    Task UploadFile(string blobName, string blobContainerName, byte[] fileData);
}