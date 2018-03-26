using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace VioletBind.UnitTests
{
    public class PropertyPathTests
    {
        [Fact]
        public void ReducesLikePaths()
        {
            var paths = PropertyPaths<Outer>.Get(o => o.Inner.Qty > 6 && o.Inner.Qty < 10);
            var reduced = PropertyPath.Reduce(paths);

            Assert.Equal(1, reduced.Count);
            Assert.Equal("Inner.Qty", reduced.Single().ToString());
        }

        [Fact]
        public void InterpretsPropertyPathsCorrectly()
        {
            var paths = PropertyPaths<Outer>.Get<int>(o => o.Inner != null ? o.Inner.Qty : 0);

            Assert.Equal(2, paths.Count);
            Assert.Contains("Inner", paths.Select(p => p.ToString()));
            Assert.Contains("Inner.Qty", paths.Select(p => p.ToString()));
        }

        public class Outer
        {
            public Inner Inner { get; set; }
        }

        public class Inner
        {
            public int Qty { get; set; }
        }
    }
}
