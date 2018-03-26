using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace VioletBind
{
    internal abstract class Binding<TTarget> : Binding
    {
        private readonly TTarget _target;

        protected Binding(TTarget target)
        {
            _target = target;
        }

        protected TTarget Target => _target;
    }
}