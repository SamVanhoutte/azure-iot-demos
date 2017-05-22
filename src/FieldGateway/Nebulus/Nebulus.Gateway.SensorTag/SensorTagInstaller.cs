using System;
using System.Collections.Generic;
using System.Text;
using Nebulus.IoT.Gateway.Core.Channel.Module;
using Nebulus.IoT.Gateway.Core.Channel.Receiver;
using StructureMap;

namespace Nebulus.Gateway.SensorTag
{
    public class SensorTagInstaller : Registry
    {
        public SensorTagInstaller()
        {
            For<IReceiver>().Add(() => new SensorTagReceiver()).Named("sensor-tag");
        }
    }
}
