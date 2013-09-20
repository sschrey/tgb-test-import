using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShippingService.Shared;
using System.Data;
using Spring.Transaction;
using Spring.Transaction.Interceptor;
using System.Data.Common;
using Spring.Data.Common;
using ShippingService.Business.Domain;

namespace ShippingService.Business.Dao
{
    public partial class ShippingServiceDAO
    {
        public IList<InventoryItem> GetInventory(string appId)
        {
            return AdoTemplate.Execute<IList<InventoryItem>>(delegate(DbCommand cmd)
            {
                cmd.CommandText = "InventoryExtract";
                cmd.CommandType = CommandType.StoredProcedure;
                var parameter = cmd.CreateParameter();
                parameter.ParameterName = "AppId";
                parameter.Value = appId;

                cmd.Parameters.Add(parameter);

                return GetInventory(cmd);
            });
        }

        public IList<InventoryItem> GetInventoryByBranch(string appId, string branch, bool inclCrossRefData = false)
        {
            return AdoTemplate.Execute<IList<InventoryItem>>(delegate(DbCommand cmd)
            {
                cmd.CommandText = inclCrossRefData ? "InventoryExtractByBranchSoldto" : "InventoryExtractByBranch";
                cmd.CommandType = CommandType.StoredProcedure;
                var parameter = cmd.CreateParameter();
                parameter.ParameterName = "AppId";
                parameter.Value = appId;

                cmd.Parameters.Add(parameter);

                parameter = cmd.CreateParameter();
                parameter.ParameterName = "Branch";
                parameter.Value = branch;

                cmd.Parameters.Add(parameter); 

                return GetInventory(cmd);
            });
        }       

        public IList<InventoryItem> GetTIMInventory()
        {
            return AdoTemplate.Execute<IList<InventoryItem>>(delegate(DbCommand cmd)
            {
                cmd.CommandText = "InventoryExtract_TIM";
                cmd.CommandType = CommandType.StoredProcedure;

                return GetInventory(cmd);
               
            });
        }

        private IList<InventoryItem> GetInventory(DbCommand cmd)
        {
            IDataReader dr = cmd.ExecuteReader();
            IList<InventoryItem> items = new List<InventoryItem>();

            while (dr.Read())
            {
                var itemNumber = dr["InternalPartNumber"].ToString();
                var item = items.FirstOrDefault(i => i.ItemNumber == itemNumber);

                if (item == null)
                {
                    item = new InventoryItem();
                    items.Add(item);
                }

                if (dr["componentproductid"] != DBNull.Value)
                {
                    item.Components.Add(
                        new InventoryComponent()
                        {
                            PartNumber = dr["componentproductid"].ToString(),
                            Quantity = Convert.ToInt32((double)dr["ComponentQuantity"]),
                            ItemNumber = dr["ComponentItemNumber"].ToString()
                        });
                }

                item.ItemNumber = itemNumber;
                item.PartNumber = dr["PartNumber"].ToString();
                item.Description = dr["Description"].ToString();
                item.Height = dr["Height"].ToString();
                item.Languages = dr["Languages"].ToString();
                item.Length = dr["length"].ToString();
                item.SoldTo = dr["Mode"].ToString();
                item.SoldToDescription = dr["ModeDesc"].ToString();
                item.ModelYear = dr["ModelYear"].ToString();
                item.OnHandStock = dr["OnHandStock"].ToString();
                item.AvailableStock = dr["AvailableStock"].ToString();
                item.PubTypeCode = dr["PubTypeCode"].ToString();
                item.PubTypeDesc = dr["PubTypeDesc"].ToString();
                item.VehicleName = dr["VehicleName"].ToString();
                item.Weight = dr["Weight"].ToString();
                item.Width = dr["Width"].ToString();
                item.VehicleBrandCode = dr["VehicleBrandCode"].ToString();
                item.VehicleBrandDescription = dr["VehicleBrandDesc"].ToString();
                item.ItemNumber = dr["InternalPartNumber"].ToString();
                item.Edition = dr["Edition"].ToString();
                item.InPlantDate = dr["InPlantDate"].ToString();

                
                if (dr["Soldtolist"] != DBNull.Value)
                    item.AddressCrossReferenceList = dr["Soldtolist"].ToString();

                if (dr["MarketCode"] != DBNull.Value)
                    item.MarketCode = dr["MarketCode"].ToString();

                if (dr["MarketText"] != DBNull.Value)
                    item.MarketText = dr["MarketText"].ToString();
                    
                
                //effective date can be empty
                if (dr["Effectivedate"] == DBNull.Value)
                    item.EffectiveDate = DateTime.Now;
                else
                    item.EffectiveDate = (DateTime)dr["Effectivedate"];

            }
            dr.Close();

            return items;
        }

