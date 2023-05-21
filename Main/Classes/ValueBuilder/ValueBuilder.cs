using Main.classes.Node;
using Main.classes.NodeVisitor;
using Main.classes.Token;

namespace Main.classes.ValueBuilder;

public class ValueBuilder : INodeVisitor {
	public object VisitBinOp(Token.Token op, INode left, INode right) {
		return op.Type switch {
			TokenType.Plus     => (decimal)left.Accept(this) + (decimal)right.Accept(this),
			TokenType.Minus    => (decimal)left.Accept(this) - (decimal)right.Accept(this),
			TokenType.Multiply => (decimal)left.Accept(this) * (decimal)right.Accept(this),
			TokenType.Divide   => (decimal)left.Accept(this) / (decimal)right.Accept(this),
			_                  => throw new Exception($"Token of type {op.Type.ToString()} cannot be evaluated.")
		};
	}

	public object VisitNum(Token.Token num) {
		return decimal.Parse(num.Value);
	}

	public object VisitUnaryOp(Token.Token op, INode node) {
		return op.Type switch {
			TokenType.Plus  => node.Accept(this),
			TokenType.Minus => -(decimal)node.Accept(this),
			_               => throw new Exception($"Token of type {op.Type.ToString()} cannot be evaluated.")
		};
	}
}
