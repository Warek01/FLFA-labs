using Main.Classes;
using Main.Classes.Exceptions.InvalidSyntax;
using Main.Classes.Num;
using Main.Classes.Operations.BinaryOperation;
using Main.Classes.Operations.UnaryOperation;
using Main.classes.Token;

namespace Main.classes.Parser;

internal class Parser {
	private Token.Token _curToken;
	private int         _curPos;
	private int         _charCount;
	private char        _curChar;
	public  string?     Text { get; }

	public Parser(string? text) {
		Text       = string.IsNullOrEmpty(text) ? string.Empty : text;
		_charCount = Text.Length;
		_curToken  = Token.Token.None();

		_curPos = -1;
		Advance();
	}

	internal Expression.Expression Parse() {
		NextToken();
		Expression.Expression node = GrabExpr();
		ExpectToken(TokenType.None);
		return node;
	}

	private Token.Token ExpectToken(TokenType tokenType) {
		if (_curToken.Type == tokenType) {
			return _curToken;
		}

		throw new InvalidSyntaxException(
			$"Invalid syntax at position {_curPos}. Expected {tokenType} but {_curToken.Type.ToString()} is given."
		);
	}

	private Expression.Expression GrabExpr() {
		Expression.Expression left = GrabTerm();

		while (_curToken.Type is TokenType.Plus or TokenType.Minus) {
			Token.Token op = _curToken;
			NextToken();
			Expression.Expression right = GrabTerm();
			left = new BinOp(op, left, right);
		}

		return left;
	}

	private Expression.Expression GrabTerm() {
		Expression.Expression left = GrabFactor();

		while (_curToken.Type is TokenType.Multiply or TokenType.Divide) {
			Token.Token op = _curToken;

			NextToken();

			Expression.Expression right = GrabFactor();
			left = new BinOp(op, left, right);
		}

		return left;
	}

	private Expression.Expression GrabFactor() {
		switch (_curToken.Type) {
			case TokenType.Plus or TokenType.Minus: {
				Expression.Expression node = GrabUnaryExpr();
				return node;
			}
			case TokenType.LeftParenthesis: {
				Expression.Expression node = GrabBracketExpr();
				return node;
			}
			default: {
				Token.Token token = ExpectToken(TokenType.Number);
				NextToken();
				return new Num(token);
			}
		}
	}

	private Expression.Expression GrabUnaryExpr() {
		Token.Token op;

		op = ExpectToken(_curToken.Type == TokenType.Plus ? TokenType.Plus : TokenType.Minus);

		NextToken();

		if (_curToken.Type is TokenType.Plus or TokenType.Minus) {
			Expression.Expression expr = GrabUnaryExpr();
			return new UnaryOp(op, expr);
		}
		else {
			Expression.Expression expr = GrabFactor();
			return new UnaryOp(op, expr);
		}
	}

	private Expression.Expression GrabBracketExpr() {
		ExpectToken(TokenType.LeftParenthesis);
		NextToken();

		Expression.Expression node = GrabExpr();

		ExpectToken(TokenType.RightParenthesis);
		NextToken();

		return node;
	}

	private void NextToken() {
		switch (_curChar) {
			case char.MinValue:
				_curToken = Token.Token.None();
				return;
			case ' ': {
				while (_curChar != char.MinValue && _curChar == ' ') {
					Advance();
				}

				if (_curChar == char.MinValue) {
					_curToken = Token.Token.None();
					return;
				}

				break;
			}
		}

		switch (_curChar) {
			case '+':
				_curToken = new Token.Token(TokenType.Plus, _curChar.ToString());
				Advance();
				return;
			case '-':
				_curToken = new Token.Token(TokenType.Minus, _curChar.ToString());
				Advance();
				return;
			case '*':
				_curToken = new Token.Token(TokenType.Multiply, _curChar.ToString());
				Advance();
				return;
			case '/':
				_curToken = new Token.Token(TokenType.Divide, _curChar.ToString());
				Advance();
				return;
			case '(':
				_curToken = new Token.Token(TokenType.LeftParenthesis, _curChar.ToString());
				Advance();
				return;
			case ')':
				_curToken = new Token.Token(TokenType.RightParenthesis, _curChar.ToString());
				Advance();
				return;
			case >= '0' and <= '9': {
				string num = string.Empty;
				while (_curChar >= '0' && _curChar <= '9') {
					num += _curChar.ToString();
					Advance();
				}

				if (_curChar == '.') {
					num += _curChar.ToString();
					Advance();

					if (_curChar >= '0' && _curChar <= '9') {
						while (_curChar >= '0' && _curChar <= '9') {
							num += _curChar.ToString();
							Advance();
						}
					}
					else {
						throw new InvalidSyntaxException(
							$"Invalid syntax at position {_curPos}. Unexpected symbol {_curChar}.");
					}
				}

				_curToken = new Token.Token(TokenType.Number, num);
				return;
			}
			default:
				throw new InvalidSyntaxException(
					$"Invalid syntax at position {_curPos}. Unexpected symbol {_curChar}.");
		}
	}

	private void Advance() {
		_curPos  += 1;
		_curChar =  _curPos < _charCount ? Text[_curPos] : char.MinValue;
	}
}
