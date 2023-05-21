using Main.classes.NodeVisitor;

namespace Main.classes.Node;

public interface INode {
	object Accept(INodeVisitor visitor);
}
