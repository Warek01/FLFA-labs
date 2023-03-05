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

			stateFrom.Transitions.Add(new KeyValuePair<char, State>(transition.Value, stateTo));
		}
	}

	private bool _isValidCharacter(char character) {
		return _alphabet.Contains(character);
	}


	public bool StringBelongToLanguage(string str) {
		State currentState = _states[0];
		var   position     = 0;

		while (position < str.Length) {
			var character = str[position];

			if (!_isValidCharacter(character)) {
				return false;
			}

			var newStateFound = false;

			foreach (var transition in currentState.Transitions) {
				if (transition.Key != character) {
					continue;
				}

				currentState = transition.Value;
				position++;
				newStateFound = true;
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
