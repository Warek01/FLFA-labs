using Main.classes;
using Main.classes.Lexer;
using Main.classes.Token;

namespace Main;

public static class Program {
	public static void Main() {
		Lexer lexer = new(@"
			let a = 123;
			func test () {}
		");
		
		while (true) {
			Token token = lexer.NextToken();
		
			Console.WriteLine($"{token.Type} {token.Literal}");
			
			if (token.Type == TokenType.Eof) {
				break;
			} 
		}
	}
}
