using Meadow.Hardware;
using Meadow.Peripherals.Leds;
using Meadow.Units;
using System;

namespace Meadow.Foundation.Leds
{
    /// <summary>
    /// Represents a Pulse-Width-Modulation (PWM) controlled RGB LED
    /// </summary>
    public partial class RgbPwmLed
    {
        static readonly Frequency DefaultFrequency = new Frequency(200, Frequency.UnitType.Hertz);

        readonly float DEFAULT_DUTY_CYCLE = 0f;

        readonly double maxRedDutyCycle = 1;
        readonly double maxGreenDutyCycle = 1;
        readonly double maxBlueDutyCycle = 1;

        /// <summary>
        /// Maximum forward voltage (3.3 Volts)
        /// </summary>
        public Voltage MAX_FORWARD_VOLTAGE => new Voltage(3.3);

        /// <summary>
        /// Minimum forward voltage (0 Volts)
        /// </summary>
        public Voltage MIN_FORWARD_VOLTAGE => new Voltage(0);

        /// <summary>
        /// Turns on LED with current color or turns it off
        /// </summary>
        public bool IsOn
        {
            get => isOn;
            set
            {
                Brightness = value ? 1 : 0;
                isOn = value;
            }
        }
        bool isOn;

        /// <summary>
        /// Gets or sets the current LED color
        /// </summary>
        public Color Color
        {
            get => color;
            set
            {
                RedPwmPort.DutyCycle = (float)(Color.R / 255.0 * maxRedDutyCycle * brightness);
                GreenPwmPort.DutyCycle = (float)(Color.G / 255.0 * maxGreenDutyCycle * brightness);
                BluePwmPort.DutyCycle = (float)(Color.B / 255.0 * maxBlueDutyCycle * brightness);
            }
        }
        private Color color = Color.White;

        /// <summary>
        /// Gets or sets the brightness of the LED, controlled by a PWM signal
        /// </summary>
        public float Brightness
        {
            get => brightness;
            set
            {
                brightness = Math.Clamp(value, 0, 1);
                Color = Color;
            }
        }
        private float brightness = 1f;

        /// <summary>
        /// The red LED port
        /// </summary>
        protected IPwmPort RedPwmPort { get; set; }

        /// <summary>
        /// The blue LED port
        /// </summary>
        protected IPwmPort BluePwmPort { get; set; }

        /// <summary>
        /// The green LED port
        /// </summary>
        protected IPwmPort GreenPwmPort { get; set; }

        /// <summary>
        /// The common type (common annode or common cathode)
        /// </summary>
        public CommonType Common { get; protected set; }

        /// <summary>
        /// The red LED forward voltage
        /// </summary>
        public Voltage RedForwardVoltage { get; protected set; }

        /// <summary>
        /// The green LED forward voltage
        /// </summary>
        public Voltage GreenForwardVoltage { get; protected set; }

        /// <summary>
        /// The blue LED forward voltage
        /// </summary>
        public Voltage BlueForwardVoltage { get; protected set; }

        /// <summary>
        /// Create instance of RgbPwmLed 
        /// </summary>
        /// <param name="redPwmPort">The PWM port for the red LED</param>
        /// <param name="greenPwmPort">The PWM port for the green LED</param>
        /// <param name="bluePwmPort">The PWM port for the blue LED</param>
        /// <param name="commonType">Common annode or common cathode</param>
        public RgbPwmLed(
            IPwmPort redPwmPort,
            IPwmPort greenPwmPort,
            IPwmPort bluePwmPort,
            CommonType commonType = CommonType.CommonCathode)
        {
            RedPwmPort = redPwmPort;
            GreenPwmPort = greenPwmPort;
            BluePwmPort = bluePwmPort;

            RedForwardVoltage = TypicalForwardVoltage.Red;
            GreenForwardVoltage = TypicalForwardVoltage.Green;
            BlueForwardVoltage = TypicalForwardVoltage.Blue;

            Common = commonType;

            maxRedDutyCycle = Helpers.CalculateMaximumDutyCycle(RedForwardVoltage);
            maxGreenDutyCycle = Helpers.CalculateMaximumDutyCycle(GreenForwardVoltage);
            maxBlueDutyCycle = Helpers.CalculateMaximumDutyCycle(BlueForwardVoltage);

            ResetPwmPorts();
        }

        /// <summary>
        /// Create instance of RgbPwmLed 
        /// </summary>
        /// <param name="redPwmPin">The PWM pin for the red LED</param>
        /// <param name="greenPwmPin">The PWM pin for the green LED</param>
        /// <param name="bluePwmPin">The PWM pin for the blue LED</param>
        /// <param name="commonType">Common annode or common cathode</param>
        public RgbPwmLed(
            IPin redPwmPin,
            IPin greenPwmPin,
            IPin bluePwmPin,
            CommonType commonType = CommonType.CommonCathode) :
            this(
                redPwmPin.CreatePwmPort(DefaultFrequency),
                greenPwmPin.CreatePwmPort(DefaultFrequency),
                bluePwmPin.CreatePwmPort(DefaultFrequency),
                commonType)
        { }

