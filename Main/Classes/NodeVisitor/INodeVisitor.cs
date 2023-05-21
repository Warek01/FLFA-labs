using Main.classes.Node;

namespace Main.classes.NodeVisitor;

public interface INodeVisitor {
	object VisitNum(Token.Token     num);
	object VisitUnaryOp(Token.Token op, INode node);
	object VisitBinOp(Token.Token   op, INode left, INode right);
}
