namespace WebApplication1.Controllers
{
    using System.IO;
    using System.Threading.Tasks;
    using Enchilada.Infrastructure.Interface;
    using Microsoft.AspNetCore.Mvc;

    public class DownloadController : Controller
    {
        private readonly IEnchiladaFilesystemResolver enchilada;

        // Keep these outside the scope of the action - 
        // otherwise the framework disposes them too early
        private IFile downloadFileReference;
        private Stream downloadFileStream;

        public DownloadController( IEnchiladaFilesystemResolver enchilada )
        {
            this.enchilada = enchilada;
        }

        public async Task<IActionResult> Index( string filename )
        {
            string filepath = $"enchilada://cats/{filename}";

            downloadFileReference = enchilada.OpenFileReference( filepath );
            downloadFileStream = await downloadFileReference.OpenReadAsync();

            if ( downloadFileReference.Size > 0 )
                Response.Headers.Add( "Content-Length", downloadFileReference.Size.ToString() );

            return File( downloadFileStream, "application/octet-stream", filename );
        }
    }
}