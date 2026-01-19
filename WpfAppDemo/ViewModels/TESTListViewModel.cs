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

        public ObservableCollection<TEST> TESTs { get; } = new();

        public DelegateCommand AddCommand { get; }
        public DelegateCommand<TEST> EditCommand { get; }
        public DelegateCommand<TEST> DeleteCommand { get; }

        public TESTListViewModel(ITESTService tESTService, IRegionManager regionManager, IBusyService busyService, IMessageService messageService)
        {
            _tESTService = tESTService;
            _regionManager = regionManager;
            _busyService = busyService;
            _messageService = messageService;

            AddCommand = new DelegateCommand(OnAdd);
            EditCommand = new DelegateCommand<TEST>(OnEdit);
            DeleteCommand = new DelegateCommand<TEST>(OnDelete);
        }

        private async void LoadDataAsync()
        {
            try
            {
                _busyService.Busy("正在加载 TEST 数据...");
                await Task.Delay(300); // 稍微延迟提升体验
                
                var data = _tESTService.GetTESTs();
                
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
                Serilog.Log.Error(ex, "加载 TEST 列表失败");
                System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    _messageService.ShowMessageAsync($"数据加载失败: {ex.Message}", "错误");
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
            if (await _messageService.ShowConfirmationAsync($"确定要删除这条记录吗?", "删除确认"))
            {
                try
                {
                    _tESTService.DeleteTEST(entity.Id);
                    LoadDataAsync();
                }
                catch (Exception ex)
                {
                    await _messageService.ShowMessageAsync($"删除失败: {ex.Message}", "错误");
                }
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext) => LoadDataAsync();
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public void OnNavigatedFrom(NavigationContext navigationContext) { }
    }
}