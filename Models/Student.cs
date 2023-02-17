using System.Collections;

namespace a;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Subject { get; set; }
    public int Age { get; set; }

    public Student(int id, string name, string subject, int age)
    {
        Id = id;
        Name = name;
        Subject = subject;
        Age = age;
    }
}