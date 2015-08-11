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
            cast(cast(max(query.OrderNumber) as int) as varchar) + '-' + cast(cast(max(query.LineNumber) as int) as varchar) as Id,
            max(query.OrderNumber) as ordernumber,
            max(query.LineNumber) as LineNumber,
            max(query.casenumber) as casenumber, 
            max(partnumber) as partnumber, 
            sum(query.quantity) as quantity, 
            max(query.partweight) as partweight,
            max(query.status) as status 
            from
            (select D.SDNXTR as Status, D.SDLNID as LineNumber, A.CDUKID as Id, A.CDDOCO as OrderNumber, 
            ('84530' + SUBSTRING(CONVERT(CHAR(10),CONVERT(INT,A.CDCROS)),5,3)) AS CaseNumber, 
            B.IMDSC1 as PartNumber,
            A.CDTQTY/100  as Quantity,
            PartWeight = isnull(umconv/10000.00,0.00)
            from PS_PROD.PRODDTA.f4620 A				
            LEFT JOIN PS_PROD.PRODDTA.F4101 B ON A.CDITM = B.IMITM 
            LEFT join PS_PROD.PRODDTA.f41002 C on b.imitm=c.umitm and b.imuom1=c.umum and rtrim(ltrim(c.umrum)) = 'kg'
            inner join PS_PROD.PRODDTA.F4211 D on 
            D.SDKCOO = A.CDKCOO 
            AND  D.SDITM = A.CDITM 
            AND D.SDMCU =  A.CDMCU 
            AND D.SDLNID = A.CDLNID 
            AND  D.SDSHPN = A.CDSHPN
            
            where A.CDDOCO = @p0 and D.SDNXTR < 578 
            and CDCROS>0) as query
            group by LineNumber
            order by LineNumber";

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
