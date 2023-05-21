using Main.classes.Expression;
using Main.classes.NodeVisitor;
using Main.classes.Token;

namespace Main.Classes.Operations.UnaryOperation;

public class UnaryOp : Expression {
	public Token      Op   { get; }
	public Expression Node { get; }

	public UnaryOp(Token op, Expression node) {
		Op   = op;
		Node = node;
	}

	public override object Accept(INodeVisitor visitor) {
		return visitor.VisitUnaryOp(Op, Node);
	}
}
