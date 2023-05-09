namespace Meadow.Foundation.Leds
{
    /// <summary>
    /// Defines a Light Emitting Diode (LED) controlled by a pulse-width-modulation
    /// (PWM) signal to limit current.
    /// </summary>
    public interface IPwmLed
    {
        /// <summary>
        /// Gets or sets a value indicating whether the LED is on.
        /// </summary>
        /// <value><c>true</c> if is on; otherwise, <c>false</c>.</value>
        bool IsOn { get; set; }

        /// <summary>
        /// Gets the brightness of the LED, controlled by a PWM signal
        /// </summary>
        public float Brightness { get; set; }
    }
}