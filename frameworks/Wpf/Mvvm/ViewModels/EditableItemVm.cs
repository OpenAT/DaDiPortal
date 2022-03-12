using Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Wpf.Mvvm.Commands;

namespace Wpf.Mvvm.ViewModels
{
    public abstract class EditableItemVm<T> : ItemVm<T>, INotifyDataErrorInfo
    {
        #region fields

        private bool _isInEditMode;
        private Dictionary<string, object> _backup;
        private Dictionary<string, List<string>> _foundErrors;

        #endregion

        #region ctors

        protected EditableItemVm(ILogger<EditableItemVm<T>> logger, T data) : base(data)
        {
            _foundErrors = new Dictionary<string, List<string>>();
            _backup = new Dictionary<string, object>();

            BeginEditCmd = new SyncDelegateCommand(BeginEdit, logger);
            EndEditCmd = new SyncDelegateCommand(EndEdit, logger);
            CancelEditCmd = new SyncDelegateCommand(CancelEdit, logger);
        }

        #endregion

        #region cmds 

        public IDelegateCommand BeginEditCmd { get; }
        public IDelegateCommand EndEditCmd { get; }
        public IDelegateCommand CancelEditCmd { get; }

        #endregion 

        #region props

        public bool IsInEditMode
        {
            get { return _isInEditMode; }
            private set { _isInEditMode = value; RaiseChanged(); }
        }

        public bool HasErrors
        {
            get => _foundErrors != null &&
                _foundErrors.Count > 0;
        }

        #endregion

        #region events

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public event Action<T> FinishedEditing;

        #endregion

        #region methods

        public IEnumerable GetErrors(string propertyName)
        {
            return !propertyName.IsEmpty() && _foundErrors.ContainsKey(propertyName)
                   ? _foundErrors[propertyName]
                   : new List<string>();
        }

        protected virtual void BeginEdit()
        {
            var properties = GetType()
                .GetProperties()
                .Where(x => x.GetSetMethod() != null &&
                    x.GetSetMethod().IsPublic &&
                    x.Name != nameof(IsSelected))
                .ToArray();

            foreach (var property in properties)
                _backup.Add(property.Name, property.GetValue(this));

            IsInEditMode = true;
        }

        protected virtual void EndEdit()
        {
            _backup.Clear();
            IsInEditMode = false;

            FinishedEditing?.Invoke(Data);
        }

        protected virtual void CancelEdit()
        {
            var properties = GetType()
               .GetProperties()
               .Where(x => x.GetSetMethod() != null &&
                   x.GetSetMethod().IsPublic &&
                   x.Name != nameof(IsSelected))
               .ToArray();

            foreach (var backupItem in _backup)
                properties
                    .Single(x => x.Name == backupItem.Key)
                    .SetValue(this, backupItem.Value);

            _backup.Clear();
            IsInEditMode = false;
        }

        #endregion
    }
}
