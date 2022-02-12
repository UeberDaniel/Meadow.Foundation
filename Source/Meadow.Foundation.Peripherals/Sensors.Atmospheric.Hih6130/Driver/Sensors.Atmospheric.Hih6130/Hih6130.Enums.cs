﻿namespace Meadow.Foundation.Sensors.Atmospheric
{
    public partial class Hih6130
    {
        /// <summary>
        ///     Valid addresses for the sensor.
        /// </summary>
        public enum Addresses : byte
        {
            /// <summary>
            /// Bus address 0x27
            /// </summary>
            Address_0x27 = 0x27,
            /// <summary>
            /// Default bus address
            /// </summary>
            Default = Address_0x27
        }
    }
}
