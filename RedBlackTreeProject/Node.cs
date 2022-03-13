using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTreeProject
{
    class Node
    {
        public double Data { get; set; }
        public bool IsRedColor { get; set; }
        public bool IsRightChild { get; set; }

        public Node? Parent { get; set; }

        private Node? _rightChild;
        public Node? RightChild 
        {
            get { return _rightChild; }

            set
            {
                _rightChild = value;

                if (_rightChild != null)
                {
                    _rightChild.IsRightChild = true;
                    _rightChild.Parent = this;
                }
            } 
        }

        private Node? _leftChild;
        public Node? LeftChild 
        {
            get { return _leftChild; }

            set
            {
                _leftChild = value;

                if (_leftChild != null)
                {
                    _leftChild.IsRightChild = false;
                    _leftChild.Parent = this;
                }
            }
        }

        public Node(double data, Node? parent)
        {
            Data = data;
            IsRedColor = true;

            Parent = parent;
            RightChild = null;
            LeftChild = null;
        }

        public Node? GetBrother()
        {
            if (Parent == null)
            {
                throw new InvalidOperationException("Root can't has brother");
            }

            if (IsRightChild)
            {
                return Parent.LeftChild;
            }
            else
            {
                return Parent.RightChild;
            }
        }

        public bool IsBrotherColorRed()
        {
            if (this.Parent == null)
            {
                throw new InvalidOperationException("Root hasn't brother.");
            }

            Node? brother;

            if (this.IsRightChild)
            {
                brother = Parent.LeftChild;
            }
            else
            {
                brother = Parent.RightChild;
            }

            if (brother == null)
            {
                return false;
            }

            return brother.IsRedColor;
        }
    }
}
