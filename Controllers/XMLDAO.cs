using Proiect_PASS_New_1.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

public class StudentXmlDAO : IStudentDAO
{
  private readonly string _xmlFilePath;

  public StudentXmlDAO(string xmlFilePath)
  {
    _xmlFilePath = xmlFilePath;
  }

  public IEnumerable<Student> GetAllStudents()
  {
    if (!File.Exists(_xmlFilePath))
    {
      throw new FileNotFoundException("XML file not found.");
    }

    XDocument xmlDoc = XDocument.Load(_xmlFilePath);
    var students = from student in xmlDoc.Descendants("Student")
                   select new Student
                   {
                     Id = (int)student.Element("Id"),
                     NrMatricol = (string)student.Element("NrMatricol"),
                     Nume = (string)student.Element("Nume"),
                     Prenume = (string)student.Element("Prenume"),
                     Medie = (double)(decimal)student.Element("Medie")
                   };
    return students.ToList();
  }

  public Student GetStudentById(int id)
  {
    if (!File.Exists(_xmlFilePath))
    {
      throw new FileNotFoundException("XML file not found.");
    }

    XDocument xmlDoc = XDocument.Load(_xmlFilePath);
    var student = xmlDoc.Descendants("Student")
                        .FirstOrDefault(s => (int)s.Element("Id") == id);
    if (student != null)
    {
      return new Student
      {
        Id = (int)student.Element("Id"),
        NrMatricol = (string)student.Element("NrMatricol"),
        Nume = (string)student.Element("Nume"),
        Prenume = (string)student.Element("Prenume"),
        Medie = (double)(decimal)student.Element("Medie")
      };
    }
    return null;
  }
  static int IDcounter = 0;
  public void AddStudent(Student student)
  {
    XDocument xmlDoc = XDocument.Load(_xmlFilePath);
    IDcounter++;
    student.Id = IDcounter;
    xmlDoc.Element("Students").Add(
        new XElement("Student",
            new XElement("Id", student.Id),
            new XElement("NrMatricol", student.NrMatricol),
            new XElement("Nume", student.Nume),
            new XElement("Prenume", student.Prenume),
            new XElement("Medie", student.Medie)
        ));

    xmlDoc.Save(_xmlFilePath);
  }

  public void DeleteStudent(int id)
  {
    XDocument xmlDoc = XDocument.Load(_xmlFilePath);

    var student = xmlDoc.Descendants("Student")
                        .FirstOrDefault(s => (int)s.Element("Id") == id);
    if (student != null)
    {
      student.Remove();
      xmlDoc.Save(_xmlFilePath);
    }
  }

  public void UpdateStudent(Student student)
  {
    XDocument xmlDoc = XDocument.Load(_xmlFilePath);


    var existingStudent = xmlDoc.Descendants("Student")
        .FirstOrDefault(s => (int)s.Element("Id") == student.Id);

    if (existingStudent != null)
    {

      existingStudent.SetElementValue("NrMatricol", student.NrMatricol);
      existingStudent.SetElementValue("Nume", student.Nume);
      existingStudent.SetElementValue("Prenume", student.Prenume);
      existingStudent.SetElementValue("Medie", student.Medie);

      xmlDoc.Save(_xmlFilePath);
    }
  }

  public void Dispose()
  {
    //empty function not needed for xml
  }
}