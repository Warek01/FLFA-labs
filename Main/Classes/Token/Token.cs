namespace Main.classes.Token;

public class Token {
	public TokenType Type  { get; private set; }
	public string    Value { get; }

	public Token(TokenType type, string value) {
		Type  = type;
		Value = value;
	}

	public static Token None() {
		return new Token(TokenType.None, "");
	}

	public override string ToString() {
		return Value;
	}
}
