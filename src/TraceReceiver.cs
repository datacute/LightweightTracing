/*
 * Copyright (c) 2025 Stephen Denne
 * https://github.com/datacute/LightweightTracing
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Datacute.LightweightTracing
{
    internal class TraceReceiver : ITraceReceiver
    {
        private static readonly DateTime StartTime = DateTime.UtcNow;
        private static readonly Stopwatch Stopwatch = Stopwatch.StartNew();

        private static long[] _timestamps = new long[LightweightTrace.DefaultCapacity];
        private static int[] _events = new int[LightweightTrace.DefaultCapacity];
        private static int _earliestIndex;
        private static int _nextIndex;
        private static int _capacity = LightweightTrace.DefaultCapacity;
        private static readonly object Lock = new object();

        private static readonly Dictionary<int, string> EmptyEventNameMap = new Dictionary<int, string>();

        public void EnsureCapacity(int capacity)
        {
            lock (Lock)
            {
                if (_timestamps.Length == capacity)
                {
                    return;
                }

                _timestamps = new long[capacity];
                _events = new int[capacity];
                _capacity = capacity;
                _earliestIndex = 0;
                _nextIndex = 0;
            }
        }
        public void Add(int eventId)
        {
            lock (Lock)
            {
                _timestamps[_nextIndex] = Stopwatch.ElapsedTicks;
                _events[_nextIndex] = eventId;

                _nextIndex = (_nextIndex + 1) % _capacity;
                if (_nextIndex == _earliestIndex)
                {
                    _earliestIndex = (_earliestIndex + 1) % _capacity;
                }
            }
        }

        public void Clear()
        {
            lock (Lock)
            {
                _earliestIndex = _nextIndex;
            }
        }

        public string GetTrace() => GetTrace(EmptyEventNameMap);

        public string GetTrace(Dictionary<int, string> eventNameMap)
        {
            var sb = new StringBuilder();
            GetTrace(sb, eventNameMap);
            return sb.ToString();
        }

        public void GetTrace(StringBuilder stringBuilder, Dictionary<int, string> eventNameMap)
        {
            lock (Lock)
            {
                var index = _earliestIndex;
                while (index != _nextIndex)
                {
                    var timestamp = _timestamps[index];
                    var eventId = _events[index];
                    var logTimestamp = StartTime.AddTicks(timestamp);
                    var eventName = eventNameMap.TryGetValue(eventId, out var name) ? name : String.Empty;
                    stringBuilder.AppendFormat("{0:o} [{1:000}] {2}", logTimestamp, eventId, eventName).AppendLine();
                    index = (index + 1) % _capacity;
                }
            }
        }
    }
}