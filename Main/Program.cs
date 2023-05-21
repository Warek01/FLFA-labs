using Main.Classes.Exceptions.InvalidSyntax;
using Main.classes.Graph;
using Main.Classes.LispFormBuilder;
using Main.classes.Parser;
using Main.classes.ValueBuilder;

Console.WriteLine("Input expression, for example 1 + 2 * 3 / (4 - 5)");
Console.WriteLine("Input Q to exit");

while (true) {
	Console.Write("\nExpression: ");
	var input = Console.ReadLine();

	if (input == "Q") {
		break;
	}

	try {
		if (input is null) {
			continue;
		}

		var parser     = new Parser(input);
		var expression = parser.Parse();
		var graph      = expression.Accept(new Graph());
		var value      = expression.Accept(new ValueBuilder());
		var lispForm   = expression.Accept(new LispFormBuilder());


		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine(value);
		Console.ResetColor();
		Console.WriteLine(graph);
		Console.WriteLine(lispForm);
	}
	catch (InvalidSyntaxException ex) {
		Console.WriteLine(ex.Message);
	}
	catch (Exception ex) {
		Console.WriteLine(ex);
	}
}
