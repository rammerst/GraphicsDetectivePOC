using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DropzoneJs.Models;
using Microsoft.WindowsAzure.Storage.Blob;

namespace DropzoneJs.Controllers
{
    public class HomeController : Controller
    {
        public CloudBlobClient Client;

        public HomeController()
        {
            CreateBlobContainer();
        }

        private void CreateBlobContainer()
        {
            var creds = new StorageCredentials("graphdetective", "+O6wDOg4Gt+pYGKjxPzhUMvdrp0Wl0IMXx+eQRrCEg5VjbqKq8yNUaPyg1KhqGdOKrHGdbDsub/IxPodTxHXNg==");
            var account = new CloudStorageAccount(creds, useHttps: true);
            Client = account.CreateCloudBlobClient();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowUploads()
        {
            var imgResultModel = new ImagesResultModel();
            ViewBag.Message = "De reeds opgeladen afbeeldingen vanuit Azure Blob storage";
            
            var sampleContainer = Client.GetContainerReference("images");
            sampleContainer.CreateIfNotExists();
            var blobs = sampleContainer.ListBlobs().OrderBy(o => o.Uri);

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
            try
            {
               var sampleContainer = Client.GetContainerReference("images");
                sampleContainer.CreateIfNotExists();

                foreach (string fileName in Request.Files)
                {
                    var file = Request.Files[fileName];
                    var blob = sampleContainer.GetBlockBlobReference(file.FileName);
                    blob.UploadFromStream(file.InputStream);                   
                }
            }
            catch (Exception ex)
            {
                var ssfsdf = ex;

            }
            return Json(new { Message = "Opladen gelukt!" });
        }
    }
}