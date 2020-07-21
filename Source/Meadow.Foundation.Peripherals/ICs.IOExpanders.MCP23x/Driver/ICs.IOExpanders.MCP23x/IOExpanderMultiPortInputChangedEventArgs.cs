﻿using System;

namespace Meadow.Foundation.ICs.IOExpanders
{
    public class IOExpanderMultiPortInputChangedEventArgs : EventArgs
    {
        public IOExpanderMultiPortInputChangedEventArgs(byte[] interruptPins, byte[] interruptValues)
        {
            InterruptPins = interruptPins;
            InterruptValues = interruptValues;
        }

        /// <summary>
        /// Which pins on which ports were interrupted.
        /// </summary>
        public byte[] InterruptPins { get; }

        /// <summary>
        /// The values of pins that were interrupted. Other bit values should be ignored.
        /// </summary>
        public byte[] InterruptValues { get; }
    }
}
