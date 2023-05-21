using Main.classes.Expression;
using Main.classes.NodeVisitor;
using Main.classes.Token;

namespace Main.Classes.Operations.BinaryOperation;

public class BinOp : Expression {
	public Token      Op    { get; }
	public Expression Left  { get; }
	public Expression Right { get; }

	public BinOp(Token op, Expression left, Expression right) {
		Op    = op;
		Left  = left;
		Right = right;
	}

	public override object Accept(INodeVisitor visitor) {
		return visitor.VisitBinOp(Op, Left, Right);
	}
}
