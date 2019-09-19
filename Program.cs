using System;
using System.IO;
using System.Linq;

namespace s3proto
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the name of the bucket to upload files to");
            var bucketName = Console.ReadLine();
            Console.WriteLine("Specify the directory containing files you wish to upload");
            var directoryPath = Console.ReadLine();
            Console.WriteLine("Enter the extension of the file you wish to upload (* for all)");
            var extension = Console.ReadLine();
            if (extension.Length == 0)
            {
                Console.WriteLine("You have to provide an extension! Exiting...");
                return;
            }

            var uploader = new S3Uploader(bucketName);

            var files = Directory.GetFiles(directoryPath, $"*.{extension}", SearchOption.TopDirectoryOnly);
            foreach(var file in files)
            {
                Console.WriteLine($"Uploading ${file}");
                uploader.UploadFileAsync(file).Wait();
            }
        }
    }
}
