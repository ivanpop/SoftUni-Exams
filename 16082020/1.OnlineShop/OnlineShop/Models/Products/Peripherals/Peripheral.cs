﻿namespace OnlineShop.Models.Products.Peripherals
{
    public abstract class Peripheral : Product, IPeripheral
    {
        private string connectionType;

        public Peripheral(int id, string manufacturer, string model, decimal price, double overallPerformance, string connectionType) : base(id, manufacturer, model, price, overallPerformance)
        {
            ConnectionType = connectionType;
        }

        public string ConnectionType
        { 
            get { return connectionType; } 
            private set { connectionType = value; } 
        }

        public override string ToString() => base.ToString() + $" Connection Type: {ConnectionType}";
    }
}