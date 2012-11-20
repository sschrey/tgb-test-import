using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ShippingService.Shared
{
    [Serializable]
    public class InventoryItem
    {
        public string PartNumber { get; set; }   
        public string Description { get; set; }
        public string PubTypeCode { get; set; }
        public string PubTypeDesc { get; set; }
        public string ModelYear { get; set; }
        public string VehicleName { get; set; }
        public string Languages { get; set; }
        public string OnHandStock { get; set; }
        public string AvailableStock { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Length { get; set; }
        public string SoldTo { get; set; }
        public string SoldToDescription { get; set; }
        public List<InventoryComponent> Components { get; set; }
        public string VehicleBrandCode { get; set; }
        public string VehicleBrandDescription { get; set; }
        public string ItemNumber { get; set; }
        public string Edition { get; set; }
        public string InPlantDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        

        public InventoryItem()
        {
            Components = new List<InventoryComponent>();
        }

        public string[] LanguageList
        {
            get { return Utils.SplitList(Languages); }
        }

        public string[] VehicleList
        {
            get { return Utils.SplitList(VehicleName); }
        }

        public int OnHandStockAsInt
        {
            get { return Utils.ToInt(OnHandStock); }
        }

        public int AvailableStockAsInt
        {
            get { return Utils.ToInt(AvailableStock); }
        }

        public int WeightAsInt
        {
            get { return Utils.ToInt(Weight); }
        }

        public int HeightAsInt
        {
            get { return Utils.ToInt(Height); }
        }

        public int WidthAsInt
        {
            get { return Utils.ToInt(Width); }
        }

        public int LengthAsInt
        {
            get { return Utils.ToInt(Length); }
        }

        public override string  ToString()
        {
            return PartNumber;
        }

        
    }
}
