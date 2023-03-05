namespace Main.classes;

public class Grammar {
	// We keep initial values to use them for converting to Automaton
	private readonly List<Transition> _initialTransitions;
	private readonly List<int>        _initialFinalStates;
	private readonly List<char>       _initialAlphabet;
	private readonly int              _initialNrOfStates;

	private readonly List<char>  _alphabet;
	private readonly List<State> _states      = new();
	private readonly List<State> _finalStates = new();

	public Grammar(List<char>       alphabet, int nrOfStates, List<int> finalStates,
	               List<Transition> transitions) {
		_initialAlphabet    = alphabet;
		_initialNrOfStates  = nrOfStates;
		_initialTransitions = transitions;
		_initialFinalStates = finalStates;

		_alphabet = alphabet;

		for (int i = 0; i < nrOfStates; i++) {
			State state = new();
			_states.Add(state);

			if (finalStates.Contains(i)) {
				_finalStates.Add(state);
			}
		}

		foreach (var transition in transitions) {
			var stateFrom = _states[transition.From];
			var stateTo   = _states[transition.To];

			if (stateFrom is null || stateTo is null) {
				throw new Exception($"Unknown state {transition.From} -> {transition.To}");
			}

			stateFrom.Transitions.Add(new KeyValuePair<char, State>(transition.Value, stateTo));
		}
	}

	public string GenerateString() {
		var   rand         = new Random();
		var   str          = "";
		State currentState = _states[0];

		while (!_finalStates.Contains(currentState)) {
			var transition = currentState.Transitions[rand.Next(0, currentState.Transitions.Count)];
			str          += transition.Key;
			currentState =  transition.Value;
		}

		return str;
	}
	
	
	public FiniteAutomaton ToFiniteAutomaton() {
		return new FiniteAutomaton(
			_initialAlphabet,
			_initialNrOfStates,
			_initialFinalStates,
			_initialTransitions
		);
	}
}
