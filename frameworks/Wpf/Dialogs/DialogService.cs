using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Wpf.Dialogs
{
    public interface IDialogService
    {
        IDialogService MapDialog(Type model, Type dialogInterface);
        void ShowDialog<T>(T parameter);
    }

    public class DialogService : IDialogService
    {
        #region fields

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly Dictionary<Type, Type> _mappings;

        #endregion

        #region ctors

        public DialogService(IServiceScopeFactory serviceScopeFactory)
        {
            _mappings = new Dictionary<Type, Type>();
            _serviceScopeFactory = serviceScopeFactory;
        }

        #endregion

        #region methods

        public IDialogService MapDialog(Type model, Type dialogInterface)
        {
            _mappings.Add(model, dialogInterface);
            return this;
        }

        public void ShowDialog<T>(T parameter)
        {
            var dlgInterface = _mappings[typeof(T)];

            using (var serviceScope = _serviceScopeFactory.CreateScope())
            {
                var dlg = (IDialog)serviceScope
                    .ServiceProvider
                    .GetService(dlgInterface);

                dlg.ViewModel.SetData(parameter);
                dlg.ShowDialog();
            }
        }

        #endregion
    }
}
