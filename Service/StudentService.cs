using instock_server_application.Data.Models;
using instock_server_application.tests.MockData;

namespace instock_server_application.Service;

public class StudentService : IStudentService {

    public List<Student> GetAllStudents() {
        return MockStudents.MockStudentData();
    }
}