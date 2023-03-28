namespace Main.classes.Lexer;

public partial class Lexer {
	private string _input;
	private int    _position     = 0;
	private int    _readPosition = 0;
	private char   _ch;

	private bool IsOnLetter => _ch is >= 'a' and <= 'z' or >= 'A' and <= 'Z' or '_';
	private bool IsOnDigit  => _ch is >= '0' and <= '9';

	public Lexer(string input) {
		_input = input;

		_readChar();
	}

	private void _readChar() {
		if (_readPosition >= _input.Length) {
			_ch = (char)0;
		}
		else {
			_ch = _input[_readPosition];
		}

		_position = _readPosition;
		_readPosition++;
	}

	private string _readIdentifier() {
		int pos = _position;

		while (IsOnLetter) {
			_readChar();
		}

		return _input.Substring(pos, _position - pos);
	}

	private void _skipWhitespace() {
		while (_ch is ' ' or '\t' or '\n' or '\r') {
			_readChar();
		}
	}

	private string _readNumber() {
		int pos = _position;

		while (IsOnDigit) {
			_readChar();
		}

		return _input.Substring(pos, _position - pos);
	}

	private char _peekChar() {
		if (_position > _input.Length) {
			return (char)0;
		}

		return _input[_readPosition];
	}
}
