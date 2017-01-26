using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Manager_Employees.Startup))]
namespace Manager_Employees
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
