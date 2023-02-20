using instock_server_application.Data.Models;
using instock_server_application.Services;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Controllers;

[ApiController]
[Route("/student")]
public class StudentController : ControllerBase {
    private static readonly StudentService StudentService = new StudentService();
    
    /// <summary>
    /// Method for getting the details about all students
    /// </summary>
    /// <remarks> No Input Data Needed </remarks>
    /// <returns> List of all Students and their Data </returns>
    [HttpGet]
    [Route("all")]
    public IEnumerable<Student> GetAllStudents() {
        return StudentService.GetAllStudents();
    }
}