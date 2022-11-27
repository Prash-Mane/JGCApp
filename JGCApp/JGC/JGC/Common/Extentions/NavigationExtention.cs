using Acr.UserDialogs;
using JGC.ViewModel;
using JGC.ViewModels;
using JGC.ViewModels.MasterViewModel;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JGC.Common.Extentions
{
    public static class NavigationExtention
    {
        public static void RegisterTypeForViewModelNavigation<TView, TViewModel>(this IContainerRegistry container) where TView : Page where TViewModel : class
        {
            var viewType = typeof(TView);
            ViewModelLocationProvider.Register(viewType.ToString(), typeof(TViewModel));
            container.RegisterForNavigation<TView>(typeof(TViewModel).FullName);
        }

        public static async Task NavigateFromMenuAsync(
            this INavigationService navigationService,
            Type viewModel,
            NavigationParameters parameters = null,
            bool? useModalNavigation = null,
            bool animated = true)
        {

            //var url = CreateNavigationUrl(typeof(MainMasterDetailViewModel), typeof(MasterNavigationViewModel), viewModel);

            // await navigationService.NavigateAsync(url, parameters, useModalNavigation, animated);
            var url = CreateNavigationUrl(typeof(MainMasterDetailViewModel), typeof(MasterNavigationViewModel), viewModel);

            await navigationService.NavigateAsync(url, parameters, useModalNavigation, animated);
        }

        public static async Task NavigateFromMenuAsync<T>(
            this INavigationService navigationService,
            NavigationParameters parameters = null,
            bool? useModalNavigation = null,
            bool animated = true)
        {
            //var currentUrl = navigationService.GetNavigationUriPath();
            //Debug.WriteLine(currentUrl);
            await navigationService.NavigateFromMenuAsync(typeof(T), parameters, useModalNavigation, animated);
        }

        public static async Task NavigateAsync<TViewModel>(
            this INavigationService navigationService,
            NavigationParameters parameters = null,
            bool? useModalNavigation = null,
            bool animated = true) where TViewModel : BindableBase
        {
            //var currentUrl = navigationService.GetNavigationUriPath();
            //Debug.WriteLine(currentUrl);
            await navigationService.NavigateAsync(typeof(TViewModel).FullName, parameters, useModalNavigation, animated);
        }

        public static async Task ResetNavigation<TMenuViewModel, TNavigationViewModel1, TViewModel>(
            this INavigationService navigationService,
            NavigationParameters parameters = null,
            bool? useModalNavigation = null,
            bool animated = true,
            params string[] deepNavigation) where TViewModel : BindableBase
        {
            await navigationService.NavigateFromMenuAsync(typeof(TViewModel), parameters, useModalNavigation, animated);
            
        }

        public static async Task ResetNavigation<TViewModel>(
         this INavigationService navigationService,
         NavigationParameters parameters = null,
         bool? useModalNavigation = null,
         bool animated = true)
        {
            var resetNavigationString = $"JGCApp:///{typeof(TViewModel).FullName}";
            await navigationService.NavigateAsync(
             new Uri(
              resetNavigationString,
              UriKind.Absolute),
             parameters,
             useModalNavigation,
             animated);
        }

        public static async Task StartFromCurrentPage<TMenuViewModel, TNavigationViewModel, TViewModel>(
                this INavigationService navigationService,
                string navigationPages,
                NavigationParameters parameters = null,
                bool? useModalNavigation = null,
                bool animated = true) where TViewModel : BindableBase
        {
            string navigationString;
            if (string.IsNullOrEmpty(navigationPages))
            {
                navigationString = $"JGCApp:///{typeof(TMenuViewModel).FullName}/{typeof(TNavigationViewModel).FullName}/{typeof(TViewModel).FullName}";
            }
            else
            {
                navigationString = $"JGCApp:///{typeof(TMenuViewModel).FullName}/{typeof(TNavigationViewModel).FullName}/{typeof(TViewModel).FullName}/{navigationPages}";
            }

            await navigationService.NavigateAsync(navigationString, parameters, useModalNavigation, animated);
        }

        public static async Task ChangeTopPage(
            this INavigationService navigationService,
            Type viewModel,
            NavigationParameters parameters = null,
            bool? useModalNavigation = null,
            bool animated = true)
        {
            await navigationService.NavigateAsync($"../{viewModel}", parameters, useModalNavigation, animated);
        }

        public static async Task ChangeTopPage<T>(
            this INavigationService navigationService,
            NavigationParameters parameters = null,
            bool? useModalNavigation = null,
            bool animated = true) where T : BindableBase
        {
            await navigationService.ChangeTopPage(typeof(T), parameters, useModalNavigation, animated);
        }

        public static string CreateNavigationUrl(params Type[] types)
        {
            var url = string.Join("/", types.Select(t => t.ToString()));
            return $"/{url}";
        }

        public static string GetPreLastVM(this INavigationService navigationService)
        {
            var currentUrl = navigationService.GetNavigationUriPath();
            var vms = currentUrl.Remove(0, 1).Split('/');
            if (vms.Length < 2)
                return "RootViewModel"; //hack

            var preLastVM = vms[vms.Length - 2];
            if (string.IsNullOrEmpty(preLastVM))
                return null;

            var vmNoNamespace = preLastVM.Split('.').Last();

            return vmNoNamespace.Split('?').First();
        }
        public static string GetNavigationPreLastVM(this INavigationService navigationService)
        {
            var currentUrl = navigationService.GetNavigationUriPath();
            var vms = currentUrl.Remove(0, 1).Split('/');
            if (vms.Length < 2)
                return "RootViewModel"; //hack

            var preLastVM = vms[vms.Length - 2];
            if (string.IsNullOrEmpty(preLastVM))
                return null;

            return preLastVM;
        }

        public static string GetLastVM(this INavigationService navigationService)
        {
            var currentUrl = navigationService.GetNavigationUriPath();
            var vms = currentUrl.Remove(0, 1).Split('/');
            if (!vms.Any())
                return null;

            var lastVM = vms.LastOrDefault();
            if (string.IsNullOrEmpty(lastVM))
                return null;

            var vmNoNamespace = lastVM.Split('.').Last();

            return vmNoNamespace.Split('?').First();
        }
 }
}
