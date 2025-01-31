﻿using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Threading.Tasks;

namespace Meadow.Foundation.Sensors.Weather
{
    /// <summary>
    /// Represents a simple switching rain gauge
    /// </summary>
    public partial class SwitchingRainGauge : SamplingSensorBase<Length>
    {
        private readonly IDigitalInterruptPort rainGaugePort;
        private DateTime lastUpdated = DateTime.MinValue;

        /// <summary>
        /// The number of times the rain tilt sensor has triggered
        /// This count is multiplied by the depth per click to
        /// calculate the rain depth
        /// </summary>
        public int ClickCount { get; protected set; }

        /// <summary>
        /// The total accumulated rain depth
        /// </summary>
        public Length RainDepth => DepthPerClick * ClickCount;

        /// <summary>
        /// The amount of rain recorded per raingauge event
        /// </summary>
        public Length DepthPerClick { get; set; }

        /// <summary>
        /// Create a new SwitchingRainGauge object with a default depth of 0.2794 per click
        /// </summary>
        /// <param name="rainSensorPin">The rain sensor pin</param>
        public SwitchingRainGauge(IPin rainSensorPin) :
            this(rainSensorPin, new Length(0.2794, Length.UnitType.Millimeters))
        { }

        /// <summary>
        /// Create a new SwitchingRainGauge object
        /// </summary>
        /// <param name="rainSensorPin">The rain sensor pin</param>
        /// <param name="depthPerClick">The depth per click</param>
        public SwitchingRainGauge(IPin rainSensorPin, Length depthPerClick) :
            this(rainSensorPin.CreateDigitalInterruptPort(InterruptMode.EdgeRising, ResistorMode.InternalPullUp, TimeSpan.FromMilliseconds(100), TimeSpan.Zero), depthPerClick)
        { }

        /// <summary>
        /// Create a new SwitchingRainGauge object
        /// </summary>
        /// <param name="rainSensorPort">The port for the rain sensor pin</param>
        /// <param name="depthPerClick">The depth per click</param>
        public SwitchingRainGauge(IDigitalInterruptPort rainSensorPort, Length depthPerClick)
        {
            DepthPerClick = depthPerClick;
            rainGaugePort = rainSensorPort;
        }

        /// <summary>
        /// Reset the rain height
        /// </summary>
        public void Reset()
        {
            ClickCount = 0;
        }

        private void RainSensorPortChanged(object sender, DigitalPortResult e)
        {
            ClickCount++;

            ChangeResult<Length> changeResult = new ChangeResult<Length>()
            {
                New = RainDepth,
                Old = RainDepth - DepthPerClick, //last reading, ClickCount will always be at least 1
            };

            if (DateTime.Now - lastUpdated >= UpdateInterval)
            {
                lastUpdated = DateTime.Now;
                RaiseEventsAndNotify(changeResult);
            }
        }

        /// <summary>
        /// Start the sensor
        /// </summary>
        public override void StartUpdating(TimeSpan? updateInterval = null)
        {
            lock (samplingLock)
            {
                if (IsSampling) { return; }

                IsSampling = true;
                rainGaugePort.Changed += RainSensorPortChanged;
            }
        }

        /// <summary>
        /// Stops sampling the sensor
        /// </summary>
        public override void StopUpdating()
        {
            lock (samplingLock)
            {
                if (!IsSampling) { return; }

                IsSampling = false;
                rainGaugePort.Changed -= RainSensorPortChanged;
            }
        }

        /// <summary>
        /// Convenience method to get the current rain depth
        /// </summary>
        protected override Task<Length> ReadSensor()
        {
            if (IsSampling == false)
            {
                throw new Exception("You must call StartUpdating before SwitchingRainGauge can track rain depth");
            }
            return Task.FromResult(RainDepth);
        }
    }
}