        public IList<OrderConfirmation> GetTIMOrderConfirmations()
        {
            string cmdText = "OrderConfirmation_TIM";

            return AdoTemplate.QueryWithRowMapper<OrderConfirmation>(CommandType.StoredProcedure, cmdText,
               new OrderConfirmationRowMapper<OrderConfirmation>());
        }

        public IList<OrderConfirmation> GetOrderConfirmations(string appId)
        {
            string cmdText = "OrderConfirmation";
            var parameters = CreateDbParameters();

            parameters.AddWithValue("appId", appId);

            return AdoTemplate.QueryWithRowMapper<OrderConfirmation>(CommandType.StoredProcedure, cmdText,
               new OrderConfirmationRowMapper<OrderConfirmation>(), parameters);
        }

        public IList<ShippingConfirmation> GetShippingConfirmations(string appId)
        {
            string cmdText = @"ShipmentConfirmation";
            var parameters = CreateDbParameters();

            parameters.AddWithValue("appId", appId);

            return AdoTemplate.QueryWithRowMapper<ShippingConfirmation>(CommandType.StoredProcedure, cmdText,
               new ShippingConfirmationRowMapper<ShippingConfirmation>(), parameters);
        }

        public IList<ShippingConfirmation> GetTIMShippingConfirmations()
        {
            string cmdText = @"ShipmentConfirmation_TIM";

            return AdoTemplate.QueryWithRowMapper<ShippingConfirmation>(CommandType.StoredProcedure, cmdText,
               new ShippingConfirmationRowMapper<ShippingConfirmation>());
        }

        
        
        [Transaction]
        public void MarkTIMShippingConfirmation(string orderId)
        {
            string cmdText = @"ShipmentConfirmation_TIM_MarkAsConfirmed";

            IDbParameters insertParams = CreateDbParameters();

            insertParams.AddWithValue("OrderId", orderId);

            AdoTemplate.ExecuteNonQuery(CommandType.StoredProcedure, cmdText, insertParams);
        }

        [Transaction]
        public void MarkShippingConfirmation(string orderId)
        {
            string cmdText = @"ShipmentConfirmation_MarkAsConfirmed";

            IDbParameters insertParams = CreateDbParameters();

            insertParams.AddWithValue("OrderId", orderId);

            AdoTemplate.ExecuteNonQuery(CommandType.StoredProcedure, cmdText, insertParams);
        }

        
        [Transaction]
        public void MarkTIMOrderConfirmation(string orderId, string lineNumber)
        {
            string cmdText = "OrderConfirmation_TIM_MarkAsConfirmed";

            IDbParameters insertParams = CreateDbParameters();

            insertParams.AddWithValue("OrderId", orderId);
            insertParams.AddWithValue("LineNumber", lineNumber);

            if (AdoTemplate.ExecuteNonQuery(CommandType.StoredProcedure, cmdText, insertParams) != 1)
            {
                throw new ApplicationException("The order confirmation updated more than one row!");
            }
        }

        [Transaction]
        public void MarkOrderConfirmation(string orderId, string lineNumber)
        {
            string cmdText = "OrderConfirmation_MarkAsConfirmed";

            IDbParameters insertParams = CreateDbParameters();

            insertParams.AddWithValue("OrderId", orderId);
            insertParams.AddWithValue("LineNumber", lineNumber);

            if (AdoTemplate.ExecuteNonQuery(CommandType.StoredProcedure, cmdText, insertParams) != 1)
            {
                throw new ApplicationException("The order confirmation updated more than one row!");
            }
        }

       
    }
}
