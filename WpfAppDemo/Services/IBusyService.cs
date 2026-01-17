namespace WpfAppDemo.Services
{
    public interface IBusyService
    {
        bool IsBusy { get; }
        void Busy(string? message = null);
        void Idle();
    }

    public class BusyService : Prism.Mvvm.BindableBase, IBusyService
    {
        private bool _isBusy;
        private string? _message;

        public bool IsBusy
        {
            get => _isBusy;
            private set => SetProperty(ref _isBusy, value);
        }

        public string? Message
        {
            get => _message;
            private set => SetProperty(ref _message, value);
        }

        public void Busy(string? message = null)
        {
            Message = message;
            IsBusy = true;
        }

        public void Idle()
        {
            IsBusy = false;
            Message = null;
        }
    }
}
