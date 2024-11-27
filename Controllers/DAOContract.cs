using Proiect_PASS_New_1.Models;
using System.Collections.Generic;

public interface IStudentDAO
{
  IEnumerable<Student> GetAllStudents();
  Student GetStudentById(int id);
  void AddStudent(Student student);
  void DeleteStudent(int id);
  void UpdateStudent(Student student);
  void Dispose();
}