using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GraphicsDetective.Startup))]
namespace GraphicsDetective
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
