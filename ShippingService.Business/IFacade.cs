using System;
using ShippingService.Business.Domain;
using System.Collections.Generic;
using ShippingService.Shared;

namespace ShippingService.Business
{
    public interface IFacade
    {
        Carrier GetCarrierById(string id);
        IList<Carrier> GetCarriers();
        Container GetContainerById(string id);
        IList<Container> GetContainers();
        Order GetTodoOrderById(string id);
        IList<Order> GetTodoOrders();
        IList<CarrierMode> GetCarrierModes();
        IList<CarrierMode> GetCarrierModes(string carrier, IList<CarrierMode> carriermodes = null, IList<CarrierModeFilter> carriermodefilters = null);
        IList<CarrierModeFilter> GetCarrierModeFilters();
        CarrierMode GetCarrierModeById(string id);
        void Pack(Order o);
        void Ship(Order o, bool updateE1);
        void ReShip(Order o);
        void SaveReturnLabel(Order o);
        int GetNextTNTConsignmentNoteNumber();
        IList<Order> GetOrders(OrderCriteria crit);
        IList<ERPOrder> GetERPOrders(string orderIds);
        void LogBarcodeScan(BarcodeScanLog log);
        void UpdateTrackingNumber(string oldTrackingNumber, string newTrackingNumber);
        void UnPack(string orderid);
        #region E1Link
        IList<InventoryItem> GetTIMInventory();
        IList<OrderConfirmation> GetTIMOrderConfirmations();
        IList<ShippingConfirmation> GetTIMShippingConfirmations();
        void MarkTIMShippingConfirmation(string orderId);
        void MarkTIMOrderConfirmation(string orderId, string lineNumber);

        IList<InventoryItem> GetInventory(string appId);
        IList<InventoryItem> GetInventoryByBranch(string appId, string branch, bool inclCrossRefData = false);
        IList<OrderConfirmation> GetOrderConfirmations(string appId);
        IList<ShippingConfirmation> GetShippingConfirmations(string appId);
        void MarkShippingConfirmation(string orderLineId);
        void MarkOrderConfirmation(string orderId, string lineNumber);
        #endregion
        
    }
}
