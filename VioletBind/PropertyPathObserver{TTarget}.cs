using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace VioletBind
{
    /// <summary>
    /// Property path observer.
    /// </summary>
    /// <typeparam name="TTarget">The type which starts the property path</typeparam>
    public class PropertyPathObserver<TTarget> : PropertyPathObserver
    {
        private readonly IReadOnlyList<PropertyInfo> _propertyPath;
        private readonly TTarget _target;
        private readonly Dictionary<string, INotifyPropertyChanged> _observedObjects;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="VioletBind.PropertyPathObserver{TTarget}"/> class.
        /// </summary>
        /// <param name="propertyPath">Property path.</param>
        /// <param name="target">Target.</param>
        public PropertyPathObserver(PropertyPath propertyPath, TTarget target)
        {
            _propertyPath = propertyPath;
            _target = target;
            _observedObjects = new Dictionary<string, INotifyPropertyChanged>();

            ObserveFromIndex(0);
        }

        /// <summary>
        /// Occurs when a property in the path changes.
        /// </summary>
        public override event EventHandler Changed;

        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <value>The target.</value>
        public TTarget Target { get => _target; }

        /// <summary>
        /// Releases all resource used by the <see cref="VioletBind.PropertyPathObserver{TTarget}"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose()"/> when you are finished using the
        /// <see cref="VioletBind.PropertyPathObserver{TTarget}"/>. The <see cref="Dispose()"/> method leaves the
        /// <see cref="VioletBind.PropertyPathObserver{TTarget}"/> in an unusable state. After calling <see cref="Dispose()"/>,
        /// you must release all references to the <see cref="VioletBind.PropertyPathObserver{TTarget}"/> so the garbage
        /// collector can reclaim the memory that the <see cref="VioletBind.PropertyPathObserver{TTarget}"/> was occupying.</remarks>
        public override void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    foreach (var kvp in _observedObjects.ToArray())
                    {
                        EndObservingAtPath(kvp.Key);
                        _observedObjects.Remove(kvp.Key);
                    }
                }

                _disposed = true;
            }
        }

        private void ObserveFromIndex(int index)
        {
            object currentValue = GetObjectInPropertyPathAtIndex(index);
            var path = string.Join(".", _propertyPath.Take(index).Select(p => p.Name));

            foreach (var property in _propertyPath.Skip(index))
            {
                // check if we have an existing observer at this path
                EndObservingAtPath(path);

                if (currentValue == null)
                {
                    break;
                }

                if (currentValue is INotifyPropertyChanged p)
                {
                    BeginObservingAtPath(path, p);
                }

                path += $"{(path != string.Empty ? "." : string.Empty)}{property.Name}";
                currentValue = property.GetValue(currentValue);
            }
        }

        private void BeginObservingAtPath(string path, INotifyPropertyChanged obj)
        {
            _observedObjects[path] = obj;
            obj.PropertyChanged += P_PropertyChanged;
        }

        private void EndObservingAtPath(string path)
        {
            if (_observedObjects.TryGetValue(path, out var existing))
            {
                existing.PropertyChanged -= P_PropertyChanged;
                _observedObjects.Remove(path);
            }
        }

        private object GetObjectInPropertyPathAtIndex(int index)
        {
            object currentValue = _target;
            for (int i = 0; i < index; i++)
            {
                currentValue = _propertyPath[i].GetValue(currentValue);
                if (currentValue == null)
                {
                    break;
                }
            }

            return currentValue;
        }

        private void P_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var index = FindIndexOfObjectInPropertyPath(sender);

            if (e.PropertyName == _propertyPath[index.Value].Name)
            {
                ObserveFromIndex(index.Value + 1);
                Changed?.Invoke(this, new EventArgs());
            }
        }

        private int? FindIndexOfObjectInPropertyPath(object o)
        {
            object leaf = _target;
            int index = 0;

            foreach (var property in _propertyPath)
            {
                if (leaf == o)
                {
                    return index;
                }

                leaf = property.GetValue(leaf);
                index++;

                if (leaf == null)
                {
                    break;
                }
            }

            return null;
        }
    }
}
