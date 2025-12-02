using System;
using System.Collections.Generic;
using System.Text;

namespace IngameScript
{
    public class GridUiHandler
    {
        public IGridUiNode RootNode { get; } = new ContainerNode();
    }

    /**
     * A node without internal content. This represents a bunch of nodes clustered together
     */
    public class ContainerNode : GridUiNodeBase
    {
    }

    /**
     * A node containing text. Internally uses a StringBuilder
     */
    public class TextNode : GridUiNodeBase
    {
        private readonly StringBuilder _internal = new StringBuilder();
        public TextNode AppendLine(String str)
        {
            _internal.AppendLine(str);
            return this;
        }

        public string Out()
        {
            string output = _internal.ToString();
            _internal.Clear();
            return output;
        }
    }

    public abstract class GridUiNodeBase : IGridUiNode
    {
        public List<IGridUiNode> Children { get; } = new List<IGridUiNode>();
        public TNode CreateChildNode<TNode>() where TNode : IGridUiNode, new()
        {
            TNode child = new TNode();
            Children.Add(child);
            return child;
        }
        
        private List<IGridUiDrawCondition> Conditions { get; } = new List<IGridUiDrawCondition>();
        public IGridUiNode AddDrawCondition(IGridUiDrawCondition condition)
        {
            Conditions.Add(condition);
            return this;
        }

    }

    /**
     * This represents the UI. It is an intermediate representation(IR) which will later
     * be turned into the actual UI by the GridUi Program
     */
    public interface IGridUiNode
    {
        List<IGridUiNode> Children { get; }
        /**
         * returns: newly created Child
         */
        TNode CreateChildNode<TNode>() where TNode : IGridUiNode, new();
        /**
         * returns: self
         */
        IGridUiNode AddDrawCondition(IGridUiDrawCondition condition);
    }

     public interface IGridUiDrawCondition
    {
    }
}