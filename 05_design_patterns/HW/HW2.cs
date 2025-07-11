﻿

using System;
using System.Collections.Generic;
using System.Threading;

namespace DesignPatterns.Homework
{
    public class Logger
    {
        // Singleton instance - khởi tạo chậm (lazy)
        private static Logger _instance;

        // Đối tượng khóa dùng để đảm bảo thread-safe
        private static readonly object _lock = new object();

        // Đếm số lần tạo instance (chỉ để kiểm tra Singleton)
        private static int _instanceCount = 0;

        // Danh sách lưu các log message
        private List<string> _logMessages;

        // Constructor riêng tư ngăn tạo instance từ bên ngoài
        private Logger()
        {
            _logMessages = new List<string>();
            _instanceCount++;
            Console.WriteLine($"Logger instance created. Instance count: {_instanceCount}");
        }

        // Property GetInstance sử dụng Double-Check Locking Pattern
        public static Logger GetInstance
        {
            get
            {
                // Kiểm tra nhanh trước khi lock
                if (_instance == null)
                {
                    // Khoá để đảm bảo thread-safe
                    lock (_lock)
                    {
                        // Kiểm tra lại để tránh tạo nhiều instance
                        if (_instance == null)
                        {
                            _instance = new Logger();
                        }
                    }
                }
                return _instance;
            }
        }

        // Property kiểm tra số instance đã tạo
        public static int InstanceCount => _instanceCount;

        // Log thông tin với level INFO
        public void LogInfo(string message)
        {
            string log = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [INFO] {message}";
            _logMessages.Add(log);
            Console.WriteLine(log);
        }

        // Log lỗi với level ERROR
        public void LogError(string message)
        {
            string log = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [ERROR] {message}";
            _logMessages.Add(log);
            Console.WriteLine(log);
        }

        // Log cảnh báo với level WARNING
        public void LogWarning(string message)
        {
            string log = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [WARNING] {message}";
            _logMessages.Add(log);
            Console.WriteLine(log);
        }

        // Hiển thị toàn bộ log
        public void DisplayLogs()
        {
            Console.WriteLine("\n----- LOG ENTRIES -----");

            if (_logMessages.Count == 0)
            {
                Console.WriteLine("No log entries found.");
            }
            else
            {
                foreach (string log in _logMessages)
                {
                    Console.WriteLine(log);
                }
            }

            Console.WriteLine("----- END OF LOGS -----\n");
        }

        // Xoá toàn bộ log
        public void ClearLogs()
        {
            _logMessages.Clear();
            Console.WriteLine("All logs have been cleared.");
        }
    }

    // Lớp mô phỏng dịch vụ người dùng
    public class UserService
    {
        private Logger _logger;

        public UserService()
        {
            _logger = Logger.GetInstance;
        }

        public void RegisterUser(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    throw new ArgumentException("Username cannot be empty");
                }

                _logger.LogInfo($"User '{username}' registered successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to register user: {ex.Message}");
            }
        }
    }

    // Lớp mô phỏng dịch vụ thanh toán
    public class PaymentService
    {
        private Logger _logger;

        public PaymentService()
        {
            _logger = Logger.GetInstance;
        }

        public void ProcessPayment(string userId, decimal amount)
        {
            try
            {
                if (amount <= 0)
                {
                    throw new ArgumentException("Payment amount must be positive");
                }

                _logger.LogInfo($"Payment of ${amount} processed for user '{userId}'");

                if (amount > 1000)
                {
                    _logger.LogWarning($"Large payment of ${amount} detected for user '{userId}'. Verification required.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Payment processing failed: {ex.Message}");
            }
        }
    }

    // Lớp test đa luồng để kiểm tra Singleton
    public class ThreadingDemo
    {
        public static void RunThreadingTest()
        {
            Console.WriteLine("\n----- THREADING TEST -----");
            Console.WriteLine("Creating logger instances from multiple threads...");

            Thread[] threads = new Thread[5];
            for (int i = 0; i < 5; i++)
            {
                threads[i] = new Thread(() =>
                {
                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: Getting logger instance");
                    Logger logger = Logger.GetInstance;
                    logger.LogInfo($"Log from thread {Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(100);
                });

                threads[i].Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine($"Threading test complete. Instance count: {Logger.InstanceCount}");
            Console.WriteLine("----- END THREADING TEST -----\n");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Singleton Pattern Homework - Logger System\n");

            Logger logger1 = Logger.GetInstance;
            Logger logger2 = Logger.GetInstance;

            Console.WriteLine($"\nAre both loggers the same instance? {ReferenceEquals(logger1, logger2)}");
            Console.WriteLine($"Total instances created: {Logger.InstanceCount} (should be 1)\n");

            ThreadingDemo.RunThreadingTest();

            UserService userService = new UserService();
            PaymentService paymentService = new PaymentService();

            userService.RegisterUser("john_doe");
            paymentService.ProcessPayment("john_doe", 99.99m);

            userService.RegisterUser("");
            paymentService.ProcessPayment("jane_doe", -50);

            paymentService.ProcessPayment("big_spender", 5000m);

            Logger.GetInstance.DisplayLogs();
            Logger.GetInstance.ClearLogs();

            Logger.GetInstance.LogInfo("Application shutting down");
            Logger.GetInstance.DisplayLogs();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
