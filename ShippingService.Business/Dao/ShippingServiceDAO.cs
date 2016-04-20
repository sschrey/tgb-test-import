using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShippingService.Business.Domain;
using Spring.Data.Generic;
using System.Data.Common;
using System.Data;
using Spring.Data.Common;
using Spring.Transaction;
using Spring.Transaction.Interceptor;
using Tweddle.Commons.Extensions;
using ShippingService.Shared;


namespace ShippingService.Business.Dao
{
    public partial class ShippingServiceDAO : AdoDaoSupport, IShippingServiceDAO
    {

        public int GetNextTNTConsignmentNoteNumber()
        {
            int nextNumber = AdoTemplate.Execute<int>(delegate(DbCommand cmd)
            {
                string cmdText = @"select top 1 * from dbo.TNTConsNoteNumber";
                cmd.CommandText = cmdText;

                IDataReader dr = cmd.ExecuteReader();

                dr.Read();

                int startingNumber = (int)dr["StartingNumber"];
                int? currentNumber = dr["CurrentNumber"].Equals(DBNull.Value) ? null : (int?)(dr["CurrentNumber"]);
                int endingNumber = (int)dr["EndingNumber"];

                dr.Close();

                if (!currentNumber.HasValue)
                {
                    currentNumber = startingNumber;
                }
                else
                {
                    currentNumber++;
                }

                if (currentNumber > endingNumber)
                    throw new ApplicationException("TNT Consignment Note numbers range exceeded, please contact TNT");

                if (currentNumber < startingNumber)
                    throw new ApplicationException("TNT Consignment Note is not in range, please contact TNT");


                cmdText = @"update dbo.TNTConsNoteNumber set CurrentNumber = @CurrentNumber";

                IDbParameters updateParams = CreateDbParameters();

                updateParams.AddWithValue("CurrentNumber", currentNumber);

                AdoTemplate.ExecuteNonQuery(CommandType.Text, cmdText, updateParams);

                return currentNumber.Value;
            });

            return nextNumber;
        }

        



        public IList<Container> GetContainers()
        {
            string cmdText = "select * from shippingservice.CartonsList";

            return AdoTemplate.QueryWithRowMapper<Container>(CommandType.Text, cmdText,
                                                  new ContainerRowMapper<Container>());
        }

        public IList<Carrier> GetCarriers()
        {
            string cmdText = @"select * from shippingservice.CarriersList
                order by carriername";

            return AdoTemplate.QueryWithRowMapper<Carrier>(CommandType.Text, cmdText,
                                                  new CarrierRowMapper<Carrier>());

            
        }

        public IList<CarrierMode> GetCarrierModes()
        {
            string cmdText = @"select * from shippingservice.ModesList
                        order by Mode";

            return AdoTemplate.QueryWithRowMapper<CarrierMode>(CommandType.Text, cmdText,
                                                  new CarrierModeRowMapper<CarrierMode>());

        }

        public IList<CarrierModeFilter> GetCarrierModeFilter()
        {
            string cmdText = @"select * from dbo.CarrierModeFilter";

            return AdoTemplate.QueryWithRowMapper<CarrierModeFilter>(CommandType.Text, cmdText,
                                                  new CarrierModeFilterRowMapper<CarrierModeFilter>());

        }

        public void Ship(Order o, bool updateE1)
        {
            if (updateE1)
            {
                foreach (OrderLine ol in o.Lines)
                {
                    string cmdText = @"[shippingservice].[UpdateStatus]";

                    IDbParameters insertParams = CreateDbParameters();

                    insertParams.AddWithValue("ORDERKEY", ol.Id.MaxLength(55));
                    insertParams.AddWithValue("MODE", o.ShippedCarrierMode.MaxLength(3));
                    insertParams.AddWithValue("CARRIER", Convert.ToInt32(o.ShippedCarrier));

                    AdoTemplate.ExecuteNonQuery(CommandType.StoredProcedure, cmdText, insertParams);
                }
            }

            foreach (PackedContainer pc in o.PackedContainers)
            {
                string cmdText = @"update packedcontainer set TrackingNumber = @TrackingNumber, UPSLabel = @UPSLabel, ShippedOn = @ShippedOn WHERE PackageCode = @PackageCode";
                IDbParameters insertParams = CreateDbParameters();

                insertParams.AddWithValue("TrackingNumber", pc.TrackingNumber);
                insertParams.AddWithValue("UPSLabel", pc.UPSLabel);
                insertParams.AddWithValue("TNTLabel", pc.TNTLabel);
                insertParams.AddWithValue("PackageCode", pc.Id);
                insertParams.AddWithValue("ShippedOn", DateTime.Now);

                AdoTemplate.ExecuteNonQuery(CommandType.Text, cmdText, insertParams);
            }
        }

