namespace Wpf.Mvvm.ViewModels
{
    public class TreeViewItemVm<T> : ViewModel
    {
        protected readonly T _data;

        private bool _isSelected;
        private bool _isExpanded;


        public TreeViewItemVm(T data)
        {
            _data = data;
        }

        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; RaiseChanged(); }
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set { _isExpanded = value; RaiseChanged(); }
        }
    }
}
