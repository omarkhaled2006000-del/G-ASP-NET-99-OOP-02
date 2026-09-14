//part two
//SMART DELIVERY MANAGMENT SYSTEM
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
class Program
{
    struct DeliveryAddress
    {
        public string street;
        public string city;
        public int BuildingNumber;

        public DeliveryAddress(string street, string city, int buildingNumber)
        {
            this.street = street;
            this.city = city;
            this.BuildingNumber = buildingNumber;
        }
        public string GetFullAddress()
        {
            return $"{street}, {city}, {BuildingNumber}";
        }
    }
    class Shipment
    {
        private string trackingCode;
        private string description;
        private decimal weight;
        private decimal deliveryFee;
        public DeliveryAddress Destination { get; set; }

        public string TrackingCode
        {
            get { return trackingCode; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    trackingCode = value;
                }
            }
        }
        public string Description
        {
            get { return description; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    description = value;
                }
            }
        }


        public decimal Weight
        {
            get { return weight; }
            set
            {
                if (value > 0)
                {
                    weight = value;
                }
            }
        }


        public decimal DeliveryFee
        {
            get { return deliveryFee; }
            set
            {
                if (value > 0)
                {
                    deliveryFee = value;
                }
            }
        }

        public virtual decimal EstimatedCost
        {
            get
            {
                return DeliveryFee + (Weight * 5);
            }
        }

        public Shipment(string trackingCode)
        {
            TrackingCode = trackingCode;
            Description = "Unknown";
            Weight = 1;
            DeliveryFee = 50;
            Destination = new DeliveryAddress("Unknown", "Unknown", 0);
        }

        public Shipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination)
        {
            TrackingCode = trackingCode;
            Description = description;
            Weight = weight;
            DeliveryFee = deliveryFee;
            Destination = destination;
        }
        public void UpdateDeliveryFee(decimal newFee)
        {
            if (newFee > 0)
            {
                DeliveryFee = newFee;
            }
        }

        public virtual void PrintShipment()
        {
            Console.WriteLine($"Tracking Code: {TrackingCode}");
            Console.WriteLine($"Description: {Description}");
            Console.WriteLine($"Weight: {Weight} kg");
            Console.WriteLine($"Delivery Fee: ${DeliveryFee}");
            Console.WriteLine($"Estimated Cost: ${EstimatedCost}");
            Console.WriteLine($"Destination Address: {Destination.GetFullAddress()}");  /*the method getfulladdresss 
                                                                                         * is in the DeliveryAddress*/
        }

    }
    class StandardShipment : Shipment
    {
        public StandardShipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination)
             : base(trackingCode, description, weight, deliveryFee, destination)
        {
        }
        public override void PrintShipment()
        {
            base.PrintShipment();
        }
    }
    class ExpressShipment : Shipment
    {
        public decimal ExtraFee { get; set; }
        public ExpressShipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination, decimal extraFee)
            : base(trackingCode, description, weight, deliveryFee, destination)
        {
            if (ExtraFee >= 0)
            {
                ExtraFee = extraFee;
            }
        }
        public override decimal EstimatedCost
        {
            get
            {
                return DeliveryFee + (Weight * 5) + ExtraFee;
            }
        }
        public override void PrintShipment()
        {
            base.PrintShipment();
            Console.WriteLine($"Extra Fee: {ExtraFee}");
        }

    }

    class InternationalShipment : Shipment
    {
        public string DestinationCountry { get; set; }
        public decimal CustomsFee { get; set; }
       /* public InternationalShipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination, decimal extraFee, string destinationCountry, decimal customsFee)
            : base(trackingCode, description, weight, deliveryFee, destination)
        {
            if (!string.IsNullOrWhiteSpace(destinationCountry))
            {
                DestinationCountry = destinationCountry;
            }
            if (customsFee >= 0)
            {
                CustomsFee = customsFee;
            }

        }*/

        public InternationalShipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination, string? destinationCountry, decimal customsFee) : base(trackingCode, description, weight, deliveryFee, destination)
        {
            if (!string.IsNullOrWhiteSpace(destinationCountry))
            {
                DestinationCountry = destinationCountry;
            }
            if (customsFee >= 0)
            {
                CustomsFee = customsFee;
            }
        }

        public override decimal EstimatedCost
        {
            get
            {
                return DeliveryFee + (Weight * 5) + CustomsFee;
            }

        }
        public override void PrintShipment()
        {
            base.PrintShipment();
            Console.WriteLine("Destination Country: " + DestinationCountry);
            Console.WriteLine("Customs Fee: " + CustomsFee);
        }
    }

    class DeliveryCenter
    {
        private Shipment[] shipments;
        public string CenterName { get; set; }

        public DeliveryCenter(string centerName)
        {
            CenterName = centerName;
            shipments = new Shipment[20];
        }
        public Shipment this[int index]
        {
            get
            {
                if (index >= 0 && index < shipments.Length)
                {
                    return shipments[index];
                }
                return default;
            }

            set
            {
                if (index >= 0 && index < 20 && index < shipments.Length)
                {
                    shipments[index] = value;
                }
            }
        }
        public Shipment this[string trackingCode]
        {
            get
            {

                for (int i = 0; i < shipments.Length; i++)
                {
                    if (shipments[i] != null && shipments[i].TrackingCode == trackingCode)
                    {
                        return shipments[i];
                    }
                }
                return default;
            }
        }
        public bool AddShipment(Shipment shipment)
        {
            for (int i = 0; i < shipments.Length; i++)
            {
                if (shipments[i] == null)
                {
                    shipments[i] = shipment;
                    return true;
                }
            }
            return false;
        }
        public bool RemoveShipment(string trackingCode)
        {
            for (int i = 0; i < shipments.Length; i++)
            {
                if (shipments[i] != null && shipments[i].TrackingCode == trackingCode)
                {
                    shipments[i] = null;
                    return true;
                }
                
            }
            return false;
        }

        public void PrintAllShipments()
        {

            Console.WriteLine($"Delivery Center: {CenterName}");

            for (int i = 0; i < shipments.Length; i++)
            {
                if (shipments[i] != null)
                {
                    Console.WriteLine($"Shipment {i + 1}:");
                    shipments[i].PrintShipment(); // Call the PrintShipment method of the Shipment class
                    Console.WriteLine();
                }
            }
        }

    }

    static void Main(string[] args)
    {
        Console.Write("enter the delivery center name : ");
       string centerName =  Console.ReadLine();
        DeliveryCenter center = new DeliveryCenter(centerName);

        //standard shipment
        Console.WriteLine();
        Console.WriteLine("=== Standard Shipment ===");

        Console.Write("Tracking Code: ");
        string trackingCode1 = Console.ReadLine();

        Console.Write("Description: ");
        string description1 = Console.ReadLine();

        Console.Write("Weight: ");
        decimal weight1 = decimal.Parse(Console.ReadLine());

        Console.Write("Delivery Fee: ");
        decimal deliveryFee1 = decimal.Parse(Console.ReadLine());

        Console.Write("City: ");
        string city1 = Console.ReadLine();

        Console.Write("Street: ");
        string street1 = Console.ReadLine();

        Console.Write("Building Number: ");
        int buildingNumber1 = int.Parse(Console.ReadLine());

        DeliveryAddress address1 =new DeliveryAddress(city1, street1, buildingNumber1);

        StandardShipment standardShipment = new StandardShipment(trackingCode1,description1,weight1,  deliveryFee1, address1);

        center.AddShipment(standardShipment);

        //Express shipment

        Console.WriteLine();
        Console.WriteLine("=== Express Shipment ===");

        Console.Write("Tracking Code: ");
        string trackingCode2 = Console.ReadLine();

        Console.Write("Description: ");
        string description2 = Console.ReadLine();

        Console.Write("Weight: ");
        decimal weight2 = decimal.Parse(Console.ReadLine());

        Console.Write("Delivery Fee: ");
        decimal deliveryFee2 = decimal.Parse(Console.ReadLine());

        Console.Write("City: ");
        string city2 = Console.ReadLine();

        Console.Write("Street: ");
        string street2 = Console.ReadLine();

        Console.Write("Building Number: ");
        int buildingNumber2 = int.Parse(Console.ReadLine());

        Console.Write("Extra Fee: ");
        decimal extraFee = decimal.Parse(Console.ReadLine());

        DeliveryAddress address2 =
            new DeliveryAddress(
                city2,
                street2,
                buildingNumber2);

        ExpressShipment expressShipment =
            new ExpressShipment(
                trackingCode2,
                description2,
                weight2,
                deliveryFee2,
                address2,
                extraFee);

        center.AddShipment(expressShipment);

        //International shipment

        Console.WriteLine();
        Console.WriteLine("=== International Shipment ===");

        Console.Write("Tracking Code: ");
        string trackingCode3 = Console.ReadLine();

        Console.Write("Description: ");
        string description3 = Console.ReadLine();

        Console.Write("Weight: ");
        decimal weight3 = decimal.Parse(Console.ReadLine());

        Console.Write("Delivery Fee: ");
        decimal deliveryFee3 = decimal.Parse(Console.ReadLine());

        Console.Write("City: ");
        string city3 = Console.ReadLine();

        Console.Write("Street: ");
        string street3 = Console.ReadLine();

        Console.Write("Building Number: ");
        int buildingNumber3 = int.Parse(Console.ReadLine());

        Console.Write("Destination Country: ");
        string destinationCountry = Console.ReadLine();

        Console.Write("Customs Fee: ");
        decimal customsFee = decimal.Parse(Console.ReadLine());

        DeliveryAddress address3 =
            new DeliveryAddress(
                city3,
                street3,
                buildingNumber3);

        InternationalShipment internationalShipment =
            new InternationalShipment(
                trackingCode3,
                description3,
                weight3,
                deliveryFee3,
                address3,
                destinationCountry,
                customsFee);

        center.AddShipment(internationalShipment);

        Console.WriteLine();
        center.PrintAllShipments();

        Console.WriteLine();
        Console.Write("Enter a tracking code to search for a shipment: ");
        string searchCode = Console.ReadLine();

        Shipment foundShipment = center[searchCode];

        if(foundShipment != null)
        {
            Console.WriteLine("Shipment found:");
            foundShipment.PrintShipment();
        }
        else
        {
            Console.WriteLine("Shipment not found");
        }

        Console.WriteLine();
        Console.Write("Enter tracking code to remove: ");

        string removeCode = Console.ReadLine();

        bool removed = center.RemoveShipment(removeCode);

        if (removed)
        {
            Console.WriteLine("Shipment removed successfully.");
        }
        else
        {
            Console.WriteLine("Shipment not found.");
        }  



        Console.WriteLine();
        Console.WriteLine("Remaining Shipments:");

        center.PrintAllShipments();

    }
}