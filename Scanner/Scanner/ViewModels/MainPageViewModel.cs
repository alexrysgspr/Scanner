using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Properties
        public DelegateCommand OpenScannerCommand { get; set; }
        public IPageDialogService _dialogService;
        #endregion

        public MainPageViewModel(INavigationService navigationService, IPageDialogService dialogService) 
            : base (navigationService)
        {
            Title = "Scanner Page";
            _dialogService = dialogService;
            OpenScannerCommand = new DelegateCommand(ExecuteOpenScannerCommand);
        }

        public async void ExecuteOpenScannerCommand() {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            if (status != PermissionStatus.Granted)
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
            else
                await NavigationService.NavigateAsync("ScannerPage");
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("ScannedItem"))
            {
                var itemResult = (string)parameters["ScannedItem"];
                await _dialogService.DisplayAlertAsync("Result", $"You have scanned {itemResult}", "Close"); 
            }
        }
    }
}
