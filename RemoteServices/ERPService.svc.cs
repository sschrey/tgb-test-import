using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ShippingService.Shared;
using System.IO;
using System.Xml.Serialization;

namespace RemoteServices
{
    public class ERPService : IERPService
    {
        public byte[] GetInventory(string appId)
        {
            var list = ApplicationContextHolder.Instance.Facade.GetInventory(appId);
            return CompressedSerializer.Compress(list, CompressedSerializer.Serializer.XML);
        }

        public byte[] GetInventoryByBranch(string appId, string branch, bool inclCrossRefData = false)
        {
            var list = ApplicationContextHolder.Instance.Facade.GetInventoryByBranch(appId, branch, inclCrossRefData);
            var tmp = CompressedSerializer.Compress(list, CompressedSerializer.Serializer.XML);


            /* Note LVDS: remove code below from comment if you want to temporary save the xml output to a text file  */

            //FileStream fs = new FileStream(@"c:\temp\test.zip", FileMode.CreateNew);

            //XmlSerializer ser = new XmlSerializer(list.GetType());
            //ser.Serialize(fs, list);

            
            ////fs.Write(tmp, 0, tmp.Length);
            //fs.Close();
            //fs.Dispose();


            return tmp;
        }

        public byte[] GetOrderConfirmations(string appId)
        {
            var list = ApplicationContextHolder.Instance.Facade.GetOrderConfirmations(appId);
            return CompressedSerializer.Compress(list, CompressedSerializer.Serializer.XML);
        }

        public byte[] GetOrders(string orderIds)
        {
            var list = ApplicationContextHolder.Instance.Facade.GetERPOrders(orderIds);
            return CompressedSerializer.Compress(list, CompressedSerializer.Serializer.XML);
        }

        public bool MarkOrderConfirmation(string orderId, string lineNumber, out string error)
        {
            error = null;
            try
            {
                ApplicationContextHolder.Instance.Facade.MarkOrderConfirmation(orderId, lineNumber);
            }
            catch (Exception exc)
            {
                error = exc.Message;
                return false;
            }
            return true;
        }

        public bool MarkShippingConfirmation(string orderLineId, out string error)
        {
            error = null;
            try
            {
                ApplicationContextHolder.Instance.Facade.MarkShippingConfirmation(orderLineId);
            }
            catch (Exception exc)
            {
                error = exc.Message;
                return false;
            }
            return true;
        }

        public byte[] GetShippingConfirmations(string appId)
        {
            var list = ApplicationContextHolder.Instance.Facade.GetShippingConfirmations(appId);
            return CompressedSerializer.Compress(list, CompressedSerializer.Serializer.XML);
        }
    }
}
