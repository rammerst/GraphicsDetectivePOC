using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using GraphicsDetective.Models;
using GraphicsDetectiveCommon;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Newtonsoft.Json;

namespace GraphicsDetective.Controllers
{
    public class HomeController : Controller
    {
        private CloudBlobClient _blobClient;
        private CloudQueue _cloudQueue;

        public HomeController()
        {
            InitAzure(ConfigurationManager.AppSettings["storageaccountName"], ConfigurationManager.AppSettings["storageaccountKey"]);
        }

        private void InitAzure(string accountName, string keyValue)
        {
            var creds = new StorageCredentials(accountName, keyValue);
            var storageAccount = new CloudStorageAccount(creds, useHttps: true);
            _blobClient = storageAccount.CreateCloudBlobClient();

            // Get context object for working with queues, and 
            // set a default retry policy appropriate for a web user interface.
            var queueClient = storageAccount.CreateCloudQueueClient();
            queueClient.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(3), 3);

            // Get a reference to the queue.
            _cloudQueue = queueClient.GetQueueReference("thumbnailrequest");
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowUploads()
        {
            var imgResultModel = new ImagesResultModel();
            ViewBag.Message = "De reeds opgeladen afbeeldingen vanuit Azure Blob storage";

            var sampleContainer = _blobClient.GetContainerReference("images");
            sampleContainer.CreateIfNotExists();
            var blobs = sampleContainer.ListBlobs();

            foreach (var blobItem in blobs)
            {
                imgResultModel.Images.Add(new ImageFromBlob(blobItem.Uri));
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
                var sampleContainer = _blobClient.GetContainerReference("images");
                sampleContainer.CreateIfNotExists();

                foreach (string fileName in Request.Files)
                {
                    var file = Request.Files[fileName];
                    if (file == null) continue;
                    var blob = sampleContainer.GetBlockBlobReference(file.FileName);
                    blob.UploadFromStream(file.InputStream);

                    var blobInfo = new BlobInformation() { BlobReferenceId = 0, BlobUri = new Uri(blob.Uri.ToString()) };
                    var queueMessage = new CloudQueueMessage(JsonConvert.SerializeObject(blobInfo));
                    _cloudQueue.AddMessage(queueMessage);
                    Trace.TraceInformation("Created queue message for bloburi {0}", blob.Uri);
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