using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShippingService.Business.Domain;
using ShippingService.Business.Dao;
using Spring.Transaction.Interceptor;
using ShippingService.Shared;

namespace ShippingService.Business
{
    public class Facade : IFacade
    {
        public virtual IShippingServiceDAO Dao {get;set;}

        public Order GetTodoOrderById(string id)
        {
            var oc = new OrderCriteria();
            oc.E1Status = "556";
            oc.Id = id;

            return Dao.GetOrders(oc).FirstOrDefault();
        }
        public IList<Carrier> GetCarriers()
        {
            return Dao.GetCarriers();
        }

        public Carrier GetCarrierById(string id)
        {
            return GetCarriers().FirstOrDefault(c => c.Id == id);
        }

        public CarrierMode GetCarrierModeById(string id)
        {
            return GetCarrierModes().FirstOrDefault(cm => cm.Id == id);
        }

        
        public Container GetContainerById(string id)
        {
            return GetContainers().FirstOrDefault(c => c.Id == id);
        }

        public IList<Order> GetOrders(OrderCriteria crit)
        {
            return Dao.GetOrders(crit);
        }

        public IList<Order> GetTodoOrders()
        {
            var oc = new OrderCriteria();
            oc.E1Status = "556";

            return Dao.GetOrders(oc);
        }

        public IList<Container> GetContainers()
        {
            return Dao.GetContainers();
        }

        public IList<CarrierMode> GetCarrierModes()
        {
            return Dao.GetCarrierModes();
        }

        [Transaction]
        public void Pack(Order o)
        {
            Dao.Pack(o);
        }

        [Transaction]
        public void Ship(Order o, bool updateE1)
        {
            Dao.Ship(o, updateE1);
            
        }

        [Transaction]
        public void ReShip(Order o)
        {
            Dao.ReShip(o);
        }

        [Transaction]
        public void SaveReturnLabel(Order o)
        {
            Dao.SaveReturnLabel(o);
        }

        [Transaction]
        public int GetNextTNTConsignmentNoteNumber()
        {
            return Dao.GetNextTNTConsignmentNoteNumber();
        }

        #region E1Link
        public IList<InventoryItem> GetTIMInventory()
        {
            return Dao.GetTIMInventory();
        }

        public IList<InventoryItem> GetInventory(string appId)
        {
            return Dao.GetInventory(appId);
        }

        public IList<ERPOrder> GetERPOrders(string orderIds)
        {
            IList<Order> orders = GetOrders(new Domain.OrderCriteria() { Ids = orderIds.Split(';') });

            return Mapping.MappableEntity<Order, ERPOrder>.Map(orders);
        }

       


        public IList<OrderConfirmation> GetTIMOrderConfirmations()
        {
            return Dao.GetTIMOrderConfirmations();
        }

        public IList<OrderConfirmation> GetOrderConfirmations(string appId)
        {
            return Dao.GetOrderConfirmations(appId);
        }

        [Transaction]
        public void MarkOrderConfirmation(string orderId, string lineNumber)
        {
            Dao.MarkOrderConfirmation(orderId, lineNumber);
        }


        [Transaction]
        public void MarkTIMOrderConfirmation(string orderId, string lineNumber)
        {
            Dao.MarkTIMOrderConfirmation(orderId, lineNumber);
        }

        [Transaction]
        public void MarkShippingConfirmation(string orderLineId)
        {
            Dao.MarkShippingConfirmation(orderLineId);
        }

        [Transaction]
        public void MarkTIMShippingConfirmation(string orderLineId)
        {
            Dao.MarkTIMShippingConfirmation(orderLineId);
        }

        public IList<ShippingConfirmation> GetTIMShippingConfirmations()
        {
            return Dao.GetTIMShippingConfirmations();
        }

        public IList<ShippingConfirmation> GetShippingConfirmations(string appId)
        {
            return Dao.GetShippingConfirmations(appId);
        }

        #endregion

        public void LogBarcodeScan(BarcodeScanLog log)
        {
            Dao.LogBarcodeScan(log);
        }

        [Transaction]
        public void UpdateTrackingNumber(string oldTrackingNumber, string newTrackingNumber)
        {
            Dao.UpdateTrackingNumber(oldTrackingNumber, newTrackingNumber);
        }

    }
}
