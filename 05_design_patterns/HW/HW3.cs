using System;
using System.Collections.Generic;

namespace DesignPatterns.Homework
{
    #region Observer Pattern Interfaces

    public interface IWeatherStation
    {
        void RegisterObserver(IWeatherObserver observer);
        void RemoveObserver(IWeatherObserver observer);
        void NotifyObservers();

        float Temperature { get; }
        float Humidity { get; }
        float Pressure { get; }
    }

    public interface IWeatherObserver
    {
        void Update(float temperature, float humidity, float pressure);
    }

    #endregion

    #region Weather Station Implementation

    public class WeatherStation : IWeatherStation
    {
        private List<IWeatherObserver> _observers;
        private float _temperature;
        private float _humidity;
        private float _pressure;

        public WeatherStation()
        {
            _observers = new List<IWeatherObserver>();
        }

        public void RegisterObserver(IWeatherObserver observer)
        {
            _observers.Add(observer); // Thêm observer vào danh sách
            Console.WriteLine("Observer registered.");
        }

        public void RemoveObserver(IWeatherObserver observer)
        {
            _observers.Remove(observer); // Xóa observer khỏi danh sách
            Console.WriteLine("Observer removed.");
        }

        public void NotifyObservers()
        {
            Console.WriteLine("Notifying observers...");

            foreach (var observer in _observers)
            {
                // Gọi Update để gửi dữ liệu mới
                observer.Update(_temperature, _humidity, _pressure);
            }
        }

        public float Temperature => _temperature;
        public float Humidity => _humidity;
        public float Pressure => _pressure;

        public void SetMeasurements(float temperature, float humidity, float pressure)
        {
            Console.WriteLine("\n--- Weather Station: Weather measurements updated ---");

            _temperature = temperature;
            _humidity = humidity;
            _pressure = pressure;

            Console.WriteLine($"Temperature: {_temperature}°C");
            Console.WriteLine($"Humidity: {_humidity}%");
            Console.WriteLine($"Pressure: {_pressure} hPa");

            NotifyObservers(); // Gửi thông báo cho observers
        }
    }

    #endregion

    #region Observer Implementations

    public class CurrentConditionsDisplay : IWeatherObserver
    {
        private float _temperature;
        private float _humidity;
        private float _pressure;

        public CurrentConditionsDisplay(IWeatherStation weatherStation)
        {
            weatherStation.RegisterObserver(this); // Tự đăng ký khi khởi tạo
        }

        public void Update(float temperature, float humidity, float pressure)
        {
            _temperature = temperature;
            _humidity = humidity;
            _pressure = pressure;
        }

        public void Display()
        {
            Console.WriteLine($"[CurrentConditions] Temp: {_temperature}°C, Humidity: {_humidity}%, Pressure: {_pressure} hPa");
        }
    }

    public class StatisticsDisplay : IWeatherObserver
    {
        private List<float> _temperatureHistory = new List<float>();

        public StatisticsDisplay(IWeatherStation weatherStation)
        {
            weatherStation.RegisterObserver(this);
        }

        public void Update(float temperature, float humidity, float pressure)
        {
            _temperatureHistory.Add(temperature);
        }

        public void Display()
        {
            float min = float.MaxValue;
            float max = float.MinValue;
            float sum = 0;

            foreach (var temp in _temperatureHistory)
            {
                if (temp < min) min = temp;
                if (temp > max) max = temp;
                sum += temp;
            }

            float avg = _temperatureHistory.Count > 0 ? sum / _temperatureHistory.Count : 0;

            Console.WriteLine($"[Statistics] Min Temp: {min}°C, Max Temp: {max}°C, Avg Temp: {avg:F2}°C");
        }
    }

    public class ForecastDisplay : IWeatherObserver
    {
        private float _lastPressure;
        private float _currentPressure;

        public ForecastDisplay(IWeatherStation weatherStation)
        {
            weatherStation.RegisterObserver(this);
        }

        public void Update(float temperature, float humidity, float pressure)
        {
            _lastPressure = _currentPressure;
            _currentPressure = pressure;
        }

        public void Display()
        {
            string forecast;

            if (_currentPressure > _lastPressure)
                forecast = "Improving weather on the way!";
            else if (_currentPressure < _lastPressure)
                forecast = "Cooler, rainy weather coming.";
            else
                forecast = "More of the same.";

            Console.WriteLine($"[Forecast] Forecast: {forecast}");
        }
    }

    #endregion

    #region Application

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Observer Pattern Homework - Weather Station\n");

            try
            {
                WeatherStation weatherStation = new WeatherStation();

                Console.WriteLine("Creating display devices...");

                var currentDisplay = new CurrentConditionsDisplay(weatherStation);
                var statisticsDisplay = new StatisticsDisplay(weatherStation);
                var forecastDisplay = new ForecastDisplay(weatherStation);

                Console.WriteLine("\nSimulating weather changes...");

                weatherStation.SetMeasurements(25.2f, 65.3f, 1013.1f);
                Console.WriteLine("\n--- Displaying Information ---");
                currentDisplay.Display();
                statisticsDisplay.Display();
                forecastDisplay.Display();

                weatherStation.SetMeasurements(28.5f, 70.2f, 1012.5f);
                Console.WriteLine("\n--- Displaying Updated Information ---");
                currentDisplay.Display();
                statisticsDisplay.Display();
                forecastDisplay.Display();

                weatherStation.SetMeasurements(22.1f, 90.7f, 1009.2f);
                Console.WriteLine("\n--- Displaying Final Information ---");
                currentDisplay.Display();
                statisticsDisplay.Display();
                forecastDisplay.Display();

                Console.WriteLine("\nRemoving CurrentConditionsDisplay...");
                weatherStation.RemoveObserver(currentDisplay);

                weatherStation.SetMeasurements(24.5f, 80.1f, 1010.3f);
                Console.WriteLine("\n--- Displaying Information After Removal ---");
                statisticsDisplay.Display();
                forecastDisplay.Display();

                Console.WriteLine("\nObserver Pattern demonstration complete.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }

    #endregion
}

