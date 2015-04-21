using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShippingService.Business.Domain
{
    public partial class Order
    {
        public Dictionary<OrderLine, PackedOrderLine> GetOrderLinesByPackedContainer(PackedContainer pc)
        {
            Dictionary<OrderLine, PackedOrderLine> lines = new Dictionary<OrderLine, PackedOrderLine>();
            foreach (OrderLine ol in this.Lines)
            {
                if (ol.Packs == null)
                    break;
                foreach (PackedOrderLine pol in ol.Packs)
                {
                    if(pol.PackedContainer.Equals(pc))
                    {
                        lines.Add(ol, pol);
                    }
                }
            }
            return lines;
        }

        public bool IsUPSOrder
        {
            get
            {
                bool isUpsOrder = false;

                foreach (var l in Lines)
                {
                    foreach (var p in l.Packs)
                    {
                        isUpsOrder = !string.IsNullOrEmpty(p.PackedContainer.UPSLabel);
                    }
                }
                return isUpsOrder;
            }
        }

        public List<string> UPSLabels
        {
            get
            {
                List<string> upsLabels = new List<string>();
                foreach (var l in Lines)
                {
                    foreach (var p in l.Packs)
                    {
                        if (!string.IsNullOrEmpty(p.PackedContainer.UPSLabel) && !upsLabels.Contains(p.PackedContainer.UPSLabel))
                        {
                            upsLabels.Add(p.PackedContainer.UPSLabel);
                        }

                        if (!string.IsNullOrEmpty(p.PackedContainer.ReturnUPSLabel) && !upsLabels.Contains(p.PackedContainer.ReturnUPSLabel))
                        {
                            upsLabels.Add(p.PackedContainer.ReturnUPSLabel);
                        }
                    }
                }
                return upsLabels;
            }
        }

        public virtual OrderStatus Status
        {
            get
            {
                OrderStatus status = OrderStatus.Unpacked;

                if (Lines.TrueForAll(ol => ol.PackQty == 0))
                {
                    status = OrderStatus.Packed;
                }

                int shippedLines = 0;
                foreach (OrderLine l in Lines)
                {
                    if (l.Packs == null)
                        break;
                    foreach (PackedOrderLine p in l.Packs)
                    {
                        if (!string.IsNullOrEmpty(p.PackedContainer.TrackingNumber))
                        {
                            shippedLines++;
                        }
                    }
                }

                if (shippedLines == Lines.Count && E1Status != E1Statusses.Unshipped)
                {
                    status = OrderStatus.Shipped;
                }

                    
                return status;
            }
        }

        public List<string> PackageCodes
        {
            get
            {
                List<string> pcCodes = new List<string>();

                foreach (PackedContainer pc in PackedContainers)
                {
                    pcCodes.Add(pc.Id);
                }
                return pcCodes;
            }
        }

        public Dictionary<PackedContainer, Dictionary<string, int>> PackedContainersWithParts
        {
            get
            {
                Dictionary<PackedContainer, Dictionary<string, int>> pcwp = new Dictionary<PackedContainer, Dictionary<string, int>>();
                
                foreach (PackedContainer pc in PackedContainers)
                {
                    int estimatedWeight = 0;
                    if (pc.Container.Weight > 0)
                        estimatedWeight += pc.Container.Weight;
                    else
                        estimatedWeight = -1;
                    Dictionary<string, int> parts = new Dictionary<string, int>();

                    foreach (OrderLine ol in Lines)
                    {
                        foreach (PackedOrderLine pol in ol.Packs)
                        {
                            if (pol.PackedContainer.Equals(pc))
                            {
                                var partname = ol.PartName + " [" + ol.PartWeight + "gr]";
                                if (parts.ContainsKey(partname))
                                    parts[partname] += pol.Qty;
                                else
                                    parts.Add(partname, pol.Qty);
                                if (estimatedWeight > 0)
                                {
                                    if (ol.PartWeight > 0)
                                        estimatedWeight += ol.PartWeight * pol.Qty;
                                    else
                                        estimatedWeight = -1;
                                }
                            }
                        }
                    }
                    pc.EstimatedWeight = estimatedWeight;
                    pcwp.Add(pc, parts);
                }

                return pcwp;
            }
        }

        public Dictionary<Container, List<PackedContainer>> PackedContainerByContainerType
        {
            get
            {
                Dictionary<Container, List<PackedContainer>> packedContainerByContainerType = new Dictionary<Container, List<PackedContainer>>();

                foreach (PackedContainer pc in PackedContainers)
                {
                    if (!packedContainerByContainerType.ContainsKey(pc.Container))
                    {
                        packedContainerByContainerType.Add(pc.Container, new List<PackedContainer>());
                    }
                    packedContainerByContainerType[pc.Container].Add(pc);
                }
                return packedContainerByContainerType;
            }
        }

        public int GetProductCountByContainerType(Container c)
        {
            int qty = 0;
            foreach (OrderLine ol in this.Lines)
            {
                if (ol.Packs == null)
                    break;
                foreach (PackedOrderLine pol in ol.Packs)
                {
                    if (pol.PackedContainer.Container.Equals(c))
                    {
                        qty += pol.Qty;
                    }
                }
            }
            return qty;
        }

        public List<PackedContainer> PackedContainers
        {
            get
            {
                List<PackedContainer> pcs = new List<PackedContainer>();

                foreach (OrderLine ol in this.Lines)
                {
                    if (ol.Packs == null)
                        continue;
                    foreach (PackedOrderLine pol in ol.Packs)
                    {
                        if (!pcs.Contains(pol.PackedContainer))
                        {
                            pcs.Add(pol.PackedContainer);
                        }
                    }
                }
                return pcs;
            }
        }

        public bool HasReturnLabel
        {
            get
            {
                bool hasReturnLabel = false;

                foreach (var l in Lines)
                {
                    foreach (var p in l.Packs)
                    {
                        hasReturnLabel = !string.IsNullOrEmpty(p.PackedContainer.ReturnUPSLabel);
                    }
                }
                return hasReturnLabel;
            }
        }
    }
}
