using Main.Classes;

namespace Main;

// V 10
// Eliminate ε productions
// Eliminate any renaming
// Eliminate inaccessible symbols
// Eliminate the non-productive symbols
// Obtain the Chomsky Normal Form

public static class Program {
	public static readonly string[] NonTerminalsSet = { "S", "A", "B", "D" };
	public static readonly string[] TerminalsSet    = { "a", "b", "d" };

	public static readonly KeyValuePair<string, string>[] ProductionsSet = {
		new("S", "dB"),
		new("S", "AB"),
		new("A", "d"),
		new("A", "dS"),
		new("A", "aAaAb"),
		new("A", "ε"),
		new("B", "a"),
		new("B", "aS"),
		new("B", "A"),
		new("D", "Aba"),
	};

	public static void Main() {
		Grammar grammar = new(NonTerminalsSet, TerminalsSet, ProductionsSet);
		Console.WriteLine(grammar);
		grammar.Normalize();
		Console.WriteLine(grammar);
	}
}
