using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using GraphicsDetective.Models;
using Microsoft.WindowsAzure.Storage.Blob;

namespace GraphicsDetective.Controllers
{
    public class HomeController : Controller
    {
        public CloudBlobClient BlobClient;

        public HomeController()
        {
            BlobClient = CreateBlobContainer(ConfigurationManager.AppSettings["storageaccountName"], ConfigurationManager.AppSettings["storageaccountKey"]);
        }

        private static CloudBlobClient CreateBlobContainer(string accountName,string keyValue)
        {
            var creds = new StorageCredentials(accountName, keyValue);
            var account = new CloudStorageAccount(creds, useHttps: true);
            return account.CreateCloudBlobClient();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowUploads()
        {
            var imgResultModel = new ImagesResultModel();
            ViewBag.Message = "De reeds opgeladen afbeeldingen vanuit Azure Blob storage";
            
            var sampleContainer = BlobClient.GetContainerReference("images");
            sampleContainer.CreateIfNotExists();
            var blobs = sampleContainer.ListBlobs();

            foreach (var blobItem in blobs)
            {
                imgResultModel.Images.Add(blobItem.Uri.ToString());
                //using (Stream outputFile = new FileStream("Downloaded.jpg", FileMode.Create))
                //{
                //    blob.DownloadToStream(outputFile);
                //}
            }
           
            return View(imgResultModel);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// to Save DropzoneJs Uploaded Files
        /// </summary>
        public ActionResult SaveDropzoneJsUploadedFiles()
        {
            var message = "Opladen gelukt!";
            try
            {
               var sampleContainer = BlobClient.GetContainerReference("images");
                sampleContainer.CreateIfNotExists();

                foreach (string fileName in Request.Files)
                {
                    var file = Request.Files[fileName];
                    if (file == null) continue;
                    var blob = sampleContainer.GetBlockBlobReference(file.FileName);
                    blob.UploadFromStream(file.InputStream);
                }
            }
            catch (Exception ex)
            {
                message = ex.ToString();

            }
            return Json(new { Message = message });
        }
    }
}