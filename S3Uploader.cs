
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace s3proto
{
    class S3Uploader
    {
        public string BucketName  {get; set; } //"*** provide bucket name ***";
        public string KeyName {get; set;}      // "*** provide a name for the uploaded object ***";
        public string FilePath {get; set;}     // "*** provide the full path name of the file to upload ***";

        // Specify your bucket region (an example region is shown).
        // TODO -- Should probably set this from the config file
        private readonly Amazon.RegionEndpoint bucketRegion = RegionEndpoint.USEast2;
        private IAmazonS3 _s3Client;

        public S3Uploader(string bucketName)
        {
            BucketName = bucketName;

            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            // get AWS credentials
            var awsAccessKey = configuration.GetSection("AWS:AWSAccessKey").Value;
            var awsSecretAccessKey = configuration.GetSection("AWS:AWSSecretAccessKey").Value;

            _s3Client = new AmazonS3Client(awsAccessKey, awsSecretAccessKey, bucketRegion);
        }

        public async Task UploadFileAsync(string filePath)
        {
            try
            {
                var fileTransferUtility =
                    new TransferUtility(_s3Client);

                // Option 1. Upload a file. The file name is used as the object key name.
                await fileTransferUtility.UploadAsync(filePath, BucketName);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }

        public async Task UploadFileAsync(string filePath, string keyName)
        {
            try
            {
                var fileTransferUtility =
                    new TransferUtility(_s3Client);

                // Option 2. Specify object key name explicitly.
                await fileTransferUtility.UploadAsync(filePath, BucketName, keyName);
                Console.WriteLine("Upload 2 completed");

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }

    }
}
