using System;

namespace OnlineShop.Models.Products
{
    public abstract class Product : IProduct
    {
        private int id;
        private string manufacturer;
        private string model;
        private decimal price;
        private double overallPerformance;

        public Product(int id, string manufacturer, string model, decimal price, double overallPerformance)
        {
            Id = id;
            Manufacturer = manufacturer;
            Model = model;
            Price = price;
            OverallPerformance = overallPerformance;
        }

        public int Id
        {
            get { return id; }
            private set 
            { 
                if(value <= 0)                
                    throw new ArgumentException(Common.Constants.ExceptionMessages.InvalidProductId);
                
                id = value;
            }
        }
        public string Manufacturer
        {
            get { return manufacturer; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(Common.Constants.ExceptionMessages.InvalidManufacturer);

                manufacturer = value;
            }
        }
        public string Model
        {
            get { return model; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(Common.Constants.ExceptionMessages.InvalidModel);

                model = value;
            }
        }
        public virtual decimal Price
        {
            get { return price; }
            private set
            {
                if (value <= 0)
                    throw new ArgumentException(Common.Constants.ExceptionMessages.InvalidPrice);

                price = value;
            }
        }
        public virtual double OverallPerformance
        {
            get { return overallPerformance; }
            private set
            {
                if (value <= 0)
                    throw new ArgumentException(Common.Constants.ExceptionMessages.InvalidOverallPerformance);

                overallPerformance = value;
            }
        }
        public override string ToString() => string.Format(Common.Constants.SuccessMessages.ProductToString, OverallPerformance, Price, GetType().Name, Manufacturer, Model, Id);
    }
}