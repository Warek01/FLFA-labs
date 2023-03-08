using Main.classes;
using System.Diagnostics;

namespace Main;

public static class Program {
	public static readonly List<Transition> Transitions = new(
		new Transition[] {
			new(0, "a", 1),
			new(1, "b", 2),
			new(2, "c", 3),
			new(3, "a", 1),
			new(1, "b", 1),
			new(0, "b", 2)
		});

	public static readonly List<char> Aphabet     = new(new[] { 'a', 'b', 'c' });
	public const           int        NrOfStates  = 4;
	public static readonly List<int>  FinalStates = new(new[] { 3 });

	public static void Main() {
		Grammar grammar = new(
			Aphabet,
			NrOfStates,
			FinalStates,
			Transitions
		);

		for (var i = 0; i < 20; i++) {
			Console.WriteLine(grammar.GenerateString());
		}

		DrawGraph();
	}


	// Draws the automaton diagram using a python library and saves it to file
	private static void DrawGraph() {
		try {
			var dataset = Transitions.Aggregate(
				"",
				(current, transition) => current + $" {transition.From} {transition.To} {transition.Value}"
			);

			ProcessStartInfo info = new() {
				FileName               = "python3",
				Arguments              = $"/home/warek/RiderProjects/FLFA-Labs/main.py {dataset}",
				RedirectStandardOutput = false,
				RedirectStandardError  = false,
				RedirectStandardInput  = false,
				UseShellExecute        = false,
				CreateNoWindow         = true
			};

			using Process process = new() {
				StartInfo = info
			};

			process.Start();
			process.WaitForExit();
		}
		catch (Exception exception) {
			Console.WriteLine("Something went wrong:");
			Console.WriteLine(exception.Message);
		}
	}
}
