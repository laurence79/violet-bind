using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace VioletBind.UnitTests
{
    public class PropertyPathObserverTests
    {
        private const string kToken = "123456";

        private readonly ITestOutputHelper output;

        public PropertyPathObserverTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void ObservesIntermediatePath()
        {
            var obj = new Body();
            var path = PropertyPaths<Body>.Get<int>(t => t.LeftArm.Thumb.LengthMM).Single();
            var observer = new PropertyPathObserver<Body>(path, obj);

            int changesObserved = 0;

            observer.Changed += (sender, e) =>
            {
                changesObserved++;
            };

            obj.LeftArm = new Arm();

            Assert.Equal(1, changesObserved);
        }

        [Fact]
        public void ObservesEndOfPath()
        {
            var obj = new Body { LeftArm = new Arm { Thumb = new Digit { LengthMM = 50 } } };

            var path = PropertyPaths<Body>.Get<int>(t => t.LeftArm.Thumb.LengthMM).Single();
            var observer = new PropertyPathObserver<Body>(path, obj);

            int changesObserved = 0;

            observer.Changed += (sender, e) =>
            {
                changesObserved++;
            };

            obj.LeftArm.Thumb.LengthMM = 51;

            Assert.Equal(1, changesObserved);
        }

        [Fact]
        public void IgnoresOphanedObject()
        {
            var obj = new Body { LeftArm = new Arm { Thumb = new Digit { LengthMM = 50 } } };

            var path = PropertyPaths<Body>.Get<int>(t => t.LeftArm.Thumb.LengthMM).Single();
            var observer = new PropertyPathObserver<Body>(path, obj);

            var oldThumb = obj.LeftArm.Thumb;

            obj.LeftArm.Thumb = new Digit();

            int changesObserved = 0;

            observer.Changed += (sender, e) =>
            {
                changesObserved++;
            };

            oldThumb.LengthMM = 51;

            Assert.Equal(0, changesObserved);
        }

        [Fact]
        public void ObservesAdoptedObject()
        {
            var obj = new Body { LeftArm = new Arm { Thumb = new Digit { LengthMM = 50 } } };

            var path = PropertyPaths<Body>.Get<int>(t => t.LeftArm.Thumb.LengthMM).Single();
            var observer = new PropertyPathObserver<Body>(path, obj);

            obj.LeftArm.Thumb = new Digit();

            int changesObserved = 0;

            observer.Changed += (sender, e) =>
            {
                changesObserved++;
            };

            obj.LeftArm.Thumb.LengthMM = 51;

            Assert.Equal(1, changesObserved);
        }

        [Fact]
        public void StopsOnDispose()
        {
            var obj = new Body { LeftArm = new Arm { Thumb = new Digit { LengthMM = 50 } } };

            var path = PropertyPaths<Body>.Get<int>(t => t.LeftArm.Thumb.LengthMM).Single();
            var observer = new PropertyPathObserver<Body>(path, obj);

            int changesObserved = 0;

            observer.Changed += (sender, e) =>
            {
                changesObserved++;
            };

            observer.Dispose();

            obj.LeftArm.Thumb.LengthMM = 51;

            Assert.Equal(0, changesObserved);
        }

        public class Body : INotifyPropertyChanged
        {
            public Arm LeftArm { get; set; }
            public Arm RightArm { get; set; }

            /*
             * Automatically implemented by Fody.PropertyChanged
             * See https://github.com/Fody/PropertyChanged
             */
#pragma warning disable 0067
            public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067
        }

        public class Arm : INotifyPropertyChanged
        {
            public Digit Thumb { get; set; }
            public Digit IndexFinger { get; set; }

            /*
             * Automatically implemented by Fody.PropertyChanged
             * See https://github.com/Fody/PropertyChanged
             */
#pragma warning disable 0067
            public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067
        }

        public class Digit : INotifyPropertyChanged
        {
            public int LengthMM { get; set; }

            /*
             * Automatically implemented by Fody.PropertyChanged
             * See https://github.com/Fody/PropertyChanged
             */
#pragma warning disable 0067
            public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067
        }
    }
}
