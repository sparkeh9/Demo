namespace EnchiladaTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Enchilada.Azure.BlobStorage;
    using Enchilada.Configuration;
    using Enchilada.Filesystem;
    using Enchilada.Ftp;
    using Enchilada.Infrastructure;
    using Shouldly;
    using Xunit;

    public class When_using_ftp
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
                    },
                    new FtpAdapterConfiguration
                    {
                        AdapterName = "dogs",
                        Host = "localhost",
                        Port = 21,
                        Username = "user",
                        Password = "password",
                        Directory = "/",
                    }
                }
            } );

            var source = enchilada.OpenFileReference( "enchilada://cats/cat.jpg" );
            var target = enchilada.OpenFileReference( "enchilada://dogs/cat2.jpg" );
            target.Exists.ShouldBeFalse();

            await target.CopyFromAsync( source );
            File.Exists( "d:\\temp\\cat2.jpg" );
        }
    }
}