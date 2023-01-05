using Meadow.Hardware;
using Meadow.Peripherals.Sensors.Motion;
using Meadow.Units;
using Meadow.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Meadow.Foundation.Sensors.Motion
{
    /// <summary>
    /// Represents the TLV493D Three-Axis, Digital Magnetometer
    /// </summary>
    public partial class Tlv493d :
        ByteCommsSensorBase<MagneticField3D>, IMagnetometer
    {
        /// <summary>
        /// Raised when the magnetic field value changes
        /// </summary>
        public event EventHandler<IChangeResult<MagneticField3D>> MagneticField3dUpdated = delegate { };

        /// <summary>
        /// Raised when the temperature value changes
        /// </summary>
        public event EventHandler<IChangeResult<Units.Temperature>> TemperatureUpdated = delegate { };

        /// <summary>
        /// The current magnetic field value
        /// </summary>
        public MagneticField3D? MagneticField3d => Conditions;

        /// <summary>
        /// Create a new Tlv493d object using the default parameters for the component
        /// </summary>
        /// <param name="address">Address of the Tvl493d</param>
        /// <param name="i2cBus">I2C bus object</param>        
        public Tlv493d(II2cBus i2cBus, byte address = (byte)Addresses.Default)
            : base(i2cBus, address, 10, 8)
        {
            Initialize();
        }

        void Initialize()
        {
            Thread.Sleep(1); //only needs 200 uSec

            CopyFactorySettings();

            SetParody(true);
        }

        void SetParody(bool enable)
        {
            SetRegisterBit(Registers.Mode1, 7, enable);
        }

        void CopyFactorySettings()
        {
            //copy Factory settings 1 
            var factory = Peripheral.ReadRegister(Registers.FactSet1);
            var value = Peripheral.ReadRegister(Registers.Mode1);
            BitHelpers.SetBit(value, 3, BitHelpers.GetBitValue(factory, 3));
            BitHelpers.SetBit(value, 4, BitHelpers.GetBitValue(factory, 4));
            Peripheral.WriteRegister(Registers.Mode1, value);

            //copy Factory settings 2
            Peripheral.WriteRegister(Registers.Res0, Peripheral.ReadRegister(Registers.FactSet2));

            //copy Factory settings 3
            factory = Peripheral.ReadRegister(Registers.FactSet3);
            value = Peripheral.ReadRegister(Registers.Mode2);

            for(int i = 0; i < 5; i++)
            {
                BitHelpers.SetBit(value, i, BitHelpers.GetBitValue(factory, i));
            }
            Peripheral.WriteRegister(Registers.Mode2, value);
        }

        /// <summary>
        /// Reset the sensor
        /// </summary>
        public void Reset()
        {

        }

        void SetRegisterBit(byte register, byte bitIndex, bool enabled = true)
        {
            var value = Peripheral.ReadRegister(register);
            value = BitHelpers.SetBit(value, bitIndex, enabled);
            Peripheral.WriteRegister(register, value);
        }

        bool GetRegisterBit(byte register, byte bitIndex)
        {
            var value = Peripheral.ReadRegister(register);
            return BitHelpers.GetBitValue(value, bitIndex);
        }

        /// <summary>
        /// Raise events for subcribers and notify of value changes
        /// </summary>
        /// <param name="changeResult">The updated sensor data</param>
        protected override void RaiseEventsAndNotify(IChangeResult<MagneticField3D> changeResult)
        {
            if (changeResult is { } mag)
            {
                MagneticField3dUpdated?.Invoke(this, new ChangeResult<MagneticField3D>(mag.New, changeResult.Old));
            }
            base.RaiseEventsAndNotify(changeResult);
        }


        /// <summary>
        /// Reads data from the sensor
        /// </summary>
        /// <returns>The latest sensor reading</returns>
        protected override Task<MagneticField3D> ReadSensor()
        {
            return Task.Run(async () =>
            {
                MagneticField3D conditions;

             //   Peripheral.ReadRegister(Registers.OUT_X_L, ReadBuffer.Span[0..9]); //9 bytes

                int x = (int)((uint)(ReadBuffer.Span[0] << 12) | (uint)(ReadBuffer.Span[1] << 4) | (uint)(ReadBuffer.Span[6] >> 4));
                int y = (int)((uint)(ReadBuffer.Span[2] << 12) | (uint)(ReadBuffer.Span[3] << 4) | (uint)(ReadBuffer.Span[7] >> 4));
                int z = (int)((uint)(ReadBuffer.Span[4] << 12) | (uint)(ReadBuffer.Span[5] << 4) | (uint)(ReadBuffer.Span[8] >> 4));

                int offset = 1 << 19;

                x -= offset;
                y -= offset;
                z -= offset;

                conditions = new MagneticField3D(
                    new MagneticField(x * 0.00625, MagneticField.UnitType.MicroTesla),
                    new MagneticField(y * 0.00625, MagneticField.UnitType.MicroTesla),
                    new MagneticField(z * 0.00625, MagneticField.UnitType.MicroTesla)
                    );

                return conditions;
            });
        }

        /// <summary>
        /// Read the sensor temperature
        /// Doesn't work in continuous mode
        /// </summary>
        /// <returns></returns>
      //  public async Task<Units.Temperature> ReadTemperature()
      //  {
        //    return new Units.Temperature((sbyte)Peripheral.ReadRegister(Registers.TEMPERATURE) * 0.8 - 75, Units.Temperature.UnitType.Celsius);
      //  }
    }
}