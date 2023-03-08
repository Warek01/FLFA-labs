using Main;
using Main.classes;
using Xunit;

namespace Tests;

public static class UnitTest {
	private static readonly Grammar Grammar = new(
		Program.Alphabet, 
		Program.NrOfStates, 
		Program.FinalStates,
		Program.Transitions
		);

	private static readonly FiniteAutomaton FiniteAutomaton = Grammar.ToFiniteAutomaton();

	[Fact]
	private static void TestGrammar() {
		for (var i = 0; i < 50; i++) {
			var str = Grammar.GenerateString();

			Assert.NotNull(str);
			Assert.NotEmpty(str);
			Assert.IsType<string>(str);
		}
	}

	[Theory]
	[InlineData("bcabbcabbcabcabc",    true)]
	[InlineData("ee",      false)]
	[InlineData("asfsdgf", false)]
	[InlineData("112345",  false)]
	private static void TestAutomatonPredefined(string str, bool expected) {
		Assert.Equal(expected, FiniteAutomaton.StringBelongToLanguage(str));
	}

	[Fact]
	private static void TestAutomatonGenerated() {
		for (int i = 0; i < 50; i++) {
			Assert.True(
				FiniteAutomaton.StringBelongToLanguage(
					Grammar.GenerateString()
				)
			);
		}
	}
}