        /// <summary>
        /// Create instance of RgbPwmLed 
        /// </summary>
        /// <param name="redPwmPin">The PWM pin for the red LED</param>
        /// <param name="greenPwmPin">The PWM pin for the green LED</param>
        /// <param name="bluePwmPin">The PWM pin for the blue LED</param>
        /// <param name="redLedForwardVoltage">The forward voltage for the red LED</param>
        /// <param name="greenLedForwardVoltage">The forward voltage for the green LED</param>
        /// <param name="blueLedForwardVoltage">The forward voltage for the blue LED</param>
        /// <param name="commonType">Common annode or common cathode</param>
        public RgbPwmLed(
            IPin redPwmPin,
            IPin greenPwmPin,
            IPin bluePwmPin,
            Voltage redLedForwardVoltage,
            Voltage greenLedForwardVoltage,
            Voltage blueLedForwardVoltage,
            CommonType commonType = CommonType.CommonCathode) :
            this(
                redPwmPin.CreatePwmPort(DefaultFrequency),
                greenPwmPin.CreatePwmPort(DefaultFrequency),
                bluePwmPin.CreatePwmPort(DefaultFrequency),
                redLedForwardVoltage,
                greenLedForwardVoltage,
                blueLedForwardVoltage,
                commonType)
        { }

        /// <summary>
        /// Create instance of RgbPwmLed
        /// </summary>
        /// <param name="redPwmPort">The PWM port for the red LED</param>
        /// <param name="greenPwmPort">The PWM port for the green LED</param>
        /// <param name="bluePwmPort">The PWM port for the blue LED</param>
        /// <param name="redLedForwardVoltage">The forward voltage for the red LED</param>
        /// <param name="greenLedForwardVoltage">The forward voltage for the green LED</param>
        /// <param name="blueLedForwardVoltage">The forward voltage for the blue LED</param>
        /// <param name="commonType">Common annode or common cathode</param>
        public RgbPwmLed(
            IPwmPort redPwmPort,
            IPwmPort greenPwmPort,
            IPwmPort bluePwmPort,
            Voltage redLedForwardVoltage,
            Voltage greenLedForwardVoltage,
            Voltage blueLedForwardVoltage,
            CommonType commonType = CommonType.CommonCathode)
        {
            ValidateForwardVoltages(redLedForwardVoltage, greenLedForwardVoltage, blueLedForwardVoltage);

            RedForwardVoltage = redLedForwardVoltage;
            GreenForwardVoltage = greenLedForwardVoltage;
            BlueForwardVoltage = blueLedForwardVoltage;

            Common = commonType;

            RedPwmPort = redPwmPort;
            GreenPwmPort = greenPwmPort;
            BluePwmPort = bluePwmPort;

            maxRedDutyCycle = Helpers.CalculateMaximumDutyCycle(RedForwardVoltage);
            maxGreenDutyCycle = Helpers.CalculateMaximumDutyCycle(GreenForwardVoltage);
            maxBlueDutyCycle = Helpers.CalculateMaximumDutyCycle(BlueForwardVoltage);

            ResetPwmPorts();
        }

        /// <summary>
        /// Validates forward voltages to ensure they're within the range MIN_FORWARD_VOLTAGE to MAX_FORWARD_VOLTAGE
        /// </summary>
        /// <param name="redLedForwardVoltage">The forward voltage for the red LED</param>
        /// <param name="greenLedForwardVoltage">The forward voltage for the green LED</param>
        /// <param name="blueLedForwardVoltage">The forward voltage for the blue LED</param>
        protected void ValidateForwardVoltages(
            Voltage redLedForwardVoltage,
            Voltage greenLedForwardVoltage,
            Voltage blueLedForwardVoltage)
        {
            if (redLedForwardVoltage < MIN_FORWARD_VOLTAGE || redLedForwardVoltage > MAX_FORWARD_VOLTAGE)
            {
                throw new ArgumentOutOfRangeException(nameof(redLedForwardVoltage), "error, forward voltage must be between 0, and 3.3");
            }

            if (greenLedForwardVoltage < MIN_FORWARD_VOLTAGE || greenLedForwardVoltage > MAX_FORWARD_VOLTAGE)
            {
                throw new ArgumentOutOfRangeException(nameof(greenLedForwardVoltage), "error, forward voltage must be between 0, and 3.3");
            }

            if (blueLedForwardVoltage < MIN_FORWARD_VOLTAGE || blueLedForwardVoltage > MAX_FORWARD_VOLTAGE)
            {
                throw new ArgumentOutOfRangeException(nameof(blueLedForwardVoltage), "error, forward voltage must be between 0, and 3.3");
            }
        }

        /// <summary>
        /// Resets all PWM ports
        /// </summary>
        protected void ResetPwmPorts()
        {
            RedPwmPort.Frequency = GreenPwmPort.Frequency = BluePwmPort.Frequency = DefaultFrequency;
            RedPwmPort.DutyCycle = GreenPwmPort.DutyCycle = BluePwmPort.DutyCycle = DEFAULT_DUTY_CYCLE;

            // invert the PWM signal if it common anode
            RedPwmPort.Inverted = GreenPwmPort.Inverted = BluePwmPort.Inverted = Common == CommonType.CommonAnode;

            RedPwmPort.Start();
            GreenPwmPort.Start();
            BluePwmPort.Start();
        }

        /// <summary>
        /// Sets the current color of the LED
        /// </summary>
        /// <param name="color">The LED color</param>
        /// <param name="brightness">Valid values are from 0 to 1, inclusive</param>
        public void SetColorWithBrightness(Color color, float brightness = 1)
        {
            if (color == Color && brightness == Brightness)
            {
                return;
            }

            Color = color;
            Brightness = brightness;
        }
    }
}