using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShippingService.Business.EF.Domain.E1;
using ShippingService.Business.EF.Domain.SNOrders;
using ShippingService.Business.EF.Facade;
using ShippingService.Business.EF.Facade.E1;
using ShippingService.Business.EF.Facade.SNOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Test
{
    [TestClass]
    public class SNOrderTest
    {
          private TestingFactory fac = new TestingFactory();
          [TestMethod]
          public void TestGetOrderLines()
          {
              var e1facade = fac.Get<E1Facade>();
              var data = GetOrderLines();
              e1facade.Setup(m => m.GetByQuery<E1OrderLine>(It.IsAny<string>(), It.IsAny<float>())).Returns(data);

              var orderlines = e1facade.Object.GetOrderLines(1234);
              Assert.AreEqual(1, orderlines.Count);
              Assert.AreEqual("C1234", orderlines[0].CaseNumber);
          }

          [TestMethod]
          public void TestGetCartons()
          {
              var e1facade = fac.Get<E1Facade>();
              var data = GetCartons();
              e1facade.Setup(m => m.GetByQuery<E1Carton>(It.IsAny<string>())).Returns(data);

              var cartons = e1facade.Object.GetCartons();
              Assert.AreEqual(1, cartons.Count);
              Assert.AreEqual("C1234", cartons[0].Id);
          }

          [TestMethod]
          public void Pack()
          {
              TestGetOrderLines();
              TestGetCartons();

              var snorderfacade = fac.Get<SNOrderFacade>();
              var e1facade = fac.Get<E1Facade>();

              PackingList lst = new PackingList();
              lst.OrderId = 1234;
              lst.PackingLines = new List<PackingLine>();
              lst.PackingLines.Add(
                  new PackingLine()
                  {
                      CartonId = "C1234",
                      OrderLineId = 1234000,
                      Quantity = 1
                  });

              var packedorderlines = snorderfacade.Object.Pack(lst, e1facade.Object);

              snorderfacade.Verify(m => m.Add<SNPackedContainer>(It.IsAny<SNPackedContainer>()), Times.Once);
              snorderfacade.Verify(m => m.Add<SNPackedOrderLine>(It.IsAny<SNPackedOrderLine>()), Times.Once);
              snorderfacade.Verify(m => m.Save(), Times.Once);

              Assert.AreEqual(1, packedorderlines.Count);
              Assert.AreEqual("1234", packedorderlines[0].OrderId);
              Assert.AreEqual("C1234", packedorderlines[0].PackedContainer.ContainerId);


          }

          public List<E1OrderLine> GetOrderLines()
          {
              List<E1OrderLine> orderlines = new List<E1OrderLine>()
              {
                  new E1OrderLine(){ Id = 1234000, CaseNumber = "C1234", OrderNumber = 1234, PartNumber = "PN1234", PartWeight = 12, Quantity = 1 }
              };
              return orderlines;
          }
          public List<E1Carton> GetCartons()
          {
              List<E1Carton> cartons = new List<E1Carton>()
              {
                  new E1Carton(){ Id = "C1234", Name="Carton1", Weight=0.8, WeightUOM = "KG" }
              };
              return cartons;
          }
    }
}
