using Ejemplo_BlobStorage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage type

namespace Ejemplo_BlobStorage.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

       [HttpPost]
       public ActionResult CargarImagen(Persona persona)
       {
            string mensaje = String.Empty;

            // verificamos si se ha seleccionado un archivo
            if (persona.MyFile != null)
            {
                try
                {
                    var extension = persona.MyFile.FileName.Split('.')[1];

                    // Obtenemos referencia a la configuración de la cuenta en el webconfig.
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                        CloudConfigurationManager.GetSetting("StorageConnectionString"));

                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                    // Obtenemos referencia al contenedor
                    CloudBlobContainer container = blobClient.GetContainerReference("img");
                    
                    // Creamos el contenedor si no existe
                    container.CreateIfNotExists();

                    // Obtenems referencia al blob 
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(Guid.NewGuid().ToString() + "." + extension);

                    // Creamos o sobreescribimos el blob
                    using (var fileStream = persona.MyFile.InputStream)
                    {
                        // cargamos el blob
                        blockBlob.UploadFromStream(fileStream);

                        // una vez que se ha cargado podemos obtener la uri accediente a la función Uri, por ejemplo blockBlob.Uri

                    }

                    mensaje = "Imagen cargada con éxito " + blockBlob.Uri;

                }
                catch(Exception e)
                {
                    mensaje = "Error al cargar la imagen " + e.ToString();
                }
                
            }
            else
            {
                mensaje = "Seleccione una imagen!";
            }

            ViewBag.Mensaje = mensaje;

            return View("Index");
       }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}