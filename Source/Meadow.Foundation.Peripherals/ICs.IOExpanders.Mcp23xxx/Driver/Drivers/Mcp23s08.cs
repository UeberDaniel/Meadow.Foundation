﻿using Meadow.Hardware;

namespace Meadow.Foundation.ICs.IOExpanders
{
    /// <summary>
    /// Represent an MCP23S08 SPI port expander
    /// </summary>
    public class Mcp23s08 : Mcp23x0x
    {
        /// <summary>
        /// Creates an Mcp23s08 object
        /// </summary>
        /// <param name="spiBus">The SPI bus</param>
        /// <param name="chipSelectPort">Chip select port</param>
        /// <param name="interruptPort">optional interupt port, needed for input interrupts</param>
        /// <param name="resetPort">Optional Meadow output port used to reset the mcp expander</param>
        public Mcp23s08(ISpiBus spiBus, IDigitalOutputPort chipSelectPort, IDigitalInterruptPort? interruptPort = null, IDigitalOutputPort? resetPort = null) :
            base(spiBus, chipSelectPort, interruptPort, resetPort)
        {
        }

        /// <summary>
        /// Creates an Mcp23s08 object
        /// </summary>
        /// <param name="spiBus">The SPI bus</param>
        /// <param name="chipSelectPin">Chip select pin</param>
        /// <param name="interruptPin">optional interupt pin, needed for input interrupts (InterruptMode: EdgeRising, ResistorMode.InternalPullDown)</param>
        public Mcp23s08(ISpiBus spiBus, IPin chipSelectPin, IPin? interruptPin = null) :
            this(spiBus, chipSelectPin.CreateDigitalOutputPort(), (interruptPin == null) ? null : interruptPin.CreateDigitalInterruptPort(InterruptMode.EdgeRising, ResistorMode.InternalPullDown))
        {
        }
    }
}