using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using JGC.DataBase.DataTables.WorkPack;
using JGC.Models.Work_Pack;
using JGC.Views;
using JGC.Views.Work_Pack;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace JGC.ViewModels.Work_Pack
{
    public class EditPunchControlViewModel : BaseViewModel, INotifyPropertyChanged
    {

        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_UserDetails> _userDetailsRepository;
        private readonly IRepository<T_UserProject> _userProjectRepository;
        private readonly IRepository<T_CWPDrawings> _CWPDrawingsRepository;
        private readonly IRepository<T_TagMilestoneStatus> _tagMilestoneStatusRepository;
        private readonly IRepository<T_TagMilestoneImages> _tagMilestoneImagesRepository;
        private readonly IRepository<T_IWP> _iwpRepository;
        private IList<T_TagMilestoneStatus> CWPTagsData;
        private List<TagMilestoneStatusModel> CWPList = new List<TagMilestoneStatusModel>();
        private T_UserDetails userDetail;
        public byte[] imageAsByte = null;
        private readonly IMedia _media;
        private T_UserProject CurrentUserProject;

        public EditPunchControlViewModel(INavigationService _navigationService,
          IUserDialogs _userDialogs,
          IHttpHelper _httpHelper,
          IMedia _media,
          ICheckValidLogin _checkValidLogin,
          IRepository<T_UserDetails> _userDetailsRepository,
          IRepository<T_UserProject> _userProjectRepository,
          IRepository<T_CWPDrawings> _CWPDrawingsRepository,
          IRepository<T_TagMilestoneStatus> _tagMilestoneStatusRepository,
          IRepository<T_TagMilestoneImages> _tagMilestoneImagesRepository,
          IRepository<T_IWP> _iwpRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._userDetailsRepository = _userDetailsRepository;
            this._userProjectRepository = _userProjectRepository;
            this._CWPDrawingsRepository = _CWPDrawingsRepository;
            this._tagMilestoneStatusRepository = _tagMilestoneStatusRepository;
            this._tagMilestoneImagesRepository = _tagMilestoneImagesRepository;
            this._iwpRepository = _iwpRepository;
            this._media = _media;
            _media.Initialize();
            _userDialogs.HideLoading();

            GetPunchLayerGridData();
           
        }

     

        private async Task GetPunchLayerGridData()
        {
           
          
            //var AdminPunchLayer = await _adminPunchLayerRepository.QueryAsync<T_AdminPunchLayer>("SELECT * FROM [T_AdminPunchLayer] WHERE [ProjectID] = '" + Settings.ProjectID + "' ORDER BY [LayerNo] ASC");
            //List<T_AdminPunchLayer> APL = new List<T_AdminPunchLayer> { new T_AdminPunchLayer { LayerName = "Test Limits", LayerNo = 2, ID = 0, } };
            //APL.AddRange(AdminPunchLayer);
            //PunchLayersList = new ObservableCollection<T_AdminPunchLayer>(APL);
            //SelectedpunchLayer = PunchLayersList.FirstOrDefault();
            //GetSelectedpunchLayer(false);
            //LoadPunchViewTabAsync();
            //GeneratePunchDataTable(CurrentPageHelper.CurrentDrawing.ID);

        }

    }
}
