using instock_server_application.Data.MockData;
using instock_server_application.Data.Models;

namespace instock_server_application.Service;

public class StudentService : IStudentService {
    private readonly MockStudent _mockData = new MockStudent();

    public List<Student> GetAllStudents() {
        return _mockData.MockStudentData();
    }
}