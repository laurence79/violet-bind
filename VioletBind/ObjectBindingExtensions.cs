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
    /// Object binding extensions.
    /// </summary>
    public static class ObjectBindingExtensions
    {
        private static ConditionalWeakTable<object, List<Binding>> _bindings = new ConditionalWeakTable<object, List<Binding>>();

        /// <summary>
        /// Bind the input values to the specified setter. Any changes to any properties on the input path will run the setter.
        /// </summary>
        /// <returns>The binding.</returns>
        /// <param name="target">Target.</param>
        /// <param name="in1">In1.</param>
        /// <param name="setter">Setter.</param>
        /// <typeparam name="TTarget">The 1st type parameter.</typeparam>
        /// <typeparam name="TIn">The 2nd type parameter.</typeparam>
        public static Binding Bind<TTarget, TIn>(
            this TTarget target,
            Expression<Func<TTarget, TIn>> in1,
            Action<TTarget, TIn> setter)
        {
            var binding = new Binding<TTarget, TIn>(in1, setter, target);
            _bindings.GetOrCreateValue(target).Add(binding);
            return binding;
        }

        /// <summary>
        /// Bind the input values to the specified setter. Any changes to any properties on the input path will run the setter.
        /// </summary>
        /// <returns>The bind.</returns>
        /// <param name="target">Target.</param>
        /// <param name="in1">In1.</param>
        /// <param name="in2">In2.</param>
        /// <param name="setter">Setter.</param>
        /// <typeparam name="TTarget">The 1st type parameter.</typeparam>
        /// <typeparam name="TIn1">The 2nd type parameter.</typeparam>
        /// <typeparam name="TIn2">The 3rd type parameter.</typeparam>
        public static Binding Bind<TTarget, TIn1, TIn2>(
            this TTarget target,
            Expression<Func<TTarget, TIn1>> in1,
            Expression<Func<TTarget, TIn2>> in2,
            Action<TTarget, TIn1, TIn2> setter)
        {
            var binding = new Binding<TTarget, TIn1, TIn2>(in1, in2, setter, target);
            _bindings.GetOrCreateValue(target).Add(binding);
            return binding;
        }

        /// <summary>
        /// Bind the input values to the specified setter. Any changes to any properties on the input path will run the setter.
        /// </summary>
        /// <returns>The bind.</returns>
        /// <param name="target">Target.</param>
        /// <param name="in1">In1.</param>
        /// <param name="in2">In2.</param>
        /// <param name="in3">In3.</param>
        /// <param name="setter">Setter.</param>
        /// <typeparam name="TTarget">The 1st type parameter.</typeparam>
        /// <typeparam name="TIn1">The 2nd type parameter.</typeparam>
        /// <typeparam name="TIn2">The 3rd type parameter.</typeparam>
        /// <typeparam name="TIn3">The 4th type parameter.</typeparam>
        public static Binding Bind<TTarget, TIn1, TIn2, TIn3>(
            this TTarget target,
            Expression<Func<TTarget, TIn1>> in1,
            Expression<Func<TTarget, TIn2>> in2,
            Expression<Func<TTarget, TIn3>> in3,
            Action<TTarget, TIn1, TIn2, TIn3> setter)
        {
            var binding = new Binding<TTarget, TIn1, TIn2, TIn3>(in1, in2, in3, setter, target);
            _bindings.GetOrCreateValue(target).Add(binding);
            return binding;
        }

        /// <summary>
        /// Disposes all bindings.
        /// </summary>
        /// <param name="target">Target.</param>
        /// <typeparam name="TTarget">The 1st type parameter.</typeparam>
        public static void DisposeAllBindings<TTarget>(this TTarget target)
        {
            if (_bindings.TryGetValue(target, out var bindings))
            {
                foreach (var binding in bindings)
                {
                    binding.Dispose();
                }

                _bindings.Remove(target);
            }
        }
    }
}
