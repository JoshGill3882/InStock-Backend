namespace a;

public class StudentService {
    private List<Student> listAllStudents = new List<Student>();

    public StudentService() {
        listAllStudents.Add(new Student(1, "Abdul", "Full Stack", 20));
        listAllStudents.Add(new Student(2, "Sam", "DevOps", 20));
        listAllStudents.Add(new Student(3, "Josh", "Backend", 21));
    }

    public List<Student> getAllStudents() {
        return listAllStudents;
    }
}