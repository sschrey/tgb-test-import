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
    // NOTE: If you change the class name "TIMService" here, you must also update the reference to "TIMService" in Web.config.
    public class TIMService : ITIMService
    {
        public byte[] GetInventory()
        {
            var list = ApplicationContextHolder.Instance.Facade.GetTIMInventory();
            var tmp = CompressedSerializer.Compress(list, CompressedSerializer.Serializer.XML);
            return tmp;
        }

        public byte[] GetOrderConfirmations()
        {
            var list = ApplicationContextHolder.Instance.Facade.GetTIMOrderConfirmations();
            return CompressedSerializer.Compress(list, CompressedSerializer.Serializer.XML);
        }

        public bool MarkOrderConfirmation(string orderId, string lineNumber, out string error)
        {
            error = null;
            try
            {
                ApplicationContextHolder.Instance.Facade.MarkTIMOrderConfirmation(orderId, lineNumber);
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
                ApplicationContextHolder.Instance.Facade.MarkTIMShippingConfirmation(orderLineId);
            }
            catch (Exception exc)
            {
                error = exc.Message;
                return false;
            }
            return true;
        }

        public byte[] GetShippingConfirmations()
        {
            var list = ApplicationContextHolder.Instance.Facade.GetTIMShippingConfirmations();
            return CompressedSerializer.Compress(list, CompressedSerializer.Serializer.XML);
        }
    }
}
