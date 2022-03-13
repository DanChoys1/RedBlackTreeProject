using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RedBlackTreeProject
{
    class RedBlackTree
    {
        private Node? _root;

        public RedBlackTree ()
        {
        }

        public void AddData(double data)
        {
            if (_root == null)
            {   
                _root = new Node (data, null);
                _root.Data = data;
                _root.IsRedColor = false;
                return;
            }

            Node? node = _root;
            Node? parent;

            bool isRightChild;

            do
            {
                parent = node;

                if (data > node.Data)
                {
                    isRightChild = true;
                    node = node.RightChild;
                }
                else
                {
                    isRightChild = false;
                    node = node.LeftChild;
                }
            }
            while (node != null);

            node = new Node (data, parent);

            if (isRightChild)
            {
                parent.RightChild = node;
            }
            else
            {
                parent.LeftChild = node;
            }

            FixInsertNode(node);

            while (_root.Parent != null)
            {
                _root = _root.Parent;
            }
        }

        public void DeleteData(double data)
        {
            Node? node = FindNode(data, _root);

            if (node == null)
            {
                return;
            }

            ChangeWithEndBranch(ref node);

            if (node.IsRedColor)
            {
                if (node.IsRightChild)
                {
                    node.Parent!.RightChild = null;
                }
                else
                {
                    node.Parent!.LeftChild = null;
                }
            }
            else
            {
                if (node.RightChild != null)
                {
                    node.Data = node.RightChild.Data;
                    node.RightChild = null;
                }
                else if (node.LeftChild != null)
                {
                    node.Data = node.LeftChild.Data;
                    node.LeftChild = null;
                }
                else if (node.Parent == null)
                {
                    _root = null;
                }
                else
                {
                    DeleteChildlessBlackNode(node);
                }
            }
        }

        public List<List<double?[]>> GetTreeDataArray()
        {
            List<List<double?[]>> treeDataArray = new List<List<double?[]>>();

            List<Node> foundTierNodes = new List<Node>();

            if (_root == null)
            {
                treeDataArray.Add(new List<double?[]>());
                treeDataArray[0].Add(new double?[2]);
                treeDataArray[0][0][0] = null;
                treeDataArray[0][0][1] = 0;
            }
            else
            {
                treeDataArray.Add(new List<double?[]>());
                treeDataArray[0].Add(new double?[2]);
                treeDataArray[0][0][0] = _root.Data;
                treeDataArray[0][0][1] = Convert.ToInt32(_root.IsRedColor);

                foundTierNodes.Add(_root);
            }

            while (foundTierNodes.Count != 0)
            {
                treeDataArray.Add(new List<double?[]>());

                int indexLastTier = treeDataArray.Count - 1;

                List<Node> newFoundTierNodes = new List<Node>();

                for (int i = 0; i < foundTierNodes.Count; i++)
                {
                    treeDataArray[indexLastTier].Add(new double?[2]);

                    if (foundTierNodes[i].LeftChild == null)
                    {
                        treeDataArray[indexLastTier][treeDataArray[indexLastTier].Count - 1][0] = null;
                        treeDataArray[indexLastTier][treeDataArray[indexLastTier].Count - 1][1] = 0;
                    }
                    else
                    {
                        treeDataArray[indexLastTier][treeDataArray[indexLastTier].Count - 1][0] = foundTierNodes[i].LeftChild.Data;
                        treeDataArray[indexLastTier][treeDataArray[indexLastTier].Count - 1][1] = Convert.ToInt32(foundTierNodes[i].LeftChild.IsRedColor);

                        newFoundTierNodes.Add(foundTierNodes[i].LeftChild);
                    }

                    treeDataArray[indexLastTier].Add(new double?[2]);

                    if (foundTierNodes[i].RightChild == null)
                    {
                        treeDataArray[indexLastTier][treeDataArray[indexLastTier].Count - 1][0] = null;
                        treeDataArray[indexLastTier][treeDataArray[indexLastTier].Count - 1][1] = 0;
                    }
                    else
                    {
                        treeDataArray[indexLastTier][treeDataArray[indexLastTier].Count - 1][0] = foundTierNodes[i].RightChild.Data;
                        treeDataArray[indexLastTier][treeDataArray[indexLastTier].Count - 1][1] = Convert.ToInt32(foundTierNodes[i].RightChild.IsRedColor);

                        newFoundTierNodes.Add(foundTierNodes[i].RightChild);
                    }
                }

                foundTierNodes = newFoundTierNodes;
            }

            return treeDataArray;
        }

        private static void FixInsertNode(Node node)
        {
            Node parent = node.Parent!;
            Node grandParent = parent.Parent!;

            if (!parent.IsRedColor)
            {
                return;
            }

            if (parent.IsBrotherColorRed())
            {
                grandParent.RightChild!.IsRedColor = false;
                grandParent.LeftChild!.IsRedColor = false;

                if(grandParent.Parent != null)
                {
                    grandParent.IsRedColor = true;

                    if (grandParent.Parent.IsRedColor)
                    {
                        FixInsertNode(grandParent);
                    }
                }
            }
            else
            {
                if (parent.IsRightChild)
                {
                    if (!node.IsRightChild)
                    {
                        RightRotation(parent);
                    }

                    LeftRotation(grandParent);
                }
                else
                {
                    if (node.IsRightChild)
                    {
                        LeftRotation(parent);
                    }

                    RightRotation(grandParent);
                }

                parent.IsRedColor = false;
                grandParent.IsRedColor = true;
                node.IsRedColor = true;
            }
        }
    
        private static void LeftRotation(Node node)
        {
            Node? parent = node.Parent;
            Node rightChild = node.RightChild!;

            if (parent != null)
            {
                if (node.IsRightChild)
                {
                    parent.RightChild = rightChild;
                }
                else
                {
                    parent.LeftChild = rightChild;
                }
            }

            rightChild.Parent = node.Parent;
            node.RightChild = rightChild.LeftChild;
            rightChild.LeftChild = node;
        }
        
        private static void RightRotation(Node node)
        {
            Node? parent = node.Parent;
            Node leftChild = node.LeftChild!;

            if (parent != null)
            {
                if (node.IsRightChild)
                {
                    parent.RightChild = leftChild;
                }
                else
                {
                    parent.LeftChild = leftChild;
                }
            }

            leftChild.Parent = node.Parent;
            node.LeftChild = leftChild.RightChild;
            leftChild.RightChild = node;
        }

        private static Node? FindNode(double data, Node? root)
        {
            if (root == null)
            {
                return null;
            }

            Node? node = root;

            do
            {
                if (data == node.Data)
                {
                    break;
                }
                else if (data > node.Data)
                {
                    node = node.RightChild;
                }
                else
                {
                    node = node.LeftChild;
                }

                if (node == null)
                {
                    return null;
                }
            }
            while (node != null);

            return node;
        }

        private static void ChangeWithEndBranch(ref Node node)
        {
            Node? endRightChild = node.RightChild;

            if (endRightChild == null)
            {
                return;
            }

            while (endRightChild.LeftChild != null)
            {
                endRightChild = endRightChild.LeftChild;
            }

            double data = endRightChild.Data;
            endRightChild.Data = node.Data;
            node.Data = data;

            node = endRightChild;
        }
    
        private static void DeleteChildlessBlackNode(Node node)
        {
            Node parent = node.Parent!;
            Node brother = node.GetBrother()!;

            if (parent.IsRedColor && (brother.LeftChild == null) && (brother.RightChild == null) )
            {
                parent.IsRedColor = false;
                brother.IsRedColor = true;
                
                if (node.IsRightChild)
                {
                    parent.RightChild = null;
                }
                else
                {
                    parent.LeftChild = null;
                }
            }
            else if (parent.IsRedColor)
            {
                parent.IsRedColor = false;
                brother.IsRedColor = true;

                if (brother.RightChild != null)
                {
                    brother.RightChild.IsRedColor = false;
                }

                if(brother.LeftChild != null)
                {
                    brother.LeftChild.IsRedColor = false;
                }

                RightRotation(parent);
            }
            else if (brother.IsRedColor && (brother.RightChild!.RightChild == null) && (brother.RightChild!.LeftChild == null) && 
                    (brother.LeftChild!.RightChild == null) && (brother.LeftChild!.LeftChild == null) )
            {
                //if ()
            }
        }
    
    }
}
