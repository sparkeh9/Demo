namespace EnchiladaTests
{
    using System;
    using Xunit;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Enchilada.Configuration;
    using Enchilada.Filesystem;
    using Enchilada.Infrastructure;

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

            var guid = Guid.NewGuid();
            var source = enchilada.OpenFileReference( "enchilada://cats/cat.jpg" );
            var target = enchilada.OpenFileReference( $"enchilada://cats/{guid}.jpg" );

            await target.CopyFromAsync( source );

            File.Exists( $"{AppContext.BaseDirectory}\\Resources\\{guid}.jpg" );
        }
    }
}