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
    /// Binding.
    /// </summary>
    public abstract class Binding : IDisposable
    {
        private readonly List<PropertyPathObserver> _observers = new List<PropertyPathObserver>();
        private bool _disposed;

        /// <summary>
        /// Releases all resource used by the <see cref="Binding"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose()"/> when you are finished using the <see cref="VioletBind.Binding"/>. The
        /// <see cref="Dispose()"/> method leaves the <see cref="VioletBind.Binding"/> in an unusable state. After
        /// calling <see cref="Dispose()"/>, you must release all references to the <see cref="VioletBind.Binding"/> so
        /// the garbage collector can reclaim the memory that the <see cref="VioletBind.Binding"/> was occupying.</remarks>
        public void Dispose()
        {
            Dispose(true);
        }

        internal abstract void Set();

        /// <summary>
        /// Adds observers.
        /// </summary>
        /// <param name="observers">Observers.</param>
        protected void AddObservers(IEnumerable<PropertyPathObserver> observers)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(Binding));
            }

            foreach (var observer in observers)
            {
                _observers.Add(observer);
                observer.Changed += Observer_Changed;
            }
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
                    foreach (var observer in _observers.ToArray())
                    {
                        observer.Dispose();
                        _observers.Remove(observer);
                    }
                }

                _disposed = true;
            }
        }

        private void Observer_Changed(object sender, EventArgs e)
        {
            Set();
        }
    }
}
