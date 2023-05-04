namespace Main.Classes;

public class Grammar {
	private List<string>                             _nonTerminals;
	private List<string>                             _terminals;
	private List<KeyValuePair<string, List<string>>> _productions = new();

	public Grammar(string[] nonTerminals, string[] terminals, KeyValuePair<string, string>[] productions) {
		_nonTerminals = new(nonTerminals);
		_terminals    = new(terminals);

		foreach (var pair in productions) {
			var prod = _productions.Find(
				p => p.Key == pair.Key
			);

			if (prod.Key is null) {
				_productions.Add(
					new KeyValuePair<string, List<string>>(
						pair.Key, new List<string>(new[] { pair.Value })
					)
				);
			}
			else {
				prod.Value.Add(pair.Value);
			}
		}
	}

	public void Normalize() {
		_eliminateEpsProductions();
		_eliminateRenaming();
		_eliminateInaccessibleSymbols();
		_eliminateNonProductiveSymbols();
		_eliminateUnusedNonTerminals();
		_eliminateUnusedTerminals();
		_eliminateDuplicateProd();
	}

	public override string ToString() {
		string str = "V_t = { ";

		foreach (var terminal in _terminals) {
			str += $"{terminal}{(terminal != _terminals.Last() ? "," : "")} ";
		}

		str += "}\nV_n = { ";

		foreach (var nonTerminal in _nonTerminals) {
			str += $"{nonTerminal}{(nonTerminal != _nonTerminals.Last() ? "," : "")} ";
		}

		str += "}\nP = {\n";

		foreach (var prod in _productions) {
			str += $"\t{prod.Key} → ";

			foreach (var dest in prod.Value) {
				str += $"{dest}{(dest != prod.Value.Last() ? "|" : "")}";
			}

			str += "\n";
		}

		str += "}\n";

		return str;
	}

	private void _eliminateEpsProductions() {
		List<string> prodsToFix = new();

		foreach (var prod in _productions) {
			bool prodContainsEps = false;
			foreach (var transition in prod.Value) {
				if (transition == "ε") {
					prodContainsEps = true;
				}
			}

			if (!prodContainsEps) {
				continue;
			}

			prod.Value.Remove("ε");
			prodsToFix.Add(prod.Key);
		}

		foreach (var prodToFix in prodsToFix) {
			foreach (var prod in _productions) {
				if (prod.Key == prodToFix) {
					continue;
				}

				List<string> prodsToAdd = new();

				foreach (var transition in prod.Value) {
					if (!transition.Contains(prodToFix)) {
						continue;
					}

					foreach (var transToFix in _productions.Find(p => p.Key == prodToFix).Value) {
						prodsToAdd.Add(transToFix);
					}
				}

				prod.Value.AddRange(prodsToAdd);
			}
		}
	}

	private void _eliminateRenaming() {
		foreach (var prod in _productions) {
			List<string> prodsToFix = new();

			foreach (var transition in prod.Value) {
				if (transition.Length == 1 && _nonTerminals.Contains(transition)) {
					prodsToFix.Add(transition);
				}
			}

			foreach (var transition in prodsToFix) {
				prod.Value.Remove(transition);
				prod.Value.AddRange(_productions.Find(p => p.Key == transition).Value);
			}
		}
	}

	private void _eliminateInaccessibleSymbols() {
		HashSet<string> visited = new();
		List<string>    queue   = new();

		void Visit(string key, int depth) {
			if (depth == _productions.Count) {
				return;
			}

			visited.Add(key);
			var current = _productions.Find(p => p.Key == key);

			foreach (var transition in current.Value) {
				foreach (var ch in transition) {
					if (_nonTerminals.Contains(ch.ToString())) {
						Visit(ch.ToString(), depth + 1);
					}
				}
			}
		}

		Visit("S", 0);

		for (int i = 0; i < _productions.Count - visited.Count; i++) {
			_productions.RemoveAt(
				_productions.FindIndex(
					p => !visited.Contains(p.Key)
				)
			);
		}
	}

	private void _eliminateNonProductiveSymbols() {
		var newNonTerminals = _cloneNonTerminals();
		var newProd         = _cloneProductions();

		for (int i = 0; i < _productions.Count; i++) {
			var pair = _productions[i];

			for (int j = 0; j < pair.Value.Count; j++) {
				var transition = _productions[i].Value[j];

				if (transition.Length == 1) {
					continue;
				}

				foreach (var ch in transition) {
					if (!_terminals.Contains(ch.ToString())) {
						continue;
					}

					string? key = newProd.Find(
						p => p.Value.Find(a => a == ch.ToString()) is not null
					).Key;


					if (key is null) {
						string nonTerminal = _generateNotContaining(newNonTerminals);
						newNonTerminals.Add(nonTerminal);
						newProd.Add(
							new KeyValuePair<string, List<string>>(nonTerminal, new(new[] { ch.ToString() }))
						);

						newProd[i].Value[j] = newProd[i].Value[j].Replace(ch.ToString(), nonTerminal);
					}
					else {
						newProd[i].Value[j] =
							newProd[i].Value[j].Replace(ch.ToString(), key);
					}
				}
			}
		}

		_nonTerminals = newNonTerminals;
		_productions  = newProd;
	}

	private string _generateNotContaining(List<string> list) {
		int min = Convert.ToInt32('A');
		int max = Convert.ToInt32('Z');

		for (int i = min; i < max; i++) {
			string strValue = Convert.ToChar(i).ToString();

			if (list.Contains(strValue)) {
				continue;
			}

			return strValue;
		}

		return Convert.ToChar(max).ToString();
	}

	private void _eliminateUnusedNonTerminals() {
		HashSet<string> used = new();

		foreach (var pair in _productions) {
			foreach (var transition in pair.Value) {
				foreach (var ch in transition) {
					if (_nonTerminals.Contains(ch.ToString())) {
						used.Add(ch.ToString());
					}
				}
			}
		}

		List<string> newNonTerminals = new();

		foreach (var symb in _nonTerminals) {
			if (used.Contains(symb)) {
				newNonTerminals.Add(symb);
			}
		}

		_nonTerminals = newNonTerminals;
	}

	private void _eliminateUnusedTerminals() {
		HashSet<string> used = new();

		foreach (var pair in _productions) {
			foreach (var transition in pair.Value) {
				foreach (var ch in transition) {
					if (_terminals.Contains(ch.ToString())) {
						used.Add(ch.ToString());
					}
				}
			}
		}

		List<string> newTerminals = new();

		foreach (var symb in _terminals) {
			if (used.Contains(symb)) {
				newTerminals.Add(symb);
			}
		}

		_terminals = newTerminals;
	}

	private void _eliminateDuplicateProd() {
		List<List<string>> newTransitions = new();

		foreach (var pair in _productions) {
			HashSet<string> set = new(pair.Value);
			newTransitions.Add(set.ToList());
		}

		for (int i = 0; i < newTransitions.Count; i++) {
			_productions[i].Value.Clear();
			_productions[i].Value.AddRange(newTransitions[i]);
		}
	}

	private List<KeyValuePair<string, List<string>>> _cloneProductions() {
		return _productions.ToList();
	}

	private List<string> _cloneNonTerminals() {
		return _nonTerminals.ToList();
	}

	private List<string> _cloneTerminals() {
		return _terminals.ToList();
	}
}
