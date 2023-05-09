namespace Meadow.Foundation.Leds
{
    /// <summary>
    /// Defines a Pulse-Width-Modulation (PWM) controlled RGB LED
    /// </summary>
    public interface IRgbPwmLed
    {
        /// <summary>
        /// Turns on LED with current color or turns it off
        /// </summary>
        bool IsOn { get; set; }

        /// <summary>
        /// Gets or sets the brightness of the LED, controlled by a PWM signal
        /// </summary>
        public float Brightness { get; set; }

        /// <summary>
        /// Gets or sets the current LED color
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Sets the current color of the LED with a specific brightness
        /// </summary>
        /// <param name="color">The LED color</param>
        /// <param name="brightness">Valid values are from 0 to 1, inclusive</param>
        public void SetColorWithBrightness(Color color, float brightness = 1);
    }
}