using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;

namespace demoAsp2.Responsitory
{
    public class BlobResponsitory
    {


        private BlobServiceClient _client1Blob;

        private BlobContainerClient clientWithName;

        public static readonly List<string> ImageExtensions = new List<string> { ".jpg", ".jpeg", ".png" };

        public BlobResponsitory(
            IAzureClientFactory<BlobServiceClient> blobClientFactory


            )
        {
            _client1Blob = blobClientFactory.CreateClient("blob1");





        }

        public async Task<string> UploadBlobFile(Stream file, string fileName)
        {



            //  trỏ vào storage
            clientWithName = _client1Blob.GetBlobContainerClient("picture1");

            ///tao ra doi tuong blob
            var blobClient = clientWithName.GetBlobClient(fileName);


            var status = await blobClient.UploadAsync(file);

            return blobClient.Uri.AbsoluteUri;


        }



        public async Task<List<string>> ListBlob()
        {
            clientWithName = _client1Blob.GetBlobContainerClient("picture1");

            List<string> list = new List<string>();

            await foreach (var blobItem in clientWithName.GetBlobsAsync())
            {
                list.Add(blobItem.Name);

            }

            return list;




        }

        public async Task<Stream> DownLoadBlobAsync(string blobFileName)
        {


            clientWithName = _client1Blob.GetBlobContainerClient("picture1");

            BlobClient file = clientWithName.GetBlobClient(blobFileName);


            if (await file.ExistsAsync())
            {
                var downloadContent = await file.DownloadAsync();

                return downloadContent.Value.Content;




            }
            return null;



        }

        public async Task DeleteBlobAsync(string blobFileName)
        {


            clientWithName = _client1Blob.GetBlobContainerClient("picture1");

            BlobClient file = clientWithName.GetBlobClient(blobFileName);


            if (await file.ExistsAsync())
            {

                file.DeleteIfExistsAsync();



            }




        }
    }








    //public void Delete Blob(string name)
    //{
    //    throw new NotImplementedException();
    //}
    //public Task<BlobObject> GetBlobFile(string name)
    //{
    //    throw new Not ImplementedException();
    //    public Task<List<string>> ListBlobs()
    //    {
    //        1 throw new Not ImplementedException();
    //    }

    //}
}


