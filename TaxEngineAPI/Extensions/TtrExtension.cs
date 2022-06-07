using TaxEngineAPI.Json.TtrShape;
using TaxEngineAPI.Json;
using TaxEngineMongoDbRepository.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxEngineAPI.Extensions
{
    public static class TtrExtension
    {
        public static TtrShape GetShapeJson(this TtrRate fullRate)
        {
            if (fullRate == null)
                return null;

            var shapes = new List<ShapeRestricted>();

            foreach (var element in fullRate.Shapes)
            {
                bool exist = shapes.Exists(x => x.ShapeName == element.Name);

                if(!exist)
                {
                    var newShape = new ShapeRestricted()
                    {
                        ShapeId = element.ShapeId,
                        ShapeName = element.Name,
                        JurisdictionName = element.JurisdictionName
                    };

                    shapes.Add(newShape);

                }
            }

            var rateId = new Rate() { RateId = fullRate.Id };

            var TtrShape = new TtrShape() { Rate = rateId, Shapes = shapes };

            return TtrShape;
        }

        public static List<TtrShapeRate> GetShapeRateJson(this TtrRate fullRate)
        {
            if (fullRate == null)
                return null;

            var shapes = new List<TtrShapeRate>();

            foreach (var element in fullRate.Shapes)
            {
                bool exist = shapes.Exists(x => x.ShapeName == element.Name);

                if (exist)
                {
                    foreach (var shape in shapes)
                    {
                        if (shape.ShapeId == element.ShapeId)
                        {
                            if (element.RateTypeId == "1")
                            {
                                shape.SalesTaxValue = element.RateValue;
                            }
                            if (element.RateTypeId == "2")
                            {
                                shape.UseTaxValue = element.RateValue;
                            }
                        }
                    }
                }

                else
                {
                    var newShape = new TtrShapeRate()
                    {
                        ShapeId = element.ShapeId,
                        ShapeName = element.Name,
                        JurisdictionName = element.JurisdictionName,
                        SalesTaxValue = element.RateTypeId == "1" ? element.RateValue : "0",
                        UseTaxValue = element.RateTypeId == "2" ? element.RateValue : "0",
                        FlateRate = element.FlatRate 
                    };

                    shapes.Add(newShape);
                }
            }


            return shapes;
        }


        public static List<TtrShapeRate> GetShapeRateById(this TtrRate fullRate, string id)
        {
           var ratesFilteredByID = new List<TtrShapeRate>();
           List<TtrShapeRate> listOfRates = GetShapeRateJson(fullRate);

            foreach(var rate in listOfRates)
            {
                if(rate.ShapeId == id)
                {
                    ratesFilteredByID.Add(rate);
                }
            }

            return ratesFilteredByID;
        }

    }
}
