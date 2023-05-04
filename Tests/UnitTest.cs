using Xunit;
using Xunit.Abstractions;

namespace Tests;

public class UnitTest {
	private readonly ITestOutputHelper _testOutputHelper;

	public UnitTest(ITestOutputHelper testOutputHelper) {
		_testOutputHelper = testOutputHelper;
	}

	[Fact]
	private void TestInput() { }
}
