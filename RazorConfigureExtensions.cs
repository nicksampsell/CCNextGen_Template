using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;

namespace CCNextGen_Template
{
    public static class RazorConfigurationExtensions
    {
        public static IMvcBuilder AddCustomRazorConfiguration(this IMvcBuilder builder)
        {
            return builder.AddRazorOptions(opt =>
            {
                opt.AreaViewLocationFormats.Add("/Pages/Shared/LayoutPartials/{0}" + RazorViewEngine.ViewExtension);
                opt.AreaViewLocationFormats.Add("/Pages/Shared/SubLayouts/{0}" + RazorViewEngine.ViewExtension);
                opt.ViewLocationFormats.Add("/Pages/Shared/LayoutPartials/{0}" + RazorViewEngine.ViewExtension);
                opt.ViewLocationFormats.Add("/Pages/Shared/SubLayouts/{0}" + RazorViewEngine.ViewExtension);
            });
        }

        public static IServiceCollection AddCustomRazorConfiguration(this IServiceCollection services)
        {
            return services.Configure<RazorViewEngineOptions>(opt =>
            {
                opt.PageViewLocationFormats.Add("/Pages/Shared/LayoutPartials/{0}" + RazorViewEngine.ViewExtension);
                opt.PageViewLocationFormats.Add("/Pages/Shared/SubLayouts/{0}" + RazorViewEngine.ViewExtension);
                opt.AreaPageViewLocationFormats.Add("/Pages/Shared/LayoutPartials/{0}" + RazorViewEngine.ViewExtension);
                opt.AreaPageViewLocationFormats.Add("/Pages/Shared/SubLayouts/{0}" + RazorViewEngine.ViewExtension);
                opt.ViewLocationFormats.Add("/Pages/Shared/LayoutPartials/{0}" + RazorViewEngine.ViewExtension);
                opt.ViewLocationFormats.Add("/Pages/Shared/SubLayouts/{0}" + RazorViewEngine.ViewExtension);
            });
        }
    }
}
