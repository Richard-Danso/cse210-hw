using System;
using System.Collections.Generic;
using System.Text;
namespace OnlineOrdering
{
    public class Address
    {
        private string _streetAddress;
        private string _city;
        private string _stateProvince;
        private string _country;

        public Address(string streetAddress, string city, string stateProvince, string country)
        {
            _streetAddress = streetAddress;
            _city = city;
            _stateProvince = stateProvince;
            _country = country;
        }

        // Returns true if the country matches USA (case-insensitive check)
        public bool IsInUsa()
        {
            return _country.ToLower() == "usa" || _country.ToLower() == "united states";
        }

        // Formats all address components neatly using newline tags
        public string GetFullAddressString()
        {
            return $"{_streetAddress}\n{_city}, {_stateProvince}\n{_country}";
        }
    }
}

namespace OnlineOrdering
{
    public class Customer
    {
        private string _name;
        private Address _address; // Composition: Customer "has-an" Address

        public Customer(string name, Address address)
        {
            _name = name;
            _address = address;
        }

        public string GetName()
        {
            return _name;
        }

        public Address GetAddress()
        {
            return _address;
        }

        // delegates calculation pattern down to the address class
        public bool LivesInUsa()
        {
            return _address.IsInUsa();
        }
    }
}

namespace OnlineOrdering
{
    public class Product
    {
        private string _name;
        private string _productId;
        private double _pricePerUnit;
        private int _quantity;

        public Product(string name, string productId, double pricePerUnit, int quantity)
        {
            _name = name;
            _productId = productId;
            _pricePerUnit = pricePerUnit;
            _quantity = quantity;
        }

        public string GetName()
        {
            return _name;
        }

        public string GetProductId()
        {
            return _productId;
        }

        // Calculates total row item expenditure
        public double GetTotalCost()
        {
            return _pricePerUnit * _quantity;
        }
    }
}

namespace OnlineOrdering
{
    public class Order
    {
        private List<Product> _products = new List<Product>();
        private Customer _customer;

        public Order(Customer customer)
        {
            _customer = customer;
        }

        public void AddProduct(Product product)
        {
            _products.Add(product);
        }

        // Sums item subtotals and flags shipping modifiers ($5 vs $35)
        public double CalculateTotalCost()
        {
            double total = 0;

            foreach (Product product in _products)
            {
                total += product.GetTotalCost();
            }

            double shippingCost = _customer.LivesInUsa() ? 5.00 : 35.00;
            return total + shippingCost;
        }

        // Formats inventory lists for logistics operations
        public string GetPackingLabel()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("--- PACKING LABEL ---");
            foreach (Product product in _products)
            {
                sb.AppendLine($"- ID: {product.GetProductId()} | Item: {product.GetName()}");
            }
            return sb.ToString();
        }

        // Formats outward-facing delivery markers
        public string GetShippingLabel()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("--- SHIPPING LABEL ---");
            sb.AppendLine(_customer.GetName());
            sb.AppendLine(_customer.GetAddress().GetFullAddressString());
            return sb.ToString();
        }
    }
}
namespace OnlineOrdering
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("==================================================");
            Console.WriteLine("            ONLINE PRODUCT ORDER SYSTEM           ");
            Console.WriteLine("==================================================\n");

            // ------------------------------------------------------------
            // ORDER 1: Domestic Customer (USA)
            // ------------------------------------------------------------
            Address address1 = new Address("123 Main St", "Rexburg", "ID", "USA");
            Customer customer1 = new Customer("John Doe", address1);
            Order order1 = new Order(customer1);

            order1.AddProduct(new Product("Waterproof Dry Bag", "DB-098", 24.99, 2));
            order1.AddProduct(new Product("Heavy Duty River Paddle", "PD-441", 45.50, 1));
            order1.AddProduct(new Product("Whistle Lifejacket Attachment", "WS-101", 3.25, 4));

            // ------------------------------------------------------------
            // ORDER 2: International Customer (Australia)
            // ------------------------------------------------------------
            Address address2 = new Address("456 Outback Way", "Sydney", "NSW", "Australia");
            Customer customer2 = new Customer("Richard Danso", address2);
            Order order2 = new Order(customer2);

            order2.AddProduct(new Product("Inflatable Whitewater Raft", "RF-773", 599.99, 1));
            order2.AddProduct(new Product("High-Pressure Foot Pump", "PP-220", 19.95, 1));

            // ------------------------------------------------------------
            // DISPLAY PROCESS
            // ------------------------------------------------------------
            
            // Displaying Order #1 Details
            Console.WriteLine(">>> PROCESSING ORDER 1 <<<");
            Console.WriteLine(order1.GetShippingLabel());
            Console.WriteLine(order1.GetPackingLabel());
            Console.WriteLine($"Total Price (Incl. Domestic Shipping): ${order1.CalculateTotalCost():F2}");
            Console.WriteLine("\n--------------------------------------------------\n");

            // Displaying Order #2 Details
            Console.WriteLine(">>> PROCESSING ORDER 2 <<<");
            Console.WriteLine(order2.GetShippingLabel());
            Console.WriteLine(order2.GetPackingLabel());
            Console.WriteLine($"Total Price (Incl. International Shipping): ${order2.CalculateTotalCost():F2}");
            Console.WriteLine("==================================================");
        }
    }
}