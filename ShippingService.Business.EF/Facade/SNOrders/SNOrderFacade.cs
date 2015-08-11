using ShippingService.Business.EF.Domain.SNOrders;
using ShippingService.Business.EF.Facade.E1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Facade.SNOrders
{
    public class SNOrderFacade : BaseFacade
    {
        private ShippingServiceData db;
        public SNOrderFacade(ShippingServiceData db)
            : base(db)
        {
            this.db = db;
        }


        /// <summary>
        /// pack, Validation rules:
        /// 1) must select a carton
        /// 2) every part must have a weight
        /// 3) selected carton must have a weight
        /// 4) one carton can not contain different case numbers
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public VMPack Pack(VMPack data)
        {
            Validation val = new Validation();
            VMPackedContainer container = null;

            if (data.Containers == null)
                data.Containers = new List<VMPackedContainer>();

            if (data.SelectedCarton == null || data.SelectedCarton.Id == null)
            {
                val.AddBrokenRule("Please choose a carton");
            }

            if (val.IsValid)
            {
                foreach (var orderline in data.OrderLines)
                {
                    int quantity = 0;
                    if (int.TryParse(orderline.RequestQuantity, out quantity))
                    {
                        if (quantity > 0)
                        {
                            var packedQuantity = orderline.PackingData.Sum(p => p.Quantity);
                            if (packedQuantity + quantity > orderline.Quantity)
                            {
                                val.AddBrokenRule("You requested too much for line " + orderline.Id);
                            }
                            else if(orderline.PartWeight == 0)
                            {
                                val.AddBrokenRule("Part " + orderline.PartNumber + " has no weight, please correct first");
                            }
                            else if (data.SelectedCarton.Weight == 0)
                            {
                                val.AddBrokenRule("Carton " + data.SelectedCarton.Name + " has no weight, please correct first");
                            }
                            else
                            {
                                if (container == null)
                                {
                                    container = new VMPackedContainer();
                                    container.OrderNumber = orderline.OrderNumber;
                                    container.Carton = data.SelectedCarton;
                                    container.CaseNumber = orderline.CaseNumber;
                                    container.Id = (data.Containers.Count() + 1).ToString();
                                }
                                if(orderline.CaseNumber != container.CaseNumber)
                                {
                                    val.AddBrokenRule("You are not allowed to pack different case numbers in one box");
                                }
                                else
                                {
                                    orderline.PackingData.Add(new VMPackingData()
                                    {
                                        CartonId = data.SelectedCarton.Id,
                                        CartonName = data.SelectedCarton.Name,
                                        Quantity = quantity
                                    });
                                    orderline.Packed = orderline.Quantity - orderline.PackingData.Sum(p => p.Quantity) == 0;

                                    container.PackedParts.Add(new VMPackedParts()
                                    {
                                        OrderLineId = orderline.Id,
                                        LineNumber = orderline.LineNumber.ToString(),
                                        PartNumber = orderline.PartNumber,
                                        PartWeight = orderline.PartWeight,
                                        Quantity = quantity
                                    });
                                    container.Count++;
                                }
                            }
                        }
                        orderline.RequestQuantity = (orderline.Quantity - orderline.PackingData.Sum(p => p.Quantity)).ToString();
                    }
                }
            }

            if (container != null)
            {
                data.Containers.Add(container);
                container.PartsWeight = container.PackedParts.Sum(p => p.PartWeight * p.Quantity);
                container.TotalWeight = container.PartsWeight + container.Carton.Weight;
            }

            data.Packed = data.OrderLines.All(ol => ol.Packed);

            data.Errors = val.BrokenRules.ToList();

            return data;
        }

        /// <summary>
        /// Validation:
        /// 1) Are all orderlines packed
        /// 2) Weight check
        /// 3) DB check existing 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Validation Save(VMPack data, string user)
        {
            Validation val = new Validation();

            if(!data.Packed)
            {
                val.AddBrokenRule("Not all orderlines are packed");
            }

            //Weight check
            if(val.IsValid)
            { 
                foreach(var container in data.Containers)
                {

                    if(string.IsNullOrEmpty(container.Weight))
                        val.AddBrokenRule("Please add weight for container " + container.Id);

                    double weightAsDouble = 0;
                    if(val.IsValid)
                    {
                        if(!Double.TryParse(container.Weight, out weightAsDouble))
                        {
                            val.AddBrokenRule("Could not parse weight for container " + container.Id);
                        }
                        double difference = 0.05;
                        double weightplus = container.TotalWeight * (1+difference);
                        double weightmin = container.TotalWeight * (1 - difference);

                        SNPackLogItem logitem = new SNPackLogItem();
                        logitem.CaseNumber = container.CaseNumber;
                        logitem.OrderId = container.OrderNumber;
                        logitem.EnteredWeight = weightAsDouble;
                        logitem.EstimatedWeight = container.TotalWeight;
                        logitem.User = user;
                        logitem.WasCorrect = true;

                        if (weightAsDouble < weightmin || weightAsDouble > weightplus)
                        {
                            logitem.WasCorrect = false;
                            val.AddBrokenRule("There is more than 5% difference between the estimated weight and the entered weight for container " + container.Id);
                        }
                        Add(logitem);
                        var logitemsave = Save();
                        val.Add(logitemsave);

                    }
                }
            }

            //db check existing
            if (val.IsValid)
            {
                val.Add(OrderLinePacked(data.OrderId.ToString(), data.OrderLines));
            }
                    
            if(val.IsValid)
            {
                DateTime createdOn = DateTime.Now;
                foreach (var container in data.Containers)
                {
                    SNPackedContainer dbcontainer = new SNPackedContainer();
                    dbcontainer.ContainerId = container.Carton.Id;
                    dbcontainer.ContainerName = container.Carton.Name;
                    dbcontainer.ContainerWeight = container.Carton.Weight;
                    dbcontainer.Weight = container.Weight;
                    dbcontainer.CaseNumber = container.CaseNumber;
                    dbcontainer.OrderId = data.OrderId.ToString();
                    dbcontainer.PartsWeight = container.PartsWeight;
                    dbcontainer.TotalWeight = container.TotalWeight;
                    dbcontainer.CreatedOn = createdOn;
                    Add(dbcontainer);

                    foreach(var orderline in container.PackedParts)
                    {
                        SNPackedOrderLine dbpackedorderline = new SNPackedOrderLine(orderline.OrderLineId);
                        dbpackedorderline.PackedContainer = dbcontainer;
                        dbpackedorderline.Partnumber = orderline.PartNumber;
                        dbpackedorderline.LineNumber = orderline.LineNumber;
                        dbpackedorderline.OrderId = data.OrderId.ToString();
                        dbpackedorderline.PartWeight = orderline.PartWeight;
                        dbpackedorderline.Quantity = orderline.Quantity;
                        dbpackedorderline.CreatedOn = createdOn;
                        Add(dbpackedorderline);
                    }
                }
            }

            if (val.IsValid)
                val = Save();

            if (val.IsValid)
            {
                data.Saved = true;
                data.SuccessMessage = "Order saved";
            }

            data.Errors = val.BrokenRules.ToList();
            
            return val;
        }

        public void Unpack(VMUnpack data)
        {
            Validation val = new Validation();
            if(string.IsNullOrEmpty(data.Id))
                val.AddBrokenRule("No Id received");

            if(val.IsValid)
            {
                //get the container
                var container = GetById<SNPackedContainer>(data.Id);
                Delete(container);
                val = Save();
            }

            data.Errors = val.BrokenRules;
        }

        private Validation OrderLinePacked(string orderid, List<VMOrderLine> orderlines)
        {
            Validation val = new Validation();
            var dborderlines = GetAll<SNPackedOrderLine>().Where(ol => ol.OrderId == orderid).ToList();
            foreach(var orderline in orderlines)
            {
                if(dborderlines.Any(dborderline => dborderline.LineNumber == orderline.LineNumber))
                {
                    val.AddBrokenRule("Order " + orderid + " with line number " + orderline.LineNumber + " is already saved");
                }
            }
            return val;
        }

        public VMPack Barcodescan(VMBarcodeScan scan, IE1Facade facade)
        {
            List<VMOrderLine> orderlines = new List<VMOrderLine>();
            List<VME1Carton> cartons = new List<VME1Carton>();
            VMPack pack = null;
            float orderidasfloat = 0;

            if (!string.IsNullOrWhiteSpace(scan.OrderId))
            {
                var orderid = scan.OrderId.Replace("-", "").Replace(" ", "").Replace("SN", "").Replace("00002", "");
                
                if (float.TryParse(orderid, out orderidasfloat))
                {
                    if(scan.Errors.Count==0)
                    { 
                        var e1orderlines = facade.GetOrderLines(orderidasfloat);
                        if (e1orderlines.Count == 0)
                            scan.Errors.Add("No order found for order " + scan.OrderId);
                        foreach (var e1orderline in e1orderlines)
                        {
                            orderlines.Add(new VMOrderLine(e1orderline));
                            if (e1orderline.PartWeight == 0)
                            {
                                scan.Errors.Add("Part " + e1orderline.PartNumber + " has no weight, please correct first");
                            }
                        }
                    }

                    if(scan.Errors.Count==0)
                    {
                        var e1cartons = facade.GetCartons();
                        foreach (var e1carton in e1cartons)
                        {
                            cartons.Add(new VME1Carton(e1carton));
                        }
                    }
                }
                else
                    scan.Errors.Add("Could not parse order " + scan.OrderId);
            }

            if (scan.Errors.Count == 0 && orderlines.Count > 0 && cartons.Count > 0)
            {
                pack = new VMPack();
                pack.OrderId = orderidasfloat;
                pack.Cartons = cartons;
                pack.OrderLines = orderlines;
                return pack;
            }
            return pack;
        }
    }
}
