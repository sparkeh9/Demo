namespace EnchiladaTests
{
   using System;
   using System.IO;
   using System.Threading.Tasks;
   using Microsoft.WindowsAzure.Storage;
   using Microsoft.WindowsAzure.Storage.Blob;
   using Xunit;

   public class When_using_raw_blob_storage_sdk
   {
      private readonly CloudStorageAccount account;
      private readonly CloudBlobClient client;
      private readonly CloudBlobContainer container;

      public When_using_raw_blob_storage_sdk()
      {
         account = CloudStorageAccount.Parse( "UseDevelopmentStorage=true" );
         client = account.CreateCloudBlobClient();
         container = client.GetContainerReference( "cats" );
      }

      [ Fact ]
      public async Task Should_upload_file()
      {
         await container.CreateIfNotExistsAsync();

         using ( Stream fileStream = File.OpenRead( $"{AppContext.BaseDirectory}\\Resources\\cat.jpg" ) )
         {
            fileStream.Position = 0;
            CloudBlockBlob blob = container.GetBlockBlobReference( "cat2.jpg" );
            await blob.UploadFromStreamAsync( fileStream );
         }
      }

      [ Fact ]
      public async Task Should_download_file()
      {
         var blob = container.GetBlockBlobReference( "cat2.jpg" );
         using ( var stream = File.Create( $"{AppContext.BaseDirectory}\\Resources\\cat.jpg" ) )
         {
            await blob.DownloadToStreamAsync( stream );
            stream.Seek( 0, SeekOrigin.Begin );
         }
      }
   }
}