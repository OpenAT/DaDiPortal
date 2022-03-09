namespace Wpf.Mvvm.ViewModels;

public abstract class ListItemVm<T> : ViewModel
{
    #region fields

    private bool _isSelected;

    #endregion

    #region ctors

    protected ListItemVm(T data)
    {
        Data = data;
    }

    #endregion

    #region props

    public bool IsSelected
    {
        get { return _isSelected; }
        set { _isSelected = value; RaiseChanged(); }
    }

    public T Data { get; private set; }

    #endregion
}
