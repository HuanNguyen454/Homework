
using System;
using System.Collections.Generic;
using System.Linq;

namespace DelegatesLinQ.Homework
{
    // Mô hình dữ liệu Sinh viên
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Major { get; set; }
        public double GPA { get; set; }
        public List<Course> Courses { get; set; } = new List<Course>();
        public DateTime EnrollmentDate { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
    }

    // Mô hình dữ liệu Khóa học
    public class Course
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public double Grade { get; set; } // Hệ số GPA 0 - 4.0
        public string Semester { get; set; }
        public string Instructor { get; set; }
    }

    // Mô hình dữ liệu Địa chỉ
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }

    // Lớp xử lý LINQ
    public class LinqDataProcessor
    {
        private List<Student> _students;

        public LinqDataProcessor()
        {
            _students = GenerateSampleData();
        }

        // Truy vấn cơ bản sử dụng LINQ
        public void BasicQueries()
        {
            Console.WriteLine("=== BASIC LINQ QUERIES ===");

            // 1. Tìm tất cả sinh viên có GPA > 3.5
            var topStudents = _students.Where(s => s.GPA > 3.5);

            // 2. Nhóm sinh viên theo chuyên ngành
            var groupedByMajor = _students.GroupBy(s => s.Major);

            // 3. Tính GPA trung bình theo chuyên ngành
            var avgGPAByMajor = _students
                .GroupBy(s => s.Major)
                .Select(g => new { Major = g.Key, AverageGPA = g.Average(s => s.GPA) });

            // 4. Tìm sinh viên học khóa "CS101"
            var enrolledInCS101 = _students.Where(s => s.Courses.Any(c => c.Code == "CS101"));

            // 5. Sắp xếp sinh viên theo ngày nhập học
            var sortedByEnrollment = _students.OrderBy(s => s.EnrollmentDate);

            // Xuất kết quả ví dụ
            foreach (var s in topStudents)
                Console.WriteLine($"Top Student: {s.Name} - GPA: {s.GPA}");
        }

        // Dữ liệu mẫu
        private List<Student> GenerateSampleData()
        {
            return new List<Student>
            {
                new Student
                {
                    Id = 1, Name = "Alice Johnson", Age = 20, Major = "Computer Science", 
                    GPA = 3.8, EnrollmentDate = new DateTime(2022, 9, 1),
                    Email = "alice.j@university.edu",
                    Address = new Address { City = "Seattle", State = "WA", ZipCode = "98101" },
                    Courses = new List<Course>
                    {
                        new Course { Code = "CS101", Name = "Intro to Programming", Credits = 3, Grade = 3.7, Semester = "Fall 2022", Instructor = "Dr. Smith" },
                        new Course { Code = "MATH201", Name = "Calculus II", Credits = 4, Grade = 3.9, Semester = "Fall 2022", Instructor = "Prof. Johnson" }
                    }
                },
                new Student
                {
                    Id = 2, Name = "Bob Wilson", Age = 22, Major = "Mathematics", 
                    GPA = 3.2, EnrollmentDate = new DateTime(2021, 9, 1),
                    Email = "bob.w@university.edu",
                    Address = new Address { City = "Portland", State = "OR", ZipCode = "97201" },
                    Courses = new List<Course>
                    {
                        new Course { Code = "MATH301", Name = "Linear Algebra", Credits = 3, Grade = 3.3, Semester = "Spring 2023", Instructor = "Dr. Brown" },
                        new Course { Code = "STAT101", Name = "Statistics", Credits = 3, Grade = 3.1, Semester = "Spring 2023", Instructor = "Prof. Davis" }
                    }
                },
                new Student
                {
                    Id = 3, Name = "Carol Davis", Age = 19, Major = "Computer Science", 
                    GPA = 3.9, EnrollmentDate = new DateTime(2023, 9, 1),
                    Email = "carol.d@university.edu",
                    Address = new Address { City = "San Francisco", State = "CA", ZipCode = "94101" },
                    Courses = new List<Course>
                    {
                        new Course { Code = "CS102", Name = "Data Structures", Credits = 4, Grade = 4.0, Semester = "Fall 2023", Instructor = "Dr. Smith" },
                        new Course { Code = "CS201", Name = "Algorithms", Credits = 3, Grade = 3.8, Semester = "Fall 2023", Instructor = "Prof. Lee" }
                    }
                }
            };
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("=== HOMEWORK 3: LINQ DATA PROCESSOR ===\n");

            var processor = new LinqDataProcessor();

            // Thực hiện các truy vấn cơ bản
            processor.BasicQueries();

            Console.ReadKey();
        }
    }
}
