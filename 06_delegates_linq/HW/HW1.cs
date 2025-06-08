
using System;


namespace DelegatesLinQ.Homework
{
    // Delegate định nghĩa khuôn mẫu cho sự kiện phép tính
    public delegate void CalculationEventHandler(string operation, double operand1, double operand2, double result);
    // Delegate định nghĩa khuôn mẫu cho sự kiện lỗi
    public delegate void ErrorEventHandler(string operation, string errorMessage);

   
    public class EventCalculator
    {
        // Sự kiện khi thực hiện phép toán thành công
        public event CalculationEventHandler OperationPerformed;
        // Sự kiện khi xảy ra lỗi
        public event ErrorEventHandler ErrorOccurred;

        // Phép cộng
        public double Add(double a, double b)
        {
            double result = a + b;
            OnOperationPerformed("Add", a, b, result);
            return result;
        }

        // Phép trừ
        public double Subtract(double a, double b)
        {
            double result = a - b;
            OnOperationPerformed("Subtract", a, b, result);
            return result;
        }

        // Phép nhân
        public double Multiply(double a, double b)
        {
            double result = a * b;
            OnOperationPerformed("Multiply", a, b, result);
            return result;
        }

        // Phép chia có xử lý lỗi chia cho 0
        public double Divide(double a, double b)
        {
            if (b == 0)
            {
                OnErrorOccurred("Divide", "Cannot divide by zero.");
                return double.NaN;
            }
            double result = a / b;
            OnOperationPerformed("Divide", a, b, result);
            return result;
        }

        // Gọi sự kiện phép toán thành công nếu có người đăng ký
        protected virtual void OnOperationPerformed(string operation, double operand1, double operand2, double result)
        {
            OperationPerformed?.Invoke(operation, operand1, operand2, result);
        }

        // Gọi sự kiện lỗi nếu có người đăng ký
        protected virtual void OnErrorOccurred(string operation, string errorMessage)
        {
            ErrorOccurred?.Invoke(operation, errorMessage);
        }
    }

    // Lớp ghi log ra console các phép toán và lỗi
    public class CalculationLogger
    {
        // Xử lý sự kiện phép toán thành công
        public void OnOperationPerformed(string operation, double operand1, double operand2, double result)
        {
            Console.WriteLine($"[LOG] {operation}: {operand1} and {operand2} = {result}");
        }

        // Xử lý sự kiện lỗi
        public void OnErrorOccurred(string operation, string errorMessage)
        {
            Console.WriteLine($"[LOG] ERROR during {operation}: {errorMessage}");
        }
    }

    // Lớp kiểm toán, đếm số phép toán đã thực hiện
    public class CalculationAuditor
    {
        private int _operationCount = 0;

        // Mỗi khi thực hiện phép toán, tăng bộ đếm
        public void OnOperationPerformed(string operation, double operand1, double operand2, double result)
        {
            _operationCount++;
        }

        // Hiển thị số lượng phép toán đã thực hiện
        public void DisplayStatistics()
        {
            Console.WriteLine($"[AUDIT] Total operations performed: {_operationCount}");
        }
    }

    // Lớp xử lý lỗi, hiển thị lỗi với màu sắc nổi bật
    public class ErrorHandler
    {
        public void OnErrorOccurred(string operation, string errorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] {operation}: {errorMessage}");
            Console.ResetColor();
        }
    }

    // Lớp chính chứa hàm Main để chạy chương trình
    public class HW1_EventCalculator
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== HOMEWORK 1: EVENT CALCULATOR ===\n");

            // Khởi tạo các đối tượng chính
            EventCalculator calculator = new EventCalculator();
            CalculationLogger logger = new CalculationLogger();
            CalculationAuditor auditor = new CalculationAuditor();
            ErrorHandler errorHandler = new ErrorHandler();

            // Gán các phương thức xử lý sự kiện cho từng sự kiện tương ứng
            calculator.OperationPerformed += logger.OnOperationPerformed;
            calculator.OperationPerformed += auditor.OnOperationPerformed;
            calculator.ErrorOccurred += logger.OnErrorOccurred;
            calculator.ErrorOccurred += errorHandler.OnErrorOccurred;

            // Thực hiện các phép toán
            calculator.Add(10, 5);
            calculator.Subtract(10, 3);
            calculator.Multiply(4, 7);
            calculator.Divide(15, 3);
            calculator.Divide(10, 0); // Gây ra lỗi chia cho 0

            // Hiển thị thống kê sau khi thực hiện phép toán
            auditor.DisplayStatistics();
            Console.ReadKey();
        }
    }
}
