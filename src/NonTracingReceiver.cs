/*
 * Copyright (c) 2025 Stephen Denne
 * https://github.com/datacute/LightweightTracing
 */

using System.Collections.Generic;
using System.Text;

namespace Datacute.LightweightTracing
{
    internal class NonTracingReceiver : ITraceReceiver
    {
        public void EnsureCapacity(int capacity)
        {
        }
        public void Add(int eventId)
        {
        }
        public string GetTrace() => string.Empty;
        public string GetTrace(Dictionary<int,string> eventNameMap) => string.Empty;
        public void GetTrace(StringBuilder stringBuilder, Dictionary<int, string> eventNameMap)
        {
        }
        public void Clear()
        {
        }
    }
}