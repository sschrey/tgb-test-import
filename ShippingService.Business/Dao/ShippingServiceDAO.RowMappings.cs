using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Data;
using System.Data;
using ShippingService.Business.Domain;
using Spring.Data.Generic;
using ShippingService.Shared;


namespace ShippingService.Business.Dao
{
    public partial class ShippingServiceDAO
    {
        public class ContainerRowMapper<T> : IRowMapper<Container>
        {
            Container IRowMapper<Container>.MapRow(IDataReader dr, int rowNum)
            {
                Container c = new Container();
                c.Id = dr["Carton"].ToString();
                c.Name = dr["Dsc"].ToString();
                c.Depth = Convert.ToInt32((double)dr["Depth"]*10);
                c.Height = Convert.ToInt32((double)dr["Height"]*10);
                c.Weight = (double)dr["Weight"];
                c.Width = Convert.ToInt32((double)dr["Width"]*10);

                if (c.Weight == 0)
                    c.Weight = 100;
                return c;
            }
        }

        public class CarrierRowMapper<T> : IRowMapper<Carrier>
        {
            Carrier IRowMapper<Carrier>.MapRow(IDataReader dr, int rowNum)
            {
                Carrier c = new Carrier();
                c.Id = dr["CarrierNumber"].ToString().Trim();
                c.Name = dr["CarrierName"].ToString().Trim();
                return c;
            }
        }

        public class CarrierModeRowMapper<T> : IRowMapper<CarrierMode>
        {
            CarrierMode IRowMapper<CarrierMode>.MapRow(IDataReader dr, int rowNum)
            {
                CarrierMode c = new CarrierMode();
                c.Id = dr["Code"].ToString().Trim();
                c.Name = dr["Mode"].ToString().Trim();
                c.Code = dr["Code"].ToString().Trim();

                return c;
            }
        }

        public class OrderConfirmationRowMapper<T> : IRowMapper<OrderConfirmation>
        {
            OrderConfirmation IRowMapper<OrderConfirmation>.MapRow(IDataReader dr, int rowNum)
            {
                OrderConfirmation o = new OrderConfirmation();

                o.ExternalOrderId = dr["ExternalOrderId"].ToString();
                o.InternalOrderId = dr["InternalOrderId"].ToString();
                o.LineNumber = dr["LineNumber"].ToString();
                o.Mode = dr["Mode"].ToString();
                o.PartNumber = dr["PartNumber"].ToString();
                o.Quantity = dr["Quantity"].ToString();
                o.Stock = dr["Stock"].ToString();
                o.ItemNumber = dr["ItemNumber"].ToString();

                return o;
            }
        }

       

        public class ShippingConfirmationRowMapper<T> : IRowMapper<ShippingConfirmation>
        {
            ShippingConfirmation IRowMapper<ShippingConfirmation>.MapRow(IDataReader dr, int rowNum)
            {
                ShippingConfirmation c = new ShippingConfirmation();

                c.ExternalOrderId = dr["ExternalOrderId"].ToString();
                c.InternalOrderId = dr["InternalOrderId"].ToString();
                c.ItemNumber = dr["ItemNumber"].ToString();
                c.Quantity = dr["Quantity"].ToString();
                c.ShipDate = dr["ShipDate"].ToString();
                c.TrackNumberList = dr["TrackingNumberList"].ToString();
                c.TransportID = dr["TransportId"].ToString();
                c.TransportType = dr["TransportType"].ToString();

                return c;
            }
        }

       
    }
}
