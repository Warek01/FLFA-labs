namespace Main.classes; 

public class Transition {
	public int From { get; }
	public int To { get; }
	public char Value { get; }

	public Transition(int from, char value, int to) {
		From  = from;
		To    = to;
		Value = value;
	}
}
