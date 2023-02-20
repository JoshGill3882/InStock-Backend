using FluentAssertions;
using instock_server_application.Controllers;
using instock_server_application.Service;
using instock_server_application.tests.MockData;
using Moq;
using Xunit;

namespace instock_server_application.tests.System.Controllers; 

public class TestStudentController {

    [Fact]
    public void GetAllStudents_ReturnsListOfStudents() {
        // Arrange
        var studentService = new Mock<IStudentService>();
        studentService.Setup(_ => _.GetAllStudents()).Returns(MockStudents.MockStudentData());
        var studentController = new StudentController();
        
        // Act
        var result = studentController.GetAllStudents();
        
        // Assert
        result.Should().BeEquivalentTo(MockStudents.MockStudentData());
    }
}