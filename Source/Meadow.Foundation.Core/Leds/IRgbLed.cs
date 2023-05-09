namespace Meadow.Foundation.Leds
{
    /// <summary>
    /// Defines an RGB Light Emitting Diode (LED).
    /// </summary>
    public interface IRgbLed
    {
        /// <summary>
        /// Gets or sets a value indicating whether the LED is on.
        /// </summary>
        bool IsOn { get; set; }

        /// <summary>
        /// Gets or sets a color to the RGB LED
        /// </summary>
        public RgbLedColors Color { get; set; }
    }
}