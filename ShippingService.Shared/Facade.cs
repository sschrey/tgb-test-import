using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;

namespace ShippingService.Shared
{
    public class Facade
    {
        

        public void SendTimInventory(IList<InventoryItem> inventory, Stream stream)
        {
            CompressedSerializer.Compress(inventory, CompressedSerializer.Serializer.XML, stream);
        }
    }
}
