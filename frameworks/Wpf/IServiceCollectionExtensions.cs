using Microsoft.Extensions.DependencyInjection;
using Wpf.Dialogs;

namespace Wpf
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddWpfFramework(this IServiceCollection services)
        {
            return services
                .AddSingleton<IDialogService, DialogService>();
        }
    }
}
