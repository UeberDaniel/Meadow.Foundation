namespace Meadow.Foundation.Leds
{
    /// <summary>
    /// Defines a simple Light Emitting Diode (LED).
    /// </summary>
    public interface ILed
    {
        /// <summary>
        /// Gets or sets a value indicating whether the LED is on.
        /// </summary>
        /// <value><c>true</c> if is on; otherwise, <c>false</c>.</value>
        bool IsOn { get; set; }
    }
}