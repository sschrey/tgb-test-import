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
        public Validation Save(VMPack data)
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

                        if (weightAsDouble < weightmin || weightAsDouble > weightplus)
                        {
                            val.AddBrokenRule("There is more than 5% difference between the estimated weight and the entered weight for container " + container.Id);
                        }
                    }
                }
            }

            //db check existing
            if (val.IsValid)
            {
                string orderid = data.OrderId.ToString();
                var checkorderline = GetAll<SNPackedContainer>().FirstOrDefault(ol => ol.OrderId == orderid);

                if (checkorderline != null)
                    val.AddBrokenRule("This order is already saved");
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

        public VMPack Barcodescan(string orderid, IE1Facade facade)
        {
            VMPack data = new VMPack();
            if (!string.IsNullOrWhiteSpace(orderid))
            {
                orderid = orderid.Replace("-", "").Replace(" ", "").Replace("SN", "").Replace("00002", "");
                float orderidasfloat;
                if (float.TryParse(orderid, out orderidasfloat))
                {
                    var orderlines = facade.GetOrderLines(orderidasfloat);
                    foreach (var orderline in orderlines)
                    {
                        data.OrderLines.Add(new VMOrderLine(orderline));
                        if (orderline.PartWeight == 0)
                        {
                            data.Errors.Add("Part " + orderline.PartNumber + " has no weight, please correct first");
                        }
                    }

                    data.Cartons = new List<VME1Carton>();
                    var cartons = facade.GetCartons();
                    foreach (var carton in cartons)
                    {
                        data.Cartons.Add(new VME1Carton(carton));
                    }
                    data.OrderId = orderidasfloat;
                }
            }
            return data;
        }
    }
}
