using instock_server_application.Data.Models;

namespace instock_server_application.Services; 

public interface IStudentService {
    List<Student> GetAllStudents();
}