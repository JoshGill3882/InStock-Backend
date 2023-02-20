using instock_server_application.Data.Models;
using instock_server_application.Service;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Controllers;

[ApiController]
[Route("/student")]
public class StudentController : ControllerBase {
    private static readonly StudentService StudentService = new StudentService();
    
    [HttpGet]
    [Route("all")]
    public IEnumerable<Student> GetAllStudents() {
        return StudentService.GetAllStudents();
    }
}