namespace EnchiladaTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Enchilada.Azure.BlobStorage;
    using Enchilada.Configuration;
    using Enchilada.Filesystem;
    using Enchilada.Infrastructure;
    using Shouldly;
    using Xunit;

    public class When_using_azure_blob_storage
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
                    new BlobStorageAdapterConfiguration
                    {
                        AdapterName = "dogs",
                        ConnectionString = "UseDevelopmentStorage=true;",
                        ContainerReference = "dogs",
                        CreateContainer = true,
                        IsPublicAccess = false
                    }
                }
            } );

            var source = enchilada.OpenFileReference( "enchilada://cats/cat.jpg" );
            var target = enchilada.OpenFileReference( "enchilada://dogs/cat2.jpg" );
            target.Exists.ShouldBeFalse();

            await target.CopyFromAsync( source );
            target.Exists.ShouldBeTrue();
        }
    }
}