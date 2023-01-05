namespace Meadow.Foundation.Sensors.Motion
{
    public partial class Tlv493d
    {
        /// <summary>
        /// Valid I2C addresses for the sensor
        /// </summary>
        public enum Addresses : byte
        {
            /// <summary>
            /// Bus address 0x5E
            /// </summary>
            Address_0x5E = 0x5E,
            /// <summary>
            /// Bus address 0x1F
            /// </summary>
            Address_0x1F = 0x1F,
            /// <summary>
            /// Default bus address
            /// </summary>
            Default = Address_0x5E
        }
    }
}