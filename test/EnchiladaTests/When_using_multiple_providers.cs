namespace EnchiladaTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Enchilada.Azure.BlobStorage;
    using Enchilada.Configuration;
    using Enchilada.Filesystem;
    using Enchilada.Ftp;
    using Enchilada.Infrastructure;
    using Xunit;

    public class When_using_multiple_providers
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
                        AdapterName = "moar_cats",
                        Host = "localhost",
                        Port = 21,
                        Username = "user",
                        Password = "password",
                        Directory = "/",
                    },
                    new BlobStorageAdapterConfiguration
                    {
                        AdapterName = "even_moar_cats",
                        ConnectionString = "UseDevelopmentStorage=true;",
                        ContainerReference = "cats",
                        CreateContainer = true,
                        IsPublicAccess = false
                    }
                }
            } );

            var filesystem = enchilada.OpenFileReference( "enchilada://cats/cat.jpg" );
            var ftp = enchilada.OpenFileReference( $"enchilada://moar_cats/{Guid.NewGuid()}.jpg" );
            var blob = enchilada.OpenFileReference( $"enchilada://even_moar_cats/{Guid.NewGuid()}.jpg" );

            await ftp.CopyFromAsync( filesystem );
            await blob.CopyFromAsync( ftp );
        }
    }
}