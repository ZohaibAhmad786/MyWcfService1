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
            SqlCommand cmd = new SqlCommand("Select * from Student", new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=DummyData;Persist Security Info=True;User ID=sa;Password=123"));
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

        public List<Courses> SCourses(string email)
        {
            List<Courses> course = new List<Courses>();
            string q = "select  distinct Title,Course.CourseCode from course LEft join studentcourses on studentcourses.coursecode = course.title where StudentCourses.CourseCode is null or StudentCourses.Email!='" + email + "'";
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

        public List<Courses> TCourses(string email)
        {
            List<Courses> course = new List<Courses>();
            string q = "select  distinct Title,Course.CourseCode from course LEft join TutorCourses on TutorCourses.coursecode = course.title where TutorCourses.CourseCode is null or TutorCourses.Email!='" + email + "'";
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
            var std = JsonConvert.DeserializeObject<StudentData>(data);
            int x;
            SqlCommand cmd = new SqlCommand("Insert into Student values('" + int.Parse(std.id.ToString()) + "','" + std.firstname + "','" + std.lastname + "','" + std.email + "')", new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=DummyData;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            x = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return x;

        }

        public /*int*/CUD InsertStudentData(Student s)//string email,string firstname,string lastname,string phoneNo,string discipline,string semester,string password,string address,string gender
        {
            //var std = JsonConvert.DeserializeObject<Student>(data);
            int x = 0;
            var query = "Insert into Student values('" + s.email + "','" + s.firstname + "','" + s.lastname + "','" + s.phoneNo + "','" + s.discipline + "','" + int.Parse(s.semester.ToString()) + "','" + s.password + "','" + s.address + "','" + Convert.ToChar(s.gender) + "','" + s.imgsrc + "')";
            SqlCommand cmd = new SqlCommand("select * from Student Where email= '" + s.email + "'", new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                return new CUD { rowEffected = x, Reason = "Already data exist" };
            }
            else
            {
                SqlCommand cmd1 = new SqlCommand(query, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
                cmd.Connection.Close();
                cmd1.Connection.Open();
                x = cmd1.ExecuteNonQuery();
                cmd1.Connection.Close();
                return new CUD { rowEffected = x, Reason = "Successfully Registered" };
            }

            System.Diagnostics.Debug.WriteLine(x, query);
            sdr.Close();


        }

        public /*int*/ CUD InsertTutortData(string email, string firstname, string lastname, string phoneNo, string address, string password, string gender)
        {
            //var td = JsonConvert.DeserializeObject<Tutor>(data);
            //
            int x = 0;
            var b = "Insert into Tutor values('" + email + "','" + firstname + "','" + lastname + "','" + phoneNo + "','" + address + "','" + password + "','" + Convert.ToChar(gender) + "')";
            SqlCommand cmd = new SqlCommand("select * from Tutor Where email= '" + email + "'", new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                return new CUD { rowEffected = x, Reason = "Already data exist" };
            }
            else
            {
                SqlCommand cmd1 = new SqlCommand(b, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
                cmd.Connection.Close();
                cmd1.Connection.Open();
                x = cmd1.ExecuteNonQuery();
                cmd1.Connection.Close();
                return new CUD { rowEffected = x, Reason = "Successfully Registered" };
            }

            System.Diagnostics.Debug.WriteLine(x, b);
            sdr.Close();
            //cmd.Connection.Close();


        }

        public CUD StudentCourses(string Course, string email)
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

        public List<Courses> StudentEnrollCourses(string email)
        {
            List<Courses> course = new List<Courses>();
            string q = "select * from StudentCourses where Email='" + email + "'";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Courses c = new Courses();
                c.CourseCode = sdr["CourseCode"].ToString();
                c.Title = sdr["Email"].ToString();
                course.Add(c);
            }
            sdr.Close();
            cmd.Connection.Close();
            return course;
        }
        public List<Courses> TutorEnrollCourses(string email)
        {
            List<Courses> course = new List<Courses>();
            string q = "select * from TutorCourses where Email='" + email + "'";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Courses c = new Courses();
                c.CourseCode = sdr["CourseCode"].ToString();
                c.Title = sdr["Email"].ToString();
                course.Add(c);
            }
            sdr.Close();
            cmd.Connection.Close();
            return course;
        }

        public /*List<Student>*/ Student StudentLogin(string email, string pass)
        {

            var a = "Select * from Student where email = '" + email + "' and Password = '" + pass + "'";
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
                    //imgsrc = sdr["imgsrc"].ToString(),
                    Error = "Data Reside In Database"
                };
            }
            else
            {
                return new Student { Error = "Email or Password Incorrect" };
            }
            //}
            sdr.Close();
            cmd.Connection.Close();
            //return st;
        }

        public CUD StudentSchedual(List<Schedual> Sc)
        {
            int x = 0;
            SqlConnection sc = new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123");
            sc.Open();
            foreach (var item in Sc)
            {
                string Query = "select * from Studentschedual Where email='" + item.AridNo + "' and timming ='" + item.Timming + "'";
                SqlCommand cmd = new SqlCommand(Query, sc);
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    Query = "update studentschedual set Monday='" + item.M + "',Tuesday='" + item.T + "',Wednesday='" + item.W + "',Thursday='" + item.Th + "',Friday='" + item.F + "',Saturday='" + item.S + "',sunday='" + item.Su + "' Where email='" + item.AridNo + "' and timming ='" + item.Timming + "'";
                    SqlCommand cmd1 = new SqlCommand(Query, sc);
                    sdr.Close();
                    x = cmd1.ExecuteNonQuery();
                }
                else
                {
                    sdr.Close();
                    Query = "insert into studentschedual values('" + item.AridNo + "','" + item.Timming + "','" + item.M + "','" + item.T + "','" + item.W + "','" + item.Th + "','" + item.F + "','" + item.S + "','" + item.Su + "')";
                    SqlCommand cmd1 = new SqlCommand(Query, sc);

                    x = cmd1.ExecuteNonQuery();
                }

            }
            sc.Close();
            return new CUD { Reason = "Schedual Updated Succesfully", rowEffected = x };
        }


        public CUD TutorSchedual(List<Schedual> Sc)
        {
            int x = 0;
            SqlConnection sc = new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123");
            sc.Open();
            foreach (var item in Sc)
            {
                string Query = "select * from tutorschedual Where email='" + item.AridNo + "' and timming ='" + item.Timming + "'";
                SqlCommand cmd = new SqlCommand(Query, sc);
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    Query = "update tutorschedual set Monday='" + item.M + "',Tuesday='" + item.T + "',Wednesday='" + item.W + "',Thursday='" + item.Th + "',Friday='" + item.F + "',Saturday='" + item.S + "',sunday='" + item.Su + "' Where email='" + item.AridNo + "' and timming ='" + item.Timming + "'";
                    SqlCommand cmd1 = new SqlCommand(Query, sc);
                    sdr.Close();
                    x = cmd1.ExecuteNonQuery();
                }
                else
                {
                    sdr.Close();
                    Query = "insert into tutorschedual values('" + item.AridNo + "','" + item.Timming + "','" + item.M + "','" + item.T + "','" + item.W + "','" + item.Th + "','" + item.F + "','" + item.S + "','" + item.Su + "')";
                    SqlCommand cmd1 = new SqlCommand(Query, sc);

                    x = cmd1.ExecuteNonQuery();
                }

            }
            sc.Close();
            return new CUD { Reason = "Schedual Updated Succesfully", rowEffected = x };
        }

        public CUD TutorCourses(string Course, string email)
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

            SqlCommand cmd = new SqlCommand("Select * from Tutor where email='" + id.Trim() + "' and password='" + pass + "'", new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
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

        public List<Days> Monday(string email, string sub)
        {
            //var query = "select distinct ts.Timming,ts.Email,ts.Monday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=ts.Email ) as [Full Name] from TutorCourses t  join TutorSchedual ts on ts.email = t.email join StudentSchedual st on st.timming =ts.Timming where  st.monday='1' and ts.monday='1' and ts.Email not like '"+email+"'  and t.CourseCode='"+sub+"' order by Timming desc";
            //var query = "select distinct sc.Timming,tc.Email,sc.Monday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email ) as [Full Name] from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email where sc.Email='" + email+"' and sc.Monday=tc.Monday  and tc.Monday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode='"+sub+"'";
            var query = "select distinct sc.Timming,tc.Email,sc.Monday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email ) as [Full Name],  (select [status] as stats from RequestTutor rt where rt.SEmail='" + email + "'  and rt.TEmail=tc.Email and rt.Timming=sc.Timming  and subject='" + sub + "') as tutorStatus from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email  where sc.Email='" + email + "' and sc.Monday=tc.Monday  and tc.Monday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode='" + sub + "'";
            List<Days> days = new List<Days>();
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                string data = sdr["tutorStatus"].ToString();
                if (sdr["tutorStatus"].ToString() == "1" || sdr["tutorStatus"].ToString() == DBNull.Value.ToString())
                {
                    Days d = new Days();
                    d.day = "Monday";
                    d.tutorName = sdr["Full Name"].ToString();
                    d.Timming = sdr["Timming"].ToString();
                    d.Email = sdr["Email"].ToString();
                    d.tutorStatus = sdr["tutorStatus"].ToString();
                    days.Add(d);
                }
            }
            sdr.Close();
            cmd.Connection.Close();
            return days;
        }

        public List<Days> Tuesday(string email, string sub)
        {
            //var query = "select distinct sc.Timming,tc.Email,sc.Tuesday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email  ) as [Full Name] from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email where sc.Email='" + email+"' and sc.Tuesday=tc.Tuesday  and tc.Tuesday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode='"+sub+"'";
            var query = "select distinct sc.Timming,tc.Email,sc.Tuesday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email ) as [Full Name],  (select [status] as stats from RequestTutor rt where rt.SEmail='" + email + "'  and rt.TEmail=tc.Email and rt.Timming=sc.Timming  and subject='" + sub + "') as tutorStatus from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email  where sc.Email='" + email + "' and sc.Tuesday=tc.Tuesday  and tc.Tuesday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode='" + sub + "'";
            List<Days> days = new List<Days>();
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                string data = sdr["tutorStatus"].ToString();
                if (sdr["tutorStatus"].ToString() == "1" || sdr["tutorStatus"].ToString() == DBNull.Value.ToString())
                {
                    Days d = new Days();
                    d.day = "Tuesday";
                    d.tutorName = sdr["Full Name"].ToString();
                    d.Timming = sdr["Timming"].ToString();
                    d.Email = sdr["Email"].ToString();
                    d.tutorStatus = sdr["tutorStatus"].ToString();
                    days.Add(d);
                }
            }
            sdr.Close();
            System.Diagnostics.Debug.WriteLine(days);
            cmd.Connection.Close();

            return days;
        }

        public List<Days> Wednesday(string email, string sub)
        {

            var query = "select distinct sc.Timming,tc.Email,sc.Wednesday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email ) as [Full Name],  (select [status] as stats from RequestTutor rt where rt.SEmail='" + email + "'  and rt.TEmail=tc.Email and rt.Timming=sc.Timming  and subject='" + sub + "') as tutorStatus from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email  where sc.Email='" + email + "' and sc.Wednesday=tc.Wednesday  and tc.Wednesday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode='" + sub + "'";
            // var query = "select distinct sc.Timming,tc.Email,sc.Wednesday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email  ) as [Full Name] from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email where sc.Email='" + email+ "' and sc.Wednesday=tc.Wednesday  and tc.Wednesday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode='" + sub+"'";
            List<Days> days = new List<Days>();
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                string data = sdr["tutorStatus"].ToString();
                if (sdr["tutorStatus"].ToString() == "1" || sdr["tutorStatus"].ToString() == DBNull.Value.ToString())
                {
                    Days d = new Days();
                    d.day = "Wednesday";
                    d.tutorName = sdr["Full Name"].ToString();
                    d.Timming = sdr["Timming"].ToString();
                    d.Email = sdr["Email"].ToString();
                    d.tutorStatus = sdr["tutorStatus"].ToString();
                    days.Add(d);
                }
            }
            sdr.Close();
            System.Diagnostics.Debug.WriteLine(days);
            cmd.Connection.Close();

            return days;
        }

        public List<Days> Thursday(string email, string sub)
        {
            //var query = "select distinct sc.Timming,tc.Email,sc.Thursday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email  ) as [Full Name] from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email where sc.Email='" + email + "' and sc.Thursday=tc.Thursday  and tc.Thursday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode='" + sub + "'";
            var query = "select distinct sc.Timming,tc.Email,sc.Thursday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email ) as [Full Name],  (select [status] as stats from RequestTutor rt where rt.SEmail='" + email + "'  and rt.TEmail=tc.Email and rt.Timming=sc.Timming  and subject='" + sub + "') as tutorStatus from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email  where sc.Email='" + email + "' and sc.Thursday=tc.Thursday  and tc.Thursday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode='" + sub + "'";
            List<Days> days = new List<Days>();
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                string data = sdr["tutorStatus"].ToString();
                if (sdr["tutorStatus"].ToString() == "1" || sdr["tutorStatus"].ToString() == DBNull.Value.ToString())
                {
                    Days d = new Days();
                    d.day = "Thursday";
                    d.tutorName = sdr["Full Name"].ToString();
                    d.Timming = sdr["Timming"].ToString();
                    d.Email = sdr["Email"].ToString();
                    d.tutorStatus = sdr["tutorStatus"].ToString();
                    days.Add(d);
                }
            }
            sdr.Close();
            System.Diagnostics.Debug.WriteLine(days);
            cmd.Connection.Close();

            return days;
        }

        public List<Days> Friday(string email, string sub)
        {
            //var query = "select distinct sc.Timming,tc.Email,sc.Friday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email  ) as [Full Name] from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email where sc.Email='" + email + "' and sc.Friday=tc.Friday  and tc.Friday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode='" + sub + "'";
            var query = "select distinct sc.Timming,tc.Email,sc.Friday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email ) as [Full Name],  (select [status] as stats from RequestTutor rt where rt.SEmail='" + email + "'  and rt.TEmail=tc.Email and rt.Timming=sc.Timming  and subject='" + sub + "') as tutorStatus from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email  where sc.Email='" + email + "' and sc.Friday=tc.Friday  and tc.Friday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode='" + sub + "'";
            List<Days> days = new List<Days>();
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                string data = sdr["tutorStatus"].ToString();
                if (sdr["tutorStatus"].ToString()== "1" || sdr["tutorStatus"].ToString() == DBNull.Value.ToString())
                {
                    Days d = new Days();
                d.day = "Friday";
                d.tutorName = sdr["Full Name"].ToString();
                d.Timming = sdr["Timming"].ToString();
                d.Email = sdr["Email"].ToString();
                d.tutorStatus = sdr["tutorStatus"].ToString();
                days.Add(d);
            }
            }
            sdr.Close();
            System.Diagnostics.Debug.WriteLine(days);
            cmd.Connection.Close();

            return days;
        }

        public List<Days> Saturday(string email, string sub)
        {
            //var query = "select distinct sc.Timming,tc.Email,sc.Saturday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email  ) as [Full Name] from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email where sc.Email='" + email + "' and sc.Saturday=tc.Saturday  and tc.Saturday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode='" + sub + "'";
            var query = "select distinct sc.Timming,tc.Email,sc.Saturday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email ) as [Full Name],  (select [status] as stats from RequestTutor rt where rt.SEmail='" + email + "'  and rt.TEmail=tc.Email and rt.Timming=sc.Timming  and subject='" + sub + "') as tutorStatus from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email  where sc.Email='" + email + "' and sc.Saturday=tc.Saturday  and tc.Saturday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode='" + sub + "'";
            List<Days> days = new List<Days>();
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                string data = sdr["tutorStatus"].ToString();
                if (sdr["tutorStatus"].ToString() == "1" || sdr["tutorStatus"].ToString() == DBNull.Value.ToString())
                {
                    Days d = new Days();
                    d.day = "Saturday";
                    d.tutorName = sdr["Full Name"].ToString();
                    d.Timming = sdr["Timming"].ToString();
                    d.Email = sdr["Email"].ToString();
                    d.tutorStatus = sdr["tutorStatus"].ToString();
                    days.Add(d);
                }
            }
            sdr.Close();
            System.Diagnostics.Debug.WriteLine(days);
            cmd.Connection.Close();

            return days;
        }

        public List<Days> Sunday(string email, string sub)
        {
            //var query = "select distinct sc.Timming,tc.Email,sc.Sunday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email  ) as [Full Name] from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email where sc.Email='" + email + "' and sc.Sunday=tc.Sunday  and tc.Sunday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode=ttc.CourseCode and ttc.CourseCode='" + sub + "'";
            var query = "select distinct sc.Timming,tc.Email,sc.Sunday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email ) as [Full Name],  (select [status] as stats from RequestTutor rt where rt.SEmail='" + email + "'  and rt.TEmail=tc.Email and rt.Timming=sc.Timming  and subject='" + sub + "') as tutorStatus from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email  where sc.Email='" + email + "' and sc.Sunday=tc.Sunday  and tc.Sunday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode='" + sub + "'";
            List<Days> days = new List<Days>();
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                string data = sdr["tutorStatus"].ToString();
                if (sdr["tutorStatus"].ToString() == "1" || sdr["tutorStatus"].ToString() == DBNull.Value.ToString())
                {
                    Days d = new Days();
                    d.day = "Sunday";
                    d.tutorName = sdr["Full Name"].ToString();
                    d.Timming = sdr["Timming"].ToString();
                    d.Email = sdr["Email"].ToString();
                    d.tutorStatus = sdr["tutorStatus"].ToString();
                    days.Add(d);
                }
            }
            sdr.Close();
            System.Diagnostics.Debug.WriteLine(days);
            cmd.Connection.Close();

            return days;
        }

        public List<Schedual> GetStudentSche(string email)
        {
            List<Schedual> schedual = new List<Schedual>();
            SqlCommand cmd = new SqlCommand("select * from studentschedual where email='" + email + "'", new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Schedual sc = new Schedual();
                sc.AridNo = sdr["email"].ToString();
                sc.Timming = sdr["Timming"].ToString();
                sc.M = sdr["Monday"].ToString();
                sc.T = sdr["Tuesday"].ToString();
                sc.W = sdr["Wednesday"].ToString();
                sc.Th = sdr["Thursday"].ToString();
                sc.F = sdr["Friday"].ToString();
                sc.S = sdr["Saturday"].ToString();
                sc.Su = sdr["Sunday"].ToString();
                schedual.Add(sc);
            }
            cmd.Connection.Close();
            return schedual;
        }


        public List<Schedual> GetTutorSche(string email)
        {
            List<Schedual> schedual = new List<Schedual>();
            SqlCommand cmd = new SqlCommand("select * from tutorschedual where email='" + email + "'", new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Schedual sc = new Schedual();
                sc.AridNo = sdr["email"].ToString();
                sc.Timming = sdr["Timming"].ToString();
                sc.M = sdr["Monday"].ToString();
                sc.T = sdr["Tuesday"].ToString();
                sc.W = sdr["Wednesday"].ToString();
                sc.Th = sdr["Thursday"].ToString();
                sc.F = sdr["Friday"].ToString();
                sc.S = sdr["Saturday"].ToString();
                sc.Su = sdr["Sunday"].ToString();
                schedual.Add(sc);
            }
            cmd.Connection.Close();
            return schedual;
        }

        public CUD TutorReq(toTutorRequest t)
        {
            int x = 0;
            string q = "select * from RequestTutor where Timming ='" + t.Timmings + "' and Temail='"+t.TuEmail+"' and Day='"+t.Day+"'";// only day has added if error Remove day only from This function..
            SqlCommand cmd1 = new SqlCommand(q, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd1.Connection.Open();
            SqlDataReader sdr = cmd1.ExecuteReader();
            if (sdr.HasRows)
            {
                cmd1.Connection.Close();
                return new CUD { rowEffected = x, Reason = "Can not Send Request at Same Time" };
            }
            else
            {
                cmd1.Connection.Close();
                string query = "insert into RequestTutor values('" + t.SEmail + "','" + t.TuEmail + "','" + t.Timmings + "','" + t.Subj + "','" + t.Day + "','" + 1 + "')";
                SqlCommand cmd = new SqlCommand(query, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
                cmd.Connection.Open();

                x = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                if (x == 1)
                {
                    return new CUD { rowEffected = x, Reason = "Request Submitted" };
                }
                else
                {
                    return new CUD { rowEffected = x, Reason = "Faild to Send Request" };
                }
            }



        }

        public List<StudentRequest> RequestAcceptOrReject(string sub, string email)
        {
            List<StudentRequest> lstSr = new List<StudentRequest>();
            string q = "select * from RequestTutor where subject='" + sub + "' and temail='" + email + "'";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                StudentRequest sr = new StudentRequest();
                sr.SEmail = sdr["sEmail"].ToString();
                sr.TEmail = sdr["Temail"].ToString();
                sr.Timming = sdr["Timming"].ToString();
                sr.Day = sdr["Day"].ToString();
                sr.Subject = sdr["Subject"].ToString();
                lstSr.Add(sr);
            }
            sdr.Close();
            cmd.Connection.Close();
            return lstSr;
        }

        public CUD StdDelCourse(string email, string sub)
        {
            int x = 0;
            string q = "delete  from StudentCourses where CourseCode='" + sub + "' and email='" + email + "'";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            x = cmd.ExecuteNonQuery();

            cmd.Connection.Close();
            if (x == 1)
            {
                return new CUD { Reason = sub + " Succesfully Deleted" };
            }
            else
            {
                return new CUD { Reason = sub + " Failed to Deleted" };
            }
        }

        public CUD TutorDelCourse(string email, string sub)
        {
            int x = 0;
            string q = "delete  from TutorCourses where CourseCode='" + sub + "' and email='" + email + "'";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            x = cmd.ExecuteNonQuery();

            cmd.Connection.Close();
            if (x == 1)
            {
                return new CUD { Reason = sub + " Succesfully Deleted" };
            }
            else
            {
                return new CUD { Reason = sub + " Failed to Deleted" };
            }
        }

        public List<toTutorRequest> TutorAcceptRequest(string email, string sub)
        {
            List<toTutorRequest> tutorReq = new List<toTutorRequest>();
            string query = "select (select Student.First_Name+' '+Student.Last_Name  from Student where Email=RequestTutor.SEmail) as [fullname],SEmail,Timming,[Day],[Subject] from RequestTutor where TEmail='" + email + "' and subject='" + sub + "' and Status=1";
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                while (sdr.Read())
                {
                    toTutorRequest t = new toTutorRequest();
                    t.SEmail = sdr["Semail"].ToString();
                    t.Subj = sdr["subject"].ToString();
                    t.Timmings = sdr["timming"].ToString();
                    t.Day = sdr["Day"].ToString();
                    t.Name = sdr["fullname"].ToString();
                    tutorReq.Add(t);
                }

                sdr.Close();
                return tutorReq;
            }
            else
            {
                toTutorRequest t = new toTutorRequest();
                t.Reason = "No Request Of This Subject";
                sdr.Close();
                return tutorReq;
            }
            cmd.Connection.Close();
        }

        public CUD TutorAcceptedRequest(toTutorRequest t)
        {
            int x = 0;
            string q = "update RequestTutor set   status='" + 2 + "'  where Temail='" + t.TuEmail + "' and semail='"+t.SEmail+"' and Day='"+t.Day+"' and timming='"+t.Timmings+"' and subject='"+t.Subj+"'  ";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            x = cmd.ExecuteNonQuery();

            cmd.Connection.Close();
            if (x == 1)
            {
                return new CUD { Reason = t.SEmail + " Request Accept" };
            }
            else
            {
                return new CUD { Reason = t.SEmail + " Failed to Accept" };
            }
        }

        public List<HeldClassess> TodayStudentClasses(string Email, string day, string datetimetoday)
        {
            string q = "select (select Email from Student where Email=rt.SEmail) as StuEmail,(select btnStatus from HeldStuClass hs where hs.Semail=rt.SEmail and hs.Temail=rt.TEmail and hs.DayTimeMonth='"+datetimetoday+ "' and hs.Timmings=rt.Timming and hs.[Day]=rt.[Day] and hs.[Subject]=rt.[Subject]) as bitStatus,(select First_Name+' '+Last_Name from Student where Email=rt.SEmail) as [fullname],[Day],[Subject],[Timming] from requesttutor rt where rt.TEmail='" + Email+"' and rt.[Day]='"+day+ "' and rt.[Status]=2";
            List<HeldClassess> Hldcls = new List<HeldClassess>();
           // string query = "select (select Student.First_Name+' '+Student.Last_Name  from Student where Email=RequestTutor.SEmail) as [fullname],SEmail,Timming,[Day],[Subject] from RequestTutor where TEmail='" + email + "' and subject='" + sub + "' and Status=1";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                while (sdr.Read())
                {
                    
                        HeldClassess t = new HeldClassess();
                        t.SEmail = sdr["StuEmail"].ToString();
                        t.Subj = sdr["subject"].ToString();
                        t.Timmings = sdr["timming"].ToString();
                        t.Day = sdr["Day"].ToString();
                        t.Name = sdr["fullname"].ToString();
                        t.status = sdr["bitStatus"].ToString();
                        Hldcls.Add(t);
                    
                }

                sdr.Close();
                return Hldcls;
            }
            else
            {
                HeldClassess t = new HeldClassess();
                t.Reason = "No Classes Today";
                sdr.Close();
                return Hldcls;
            }
            cmd.Connection.Close();
        }

        public CUD TutorHeldStudentClassses(HeldClassess t)
        {
            int x = 0;
            string Held = "Held";
            string q = "insert into HeldStuClass values('"+t.SEmail+ "','" + t.TuEmail + "','" + t.Timmings + "','" + t.Day + "','" + t.Subj + "','"+Held.ToString()+"','"+t.DateTimeToday+"','"+1+"')";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            x = cmd.ExecuteNonQuery();

            cmd.Connection.Close();
            if (x == 1)
            {
                return new CUD { Reason = "Held Class" ,rowEffected=x};
            }
            else
            {
                return new CUD { Reason = "Faild to Held Class", rowEffected = x };
            }
        }
        public CUD TutorCancelStudentClassses(HeldClassess t)
        {
            int x = 0;
            string Held = "Cancel";
            string q = "insert into HeldStuClass values('" + t.SEmail + "','" + t.TuEmail + "','" + t.Timmings + "','" + t.Day + "','" + t.Subj + "','" + Held.ToString() + "','" + t.DateTimeToday + "','" + 1 + "')";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            x = cmd.ExecuteNonQuery();

            cmd.Connection.Close();
            if (x == 1)
            {
                return new CUD { Reason = "Cancel Class" ,rowEffected=x};
            }
            else
            {
                return new CUD { Reason = "Faild to cancel Class" ,rowEffected=x};
            }
        }

        public CUD TutorRejectedRequest(toTutorRequest t)
        {
            int x = 0;
            string val = null;
            string q = "delete from requestTutor  where Temail='" + t.TuEmail + "' and semail='" + t.SEmail + "' and Day='" + t.Day + "' and timming='" + t.Timmings + "' and subject='" + t.Subj + "'  ";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(@"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123"));
            cmd.Connection.Open();
            x = cmd.ExecuteNonQuery();

            cmd.Connection.Close();
            if (x == 1)
            {
                return new CUD { Reason = t.SEmail + " Request Reject" };
            }
            else
            {
                return new CUD { Reason = t.SEmail + " Failed to Accept" };
            }
        }
    }
}
