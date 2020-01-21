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
        #region | Fields |
        private readonly double _widthLimit;
        #endregion | Fields |

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
        public double? Normalized { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsValid
        {
            get
            {
                return _widthLimit <= 0 || Width < _widthLimit;
            }
        }
        #endregion | Properties |

        #region | Constructor |
        /// <summary>
        /// 
        /// </summary>
        /// <param name="widthLimit"></param>
        /// <remarks>Set <cref="widthLimit"/> with less or equals to <c="0"/> to elliminate Width to <cref="widthLimit"/> validation.</remarks>
        public Record(double widthLimit)
        {
            _widthLimit = widthLimit;
        }
        #endregion | Constructor |

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
        internal static Record Deserialize(double widthLimit, string line)
        {
            Record res = null;

            if (!string.IsNullOrEmpty(line))
            {
                string[] tokens = line.Split(',');
                int widthIdx = 0;
                if (tokens.Length > 0)
                {
                    res = new Record(widthLimit);

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
            if(IsValid)
                Normalized = Width - avg;

            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(Position)}:{Position};{nameof(IsValid)}:{IsValid}(Limit:{_widthLimit});{nameof(Width)}:{Width};{nameof(Normalized)}:{Normalized}";
        }

        #endregion | Methods |
    }
}
