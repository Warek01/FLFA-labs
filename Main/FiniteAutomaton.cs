namespace Main;

public class FiniteAutomaton {
	private class State {
		public enum StateType {
			Start,
			Intermediate,
			Final
		}

		public List<string> Transitions { get; init; } = new();
		public char         Key         { get; init; }
		public StateType    Type        { get; init; }
	}

	private readonly List<char>   _alphabet;
	private readonly List<string> _nonTerminals;
	private readonly List<State>  _states;
	private readonly List<char>   _stateNames;


	public FiniteAutomaton(List<char>                       terminals, List<string> nonTerminals,
	                       List<KeyValuePair<char, string>> productions) {
		_states     = new();
		_stateNames = new();

		_alphabet     = terminals;
		_nonTerminals = nonTerminals;

		var keys         = productions.Select(production => production.Key).ToList();
		var iteratedKeys = new List<char>();

		foreach (var key in keys) {
			if (iteratedKeys.Contains(key)) {
				continue;
			}

			List<string> transitions = productions
			                           .Where(p => p.Key == key)
			                           .Select(p => p.Value)
			                           .ToList();

			var type = State.StateType.Intermediate;

			if (key == 'S') {
				type = State.StateType.Start;
			}

			var state1 = new State { Key = key, Transitions = transitions, Type = type };
			_states.Add(state1);
			_stateNames.Add(key);
			iteratedKeys.Add(key);
		}

		var state = new State { Key = 'X', Transitions = new(), Type = State.StateType.Final };
		_states.Add(state);
		_stateNames = iteratedKeys;
	}

	private bool _isValidCharacter(char character) {
		return _nonTerminals
		       .Union(
			       _alphabet
				       .Select(c => c.ToString())
				       .ToList()
		       )
		       .Contains(character.ToString());
	}

	private State? _findState(char key) {
		foreach (var state in _states) {
			if (state.Key == key) {
				return state;
			}
		}

		return null;
	}

	private State? _nextState(char input, State state) {
		foreach (string transition in state.Transitions) {
			if (transition[0] == input) {
				if (transition.Length == 1) {
					return _findState('X');
				}

				for (int i = 1; i < transition.Length; i++) {
					if (_stateNames.Contains(transition[i])) {
						return _findState(transition[i]);
					}
				}
			}
		}

		return null;
	}

	public bool StringBelongToLanguage(string str) {
		var    currentState = _findState('S')!;
		string s            = "";

		foreach (char character in str) {
			if (
				(currentState.Type == State.StateType.Final && s.Length != str.Length)
				|| !_isValidCharacter(character)
			) {
				return false;
			}

			currentState = _nextState(character, currentState);

			if (currentState is null) {
				return false;
			}

			s += character;
		}

		return currentState.Type == State.StateType.Final;
	}
}
