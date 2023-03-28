namespace Main.classes.Token; 

public static class TokenType {
	public const string Illegal = "ILLEGAL";
	public const string Eof     = "EOF";

	// Identifiers + literals
	public const string Ident = "IDENT";
	public const string Int   = "INT";

	// Operators
	public const string Plus   = "+";
	public const string Minus  = "-";
	public const string Assign = "=";
	public const string Slash  = "/";
	public const string Ast    = "*";
	public const string Lt     = "<";
	public const string Gt     = ">";
	public const string Eq     = "==";
	public const string Ne     = "!=";
	public const string Bang   = "!";

	// Delimiters
	public const string Comma     = ",";
	public const string Semicolon = ";";
	public const string Lparen    = "(";
	public const string Rparen    = ")";
	public const string Lbrace    = "{";
	public const string Rbrace    = "}";

	// Keywords
	public const string Function = "FUNCTION";
	public const string Let      = "LET";
	public const string Return   = "RETURN";
	public const string If       = "IF";
	public const string Else     = "ELSE";
	public const string True     = "TRUE";
	public const string False    = "FALSE";
}