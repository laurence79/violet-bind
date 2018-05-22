using System;
using System.ComponentModel;
using Xunit;

namespace VioletBind.UnitTests
{
    public class BindingTests
    {
        [Fact]
        public void Bindings_Works()
        {
            using (var thing = new Thing())
            {
                thing.Count1 = 10;
                thing.Count2 = 20;

                Assert.Equal(30, thing.Total);
            }
        }

        [Fact]
        public void Bindings_SubWorks()
        {
            using (var thing = new Thing2())
            {
                thing.Subthing1.Quantity = 10;
                thing.Subthing2.Quantity = 20;

                Assert.Equal(30, thing.Total);
            }
        }
    }

    public class Thing : IDisposable, INotifyPropertyChanged
    {
        public int Count1 { get; set; }
        public int Count2 { get; set; }
        public int Total { get; set; }

        public Thing()
        {
            this.Bind(
                in1: v => v.Count1,
                in2: v => v.Count2,
                setter: (v, in1, in2) =>
            {
                v.Total = in1 + in2;
            });
        }
        /*
         * Automatically implemented by Fody.PropertyChanged
         * See https://github.com/Fody/PropertyChanged
         */
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        public void Dispose()
        {
            this.DisposeAllBindings();
        }
    }

    public class Thing2 : IDisposable, INotifyPropertyChanged
    {
        public SubThing Subthing1 { get; set; }
        public SubThing Subthing2 { get; set; }
        public int Total { get; set; }

        public Thing2()
        {
            Subthing1 = new SubThing();
            Subthing2 = new SubThing();

            this.Bind(
                in1: v => v.Subthing1.Quantity,
                in2: v => v.Subthing2.Quantity,
                setter: (v, in1, in2) =>
                {
                    v.Total = in1 + in2;
                });
        }
        /*
         * Automatically implemented by Fody.PropertyChanged
         * See https://github.com/Fody/PropertyChanged
         */
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        public void Dispose()
        {
            this.DisposeAllBindings();
        }
    }

    public class SubThing : INotifyPropertyChanged
    {
        public int Quantity { get; set; }

        /*
         * Automatically implemented by Fody.PropertyChanged
         * See https://github.com/Fody/PropertyChanged
         */
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067
    }
}
