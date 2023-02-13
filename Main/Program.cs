namespace Main;

public static class Program {
	public static string[] Vn = { "S", "B", "L" };
	public static string[] Vt = { "a", "b", "c" };

	public static string[,] P = {
		{ "S", "aB" },
		{ "B", "bB" },
		{ "B", "cL" },
		{ "L", "cL" },
		{ "L", "aS" },
		{ "L", "b" }
	};

	public static void Main(string[] args) {
		var grammar = new Grammar(Vn, Vt, P, "S");

		Console.WriteLine(grammar.GenerateString());
		Console.WriteLine(grammar.GenerateString());
		Console.WriteLine(grammar.GenerateString());
		Console.WriteLine(grammar.GenerateString());
		Console.WriteLine(grammar.GenerateString());
	}
}
