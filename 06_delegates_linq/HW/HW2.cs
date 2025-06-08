
using System;
using System.Collections.Generic;

namespace DelegatesLinQ.Homework
{
    // Delegate dùng để xử lý dữ liệu từng bước
    public delegate string DataProcessor(string input);
    // Delegate để log thông tin sau mỗi bước xử lý
    public delegate void ProcessingEventHandler(string stage, string input, string output);

    // Lớp chính thực hiện chuỗi xử lý dữ liệu
    public class DataProcessingPipeline
    {
        // Sự kiện phát sinh sau mỗi bước xử lý
        public event ProcessingEventHandler ProcessingStageCompleted;

        // Xóa toàn bộ khoảng trắng khỏi chuỗi
        public static string RemoveSpaces(string input)
        {
            string output = input.Replace(" ", "");
            return output;
        }

        // Chuyển toàn bộ chuỗi thành chữ hoa
        public static string ToUpperCase(string input)
        {
            string output = input.ToUpper();
            return output;
        }

        // Thêm timestamp vào đầu chuỗi
        public static string AddTimestamp(string input)
        {
            string output = $"[{DateTime.Now}] {input}";
            return output;
        }

        // Đảo ngược chuỗi
        public static string ReverseString(string input)
        {
            char[] chars = input.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }

        // Mã hóa chuỗi theo Base64
        public static string EncodeBase64(string input)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(bytes);
        }

        // Kiểm tra dữ liệu đầu vào, ném lỗi nếu null hoặc rỗng
        public static string ValidateInput(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Input cannot be null or empty.");
            return input;
        }

        // Thực hiện chuỗi xử lý dữ liệu
        public string ProcessData(string input, DataProcessor pipeline)
        {
            string currentInput = input;
            string currentOutput = input;
            foreach (Delegate step in pipeline.GetInvocationList())
            {
                try
                {
                    currentOutput = ((DataProcessor)step)(currentInput);
                    OnProcessingStageCompleted(step.Method.Name, currentInput, currentOutput);
                    currentInput = currentOutput;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] {step.Method.Name}: {ex.Message}");
                    throw;
                }
            }
            return currentOutput;
        }

        // Phát sinh sự kiện sau mỗi bước xử lý
        protected virtual void OnProcessingStageCompleted(string stage, string input, string output)
        {
            ProcessingStageCompleted?.Invoke(stage, input, output);
        }
    }

    // Lớp ghi log quá trình xử lý
    public class ProcessingLogger
    {
        public void OnProcessingStageCompleted(string stage, string input, string output)
        {
            Console.WriteLine($"[LOG] Stage: {stage}, Input: '{input}', Output: '{output}'");
        }
    }

    // Lớp theo dõi hiệu năng xử lý
    public class PerformanceMonitor
    {
        private Dictionary<string, List<double>> timings = new Dictionary<string, List<double>>();

        public void OnProcessingStageCompleted(string stage, string input, string output)
        {
            double timeMs = new Random().NextDouble() * 10; // Giả lập thời gian xử lý
            if (!timings.ContainsKey(stage)) timings[stage] = new List<double>();
            timings[stage].Add(timeMs);
        }

        public void DisplayStatistics()
        {
            Console.WriteLine("[PERFORMANCE] Average time per stage:");
            foreach (var entry in timings)
            {
                double avg = 0;
                if (entry.Value.Count > 0)
                    avg = entry.Value.Average();
                Console.WriteLine($" - {entry.Key}: {avg:F2}ms");
            }
        }
    }

    public class DelegateChain
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== HOMEWORK 2: CUSTOM DELEGATE CHAIN ===\n");

            // Tạo pipeline và các logger
            DataProcessingPipeline pipeline = new DataProcessingPipeline();
            ProcessingLogger logger = new ProcessingLogger();
            PerformanceMonitor monitor = new PerformanceMonitor();

            // Đăng ký sự kiện
            pipeline.ProcessingStageCompleted += logger.OnProcessingStageCompleted;
            pipeline.ProcessingStageCompleted += monitor.OnProcessingStageCompleted;

            // Tạo chuỗi xử lý ban đầu
            DataProcessor processingChain = DataProcessingPipeline.ValidateInput;
            processingChain += DataProcessingPipeline.RemoveSpaces;
            processingChain += DataProcessingPipeline.ToUpperCase;
            processingChain += DataProcessingPipeline.AddTimestamp;

            // Kiểm thử pipeline
            string testInput = "Hello World from C#";
            Console.WriteLine($"Input: {testInput}");
            string result = pipeline.ProcessData(testInput, processingChain);
            Console.WriteLine($"Output: {result}\n");

            // Thêm các bước xử lý khác
            processingChain += DataProcessingPipeline.ReverseString;
            processingChain += DataProcessingPipeline.EncodeBase64;
            result = pipeline.ProcessData("Extended Pipeline Test", processingChain);
            Console.WriteLine($"Extended Output: {result}\n");

            // Gỡ bỏ bước đảo chuỗi
            processingChain -= DataProcessingPipeline.ReverseString;
            result = pipeline.ProcessData("Without Reverse", processingChain);
            Console.WriteLine($"Modified Output: {result}\n");

            // Hiển thị thống kê hiệu năng
            monitor.DisplayStatistics();

            // Thử xử lý lỗi
            try
            {
                pipeline.ProcessData(null, processingChain);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPTION HANDLED] {ex.Message}");
            }

            Console.ReadKey();
        }
    }
}
