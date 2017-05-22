using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using SensorTag;

namespace Nebulus.Demos.SensorTag
{
    public class SensorTagClient
    {
        global::SensorTag.SensorTag sensor;
        private string _btConnectionString;
        public static double LightIntensity { get; set; }


        public SensorTagClient(string bluetoothDevice)
        {
            _btConnectionString = bluetoothDevice;
        }

        public async Task<bool> Connect()
        {
            // find a matching sensor
            // todo: let user choose which one to play with.
            var deviceList = await global::SensorTag.SensorTag.FindAllDevices();
            foreach (var sensorTag in deviceList)
            {
                if (!sensorTag.DeviceAddress.Equals(_btConnectionString,
                    StringComparison.InvariantCultureIgnoreCase)) continue;
                sensor = sensorTag;
                return true;
            }
            return sensor != null;
        }

        public async Task RegisterLightIntensity(bool register)
        {
            try
            {
                if (register)
                {
                    await sensor.LightIntensity.StartReading();
                    sensor.LightIntensity.LightMeasurementValueChanged -= OnLightMeasurementValueChanged;
                    sensor.LightIntensity.LightMeasurementValueChanged += OnLightMeasurementValueChanged;
                }
                else
                {
                    await sensor.LightIntensity.StopReading();
                    sensor.LightIntensity.LightMeasurementValueChanged -= OnLightMeasurementValueChanged;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("### Error registering LightIntensity: " + ex.Message);
            }
        }

        private void OnLightMeasurementValueChanged(object sender, LightIntensityMeasurementEventArgs e)
        {
            var m = e.Measurement;
            LightIntensity = m.Lux;
        }
    }
}