        public void ReShip(Order o)
        {
            foreach (PackedContainer pc in o.PackedContainers)
            {
                string cmdText = @"update packedcontainer set TrackingNumber = @TrackingNumber, UPSLabel = @UPSLabel, ShippedOn = @ShippedOn WHERE PackageCode = @PackageCode";
                IDbParameters insertParams = CreateDbParameters();

                insertParams.AddWithValue("TrackingNumber", pc.TrackingNumber);
                insertParams.AddWithValue("UPSLabel", pc.UPSLabel);
                insertParams.AddWithValue("PackageCode", pc.Id);
                insertParams.AddWithValue("ShippedOn", DateTime.Now);

                AdoTemplate.ExecuteNonQuery(CommandType.Text, cmdText, insertParams);
            }
        }

        public void UpdateShippingDate(Order o, DateTime newShippingDate)
        {
            foreach (PackedContainer pc in o.PackedContainers)
            {
                string cmdText = @"update packedcontainer set ShippedOn = @ShippedOn WHERE PackageCode = @PackageCode";
                IDbParameters insertParams = CreateDbParameters();
                
                insertParams.AddWithValue("PackageCode", pc.Id);
                insertParams.AddWithValue("ShippedOn", DateTime.Now);

                AdoTemplate.ExecuteNonQuery(CommandType.Text, cmdText, insertParams);

            }
        }

        public void SaveReturnLabel(Order o)
        {
            foreach (PackedContainer pc in o.PackedContainers)
            {
                string cmdText = @"update packedcontainer set ReturnUPSLabel = @ReturnUPSLabel WHERE PackageCode = @PackageCode";
                IDbParameters insertParams = CreateDbParameters();
                
                insertParams.AddWithValue("ReturnUPSLabel", pc.ReturnUPSLabel);
                insertParams.AddWithValue("PackageCode", pc.Id);

                AdoTemplate.ExecuteNonQuery(CommandType.Text, cmdText, insertParams);
            }
        }
        public void LogBarcodeScan(BarcodeScanLog log)
        {
            string cmdText = @"insert into BarcodeScanLog(UserName, CreatedOn, PickSlipScan, BoxScan, Success)
                                values(@UserName, @CreatedOn, @PickSlipScan, @BoxScan, @Success)";

            IDbParameters insertParams = CreateDbParameters();
            insertParams.AddWithValue("UserName", log.UserName);
            insertParams.AddWithValue("CreatedOn", log.CreatedOn);
            insertParams.AddWithValue("PickSlipScan", log.PickSlipScan);
            insertParams.AddWithValue("BoxScan", log.BoxScan);
            insertParams.AddWithValue("Success", log.Success);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, cmdText, insertParams);

        }

