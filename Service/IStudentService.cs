using instock_server_application.Data.Models;

namespace instock_server_application.Service; 

public interface IStudentService {
    List<Student> GetAllStudents();
}