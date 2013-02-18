using System;
using System.Collections.Generic;
using ShippingService.Business.Domain;
using ShippingService.Shared;
namespace ShippingService.Business.Dao
{
    public interface IShippingServiceDAO
    {
        IList<Order> GetOrders(OrderCriteria crit);
        IList<Carrier> GetCarriers();
        IList<Container> GetContainers();
        IList<CarrierMode> GetCarrierModes();
        void Pack(Order o);
        void Ship(Order o, bool updateE1);
        void ReShip(Order o);
        int GetNextTNTConsignmentNoteNumber();
        void SaveReturnLabel(Order o);
        void LogBarcodeScan(BarcodeScanLog log);
        void UpdateTrackingNumber(string oldTrackingNumber, string newTrackingNumber);

        #region E1Link
        IList<InventoryItem> GetTIMInventory();
        IList<OrderConfirmation> GetTIMOrderConfirmations();
        IList<ShippingConfirmation> GetTIMShippingConfirmations();
        void MarkTIMShippingConfirmation(string orderLineId);
        void MarkTIMOrderConfirmation(string orderId, string lineNumber);

        IList<InventoryItem> GetInventory(string appId);
        IList<InventoryItem> GetInventoryByBranch(string appId, string branch);
        IList<OrderConfirmation> GetOrderConfirmations(string appId);
        IList<ShippingConfirmation> GetShippingConfirmations(string appId);
        void MarkShippingConfirmation(string orderLineId);
        void MarkOrderConfirmation(string orderId, string lineNumber);
        #endregion


        
    }
}
