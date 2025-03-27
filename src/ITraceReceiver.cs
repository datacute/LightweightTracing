/*
 * Copyright (c) 2025 Stephen Denne
 * https://github.com/datacute/LightweightTracing
 */

using System.Collections.Generic;
using System.Text;

namespace Datacute.LightweightTracing
{
    internal interface ITraceReceiver
    {
        void EnsureCapacity(int capacity);
        void Add(int eventId);
        void Clear();
        string GetTrace();
        string GetTrace(Dictionary<int,string> eventNameMap);
        void GetTrace(StringBuilder stringBuilder, Dictionary<int,string> eventNameMap);
    }
}