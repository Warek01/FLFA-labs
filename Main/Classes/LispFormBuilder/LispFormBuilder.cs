using Main.classes.Node;
using Main.classes.NodeVisitor;
using Main.classes.Token;

namespace Main.Classes.LispFormBuilder; 

public class LispFormBuilder : INodeVisitor {
	public object VisitBinOp(Token op, INode left, INode right) {
		return $"({op} {left.Accept(this)} {right.Accept(this)})";
	}

	public object VisitNum(Token num) {
		return num.ToString();
	}

	public object VisitUnaryOp(Token op, INode node) {
		return $"({op} {node.Accept(this)})";
	}
}
