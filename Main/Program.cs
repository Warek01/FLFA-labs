using Main.classes;

namespace Main;

public static class Program {
	public static void Main() {
		Grammar grammar = new(
			new List<char>(new[] { 'a', 'b', 'c' }),
			4,
			new List<int>(new[] { 3 }),
			new List<Transition>(
				new Transition[] {
					new(0, 'a', 1),
					new(1, 'b', 1),
					new(1, 'c', 2),
					new(2, 'a', 0),
					new(2, 'c', 2),
					new(2, 'b', 3)
				}
			)
		);

		FiniteAutomaton automaton = grammar.ToFiniteAutomaton();

		for (int i = 0; i < 10; i++) {
			var str = grammar.GenerateString();
			Console.WriteLine($"{str} - {automaton.StringBelongToLanguage(str)}");
		}
	}
}
