using Azure.Storage.Blobs;
using MusicApi.Models;

namespace MusicApi.Helpers
{
    public static class FileHelper       // we declare it static so we don't have to instantiate FileHelper object
    {
        public static async Task<string> UploadImageAsync(IFormFile file)
        {
            string connectionString = @"DefaultEndpointsProtocol=https;AccountName=mymusicstorage;AccountKey=Nm+/69964mSzDhnQs9fpuCMphojD+i18Mh6Dhn4bQSYOY5fCzAge7SibhfAP8F/IGPw/9vfu3eqc+ASttvgR9Q==;EndpointSuffix=core.windows.net";           // Blob Storage connection string
            string containerName = "songscover";

            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName); // purpose of this is to get refernce to container 
            BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);                          //We use blobclient class when we want to access or retrieve something from blobstorage
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream);         //To this point was picture uploading to blob stroage container    
            return blobClient.Uri.AbsoluteUri;            //We access the  URL of image stored on azzure Blobstorage

        }

        public static async Task<string> UploadFileAsync(IFormFile file)
        {
            string connectionString = @"DefaultEndpointsProtocol=https;AccountName=mymusicstorage;AccountKey=Nm+/69964mSzDhnQs9fpuCMphojD+i18Mh6Dhn4bQSYOY5fCzAge7SibhfAP8F/IGPw/9vfu3eqc+ASttvgR9Q==;EndpointSuffix=core.windows.net";           // Blob Storage connection string
            string containerName = "audiofiles";

            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName); // purpose of this is to get refernce to container 
            BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);                          //We use blobclient class when we want to access or retrieve something from blobstorage
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream);         //To this point was picture uploading to blob stroage container    
            return blobClient.Uri.AbsoluteUri;            //We access the  URL of image stored on azzure Blobstorage

        }
    }
}
