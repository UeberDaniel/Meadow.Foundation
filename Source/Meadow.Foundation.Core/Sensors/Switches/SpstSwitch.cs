﻿using Meadow.Hardware;
using Meadow.Peripherals.Switches;
using System;
using System.Threading.Tasks;

namespace Meadow.Foundation.Sensors.Switches
{
    /// <summary>
    /// Represents a simple, on/off, Single-Pole-Single-Throw (SPST) switch that closes a circuit 
    /// to either ground/common or high. 
    /// 
    /// Use the SwitchCircuitTerminationType to specify whether the other side of the switch
    /// terminates to ground or high.
    /// </summary>
    public class SpstSwitch : ISwitch
    {
        /// <summary>
        /// Describes whether or not the switch circuit is closed/connected (IsOn = true), or open (IsOn = false).
        /// </summary>
        public bool IsOn
        {
            get => DigitalIn.State;
            protected set => Changed(this, new EventArgs());
        }

        /// <summary>
        /// Raised when the switch circuit is opened or closed.
        /// </summary>
        public event EventHandler Changed = delegate { };

        /// <summary>
        /// Returns the DigitalInputPort.
        /// </summary>
        protected IDigitalInterruptPort DigitalIn { get; set; }

        /// <summary>
        /// Instantiates a new SpstSwitch object connected to the specified digital pin, and with the specified CircuitTerminationType in the type parameter.
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="interruptMode"></param>
        /// <param name="resistorMode"></param>
        public SpstSwitch(IPin pin, InterruptMode interruptMode, ResistorMode resistorMode) :
            this(pin.CreateDigitalInterruptPort(interruptMode, resistorMode, TimeSpan.FromMilliseconds(20), TimeSpan.Zero))
        { }

        /// <summary>
        /// Instantiates a new SpstSwitch object connected to the specified digital pin, and with the specified CircuitTerminationType in the type parameter.
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="interruptMode"></param>
        /// <param name="resistorMode"></param>
        /// <param name="debounceDuration"></param>
        /// <param name="glitchFilterCycleCount"></param>
        public SpstSwitch(IPin pin, InterruptMode interruptMode, ResistorMode resistorMode, TimeSpan debounceDuration, TimeSpan glitchFilterCycleCount) :
            this(pin.CreateDigitalInterruptPort(interruptMode, resistorMode, debounceDuration, glitchFilterCycleCount))
        { }

        /// <summary>
        /// Creates a SpstSwitch on a especified interrupt port
        /// </summary>
        /// <param name="interruptPort"></param>
        public SpstSwitch(IDigitalInterruptPort interruptPort)
        {
            DigitalIn = interruptPort;
            DigitalIn.Changed += DigitalInChanged;
        }

        /// <summary>
        /// Event handler when switch value has been changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DigitalInChanged(object sender, DigitalPortResult e)
        {
            IsOn = DigitalIn.State;
        }

        /// <summary>
        /// Convenience method to get the current sensor reading
        /// </summary>
        public Task<bool> Read() => Task.FromResult(IsOn);
    }
}