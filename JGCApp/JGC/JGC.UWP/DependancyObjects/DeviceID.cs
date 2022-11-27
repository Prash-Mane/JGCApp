using JGC.Common.Interfaces;
using JGC.UWP.DependancyObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.System.Profile;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceID))]
namespace JGC.UWP.DependancyObjects
{
    public class DeviceID : IDeviceID
    {
        public async Task<string> GetDeviceID()
        {
            string id = null;

                try
                {
                    if (ApiInformation.IsTypePresent("Windows.System.Profile.SystemIdentification"))
                    {
                        var systemId = SystemIdentification.GetSystemIdForPublisher();

                        // Make sure this device can generate the IDs
                        if (systemId.Source != SystemIdentificationSource.None)
                        {
                            // The Id property has a buffer with the unique ID
                            var hardwareId = systemId.Id;
                            var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(hardwareId);

                            var bytes = new byte[hardwareId.Length];
                            dataReader.ReadBytes(bytes);

                            id = Convert.ToBase64String(bytes);
                        }
                    }

                    if (id == null && ApiInformation.IsTypePresent("Windows.System.Profile.HardwareIdentification"))
                    {
                        var token = HardwareIdentification.GetPackageSpecificToken(null);
                        var hardwareId = token.Id;
                        var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(hardwareId);

                        var bytes = new byte[hardwareId.Length];
                        dataReader.ReadBytes(bytes);

                        id = Convert.ToBase64String(bytes);
                    }

                    if (id == null)
                    {
                        id = "unsupported";
                    }

                }
                catch (Exception)
                {
                    id = "unsupported";
                }

                return id;
         
        
        }
    }
}
