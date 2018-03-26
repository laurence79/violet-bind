using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace VioletBind
{
    internal class Binding<TTarget, TIn> : Binding<TTarget>
    {
        private readonly Func<TTarget, TIn> _in;
        private readonly Action<TTarget, TIn> _setter;

        public Binding(Expression<Func<TTarget, TIn>> source, Action<TTarget, TIn> setter, TTarget target)
            : base(target)
        {
            _in = source.Compile();
            _setter = setter;
            Set();

            AddObservers(
                PropertyPaths<TTarget>.Get(source)
                    .Select(p => new PropertyPathObserver<TTarget>(p, target)));
        }

        internal override sealed void Set()
        {
            _setter(Target, _in(Target));
        }
    }
}
