using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ejemplo_BlobStorage.Startup))]
namespace Ejemplo_BlobStorage
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
