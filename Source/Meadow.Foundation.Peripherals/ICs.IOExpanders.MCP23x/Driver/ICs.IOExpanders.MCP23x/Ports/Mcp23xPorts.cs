﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meadow.Foundation.ICs.IOExpanders.Ports;
using Meadow.Hardware;

namespace Meadow.Foundation.ICs.IOExpanders
{
    public class Mcp23xPorts : IMcpGpioPorts
    {
        private readonly McpGpioPort[] _ports;

        public Mcp23xPorts(params McpGpioPort[] ports)
        {
            _ports = ports;
            AllPins = _ports.Length == 1 ? _ports[0].AllPins : _ports.SelectMany(p => p.AllPins).ToArray();
        }

        public IList<IPin> AllPins { get; }

        public int Count => AllPins.Count;

        public McpGpioPort this[int index] => _ports[index];

        public IEnumerator<McpGpioPort> GetEnumerator()
        {
            return (IEnumerator<McpGpioPort>) _ports.GetEnumerator();
        }

        public int GetPortIndex(McpGpioPort port)
        {
            var index = Array.IndexOf(_ports, port);
            if (index < 0)
            {
                throw new ArgumentException("Provided port is not part of this device.", nameof(port));
            }

            return index;
        }

        public int GetPortIndexOfPin(IPin pin)
        {
            for (var i = 0; i < Count; i++)
            {
                if (this[i].AllPins.Contains(pin))
                {
                    return i;
                }
            }

            throw new ArgumentException("Pin is not from this port set", nameof(pin));
        }

        public McpGpioPort GetPortOfPin(IPin pin)
        {
            return this[GetPortIndexOfPin(pin)];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
