namespace Main.classes; 

public class Transition {
	public int From { get; }
	public int To { get; }
	public string Value { get; }

	public Transition(int from, string value, int to) {
		From  = from;
		To    = to;
		Value = value;
	}
}
