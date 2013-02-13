using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RemoteServices
{    
    [ServiceContract]
    public interface IERPService
    {
        [OperationContract]
        byte[] GetInventory(string appId);

        [OperationContract]
        byte[] GetOrderConfirmations(string appId);

        [OperationContract]
        byte[] GetOrders(string orderIds);

        [OperationContract]
        bool MarkOrderConfirmation(string orderId, string lineNumber, out string error);

        [OperationContract]
        bool MarkShippingConfirmation(string orderId, out string error);

        [OperationContract]
        byte[] GetShippingConfirmations(string appId);

    }
}
