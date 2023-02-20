using instock_server_application.Data.MockData;
using instock_server_application.Data.Models;

namespace instock_server_application.Service;

public class StudentService : IStudentService {

    public List<Student> GetAllStudents() {
        return MockStudents.MockStudentData();
    }
}