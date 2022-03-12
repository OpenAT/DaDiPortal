using System.Threading.Tasks;

namespace Wpf.Mvvm.ViewModels;

public abstract class ItemVm<T> : ViewModel
{
    #region fields

    private bool _isSelected;

    #endregion

    #region ctors

    protected ItemVm(T data)
    {
        Data = data;
    }

    #endregion

    #region props

    public bool IsSelected
    {
        get { return _isSelected; }
        set 
        { 
            _isSelected = value; 
            RaiseChanged();
            OnSelected();
        }
    }

    public T Data { get; private set; }

    #endregion

    #region methods

    protected virtual Task OnSelected() 
        => Task.CompletedTask;

    #endregion
}
