using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using WpfAppDemo.Models;
using WpfAppDemo.Services;

namespace WpfAppDemo.ViewModels
{
    public class TESTEditViewModel : BindableBase, INavigationAware
    {
        private readonly ITESTService _tESTService;
        private readonly IRegionManager _regionManager;
        private TEST _entity = new TEST();
        private string _title = string.Empty;

        public TEST Entity
        {
            get => _entity;
            set => SetProperty(ref _entity, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public TESTEditViewModel(ITESTService tESTService, IRegionManager regionManager)
        {
            _tESTService = tESTService;
            _regionManager = regionManager;

            SaveCommand = new DelegateCommand(OnSave);
            CancelCommand = new DelegateCommand(OnCancel);
        }

        private void OnSave()
        {
            if (Entity == null) return;

            if (Entity.Id == 0)
                _tESTService.AddTEST(Entity);
            else
                _tESTService.UpdateTEST(Entity);

            _regionManager.Regions["ContentRegion"].NavigationService.Journal.GoBack();
        }

        private void OnCancel()
        {
            _regionManager.Regions["ContentRegion"].NavigationService.Journal.GoBack();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("TEST"))
            {
                var entity = navigationContext.Parameters.GetValue<TEST>("TEST");
                if (entity != null)
                {
                    Entity = entity;
                    Title = "编辑 TEST";
                    return;
                }
            }
            
            Entity = new TEST();
            Title = "新增 TEST";
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public void OnNavigatedFrom(NavigationContext navigationContext) { }
    }
}