        public void UnPack(string orderid)
        {
            var orders = GetOrders(new OrderCriteria() { Id = orderid, E1Status="556" });
            if(orders.Count == 1)
            {
                if (orders[0].Status == OrderStatus.Packed)
                {
                    
                    var containers = AdoTemplate.Execute<List<string>>(delegate(DbCommand cmd)
                    {
                        List<string> ctrs  = new List<string>();
                        string cmdText = @"select packedcontainerid from packedorderline
                        where orderlineid like @orderid";

                        var parameter = cmd.CreateParameter();
                        parameter.ParameterName = "orderid";
                        parameter.Value = orderid + "-%";
                        cmd.Parameters.Add(parameter);

                        cmd.CommandText = cmdText;


                        var r = cmd.ExecuteReader();

                        while(r.Read())
                        {
                            ctrs.Add(r["packedcontainerid"].ToString());
                        }
                        r.Close();
                        return ctrs;
                    });

                    string cmdText2 = @"delete from packedorderline
                    where orderlineid like @orderid";

                    IDbParameters updateParams2 = CreateDbParameters();
                    updateParams2.AddWithValue("orderid", orderid + "-%");

                    AdoTemplate.ExecuteNonQuery(CommandType.Text, cmdText2, updateParams2);

                    foreach(var id in containers)
                    {
                        string cmdText3 = @"delete from packedcontainer
                        where packagecode = @id";

                        IDbParameters updateParams3 = CreateDbParameters();
                        updateParams3.AddWithValue("id", id);

                        AdoTemplate.ExecuteNonQuery(CommandType.Text, cmdText3, updateParams3);
                    }
   
                }
            }
        }

        public void Pack(Order o)
        {

            List<PackedContainer> pcs = o.PackedContainers;

            

            foreach (PackedContainer pc in pcs)
            {
                int code = GeneratePackageCode();
                string packageCode = DateTime.Now.Year.ToString() + code.ToString("000000");

                pc.Id = packageCode;

                string cmdText = @"insert into dbo.PackedContainer(PackageCode, ContainerId, Weight)
                                values(@PackageCode, @ContainerId, @Weight)";

                IDbParameters insertParams = CreateDbParameters();

                insertParams.AddWithValue("PackageCode", packageCode);
                insertParams.AddWithValue("ContainerId", pc.Container.Id);
                insertParams.AddWithValue("Weight", pc.Weight);

                AdoTemplate.ExecuteNonQuery(CommandType.Text, cmdText, insertParams);
            }

            foreach (OrderLine ol in o.Lines)
            {
                if(ol.PackQty != 0)
                {
                    throw new ApplicationException(string.Format("orderline [{0}] was incorrectly packed, packqty was {1}", ol.Id, ol.PackQty));
                }

                foreach (PackedOrderLine pol in ol.Packs)
                {
                    string cmdText = @"insert into dbo.PackedOrderLine(OrderLineId, PackedContainerId, Qty)
                                values(@OrderLineId, @PackedContainerId, @Qty)";

                    IDbParameters insertParams = CreateDbParameters();

                    insertParams.AddWithValue("OrderLineId", ol.Id);
                    insertParams.AddWithValue("PackedContainerId", pol.PackedContainer.Id);
                    insertParams.AddWithValue("Qty", pol.Qty);

                    AdoTemplate.ExecuteNonQuery(CommandType.Text, cmdText, insertParams);
                }
            }
        }

        private int GeneratePackageCode()
        {
            int code = (int)AdoTemplate.ExecuteScalar(CommandType.Text, "select top 1 packedcontainercode from uniquekeys");
            int year = (int)AdoTemplate.ExecuteScalar(CommandType.Text, "select top 1 packedcontaineryear from uniquekeys");

            if (year != DateTime.Now.Year)
            {
                code = 1;
                year = DateTime.Now.Year;
                AdoTemplate.ExecuteNonQuery(CommandType.Text, "update uniquekeys set packedcontaineryear = " + year);
            }
            else
            {
                code++;
            }
            AdoTemplate.ExecuteNonQuery(CommandType.Text, "update uniquekeys set packedcontainercode = " + code);
            return code;
        }

        

