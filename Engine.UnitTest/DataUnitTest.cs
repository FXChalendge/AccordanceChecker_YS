using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Engine.UnitTest
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class DataUnitTest
    {
        #region | Fields |
        private const double WIDTH_LIMIT = 9.0;
        private const int QTY = 10;
        private const int SHIFT = 3;

        //private RecordsNodesCircularList _rnclBase = null;
        //private RecordsNodesCircularList _rnclToAdjust = null;
        #endregion | Fields |
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void AddRecord_First_Test()
        {
            RecordsNodesCircularList<int> rncl = new RecordsNodesCircularList<int>();
            rncl.Add(new Record<int>(WIDTH_LIMIT) { Position = 0, Width = 0.1 });

            Assert.IsTrue(rncl.Count == 1, "wromg quantity");

            Assert.IsNotNull(rncl.Head, "empty Head");
            Assert.IsNotNull(rncl.Tail, "empty Tail");
        }

        [TestMethod]
        public void AddRecords_Multiple_Test()
        {
            RecordsNodesCircularList<int> rncl = new RecordsNodesCircularList<int>();
            for (int i = 0; i < QTY; i++)
            {
                rncl.Add(new Record<int>(WIDTH_LIMIT) { Position = i, Width = i / 10.0 });
            }
            Assert.IsTrue(rncl.Count == QTY, "wromg quantity");

            Assert.IsNotNull(rncl.Head, "empty Head");
            Assert.IsNotNull(rncl.Tail, "empty Tail");

            Assert.IsTrue(rncl.Head.Data.Position == 0, "wromg Head data");
            Assert.IsTrue(rncl.Head.Next?.Data.Position == 1, "wromg Head Next data");
            Assert.IsTrue(rncl.Head.Previous?.Data.Position == QTY-1, "wromg Head Previous data");

            Assert.IsTrue(rncl.Tail.Data.Position == QTY-1, "wromg Tail data");
            Assert.IsTrue(rncl.Tail.Next?.Data.Position == 0, "wromg Tail Next data");
            Assert.IsTrue(rncl.Tail.Previous?.Data.Position == QTY-2, "wromg Tail Previous data");
        }

        [TestMethod]
        public void Normalization_Test()
        {
            RecordsNodesCircularList<int> rncl = new RecordsNodesCircularList<int>();
            for (int i = 0; i < QTY; i++)
            {
                rncl.Add(new Record<int>(WIDTH_LIMIT) { Position = i, Width = i / 10.0 });
            }
            Assert.IsTrue(rncl.Count == QTY, "wromg quantity");

            try
            {
                rncl.Normalize();
                Assert.IsTrue(rncl.IsNormalized, "normalization failed");
            }
            catch(Exception ex)
            {
                if(ex is ArgumentException)
                {
                    Assert.Fail(ex.Message);
                }
            }
        }

        [TestMethod]
        public void Adjustment_Test()
        {
            RecordsNodesCircularList<int> rnclBase = new RecordsNodesCircularList<int>();
            RecordsNodesCircularList<int> rnclToAdjust = new RecordsNodesCircularList<int>();
            for (int i = 0; i < QTY; i++)
            {
                rnclBase.Add(new Record<int>(WIDTH_LIMIT) { Position = i, Width = i / 10.0 });
                int shiftedPos = (i + SHIFT) % QTY;
                rnclToAdjust.Add(new Record<int>(WIDTH_LIMIT) { Position = shiftedPos, Width = shiftedPos / 10.0 });
            }
            Assert.IsTrue(rnclBase.Count == QTY, "wromg quantity (base list)");
            Assert.IsTrue(rnclToAdjust.Count == QTY, "wromg quantity (toAdjust list");

            Assert.IsTrue(rnclBase.Head.Data.Position == rnclToAdjust.Head.Data.Position - SHIFT, "Records list for adjustment is wrong");

            try
            {
                rnclBase.Adjust(rnclToAdjust);
                Assert.IsTrue(rnclToAdjust.IsAdjusted, "adjustment failed");
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                {
                    Assert.Fail(ex.Message);
                }
            }
        }
    }
}
