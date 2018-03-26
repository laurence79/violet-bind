using System;

namespace VioletBind
{
    /// <summary>
    /// Property path observer.
    /// </summary>
    public abstract class PropertyPathObserver : IDisposable
    {
        /// <summary>
        /// Occurs when a property in the path changes.
        /// </summary>
        public abstract event EventHandler Changed;

        /// <summary>
        /// Releases all resource used by the <see cref="VioletBind.PropertyPathObserver"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="VioletBind.PropertyPathObserver"/>.
        /// The <see cref="Dispose"/> method leaves the <see cref="VioletBind.PropertyPathObserver"/> in an unusable
        /// state. After calling <see cref="Dispose"/>, you must release all references to the
        /// <see cref="VioletBind.PropertyPathObserver"/> so the garbage collector can reclaim the memory that the
        /// <see cref="VioletBind.PropertyPathObserver"/> was occupying.</remarks>
        public abstract void Dispose();
    }
}