        public IList<Order> GetOrders(OrderCriteria crit)
        {
            return AdoTemplate.Execute<IList<Order>>(delegate(DbCommand cmd)
            {
                IList<Order> orders = new List<Order>();
                var containers = GetContainers();

                string cmdText = @"select a.[OrderID] as E1OrderID
                                          ,a.[OrderLineID] as E1OrderLineID
                                          ,a.[SalesOrderCompany]
                                          ,a.[SalesOrderType]
                                          ,a.[SalesOrderNumber]
                                          ,a.[CustomerPONumber]
                                          ,a.[Line]
                                          ,a.[Item]
                                          ,a.[ItemDesc]
                                          ,a.[Qty]
                                          ,a.[Carrier]
                                          ,a.[CarrierCode]
                                          ,a.[Mode]
                                          ,a.[ModeCode]
                                          ,a.[CompanyName]
                                          ,a.[AttentionName]
                                          ,a.[PhoneNumber]
                                          ,a.[AddressLine1]
                                          ,a.[AddressLine2]
                                          ,a.[AddressLine3]
                                          ,a.[City]
                                          ,a.[State]
                                          ,a.[PostalCode]
                                          ,a.[CountryCode]
                                          ,a.[ItemWeight]
                                          ,a.[UnitPrice]
                                          ,a.[Status]
                                          ,a.[ZoneNumberDescription]
                                          ,a.[ShipToCode]
                                          ,a.[ShipToCompanyName]
                                          ,a.[ShipToAttentionName]
                                          ,a.[ShipToPhoneNumber]
                                          ,a.[ShipToAddressLine1]
                                          ,a.[ShipToAddressLine2]
                                          ,a.[ShipToCity]
                                          ,a.[ShipToState]
                                          ,a.[ShipToPostalCode]
                                          ,a.[ShipToCountryCode]
                                          ,a.[SoldToCode]
                                          ,a.[SoldToCompanyName]
                                          ,a.[SoldToAttentionName]
                                          ,a.[SoldToPhoneNumber]
                                          ,a.[SoldToAddressLine1]
                                          ,a.[SoldToAddressLine2]
                                          ,a.[SoldToCity]
                                          ,a.[SoldToState]
                                          ,a.[SoldToPostalCode]
                                          ,a.[SoldToCountryCode]
                                          ,a.[InvoiceNumber]
                                          ,a.[InvoiceDate]
                                          ,pol.[OrderLineId] as packedorderlineid
                                          ,pol.[PackedContainerId]
                                          ,pol.[Qty] as packedqty
                                          ,pc.[PackageCode]
                                          ,pc.[ContainerId]
                                          ,pc.[Weight]
                                          ,pc.[TrackingNumber]
                                          ,pc.[UPSLabel]
                                          ,pc.[TNTLabel]
                                          ,pc.[ShipConfirmedOn]
                                          ,pc.[ReturnUPSLabel]
                                          ,pc.[ShippedOn]
                                            from shippingservice.AllOrdersInvoiceNumIncl a
                left outer join dbo.PackedOrderLine pol on a.orderlineid = pol.orderlineid
                left outer join packedcontainer pc on pol.packedcontainerid = pc.packagecode";

                List<string> whereClauses = new List<string>();

                if (!string.IsNullOrEmpty(crit.Id))
                {
                    whereClauses.Add("a.[OrderID] = @id");
                    var p = cmd.CreateParameter();
                    p.ParameterName = "id";
                    p.Value = crit.Id;
                    cmd.Parameters.Add(p);
                }

                if (crit.CustomerPOs != null && crit.CustomerPOs.Count()>0)
                {
                    var pStr = string.Empty;
                    foreach (var pId in crit.CustomerPOs)
                        pStr += string.Format("'{0}',", pId);

                    pStr = pStr.TrimEnd(new char[] { ',' });
                    whereClauses.Add(string.Format("a.[CustomerPONumber] in ({0})", pStr));
                }

                if (crit.Ids != null && crit.Ids.Count() > 0)
                {
                    var pStr = string.Empty;
                    foreach (var pId in crit.Ids)
                        pStr += string.Format("'{0}',", pId);

                    pStr = pStr.TrimEnd(new char[] { ',' });
                    whereClauses.Add(string.Format("a.[OrderID] in ({0})", pStr));
                }



                if (!string.IsNullOrEmpty(crit.E1Status))
                {
                    whereClauses.Add("a.Status = @Status");
                    var p = cmd.CreateParameter();
                    p.ParameterName = "Status";
                    p.Value = crit.E1Status;
                    cmd.Parameters.Add(p);
                }

                if (!string.IsNullOrEmpty(crit.TrackingNumber))
                {
                    whereClauses.Add("pc.TrackingNumber = @TrackingNumber");
                    var p = cmd.CreateParameter();
                    p.ParameterName = "TrackingNumber";
                    p.Value = crit.TrackingNumber;
                    cmd.Parameters.Add(p);
                }

                if (crit.ShippedDateFrom.HasValue)
                {
                    whereClauses.Add("pc.ShippedOn > @ShippedDateFrom");
                    var p = cmd.CreateParameter();
                    p.ParameterName = "ShippedDateFrom";
                    p.Value = crit.ShippedDateFrom;
                    cmd.Parameters.Add(p);

                    whereClauses.Add("pc.ShippedOn < @ShippedDateTo");
                    var p2 = cmd.CreateParameter();
                    p2.ParameterName = "ShippedDateTo";
                    p2.Value = crit.ShippedDateTo.AddDays(1);
                    cmd.Parameters.Add(p2);
                }

                if (!string.IsNullOrEmpty(crit.Carrier))
                {
                    whereClauses.Add("a.CarrierCode = @Carrier");
                    var p = cmd.CreateParameter();
                    p.ParameterName = "Carrier";
                    p.Value = crit.Carrier;
                    cmd.Parameters.Add(p);
                }

                
                

                if (whereClauses.Count != 0)
                    cmdText += " where ";

                for (int i=0; i<whereClauses.Count;i++)
                {
                    cmdText += whereClauses[i];

                    if (i != whereClauses.Count - 1)
                        cmdText += " and ";
                }
                cmdText += " order by a.OrderLineID";
                cmd.CommandText = cmdText;
                
                IDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    string id = dr["E1OrderID"].ToString();
                    Order o = orders.FirstOrDefault(order => order.Id == id);

                    if (o == null)
                    {
                        o = new Order();
                        orders.Add(o);
                    }

                    o.OrderNumber = dr["SalesOrderNumber"].ToString();
                    o.InvoiceNumber = dr["InvoiceNumber"].ToString();
                    
                    DateTime invoiceDate;
                    if (dr["InvoiceDate"] != null && DateTime.TryParse(dr["InvoiceDate"].ToString(), out invoiceDate))
                        o.InvoiceDate = invoiceDate;                    
                    
                    o.CustomerPONumber = dr["CustomerPONumber"].ToString();
                    o.MainAddress.AddressLine1 = dr["AddressLine1"].ToString();
                    o.MainAddress.AddressLine2 = dr["AddressLine2"].ToString();
                    o.MainAddress.AddressLine3 = dr["AddressLine3"].ToString();
                    o.MainAddress.State = dr["State"].ToString();
                    o.MainAddress.AttentionName = dr["AttentionName"].ToString();
                    o.MainAddress.City = dr["City"].ToString();
                    o.MainAddress.CompanyName = dr["CompanyName"].ToString();
                    o.MainAddress.CountryCode = dr["CountryCode"].ToString();
                    o.Id = id;
                    o.MainAddress.PhoneNumber = dr["PhoneNumber"].ToString();
                    o.MainAddress.PostalCode = dr["PostalCode"].ToString();
                    o.ProposedCarrier = dr["CarrierCode"].ToString().Trim();
                    o.ProposedCarrierMode = dr["ModeCode"].ToString().Trim();
                    o.E1Status = dr["Status"].ToString();
                    o.ZoneNumberDescription = dr["ZoneNumberDescription"].ToString();

                    o.ShipToAddress.ShipToCode = dr["ShipToCode"].ToString();
                    o.ShipToAddress.AddressLine1 = dr["ShipToAddressLine1"].ToString();
                    o.ShipToAddress.AddressLine2 = dr["ShipToAddressLine2"].ToString();
                    o.ShipToAddress.State = dr["ShipToState"].ToString();
                    o.ShipToAddress.AttentionName = dr["ShipToAttentionName"].ToString();
                    o.ShipToAddress.City = dr["ShipToCity"].ToString();
                    o.ShipToAddress.CompanyName = dr["ShipToCompanyName"].ToString();
                    o.ShipToAddress.CountryCode = dr["ShipToCountryCode"].ToString();
                    o.ShipToAddress.PhoneNumber = dr["ShipToPhoneNumber"].ToString();
                    o.ShipToAddress.PostalCode = dr["ShipToPostalCode"].ToString();

                    o.SoldToAddress.SoldToCode = dr["SoldToCode"].ToString();
                    o.SoldToAddress.AddressLine1 = dr["SoldToAddressLine1"].ToString();
                    o.SoldToAddress.AddressLine2 = dr["SoldToAddressLine2"].ToString();
                    o.SoldToAddress.State = dr["SoldToState"].ToString();
                    o.SoldToAddress.AttentionName = dr["SoldToAttentionName"].ToString();
                    o.SoldToAddress.City = dr["SoldToCity"].ToString();
                    o.SoldToAddress.CompanyName = dr["SoldToCompanyName"].ToString();
                    o.SoldToAddress.CountryCode = dr["SoldToCountryCode"].ToString();
                    o.SoldToAddress.PhoneNumber = dr["SoldToPhoneNumber"].ToString();
                    o.SoldToAddress.PostalCode = dr["SoldToPostalCode"].ToString();

                    string orderlineid = dr["E1orderlineid"].ToString();

                    if (o.Lines == null)
                    {
                        o.Lines = new List<OrderLine>();
                    }


                    OrderLine ol = o.Lines.FirstOrDefault(orderline => orderline.Id == orderlineid);
                    if (ol == null)
                    {
                        ol = new OrderLine();
                        o.Lines.Add(ol);
                    }

                    ol.Id = dr["E1orderlineid"].ToString();
                    ol.LineNumber = dr["Line"].ToString();
                    ol.OrderQty = Convert.ToInt32((double)dr["Qty"]);
                    ol.PartId = dr["Item"].ToString();
                    ol.PartName = dr["ItemDesc"].ToString();
                    ol.UnitPrice = (int)((double)dr["UnitPrice"] * 100);

                    ol.PartWeight = (int)(dr["ItemWeight"].Equals(DBNull.Value) ? 0 : (double)dr["ItemWeight"]);

                    string packedOrderLineID = dr["packedorderlineid"].ToString();

                    if (string.IsNullOrEmpty(packedOrderLineID))
                    {
                        continue;
                    }

                    if (ol.Packs == null)
                    {
                        ol.Packs = new List<PackedOrderLine>();
                    }

                    var packedOrderLine = new PackedOrderLine() { Qty = (int)dr["packedqty"] };
                    packedOrderLine.PackedContainer = o.PackedContainers.FirstOrDefault(opc => opc.Id == dr["packagecode"].ToString());
                    if (packedOrderLine.PackedContainer == null)
                    {
                        packedOrderLine.PackedContainer = new PackedContainer()
                        {
                            Container = containers.First(c => c.Id == dr["containerid"].ToString()),
                            Weight = (int)dr["weight"],
                            Id = dr["packagecode"].ToString(),
                            TrackingNumber = dr["TrackingNumber"].ToString(),
                            UPSLabel = dr["UPSLabel"].ToString(),
                            TNTLabel = dr["TNTLabel"].ToString(),
                            ShippedOn = dr["ShippedOn"] == DBNull.Value? null : (DateTime?)dr["ShippedOn"]
                        };
                    }
                    ol.Packs.Add(packedOrderLine);
                    
                }

                dr.Close();

                return orders;

            });
            
        }

        public void UpdateTrackingNumber(string oldTrackingNumber, string newTrackingNumber)
        {

            string cmdText = @"update PackedContainer
                                set trackingnumber = @newTrackingNumber
                                where trackingnumber = @oldTrackingNumber";
                                

            IDbParameters updateParams = CreateDbParameters();

            updateParams.AddWithValue("oldTrackingNumber", oldTrackingNumber);
            updateParams.AddWithValue("newTrackingNumber", newTrackingNumber);
            

            AdoTemplate.ExecuteNonQuery(CommandType.Text, cmdText, updateParams);
            
        }
    }
}
