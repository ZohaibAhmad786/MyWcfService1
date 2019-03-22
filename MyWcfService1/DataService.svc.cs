using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MyWcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DataService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DataService.svc or DataService.svc.cs at the Solution Explorer and start debugging.
    public class DataService : IDataService
    {
        public List<StudentData> AllStudents()
        {
            List<StudentData> st = new List<StudentData>();
            SqlCommand cmd = new SqlCommand("Select * from Student",new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=DummyData;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                StudentData s = new StudentData();
                s.id = int.Parse(sdr["id"].ToString());
                s.firstname = sdr["firstName"].ToString();
                s.lastname = sdr["lastName"].ToString();
                s.email = sdr["email"].ToString();
                st.Add(s);
            }
            sdr.Close();
            cmd.Connection.Close();
            return st;
        }

        public List<Courses> Courses()
        {
            List<Courses> course = new List<Courses>();
            string q = "select * from Course";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Courses c = new Courses();
                c.CourseCode = sdr["CourseCode"].ToString();
                c.Title = sdr["Title"].ToString();
                course.Add(c);
            }
            sdr.Close();
            cmd.Connection.Close();
            return course;
        }

        public int InsertNewRecord(string data)
        {
            var std=JsonConvert.DeserializeObject<StudentData>(data);
            int x;
            SqlCommand cmd = new SqlCommand("Insert into Student values('"+int.Parse(std.id.ToString()) +"','"+std.firstname+"','"+std.lastname+"','"+std.email+"')", new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=DummyData;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            x = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return x;

        }

        public /*int*/CUD InsertStudentData(string email,string firstname,string lastname,string phoneNo,string discipline,string semester,string password,string address,string gender)
        {
            //var std = JsonConvert.DeserializeObject<Student>(data);
            int x=0;
            var query = "Insert into Student values('" + email + "','" + firstname + "','" + lastname + "','" + phoneNo + "','" + discipline + "','" + int.Parse(semester.ToString()) + "','" + password + "','" + address + "','" + Convert.ToChar(gender) + "')";
            SqlCommand cmd = new SqlCommand("select * from Student Where email= '" + email + "'", new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                return new CUD { rowEffected = x, Reason = "Already data exist" };
            }
            else
            {
                SqlCommand cmd1 = new SqlCommand(query, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));

                x = cmd1.ExecuteNonQuery();

                return new CUD { rowEffected = x, Reason = "Successfully Registered" };
            }

            System.Diagnostics.Debug.WriteLine(x, query);
            sdr.Close();
            cmd.Connection.Close();

        }

        public /*int*/ CUD InsertTutortData(string email,string firstname,string lastname,string phoneNo,string address,string password,string gender)
        {
            //var td = JsonConvert.DeserializeObject<Tutor>(data);
            //
            int x=0;
            var b = "Insert into Tutor values('" + email + "','" + firstname + "','" + lastname + "','" + phoneNo + "','" + address + "','" + password + "','" + Convert.ToChar(gender) + "')";
            SqlCommand cmd = new SqlCommand("select * from Tutor Where email= '"+email+"'", new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                return new CUD { rowEffected = x,Reason="Already data exist" };
            }
            else
            {
                SqlCommand cmd1 = new SqlCommand(b, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
                
                x = cmd1.ExecuteNonQuery();
                
                return new CUD { rowEffected = x , Reason="Successfully Registered" };
            }
                
            System.Diagnostics.Debug.WriteLine(x,b);
            sdr.Close();
            cmd.Connection.Close();

            
        }

        public CUD StudentCourses(string Course,string email)
        {
            int x = 0;
            var q = "select * from StudentCourses Where email= '" + email + "' and coursecode ='" + Course + "'";
            var b = "Insert into StudentCourses values('" + Course + "','" + email + "')";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            SqlCommand cmd2 = new SqlCommand(b, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            System.Diagnostics.Debug.WriteLine(q);
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                return new CUD { rowEffected = x, Reason = "Already Subject exist" };
            }
            else
            {
                //System.Diagnostics.Debug.WriteLine(q);
                cmd.Connection.Close();
                cmd2.Connection.Open();
                // cmd1.Connection.Open();
                x = cmd2.ExecuteNonQuery();
                cmd2.Connection.Close();
                return new CUD { rowEffected = x, Reason = "Successfully Subject Addeds" };
            }

            //System.Diagnostics.Debug.WriteLine(x, b);
            sdr.Close();
            

        }

        public /*List<Student>*/ Student StudentLogin(string email, string pass)
        {
            
            var a = "Select * from Student where email = '"+email+ "' and Password = '"+ pass+"'";
            SqlCommand cmd = new SqlCommand(a, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            System.Diagnostics.Debug.WriteLine(a);
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            //while (sdr.Read())
            //{
            //    Student s = new Student();

            if (sdr.HasRows)
            {
                sdr.Read();
                return new Student
                {
                    firstname = sdr["First_Name"].ToString(),
                    lastname = sdr["Last_Name"].ToString(),
                    email = sdr["Email"].ToString(),
                    address = sdr["Discipline"].ToString(),
                    discipline = sdr["Discipline"].ToString(),
                    gender = Convert.ToChar(sdr["gender"].ToString()),
                    password = sdr["password"].ToString(),
                    semester = int.Parse(sdr["Semester_No"].ToString()),
                    phoneNo = sdr["Phone_No"].ToString(),
                    Error="Data Reside In Database"
                };
            }else
            {
                return new Student { Error = "Email or Password Incorrect" };
            }
            //}
            sdr.Close();
            cmd.Connection.Close();
            //return st;
        }

        public CUD StudentSchedual(string dataSource1)
        {
           // var obj = JsonConvert.DeserializeObject<dynamic>(dataSource1);
            //var x = dataSource1.Length;
            return new CUD { Reason = "hellow" };
        }

        public CUD TutorCourses(string Course,string email)
        {
            int x = 0;
            var b = "Insert into TutorCourses values('" + Course + "','" + email + "')";
            SqlCommand cmd = new SqlCommand("select * from TutorCourses Where email= '" + email + "' and coursecode ='" + Course + "'", new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                return new CUD { rowEffected = x, Reason = "Already Subject   exist" };
            }
            else
            {
                SqlCommand cmd1 = new SqlCommand(b, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
                cmd.Connection.Close();
                cmd1.Connection.Open();
                x = cmd1.ExecuteNonQuery();
                cmd1.Connection.Close();
                return new CUD { rowEffected = x, Reason = "Successfully Subject Addeds" };
            }

            System.Diagnostics.Debug.WriteLine(x, b);
            sdr.Close();
            cmd.Connection.Close();
        }

        public Tutor TutorLogin(string id, string pass)
        {
          
            SqlCommand cmd = new SqlCommand("Select * from Tutor where email='"+id+"' and password='"+pass+"'", new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            //while (sdr.Read())
            //{
            //    Tutor s = new Tutor();

            if (sdr.HasRows)
            {
                sdr.Read();
                return new Tutor
                {
                    firstname = sdr["First_Name"].ToString(),
                    lastname = sdr["Last_Name"].ToString(),
                    email = sdr["Email"].ToString(),
                    address = sdr["address"].ToString(),
                    gender = Convert.ToChar(sdr["gender"].ToString()),
                    password = sdr["password"].ToString(),
                    phoneNo = sdr["Phone_No"].ToString(),
                    Error = "Data Reside In Database"
                };
            }
            else
            {
                return new Tutor { Error = "Email or Password Incorrect" };
            }
            //}
            sdr.Close();
            cmd.Connection.Close();
            //return st;
        }




        //public StudentData DoWork()
        //{

        //    return new StudentData { Email = "Ahmadzohaib369@gmail.com", Last_Name = "Faraz", First_Name = "Ali", id = 1  };
        //}
    }
}
