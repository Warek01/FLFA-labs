using Main.classes.Token;

namespace Main.classes.Lexer;

public partial class Lexer {
	public Token.Token NextToken() {
		Token.Token token = new(TokenType.Illegal, _ch.ToString());

		_skipWhitespace();

		switch (_ch) {
			case (char)0:
				token = new(TokenType.Eof, "");
				break;
			case '=':
				if (_peekChar() == '=') {
					char ch = _ch;
					_readChar();
					token = new(TokenType.Eq, $"{ch}{_ch}");
				}
				else {
					token = new(TokenType.Assign, _ch.ToString());
				}

				break;
			case '+':
				token = new(TokenType.Plus, _ch.ToString());
				break;
			case '-':
				token = new(TokenType.Minus, _ch.ToString());
				break;
			case '(':
				token = new(TokenType.Lparen, _ch.ToString());
				break;
			case ')':
				token = new(TokenType.Rparen, _ch.ToString());
				break;
			case '{':
				token = new(TokenType.Lbrace, _ch.ToString());
				break;
			case '}':
				token = new(TokenType.Rbrace, _ch.ToString());
				break;
			case ',':
				token = new(TokenType.Comma, _ch.ToString());
				break;
			case ';':
				token = new(TokenType.Semicolon, _ch.ToString());
				break;
			case '/':
				token = new(TokenType.Slash, _ch.ToString());
				break;
			case '*':
				token = new(TokenType.Ast, _ch.ToString());
				break;
			case '>':
				token = new(TokenType.Gt, _ch.ToString());
				break;
			case '<':
				token = new(TokenType.Lt, _ch.ToString());
				break;
			case '!':
				if (_peekChar() == '=') {
					char ch = _ch;
					_readChar();
					token = new(TokenType.Ne, $"{ch}{_ch}");
				}
				else {
					token = new(TokenType.Bang, _ch.ToString());
				}

				break;
			default:
				if (IsOnLetter) {
					string literal = _readIdentifier();
					token = new(Keywords.Keywords.LookupIdent(literal), literal);
				}
				else if (IsOnDigit) {
					token = new(TokenType.Int, _readNumber());
					return token;
				}

				break;
		}

		_readChar();

		return token;
	}
}
