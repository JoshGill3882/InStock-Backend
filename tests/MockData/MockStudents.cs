using instock_server_application.Data.Models;

namespace instock_server_application.tests.MockData; 

public static class MockStudents {
    public static List<Student> MockStudentData() {
        return new List<Student> {
            {
                new Student(1, "Abdul", "Software Engineering", 20)
            }, 
            {
                new Student(2, "John", "Computer Science", 22)
            },
            {
                new Student(3, "Steve", "Electrical Engineering", 25)
            }
        };
    }
}
