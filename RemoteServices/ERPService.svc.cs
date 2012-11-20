using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ShippingService.Shared;
using System.IO;

namespace RemoteServices
{
    public class ERPService : IERPService
    {
        public byte[] GetInventory(string appId)
        {
            var list = ApplicationContextHolder.Instance.Facade.GetInventory(appId);
            return CompressedSerializer.Compress(list, CompressedSerializer.Serializer.XML);
        }

        public byte[] GetOrderConfirmations(string appId)
        {
            var list = ApplicationContextHolder.Instance.Facade.GetOrderConfirmations(appId);
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
