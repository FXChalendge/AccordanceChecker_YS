using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.UnitTest
{
    [TestClass]
    public class AgentUnitTest
    {
        #region | Fields |
        private readonly double[] WIDTH_LIMIT = { 0, 9.0 };
        private readonly double[] EXPECTED_ACCORDANCE = { 2.2986845406196887, 1.8998355191963332 };        
        private const int QTY = 10;
        private const int SHIFT = 3;

        private int _selCase = 1;

        private RecordsNodesCircularList<int> _rnclBase = null;
        private RecordsNodesCircularList<int> _rnclToAdjust = null;
        #endregion | Fields |

        #region | Properties |
        #endregion | Properties |

        [TestInitialize]
        public void Init()
        {
            _rnclBase = new RecordsNodesCircularList<int>();
            _rnclToAdjust = new RecordsNodesCircularList<int>();

            for (int i = 0; i < QTY; i++)
            {
                _rnclBase.Add(new Record<int>(WIDTH_LIMIT[_selCase]) { Position = i, Width = i / 10.0 + i % 3 });
                int shiftedPos = (i + SHIFT) % QTY;
                _rnclToAdjust.Add(new Record<int>(WIDTH_LIMIT[_selCase]) { Position = shiftedPos, Width = shiftedPos + i / 10.0 + i % 3 });
            }
            Assert.IsTrue(_rnclBase.Count == QTY, "wromg quantity (base list)");
            Assert.IsTrue(_rnclToAdjust.Count == QTY, "wromg quantity (toAdjust list");

            Assert.IsTrue(_rnclBase.Head.Data.Position == _rnclToAdjust.Head.Data.Position - SHIFT, "Records list for adjustment is wrong");
        }

        [TestCleanup]
        public void Clean()
        {
            _rnclBase = null;
            _rnclToAdjust = null;
        }

        [TestMethod]
        public void Correlate_Test()
        {
            Agent<int> agnt = new Agent<int>(WIDTH_LIMIT[_selCase]);
            agnt.Corellate(_rnclBase, _rnclToAdjust);
            Assert.IsTrue(agnt.IsCorrelated, "correlation failed");
        }

        [TestMethod]
        public void Accordance_Test()
        {
            double maxPecentileRank = 0.9;
            Agent<int> agnt = new Agent<int>(WIDTH_LIMIT[_selCase]);
            agnt.Corellate(_rnclBase, _rnclToAdjust);
            Assert.IsTrue(agnt.IsCorrelated, "correlation failed");

            double stdev = agnt.Accordance(maxPecentileRank);

            Assert.AreEqual<double>(EXPECTED_ACCORDANCE[_selCase], stdev, "Accordance failed");
        }

        [TestMethod]
        public void Accordance_4Dani_Test()
        {
            double[] basic = { 1, 3, 5, 7, 9 };
            double[] adjust = {  3.3, 5.1, 6.8, 8.6, 0.96 };
            _rnclBase.Clear();
            _rnclToAdjust.Clear();

            for (int i = 0; i < basic.Length; i++)
            {
                _rnclBase.Add(new Record<int>(WIDTH_LIMIT[0]) { Position = i, Width = basic[i] });
                _rnclToAdjust.Add(new Record<int>(WIDTH_LIMIT[0]) { Position = i, Width = adjust[i] });
            }

            double maxPecentileRank = 0.8;
            Agent<int> agnt = new Agent<int>(WIDTH_LIMIT[_selCase]);
            agnt.Corellate(_rnclBase, _rnclToAdjust);
            Assert.IsTrue(agnt.IsCorrelated, "correlation failed");

            double stdev = agnt.Accordance(maxPecentileRank);

            Assert.AreEqual<double>(0.18384776310850232, stdev, "Accordance failed");
        }
    }
}
