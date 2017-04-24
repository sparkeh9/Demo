using System;
using Xunit;

namespace EnchiladaTests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Enchilada.Configuration;
    using Enchilada.Filesystem;
    using Enchilada.Infrastructure;
    using Shouldly;

    public class When_using_local_filesystem
    {
        [ Fact ]
        public async Task Should_copy_from_one_location_to_another()
        {
            var enchilada = new EnchiladaFileProviderResolver( new EnchiladaConfiguration
            {
                Adapters = new List<IEnchiladaAdapterConfiguration>
                {
                    new FilesystemAdapterConfiguration
                    {
                        AdapterName = "cats",
                        Directory = $"{AppContext.BaseDirectory}\\Resources"
                    }
                }
            } );
            
            var source = enchilada.OpenFileReference( "enchilada://cats/cat.jpg" );
            var target = enchilada.OpenFileReference( "enchilada://cats/cat2.jpg" );

            await target.CopyFromAsync( source );

            File.Exists($"{AppContext.BaseDirectory}\\Resources\\cat2.jpg");
        }
    }
}