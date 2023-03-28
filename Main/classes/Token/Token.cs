namespace Main.classes.Token;

public class Token {
	public string Type;
	public string Literal;

	public Token(string t, string l) =>
		(Type, Literal) = (t, l);
}
