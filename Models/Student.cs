namespace instock_server_application.Models;

/// <summary>
/// Basic Model for a Student
/// </summary>
public class Student {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Subject { get; set; }
    public int Age { get; set; }

    /// <summary>
    /// All Args Constructor for a Student
    /// </summary>
    /// <param name="id"> Student's ID </param>
    /// <param name="name"> Student's Name </param>
    /// <param name="subject"> Student's Subject </param>
    /// <param name="age"> Student's Age </param>
    public Student(int id, string name, string subject, int age) {
        Id = id;
        Name = name;
        Subject = subject;
        Age = age;
    }
}