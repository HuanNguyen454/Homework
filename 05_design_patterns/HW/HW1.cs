﻿using System;

using System;

namespace DesignPatterns.Homework
{
    // This homework is based on the Factory Method Pattern
    // You will implement a factory method pattern for creating different types of vehicles

    // Base Product interface
    public interface IVehicle
    {
        void Drive();
        void DisplayInfo();
    }

    // Concrete Products
    public class Car : IVehicle
    {
        public string Model { get; private set; }
        public int Year { get; private set; }

        public Car(string model, int year)
        {
            Model = model;
            Year = year;
        }

        public void Drive()
        {
            Console.WriteLine($"Driving {Model} car on the road");
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Car: {Model}, Year: {Year}");
        }
    }

    public class Motorcycle : IVehicle
    {
        public string Brand { get; private set; }
        public int EngineCapacity { get; private set; }

        public Motorcycle(string brand, int engineCapacity)
        {
            Brand = brand;
            EngineCapacity = engineCapacity;
        }

        public void Drive()
        {
            Console.WriteLine($"Riding {Brand} motorcycle with {EngineCapacity}cc engine");
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Motorcycle: {Brand}, Engine: {EngineCapacity}cc");
        }
    }

    // Implemented Truck class
    public class Truck : IVehicle
    {
        public int LoadCapacity { get; private set; } // in tons
        public string FuelType { get; private set; }

        public Truck(int loadCapacity, string fuelType)
        {
            LoadCapacity = loadCapacity;
            FuelType = fuelType;
        }

        public void Drive()
        {
            Console.WriteLine($"Driving a truck with {LoadCapacity} tons load capacity using {FuelType} fuel.");
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Truck: Load Capacity = {LoadCapacity} tons, Fuel Type = {FuelType}");
        }
    }

    // Abstract Creator
    public abstract class VehicleFactory
    {
        // Factory Method
        public abstract IVehicle CreateVehicle();

        // Operation that uses the factory method
        public void OrderVehicle()
        {
            IVehicle vehicle = CreateVehicle();

            Console.WriteLine("Ordering a new vehicle...");
            vehicle.DisplayInfo();
            vehicle.Drive();
            Console.WriteLine("Vehicle delivered!\n");
        }
    }

    // Concrete Creators
    public class CarFactory : VehicleFactory
    {
        private string _model;
        private int _year;

        public CarFactory(string model, int year)
        {
            _model = model;
            _year = year;
        }

        public override IVehicle CreateVehicle()
        {
            return new Car(_model, _year);
        }
    }

    // Implemented MotorcycleFactory
    public class MotorcycleFactory : VehicleFactory
    {
        private string _brand;
        private int _engineCapacity;

        public MotorcycleFactory(string brand, int engineCapacity)
        {
            _brand = brand;
            _engineCapacity = engineCapacity;
        }

        public override IVehicle CreateVehicle()
        {
            return new Motorcycle(_brand, _engineCapacity);
        }
    }

    // Implemented TruckFactory
    public class TruckFactory : VehicleFactory
    {
        private int _loadCapacity;
        private string _fuelType;

        public TruckFactory(int loadCapacity, string fuelType)
        {
            _loadCapacity = loadCapacity;
            _fuelType = fuelType;
        }

        public override IVehicle CreateVehicle()
        {
            return new Truck(_loadCapacity, _fuelType);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Factory Method Pattern Homework\n");

            // Create a car using factory
            VehicleFactory carFactory = new CarFactory("Tesla Model 3", 2023);
            carFactory.OrderVehicle();

            // Create a motorcycle using the MotorcycleFactory
            // Use brand "Harley Davidson" and engine capacity 1450
            VehicleFactory motorcycleFactory = new MotorcycleFactory("Harley Davidson", 1450);
            motorcycleFactory.OrderVehicle();

            // Create a truck using the TruckFactory
            // Use load capacity 10 tons and fuel type "Diesel"
            VehicleFactory truckFactory = new TruckFactory(10, "Diesel");
            truckFactory.OrderVehicle();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}

