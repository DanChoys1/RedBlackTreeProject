using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using RedBlackTreeProject;

namespace RedBlackTreeTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void InsertTest()
        {
            // Arrange
            double[] insertValues = { 12, 32, 54, 23, 87, 43, 0, 98 };
            double?[,] valuesInTree = new double?[17, 2] { {32, 0}, 
                                                           {12, 0}, {54, 1},
                                                           {0, 1}, {23, 1}, {43, 0}, {87, 0},
                                                           {null, 0}, {null, 0}, {null, 0}, {null, 0}, {null, 0}, {null, 0}, {null, 0}, {98, 1},
                                                           {null, 0}, {null, 0}};

            // Act
            RedBlackTree tree = new RedBlackTree();
            
            foreach(double value in insertValues)
            {
                tree.AddData(value);
            }

            List<List<double?[]>> treeDataArray = tree.GetTreeDataArray();

            // Assert
            int indexValue = 0;

            foreach (List<double?[]> itemsList in treeDataArray)
            {
                foreach (double?[] value in itemsList)
                {
                    Assert.AreEqual(valuesInTree[indexValue, 0], value[0]);
                    Assert.AreEqual(valuesInTree[indexValue, 1], value[1]);

                    indexValue++;
                }
            }
        }

        [TestMethod]
        public void DeleteTest()
        {
            // Arrange
            double[] insertValues = { 12, 32, 54, 23, 87, 43, 0, 98 };
            double deleteValue = 98;
            double?[,] valuesInTree = new double?[15, 2] { {32, 0},
                                                           {12, 0}, {54, 1},
                                                           {0, 1}, {23, 1}, {43, 0}, {87, 0},
                                                           {null, 0}, {null, 0}, {null, 0}, {null, 0}, {null, 0}, {null, 0}, {null, 0}, {null, 0}};

            // Act
            RedBlackTree tree = new RedBlackTree();

            foreach (double value in insertValues)
            {
                tree.AddData(value);
            }

            tree.DeleteData(deleteValue);

            List<List<double?[]>> treeDataArray = tree.GetTreeDataArray();

            // Assert
            int indexValue = 0;

            foreach (List<double?[]> itemsList in treeDataArray)
            {
                foreach (double?[] value in itemsList)
                {
                    Assert.AreEqual(valuesInTree[indexValue, 0], value[0]);
                    Assert.AreEqual(valuesInTree[indexValue, 1], value[1]);

                    indexValue++;
                }
            }
        }
    }
}
