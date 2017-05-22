using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.Web.Http;
using Windows.Devices.Sensors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nebulus.Demos.SensorTag.Web
{
    public class SensorTagController : ApiController
    {
        private LightSensor lightSensor;
        private Gyrometer gyroMeter;
        private Magnetometer magnetometer;

        public SensorTagController()
        {
            lightSensor = LightSensor.GetDefault();
            gyroMeter = Gyrometer.GetDefault();
            magnetometer = Magnetometer.GetDefault();
        }

        // GET api/values 
        [Route("sensortag")]
        public SensorReading Get()
        {
            var reading = new SensorReading();
            if (lightSensor != null)
            {
                reading.Lux = lightSensor.GetCurrentReading().IlluminanceInLux;
            }
            if (gyroMeter != null)
            {
                var gyroReading = gyroMeter.GetCurrentReading();
                reading.GyroValue = new XYZValue
                {
                    X = gyroReading.AngularVelocityX,
                    Y = gyroReading.AngularVelocityY,
                    Z = gyroReading.AngularVelocityZ
                };
            }
            if (magnetometer != null)
            {
                var magReading = magnetometer.GetCurrentReading();
                reading.MagnetoValue = new XYZValue
                {
                    X = magReading.MagneticFieldX,
                    Y = magReading.MagneticFieldY,
                    Z = magReading.MagneticFieldZ
                };
            }
            Console.WriteLine("returning sensor reading: " + JsonConvert.SerializeObject(reading));
            
            return reading;
        }

    }

    public class XYZValue
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }



    public class SensorReading
    {
        public double Lux { get; set; }
        public XYZValue GyroValue { get; set; }
        public XYZValue MagnetoValue { get; set; }
    }
}
