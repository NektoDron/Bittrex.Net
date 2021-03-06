﻿using System;
using System.Collections.Generic;
using System.Text;
using Bittrex.Net.Objects.V3;

namespace Bittrex.Net.Sockets
{
    /// <summary>
    /// Symbol tickers update
    /// </summary>
    public class BittrexTickersUpdate
    {
        /// <summary>
        /// Sequence
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// Ticker per symbol
        /// </summary>
        public IEnumerable<BittrexTickV3> Deltas { get; set; } = new List<BittrexTickV3>();
    }
}
