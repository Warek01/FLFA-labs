namespace Main.classes;

public class FiniteAutomaton {
	// We keep initial values to use them for converting to Grammar
	private readonly List<Transition> _initialTransitions;
	private readonly List<int>        _initialFinalStates;
	private readonly List<char>       _initialAlphabet;
	private readonly int              _initialNrOfStates;

	private readonly List<char>  _alphabet;
	private readonly List<State> _states      = new();
	private readonly List<State> _finalStates = new();

	public FiniteAutomaton(List<char>       alphabet, int nrOfStates, List<int> finalStates,
	                       List<Transition> transitions) {
		_initialAlphabet    = alphabet;
		_initialNrOfStates  = nrOfStates;
		_initialTransitions = transitions;
		_initialFinalStates = finalStates;

		_alphabet = alphabet;

		for (var i = 0; i < nrOfStates; i++) {
			State state = new();
			_states.Add(state);

			if (finalStates.Contains(i)) {
				_finalStates.Add(state);
			}
		}

		foreach (Transition transition in transitions) {
			State stateFrom = _states[transition.From];
			State stateTo   = _states[transition.To];

			if (stateFrom is null || stateTo is null) {
				throw new Exception($"Unknown state {transition.From} -> {transition.To}");
			}

			stateFrom.Transitions.Add(new KeyValuePair<string, State>(transition.Value, stateTo));
		}
		
		_convertToDeterministic();

		// Sort transitions list so they start with the most long ones
		foreach (State state in _states) {
			state.Transitions.Sort(
				(a, b) => a.Key.Length == b.Key.Length
					? 0
					: a.Key.Length > b.Key.Length
						? -1
						: 1
			);
		}

		for (int i = 0; i < _states.Count; i++) {
			var state = _states[i];
		
			foreach (var t in state.Transitions) {
				Console.WriteLine($"({i}, {t.Key}) -> {_states.FindIndex(a => a == t.Value)}");
			}
		}
	}

	private void _convertToDeterministic() {
		foreach (State state in _states) {
			var transitionsToAdd    = new List<KeyValuePair<string, State>>();
			var transitionsToRemove = new List<KeyValuePair<string, State>>();

			foreach (var transition1 in state.Transitions) {
				foreach (var transition2 in state.Transitions) {
					if (Equals(transition1, transition2)) {
						continue;
					}

					if (transition1.Key == transition2.Key) {
						var t = transition2.Value == state ? transition1 : transition2;
						foreach (var transition in t.Value.Transitions) {
							transitionsToAdd.Add(
								new KeyValuePair<string, State>(t.Key + transition.Key, transition.Value)
							);
						}

						transitionsToRemove.Add(t);
					}
				}
			}

			state.Transitions.AddRange(transitionsToAdd);

			foreach (var transition in transitionsToRemove) {
				state.Transitions.Remove(transition);
			}
		}
	}

	public bool StringBelongToLanguage(string str) {
		State currentState = _states[0];
		var   position     = 0;

		while (position < str.Length) {
			var newStateFound = false;

			foreach (var transition in currentState.Transitions) {
				if (!str[position..].StartsWith(transition.Key)) {
					continue;
				}

				currentState  =  transition.Value;
				position      += transition.Key.Length;
				newStateFound =  true;
				break;
			}

			if (!newStateFound) {
				return false;
			}
		}
 
		return _finalStates.Contains(currentState);
	}

	public Grammar ToGrammar() {
		return new Grammar(
			_initialAlphabet,
			_initialNrOfStates,
			_initialFinalStates,
			_initialTransitions
		);
	}
}
