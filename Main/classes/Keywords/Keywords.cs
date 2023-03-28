using Main.classes.Token;

namespace Main.classes.Keywords;

public static class Keywords {
	private static readonly Dictionary<string, string> KeywordsDict = new(new[] {
		new KeyValuePair<string, string>("fn",     TokenType.Function),
		new KeyValuePair<string, string>("let",    TokenType.Let),
		new KeyValuePair<string, string>("true",   TokenType.True),
		new KeyValuePair<string, string>("false",  TokenType.False),
		new KeyValuePair<string, string>("return", TokenType.Return),
		new KeyValuePair<string, string>("if",     TokenType.If),
		new KeyValuePair<string, string>("else",   TokenType.Else),
	});

	public static string LookupIdent(string ident) {
		return KeywordsDict.ContainsKey(ident) ? KeywordsDict[ident] : TokenType.Ident;
	}
}
