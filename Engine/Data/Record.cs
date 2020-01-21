using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    /// <summary>
    /// 
    /// </summary>
    public class Record
        : INormalized<double>
    {
        #region | Properties |
        /// <summary>
        /// 
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Normalized { get; protected set; }
        #endregion | Properties |

        #region | Methods |
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal string Serialize()
        {
            return $"{Position},{Width}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        internal static Record Deserialize(string line)
        {
            Record res = null;

            if (!string.IsNullOrEmpty(line))
            {
                string[] tokens = line.Split(',');
                int widthIdx = 0;
                if (tokens.Length > 0)
                {
                    res = new Record();

                    if (tokens.Length > 1)
                        widthIdx = 1;

                    res.Width = Double.Parse(tokens[widthIdx]);

                    if (widthIdx > 0)
                        res.Position = Int32.Parse(tokens[0]);
                }
            }
            return res;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="avg"></param>
        /// <returns></returns>
        public INormalized<double> Normalize(double avg)
        {
            Normalized = Width - avg;

            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(Position)}:{Position};{nameof(Width)}:{Width};{nameof(Normalized)}:{Normalized}";
        }

        #endregion | Methods |
    }
}
