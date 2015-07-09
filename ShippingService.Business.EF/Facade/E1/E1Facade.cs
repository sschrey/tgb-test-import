using ShippingService.Business.EF.Domain.E1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Facade.E1
{
    public class E1Facade: BaseFacade, IE1Facade
    {
        private E1Data db;
        public E1Facade(E1Data db)
            : base(db)
        {
            this.db = db;
        }

        public List<E1OrderLine> GetOrderLines(float orderid)
        {
            var sqlQuery = @"
            select 
            cast(cast(max(query.OrderNumber) as int) as varchar) + '-' + cast(row_number() OVER (order by partnumber) as varchar) as Id,
            max(query.OrderNumber) as ordernumber,
            max(query.casenumber) as casenumber, 
            partnumber, 
            sum(query.quantity) as quantity, 
            max(query.partweight) as partweight 
            from
            (select A.CDUKID as Id, A.CDDOCO as OrderNumber, 
            ('84530' + SUBSTRING(CONVERT(CHAR(10),CONVERT(INT,A.CDCROS)),5,3)) AS CaseNumber, 
            B.IMDSC1 as PartNumber,
            A.CDTQTY/100  as Quantity,
            PartWeight = isnull(umconv/10000.00,0.00)
            from PS_PROD.PRODDTA.f4620 a				
            LEFT JOIN PS_PROD.PRODDTA.F4101 B 
            LEFT join PS_PROD.PRODDTA.f41002 C on b.imitm=c.umitm and b.imuom1=c.umum and rtrim(ltrim(c.umrum)) = 'kg'
            ON A.CDITM = B.IMITM 
            where A.CDDOCO = @p0
            and CDCROS>0) as query
            group by partnumber
            order by PartNumber";

            var orders = GetByQuery<E1OrderLine>(sqlQuery, orderid);

            return orders;
        }

        public List<E1Carton> GetCartons()
        {
            var sqlquery = "select carton as Id, Dsc as Name, Weight, WeightUOM from shippingservice.CartonsList";
            var cartons = GetByQuery<E1Carton>(sqlquery);

            return cartons;

        }
    }
}
