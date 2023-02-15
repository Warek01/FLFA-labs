namespace Main;

public class Grammar {
	private List<string>       _Vn;
	private List<string>       _Vt;
	private List<List<string>> _P;
	private string             _S;

	private bool _isTerminal(string s) {
		return _Vt.Contains(s);
	}

	private bool _isNonTerminal(string s) {
		return _Vn.Contains(s);
	}

	public Grammar(string[] Vn, string[] Vt, string[,] P, string S) {
		_Vn = new(Vn);
		_Vt = new(Vt);
		_S  = S;

		_P = new();

		for (int i = 0; i < P.Length / 2; i++) {
			var rule = new List<string> {
				P[i, 0],
				P[i, 1]
			};

			_P.Add(rule);
		}
	}

	public string GenerateString() {
		var    rand          = new Random();
		string str           = _S;
		bool   shouldIterate = true;

		while (shouldIterate) {
			shouldIterate = false;

			foreach (char character in str) {
				if (_isNonTerminal(character.ToString()) || character == 'S') {
					shouldIterate = true;

					var matches = new List<string>();

					foreach (var ls in _P) {
						if (ls[0].Contains(character.ToString())) {
							matches.Add(ls[1]);
						}
					}

					str = str.Replace(
						character.ToString(),
						matches[rand.Next(0, matches.Count)]
					);
				}
			}
		}

		return str;
	}

	public FiniteAutomaton ToFiniteAutomaton() {
		var p = new List<KeyValuePair<char, string>>();

		foreach (var prod in _P) {
			p.Add(new KeyValuePair<char, string>(prod[0][0], prod[1]));
		}

		return new FiniteAutomaton(_Vt.Select(t => t[0]).ToList(), _Vn, p);
	}
}
