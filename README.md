# violet-bind
C# Bindings over complex property paths

Example usage, better documentation to follow.

```
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using VioletBind;

namespace Example
{
    internal class ViewModel : INotifyPropertyChanged, IDisposable
    {
        private bool _disposed;

        public ViewModel()
        {
            this.Bind(
                in1: t => t.CompletedItems.Count,
                in2: t => t.Items.Count,
                setter: (target, completedCount, itemCount) =>
            {
                target.IsComplete = statusCode != null && statusCode != "NEW";
                target.CanComplete = itemCount > 0 && completedCount == itemCount && statusCode == "NEW";
                target.IsInProgress = completedCount != itemCount && statusCode == "NEW";
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<ItemViewModel> Items { get; private set; }

        public ObservableCollection<ItemViewModel> CompletedItems { get; private set; }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    this.DisposeAllBindings();
                }

                _disposed = true;
            }
        }
    }
}
```