namespace WebApplication1.Controllers
{
    using System.IO;
    using System.Threading.Tasks;
    using Enchilada.Infrastructure.Interface;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly IEnchiladaFilesystemResolver enchilada;

        public HomeController( IEnchiladaFilesystemResolver enchilada )
        {
            this.enchilada = enchilada;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ HttpPost ]
        public async Task<IActionResult> Index( IFormFile file )
        {
            if ( file == null )
                return View( false );

            string filepath = $"enchilada://cats/{file.FileName}";

            using ( Stream filestream = file.OpenReadStream() )
            using ( IFile fileReference = enchilada.OpenFileReference( filepath ) )
                await fileReference.CopyFromAsync( filestream );

            return View( true );
        }
    }
}