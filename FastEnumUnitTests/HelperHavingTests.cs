using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace FastEnumUnitTests
{
    public abstract class HelperHavingTests
    {
        public ITestOutputHelper Helper { get; }

        protected HelperHavingTests(ITestOutputHelper helper) =>
            Helper = helper ?? throw new ArgumentNullException(nameof(helper));
    }

    public abstract class FixtureAndHelperHavingTests<TFixture> : HelperHavingTests, IClassFixture<TFixture>
        where TFixture : class
    {
        public TFixture Fixture { get; }

        protected FixtureAndHelperHavingTests(ITestOutputHelper helper, TFixture fixture) : base(helper) =>
            Fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
    }
}
