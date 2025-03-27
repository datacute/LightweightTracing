/*
 * Copyright (c) 2025 Stephen Denne
 * https://github.com/datacute/LightweightTracing
 */

using System.Collections.Generic;
using System.Text;

namespace Datacute.LightweightTracing
{
    public static class LightweightTrace
    {
        private static readonly ITraceReceiver NonTracingReceiver = new NonTracingReceiver();
        private static readonly ITraceReceiver TraceReceiver = new TraceReceiver();

        private static ITraceReceiver _receiver = TraceReceiver;
        internal const int DefaultCapacity = 1000;
        private static int _capacity = DefaultCapacity;

        public static void EnableTracing() => EnableTracing(_capacity);
        
        public static void EnableTracing(int capacity)
        {
            _capacity = capacity;
            TraceReceiver.EnsureCapacity(capacity);
            _receiver = TraceReceiver;
        }

        public static void DisableTracing() => _receiver = NonTracingReceiver;

        public static void Add(int eventId) => _receiver.Add(eventId);
        
        public static void Clear() => _receiver.Clear();
        
        public static string GetTrace() => _receiver.GetTrace();
        public static string GetTrace(Dictionary<int,string> eventNameMap) => _receiver.GetTrace(eventNameMap);
        public static void GetTrace(StringBuilder stringBuilder, Dictionary<int,string> eventNameMap) => _receiver.GetTrace(stringBuilder, eventNameMap);
    }
}