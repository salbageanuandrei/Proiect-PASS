using Proiect_PASS_New_1.Data;
using Proiect_PASS_New_1.Models;
using System.Collections.Generic;
using System.Linq;

public class StudentSqlDAO : IStudentDAO
{
  private readonly StudentContext _dbContext;

  public StudentSqlDAO(StudentContext dbContext)
  {
    _dbContext = dbContext;
  }

  public IEnumerable<Student> GetAllStudents()
  {
    return _dbContext.Students.ToList();
  }

  public Student GetStudentById(int id)
  {
    return _dbContext.Students.Find(id);
  }

  public void AddStudent(Student student)
  {
    _dbContext.Students.Add(student);
    _dbContext.SaveChanges();
  }

  public void DeleteStudent(int id)
  {
    var student = _dbContext.Students.Find(id);
    if (student != null)
    {
      _dbContext.Students.Remove(student);
      _dbContext.SaveChanges();
    }
  }

  public void Dispose()
  {
    _dbContext.Dispose();
  }


  public void UpdateStudent(Student student)
  {
    var existingStudent = _dbContext.Students.Find(student.Id);
    if (existingStudent != null)
    {

      existingStudent.NrMatricol = student.NrMatricol;
      existingStudent.Nume = student.Nume;
      existingStudent.Prenume = student.Prenume;
      existingStudent.Medie = student.Medie;

      _dbContext.SaveChanges();
    }
  }
}