using instock_server_application.Data.Models;
using instock_server_application.tests.MockData;

namespace instock_server_application.Services;

public class StudentService : IStudentService {

    public List<Student> GetAllStudents() {
        return MockStudents.MockStudentData();
    }
}