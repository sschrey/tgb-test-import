using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShippingService.Shared;
using System.Configuration;

namespace RemoteServices.Client
{
    public class Facade
    {
        public IList<InventoryItem> GetTIMInventory()
        {
            var client = GetTIMClient();
            
            var res = client.GetInventory();

            client.CloseConnection();

            return CompressedSerializer.Decompress<List<InventoryItem>>(res, CompressedSerializer.Serializer.XML);
        }

        public IList<InventoryItem> GetInventory(string appId)
        {
            var client = GetClient();

            var res = client.GetInventory(appId);

            client.CloseConnection();

            return CompressedSerializer.Decompress<List<InventoryItem>>(res, CompressedSerializer.Serializer.XML);
        }

        public IList<InventoryItem> GetInventoryByBranch(string appId, string branch)
        {
            var client = GetClient();

            var res = client.GetInventoryByBranch(appId, branch);

            client.CloseConnection();

            return CompressedSerializer.Decompress<List<InventoryItem>>(res, CompressedSerializer.Serializer.XML);
        }

        public IList<OrderConfirmation> GetTIMOrderConfirmations()
        {
            var client = GetTIMClient();

            var res = client.GetOrderConfirmations();

            return CompressedSerializer.Decompress<List<OrderConfirmation>>(res, CompressedSerializer.Serializer.XML);
        }

        public IList<ERPOrder> GetERPOrders(string orderIds)
        {
            var client = GetClient();

            var res = client.GetOrders(orderIds);

            return CompressedSerializer.Decompress<List<ERPOrder>>(res, CompressedSerializer.Serializer.XML);
        }

        public IList<OrderConfirmation> GetOrderConfirmations(string appId)
        {
            var client = GetClient();

            var res = client.GetOrderConfirmations(appId);

            return CompressedSerializer.Decompress<List<OrderConfirmation>>(res, CompressedSerializer.Serializer.XML);
        }


        public IList<ShippingConfirmation> GetTIMShippingConfirmations()
        {
            var client = GetTIMClient();

            var res = client.GetShippingConfirmations();

            return CompressedSerializer.Decompress<List<ShippingConfirmation>>(res, CompressedSerializer.Serializer.XML);
        }

        public IList<ShippingConfirmation> GetShippingConfirmations(string appId)
        {
            var client = GetClient();

            var res = client.GetShippingConfirmations(appId);

            return CompressedSerializer.Decompress<List<ShippingConfirmation>>(res, CompressedSerializer.Serializer.XML);
        }

        public bool MarkTIMShippingConfirmation(string orderId, out string error)
        {
            var client = GetTIMClient();

            var res = client.MarkShippingConfirmation(out error, orderId);

            client.CloseConnection();

            return res;
        }

        public bool MarkShippingConfirmation(string orderId, out string error)
        {
            var client = GetClient();

            var res = client.MarkShippingConfirmation(out error, orderId);

            client.CloseConnection();

            return res;
        }

        public bool MarkTIMOrderConfirmation(string orderId, string lineNumber, out string error)
        {
            var client = GetTIMClient();

            var res = client.MarkOrderConfirmation(out error, orderId, lineNumber);

            client.CloseConnection();

            return res;
        }

        public bool MarkOrderConfirmation(string orderId, string lineNumber, out string error)
        {
            var client = GetClient();

            var res = client.MarkOrderConfirmation(out error, orderId, lineNumber);

            client.CloseConnection();

            return res;
        }

        private TIMService.TIMServiceClient GetTIMClient()
        {
            TIMService.TIMServiceClient client = new TIMService.TIMServiceClient();

            client.ClientCredentials.Windows.ClientCredential = new System.Net.NetworkCredential(
                ConfigurationManager.AppSettings["WebServiceUserName"],
                ConfigurationManager.AppSettings["WebServicePassword"],
                ConfigurationManager.AppSettings["WebServiceDomain"]
                );

            return client;
        }

        private ERPService.ERPServiceClient GetClient()
        {
            ERPService.ERPServiceClient client = new ERPService.ERPServiceClient();

            client.ClientCredentials.Windows.ClientCredential = new System.Net.NetworkCredential(
                ConfigurationManager.AppSettings["WebServiceUserName"],
                ConfigurationManager.AppSettings["WebServicePassword"],
                ConfigurationManager.AppSettings["WebServiceDomain"]
                );

            return client;
        }

        
    }
}
