using a;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Controllers;

[ApiController]
[Route("/student")]
public class StudentController : ControllerBase {
    private static readonly StudentService _studentService = new StudentService();
    
    [HttpGet]
    [Route("all")]
    public IEnumerable<Student> GetAllStudents() {
        return _studentService.getAllStudents();
    }
}