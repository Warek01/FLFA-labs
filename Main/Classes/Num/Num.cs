using Main.classes.Expression;
using Main.classes.NodeVisitor;
using Main.classes.Token;

namespace Main.Classes.Num;

public class Num : Expression {
	public Token Token { get; }

	public Num(Token token) {
		Token = token;
	}

	public override object Accept(INodeVisitor visitor) {
		return visitor.VisitNum(Token);
	}
}
