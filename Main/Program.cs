namespace Main;

public static class Program {
	public static readonly string[] Vn = { "S", "B", "L" };
	public static readonly string[] Vt = { "a", "b", "c" };

	public static readonly string[,] P = {
		{ "S", "aB" },
		{ "B", "bB" },
		{ "B", "cL" },
		{ "L", "cL" },
		{ "L", "aS" },
		{ "L", "b" }
	};

	public static void Main() {
		Grammar grammar = new(Vn, Vt, P, "S");

		for (int i = 0; i < 20; i++) {
			Console.WriteLine(grammar.GenerateString());
		}
	}
}
