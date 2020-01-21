﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.UnitTest
{
    [TestClass]
    public class AgentUnitTest
    {
        #region | Fields |
        private const int QTY = 10;
        private const int SHIFT = 3;

        private RecordsNodesCircularList _rnclBase = null;
        private RecordsNodesCircularList _rnclToAdjust = null;
        #endregion | Fields |

        #region | Properties |
        #endregion | Properties |

        [TestInitialize]
        public void Init()
        {
            _rnclBase = new RecordsNodesCircularList();
            _rnclToAdjust = new RecordsNodesCircularList();

            for (int i = 0; i < QTY; i++)
            {
                _rnclBase.Add(new Record { Position = i, Width = i / 10.0 + i % 3 });
                int shiftedPos = (i + SHIFT) % QTY;
                _rnclToAdjust.Add(new Record { Position = shiftedPos, Width = shiftedPos + i / 10.0 + i % 3 });
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
            Agent agnt = new Agent();
            agnt.Corellate(_rnclBase, _rnclToAdjust);
            Assert.IsTrue(agnt.IsCorrelated, "correlation failed");
        }

        [TestMethod]
        public void Accordance_Test()
        {
            double maxPecentileRank = 0.9;
            Agent agnt = new Agent();
            agnt.Corellate(_rnclBase, _rnclToAdjust);
            Assert.IsTrue(agnt.IsCorrelated, "correlation failed");

            double stdev = agnt.Accordance(maxPecentileRank);

            Assert.AreEqual<double>(2.2986845406196887, stdev, "Accordance failed");
        }
    }
}
