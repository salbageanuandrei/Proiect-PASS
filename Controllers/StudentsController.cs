using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Proiect_PASS_New_1.Data;
using Proiect_PASS_New_1.Models;

namespace Proiect_PASS_New_1
{

  public class StudentsController : Controller
  {
    public IStudentDAO _studentDAO;


    public StudentsController()
    {

    }

    static string daoconfig = null;

    static bool donotdispose = true;
    [HttpPost]
    public ActionResult UpdateConfig(string configValue)
    {
      daoconfig = configValue;
      if ((daoconfig != "xml") && (daoconfig != "sql"))
      {
        donotdispose = true;

      }
      else
      {
        donotdispose = false;
      }

      return View("../Home/Index");

    }

    void checkConfig()
    {
      if (daoconfig == "xml")
      {
        string xmlFilePath = Server.MapPath("~/App_Data/student.xml");
        _studentDAO = new StudentXmlDAO(xmlFilePath);
      }
      else if (daoconfig == "sql")
      {
        _studentDAO = new StudentSqlDAO(new StudentContext());
      }
    }

    // GET: Students
    public ActionResult Index()
    {


      checkConfig();
      if (donotdispose == false)
      {
        var students = _studentDAO.GetAllStudents();

        return View(students);
      }
      else
      {
        return View("../Home/Index");
      }

    }

    // GET: Students/Details/5
    public ActionResult Details(int? id)
    {
      checkConfig();
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }

      var student = _studentDAO.GetStudentById((int)id);
      if (student == null)
      {
        return HttpNotFound();
      }
      return View(student);
    }

    // GET: Students/Create
    public ActionResult Create()
    {
      return View();
    }

    // POST: Students/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "Id,NrMatricol,Nume,Prenume,Medie")] Student student)
    {
      checkConfig();
      if (ModelState.IsValid)
      {
        _studentDAO.AddStudent(student);

        return RedirectToAction("Index");
      }

      return View(student);
    }

    // GET: Students/Edit/5
    public ActionResult Edit(int? id)
    {
      checkConfig();
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      var student = _studentDAO.GetStudentById((int)id);
      if (student == null)
      {
        return HttpNotFound();
      }
      return View(student);
    }

    // POST: Students/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "Id,NrMatricol,Nume,Prenume,Medie")] Student student)
    {
      checkConfig();
      if (ModelState.IsValid)
      {


        _studentDAO.UpdateStudent(student);
        return RedirectToAction("Index", new { dataSource = Request["dataSource"] });

      }
      return View(student);
    }

    // GET: Students/Delete/5
    public ActionResult Delete(int? id)
    {
      checkConfig();
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      var student = _studentDAO.GetStudentById((int)id);
      if (student == null)
      {
        return HttpNotFound();
      }
      return View(student);
    }

    // POST: Students/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      checkConfig();
      _studentDAO.DeleteStudent(id);
      return RedirectToAction("Index");
    }

    protected override void Dispose(bool disposing)
    {
      checkConfig();
      if (donotdispose == true)
      {
        //do nothing
      }
      else
      {
        if (disposing)
        {
          _studentDAO.Dispose();
        }
        base.Dispose(disposing);
      }
    }
    // POST: Students/Delete/5



    int CalculareHoroscop(string NrMatricol, string Nume)
    {
      int summatricol = 0;
      int sumnume = 0;
      byte[] asciiBytesmatricol = Encoding.ASCII.GetBytes(NrMatricol);
      byte[] asciiBytesNume = Encoding.ASCII.GetBytes(Nume);

      foreach (byte b in asciiBytesmatricol)
      {
        summatricol += b;
      }

      foreach (byte b in asciiBytesNume)
      {
        sumnume += b;
      }
      int sumtotal = summatricol + sumnume;
      int modulooperation = sumtotal % 2;
      return modulooperation;
    }


    int CalcularePreziceZiua(string Nume)
    {
      byte[] asciiBytesNume = Encoding.ASCII.GetBytes(Nume);
      DateTime dateTime = DateTime.Now;

      int preziceziuamodulo = (asciiBytesNume[0] + dateTime.Day) % 2;
      return preziceziuamodulo;

    }

    public ActionResult Submit(int id)
    {
      checkConfig();
      int modulooperation = 0;


      var student = _studentDAO.GetStudentById((int)id);
      modulooperation = CalculareHoroscop(student.NrMatricol, student.Nume);

      if (modulooperation == 1)
      {
        ViewData["ViewCustomer1"] = "MEDIA Dumneavoastra va creste !!";
      }
      else if (modulooperation == 0)
      {
        ViewData["ViewCustomer1"] = "MEDIA Dumneavoastra va descreste !!";
      }
      return View();

    }


    // GET: Students

    public ActionResult PreziceZiuaReturn()
    {
      checkConfig();
      var students = _studentDAO.GetAllStudents();
      return View(students);
    }
    [HttpPost]
    public ActionResult PreziceZiuaReturn(string NrMatricol)
    {
      int Preziceziua = 0;

      checkConfig();
      var students = _studentDAO.GetAllStudents();
      if (!string.IsNullOrEmpty(NrMatricol))
      {

        var student = students.Where(c => c.NrMatricol == NrMatricol).FirstOrDefault();
        if (student != null)
        {
          Preziceziua = CalcularePreziceZiua(student.Nume);
          if (Preziceziua == 1)
          {
            ViewData["ViewCustomer3"] = "Astazi  " + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + "  studentul cu Nr. Matricol   " + student.NrMatricol + "  va avea o zi buna !";
          }
          else
          {
            ViewData["ViewCustomer3"] = "Pentru acest student nu pot spune ca va avea o zi buna !";
          }
        }
        else
        {
          ViewData["ViewCustomer3"] = "Studentul cu Nr. Matricol " + NrMatricol + " nu a fost gasit.";
        }
      }
      else
      {
        ViewData["ViewCustomer3"] = "Nr. Matricol este invalid sau nu a fost furnizat.";
      }

      return View(students);

    }

    public ActionResult SubmitPrognoza()
    {
      int count = 0;
      var array = Enumerable.Repeat(-1, 100).ToArray();
      int index = 0;
      checkConfig();
      var students = _studentDAO.GetAllStudents();
      foreach (Student s in students)
      {
        if (s.Medie > 8)
        {

          array[index] = CalculareHoroscop(s.NrMatricol, s.Nume);
          index++;
        }
      }

      for (int i = 0; i < index; i++)
      {
        if (array[i] == 0)
        {
          count++;

        }
      }
      ViewData["ViewCustomer2"] = "Sunt  " + count + "  Studenți cu media peste 8 a căror medie va descrește";
      return View();
    }

    [HttpPost, ActionName("Submit")]
    [ValidateAntiForgeryToken]

    public ActionResult SubmitConfirmed(int id)
    {
      checkConfig();
      Student student = _studentDAO.GetStudentById((int)id);

      return RedirectToAction("Submit");
    }
  }
}
