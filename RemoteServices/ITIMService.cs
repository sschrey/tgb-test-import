using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace RemoteServices
{
    [ServiceContract]
    public interface ITIMService
    {
        [OperationContract]
        byte[] GetInventory();

        [OperationContract]
        byte[] GetOrderConfirmations();

        [OperationContract]
        bool MarkOrderConfirmation(string orderId, string lineNumber, out string error);

        [OperationContract]
        bool MarkShippingConfirmation(string orderId, out string error);

        [OperationContract]
        byte[] GetShippingConfirmations();

    }
}
