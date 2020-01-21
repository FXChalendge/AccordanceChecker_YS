using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Engine.UnitTest
{
    [TestClass]
    public class DataUnitTest
    {
        [TestMethod]
        public void AddRecord_First_Test()
        {
            RecordsNodesCircularList rncl = new RecordsNodesCircularList();
            rncl.Add(new Record { Position = 0, Width = 0.1 });

            Assert.IsTrue(rncl.Count == 1, "wromg quantity");

            Assert.IsNotNull(rncl.Head, "empty Head");
            Assert.IsNotNull(rncl.Tail, "empty Tail");
        }

        [TestMethod]
        public void AddRecords_Multiple_Test()
        {
            const int qty = 10;
            RecordsNodesCircularList rncl = new RecordsNodesCircularList();
            for (int i = 0; i < qty; i++)
            {
                rncl.Add(new Record { Position = i, Width = i / 10.0 });
            }
            Assert.IsTrue(rncl.Count == qty, "wromg quantity");

            Assert.IsNotNull(rncl.Head, "empty Head");
            Assert.IsNotNull(rncl.Tail, "empty Tail");

            Assert.IsTrue(rncl.Head.Data.Position == 0, "wromg Head data");
            Assert.IsTrue(rncl.Head.Next?.Data.Position == 1, "wromg Head Next data");
            Assert.IsTrue(rncl.Head.Previous?.Data.Position == qty-1, "wromg Head Previous data");

            Assert.IsTrue(rncl.Tail.Data.Position == qty-1, "wromg Tail data");
            Assert.IsTrue(rncl.Tail.Next?.Data.Position == 0, "wromg Tail Next data");
            Assert.IsTrue(rncl.Tail.Previous?.Data.Position == qty-2, "wromg Tail Previous data");
        }

        [TestMethod]
        public void Normalization_Test()
        {
            const int qty = 10;
            RecordsNodesCircularList rncl = new RecordsNodesCircularList();
            for (int i = 0; i < qty; i++)
            {
                rncl.Add(new Record { Position = i, Width = i / 10.0 });
            }
            Assert.IsTrue(rncl.Count == qty, "wromg quantity");

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
            const int qty = 10;
            const int shift = 3;
            RecordsNodesCircularList rnclBase = new RecordsNodesCircularList();
            RecordsNodesCircularList rnclToAdjust = new RecordsNodesCircularList();
            for (int i = 0; i < qty; i++)
            {
                rnclBase.Add(new Record { Position = i, Width = i / 10.0 });
                rnclToAdjust.Add(new Record { Position = (i + shift) % qty, Width = (i + shift) % qty / 10.0 });
            }
            Assert.IsTrue(rnclBase.Count == qty, "wromg quantity (base list)");
            Assert.IsTrue(rnclToAdjust.Count == qty, "wromg quantity (toAdjust list");

            Assert.IsTrue(rnclBase.Head.Data.Position == rnclToAdjust.Head.Data.Position - shift, "Records list for adjustment is wrong");

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
