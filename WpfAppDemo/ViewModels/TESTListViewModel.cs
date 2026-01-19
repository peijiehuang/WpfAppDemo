using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WpfAppDemo.Models;
using WpfAppDemo.Services;

namespace WpfAppDemo.ViewModels
{
    public class TESTListViewModel : BindableBase, INavigationAware
    {
        private readonly ITESTService _tESTService;
        private readonly IRegionManager _regionManager;
        private readonly IBusyService _busyService;
        private readonly IMessageService _messageService;
        private string _searchText = string.Empty;

        public ObservableCollection<TEST> TESTs { get; } = new();

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public DelegateCommand AddCommand { get; }
        public DelegateCommand SearchCommand { get; }
        public DelegateCommand<TEST> EditCommand { get; }
        public DelegateCommand<TEST> DeleteCommand { get; }

        public TESTListViewModel(ITESTService tESTService, IRegionManager regionManager, IBusyService busyService, IMessageService messageService)
        {
            _tESTService = tESTService;
            _regionManager = regionManager;
            _busyService = busyService;
            _messageService = messageService;

            AddCommand = new DelegateCommand(OnAdd);
            SearchCommand = new DelegateCommand(LoadDataAsync);
            EditCommand = new DelegateCommand<TEST>(OnEdit);
            DeleteCommand = new DelegateCommand<TEST>(OnDelete);
        }

        private async void LoadDataAsync()
        {
            try
            {
                _busyService.Busy("正在查询...");
                await Task.Delay(200); 
                
                var data = _tESTService.GetTESTs(SearchText);
                
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    TESTs.Clear();
                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            TESTs.Add(item);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "查询 TEST 列表失败");
                System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    _messageService.ShowMessageAsync($"查询失败: {ex.Message}", "Common_Error");
                }));
            }
            finally
            {
                _busyService.Idle();
            }
        }

        private void OnAdd()
        {
            _regionManager.RequestNavigate("ContentRegion", "TESTEditView");
        }

        private void OnEdit(TEST entity)
        {
            var parameters = new NavigationParameters { { "TEST", entity } };
            _regionManager.RequestNavigate("ContentRegion", "TESTEditView", parameters);
        }

        private async void OnDelete(TEST entity)
        {
            var msg = System.Windows.Application.Current.TryFindResource("Common_DeleteMessage")?.ToString() ?? "确定要删除这条记录吗?";
            if (await _messageService.ShowConfirmationAsync(msg, "Common_DeleteConfirm"))
            {
                try
                {
                    _tESTService.DeleteTEST(entity.Id);
                    LoadDataAsync();
                }
                catch (Exception ex)
                {
                    var errorTitle = System.Windows.Application.Current.TryFindResource("Common_Error")?.ToString() ?? "错误";
                    await _messageService.ShowMessageAsync($"删除失败: {ex.Message}", errorTitle);
                }
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext) => LoadDataAsync();
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public void OnNavigatedFrom(NavigationContext navigationContext) { }
    }
}
