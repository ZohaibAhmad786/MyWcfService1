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
        string connectionString = @"Data Source=DESKTOP-ILO8D81\SQLEXPRESS;Initial Catalog=FYPDatabase;Persist Security Info=True;User ID=sa;Password=123";
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
            List<Courses> allcourse = new List<Courses>();
            List<Courses> senrollcourse = new List<Courses>();
            string query1 = "select * from course";
            string query2 = "select * from studentcourses where email='" + email + "'";
            SqlCommand cmd = new SqlCommand(query1, new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Courses c = new Courses();
                c.CourseCode = sdr["CourseCode"].ToString();
                c.Title = sdr["Title"].ToString();
                allcourse.Add(c);
            }
            sdr.Close();
            cmd.Connection.Close();

            SqlCommand cmd1 = new SqlCommand(query2, new SqlConnection(connectionString));
            cmd1.Connection.Open();
            SqlDataReader sdr1 = cmd1.ExecuteReader();
            while (sdr1.Read())
            {
                Courses c = new Courses();
                c.CourseCode = sdr1["CourseCode"].ToString();
                //c.Title = sdr1["Title"].ToString();
                senrollcourse.Add(c);
            }
            sdr1.Close();
            cmd1.Connection.Close();

            foreach (var item in allcourse.ToList())
            {
                foreach (var item1 in senrollcourse.ToList())
                {
                    if (item.Title == item1.CourseCode)
                    {
                        allcourse.Remove(item);
                    }
                }
            }



            return allcourse;
        }

        public List<Courses> TCourses(string email)
        {
            List<Courses> allcourse = new List<Courses>();
            List<Courses> senrollcourse = new List<Courses>();
            string query1 = "select * from course";
            string query2 = "select * from TutorCourses where email='" + email + "'";
            SqlCommand cmd = new SqlCommand(query1, new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Courses c = new Courses();
                c.CourseCode = sdr["CourseCode"].ToString();
                c.Title = sdr["Title"].ToString();
                allcourse.Add(c);
            }
            sdr.Close();
            cmd.Connection.Close();

            SqlCommand cmd1 = new SqlCommand(query2, new SqlConnection(connectionString));
            cmd1.Connection.Open();
            SqlDataReader sdr1 = cmd1.ExecuteReader();
            while (sdr1.Read())
            {
                Courses c = new Courses();
                c.CourseCode = sdr1["CourseCode"].ToString();
                //c.Title = sdr1["Title"].ToString();
                senrollcourse.Add(c);
            }
            sdr1.Close();
            cmd1.Connection.Close();

            foreach (var item in allcourse.ToList())
            {
                foreach (var item1 in senrollcourse.ToList())
                {
                    if (item.Title == item1.CourseCode)
                    {
                        allcourse.Remove(item);
                    }
                }
            }



            return allcourse;
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
            var query = "Insert into Student values('" + s.email + "','" + s.firstname + "','" + s.lastname + "','" + s.phoneNo + "','" + s.password + "','" + s.address + "','" + Convert.ToChar(s.gender) + "','" + s.imgsrc + "','" + s.Type + "','" + s.CNIC + "')";
            SqlCommand cmd = new SqlCommand("select * from Tutor Where email= '" + s.email + "'", new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                cmd.Connection.Close();
                sdr.Close();
                return new CUD { rowEffected = x, Reason = "Email Already Taken" };
            }
            else
            {
                cmd.Connection.Close();
                sdr.Close();
                SqlCommand cmd1 = new SqlCommand("select * from  Student where email= '" + s.email + "'", new SqlConnection(connectionString));
                cmd1.Connection.Open();
                SqlDataReader sdr1 = cmd1.ExecuteReader();
                if (sdr1.HasRows)
                {
                    cmd1.Connection.Close();
                    sdr1.Close();
                    return new CUD { rowEffected = x, Reason = " Email Already Taken" };
                }
                else
                {
                    cmd1.Connection.Close();
                    sdr1.Close();
                    SqlCommand cmd3 = new SqlCommand("select * from PArent where email='" + s.email + "'", new SqlConnection(connectionString));
                    cmd3.Connection.Open();
                    SqlDataReader sdr4 = cmd3.ExecuteReader();
                    if (sdr4.HasRows)
                    {
                        cmd3.Connection.Close();
                        sdr4.Close();
                        return new CUD { rowEffected = x, Reason = "Email Already Taken" };
                    }
                    else
                    {
                        SqlCommand cmd2 = new SqlCommand(query, new SqlConnection(connectionString));

                        cmd2.Connection.Open();
                        x = cmd2.ExecuteNonQuery();
                        cmd2.Connection.Close();
                        return new CUD { rowEffected = x, Reason = "Successfully Registered" };
                    }
                }
            }

            System.Diagnostics.Debug.WriteLine(x, query);




        }

        public /*int*/ CUD InsertTutortData(Tutor t)
        {
            //var td = JsonConvert.DeserializeObject<Tutor>(data);
            //
            int x = 0;
            var b = "Insert into Tutor values('" + t.email + "','" + t.firstname + "','" + t.lastname + "','" + t.phoneNo + "','" + t.address + "','" + t.password + "','" + Convert.ToChar(t.gender) + "','" + t.imgsrc + "','" + t.Type + "','" + t.CNIC + "')";
            SqlCommand cmd = new SqlCommand("select * from Tutor Where email= '" + t.email + "' or CNIC='" + t.CNIC + "'", new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                cmd.Connection.Close();
                sdr.Close();
                return new CUD { rowEffected = x, Reason = "Email or CNIC Already Taken" };
            }
            else
            {
                cmd.Connection.Close();
                sdr.Close();
                SqlCommand cmd2 = new SqlCommand("select * from Student Where email= '" + t.email + "'", new SqlConnection(connectionString));
                cmd2.Connection.Open();
                SqlDataReader sdr1 = cmd2.ExecuteReader();
                if (sdr1.HasRows)
                {
                    cmd2.Connection.Close();
                    sdr1.Close();
                    return new CUD { rowEffected = x, Reason = "Email  Already Taken" };
                }
                else
                {
                    cmd2.Connection.Close();
                    sdr1.Close();
                    SqlCommand cmd3 = new SqlCommand("select * from PArent where email='" + t.email + "' or cnic='" + t.CNIC + "'", new SqlConnection(connectionString));
                    cmd3.Connection.Open();
                    SqlDataReader sdr4 = cmd3.ExecuteReader();
                    if (sdr4.HasRows)
                    {
                        cmd3.Connection.Close();
                        sdr4.Close();
                        return new CUD { rowEffected = x, Reason = "Email or CNIC Already Taken" };
                    }
                    else
                    {
                        SqlCommand cmd1 = new SqlCommand(b, new SqlConnection(connectionString));
                        cmd1.Connection.Open();
                        x = cmd1.ExecuteNonQuery();
                        cmd1.Connection.Close();
                        return new CUD { rowEffected = x, Reason = "Successfully Registered" };
                    }
                }
            }

            System.Diagnostics.Debug.WriteLine(x, b);
        }

        public /*int*/ CUD InsertParentData(Parent p)
        {
            //var td = JsonConvert.DeserializeObject<Tutor>(data);
            //
            int x = 0;
            var b = "Insert into Parent values('" + p.email + "','" + p.firstname + "','" + p.lastname + "','" + p.phoneNo + "','" + p.address + "','" + p.password + "','" + Convert.ToChar(p.gender) + "','" + p.imgsrc + "','" + p.Type + "','" + p.CNIC + "')";
            SqlCommand cmd = new SqlCommand("select * from Tutor Where email= '" + p.email + "' or CNIC='" + p.CNIC + "'", new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                cmd.Connection.Close();
                sdr.Close();
                return new CUD { rowEffected = x, Reason = "Email or CNIC Already Taken" };
            }
            else
            {
                cmd.Connection.Close();
                sdr.Close();
                SqlCommand cmd2 = new SqlCommand("select * from Student Where email= '" + p.email + "'", new SqlConnection(connectionString));
                cmd2.Connection.Open();
                SqlDataReader sdr1 = cmd2.ExecuteReader();
                if (sdr1.HasRows)
                {
                    cmd2.Connection.Close();
                    sdr1.Close();
                    return new CUD { rowEffected = x, Reason = "Email Already Taken" };
                }
                else
                {
                    cmd2.Connection.Close();
                    sdr1.Close();
                    SqlCommand cmd3 = new SqlCommand("select * from PArent where email='" + p.email + "' or cnic='" + p.CNIC + "'", new SqlConnection(connectionString));
                    cmd3.Connection.Open();
                    SqlDataReader sdr4 = cmd3.ExecuteReader();
                    if (sdr4.HasRows)
                    {
                        cmd3.Connection.Close();
                        sdr4.Close();
                        return new CUD { rowEffected = x, Reason = "Email or CNIC Already Taken" };
                    }
                    else
                    {
                        cmd3.Connection.Close();
                        sdr4.Close();
                        SqlCommand cmd1 = new SqlCommand(b, new SqlConnection(connectionString));
                        cmd1.Connection.Open();
                        x = cmd1.ExecuteNonQuery();
                        cmd1.Connection.Close();
                        return new CUD { rowEffected = x, Reason = "Successfully Registered" };
                    }
                }
            }

            System.Diagnostics.Debug.WriteLine(x, b);
        }

        public CUD StudentCourses(string Course, string email)
        {
            int x = 0;
            var q = "select * from StudentCourses Where email= '" + email + "' and coursecode ='" + Course + "'";
            var b = "Insert into StudentCourses values('" + Course + "','" + email + "')";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(connectionString));
            SqlCommand cmd2 = new SqlCommand(b, new SqlConnection(connectionString));
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
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(connectionString));
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


        public List<Courses> StudentFindTutor(string email)
        {
            List<Courses> course = new List<Courses>();
            string q = "select * from StudentCourses where Email='" + email + "'";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(connectionString));
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

            string query = "select * from RequestTutor where Semail='" + email + "' and Status=2";
            SqlCommand cmd1 = new SqlCommand(query, new SqlConnection(connectionString));
            cmd1.Connection.Open();
            SqlDataReader sdr1 = cmd1.ExecuteReader();

            while (sdr1.Read())
            {
                foreach (var item in course.ToList())
                {
                    if (sdr1["subject"].ToString() == item.CourseCode)
                    {
                        course.Remove(item);
                    }
                }

            }
            sdr1.Close();
            cmd1.Connection.Close();
            return course;
        }
        public List<Courses> TutorEnrollCourses(string email)
        {
            List<Courses> course = new List<Courses>();
            string q = "select * from TutorCourses where Email='" + email + "'";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(connectionString));
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
            SqlCommand cmd = new SqlCommand(a, new SqlConnection(connectionString));
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
            int counter = 0;
            SqlConnection sc = new SqlConnection(connectionString);
            sc.Open();
            foreach (var item in Sc)
            {
                if (item.M == "B" || item.M == "R")
                {
                    item.M = "1";
                }
                if (item.T == "B" || item.T == "R")
                {
                    item.T = "1";
                }
                if (item.W == "B" || item.W == "R")
                {
                    item.W = "1";
                }
                if (item.Th == "B" || item.Th == "R")
                {
                    item.Th = "1";
                }
                if (item.F == "B" || item.F == "R")
                {
                    item.F = "1";
                }
                if (item.S == "B" || item.S == "R")
                {
                    item.S = "1";
                }
                if (item.Su == "B" || item.Su == "R")
                {
                    item.Su = "1";
                }
                counter++;
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
                    Query = "insert into studentschedual values('" + item.AridNo + "','" + item.Timming + "','" + item.M + "','" + item.T + "','" + item.W + "','" + item.Th + "','" + item.F + "','" + item.S + "','" + item.Su + "','" + counter + "')";
                    SqlCommand cmd1 = new SqlCommand(Query, sc);

                    x = cmd1.ExecuteNonQuery();
                }

            }
            counter = 0;
            sc.Close();
            return new CUD { Reason = "Schedual Updated Succesfully", rowEffected = x };
        }


        public CUD TutorSchedual(List<Schedual> Sc)
        {
            int x = 0;
            int counter = 0;
            SqlConnection sc = new SqlConnection(connectionString);
            sc.Open();
            foreach (var item in Sc)
            {
                if (item.M == "B" || item.M == "R")
                {
                    item.M = "1";
                }
                if (item.T == "B" || item.T == "R")
                {
                    item.T = "1";
                }
                if (item.W == "B" || item.W == "R")
                {
                    item.W = "1";
                }
                if (item.Th == "B" || item.Th == "R")
                {
                    item.Th = "1";
                }
                if (item.F == "B" || item.F == "R")
                {
                    item.F = "1";
                }
                if (item.S == "B" || item.S == "R")
                {
                    item.S = "1";
                }
                if (item.Su == "B" || item.Su == "R")
                {
                    item.Su = "1";
                }
                counter++;
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
                    Query = "insert into tutorschedual values('" + item.AridNo + "','" + item.Timming + "','" + item.M + "','" + item.T + "','" + item.W + "','" + item.Th + "','" + item.F + "','" + item.S + "','" + item.Su + "','" + counter + "')";
                    SqlCommand cmd1 = new SqlCommand(Query, sc);

                    x = cmd1.ExecuteNonQuery();
                }

            }
            counter = 0;
            sc.Close();
            return new CUD { Reason = "Schedual Updated Succesfully", rowEffected = x };
        }

        public CUD TutorCourses(string Course, string email, string fees)
        {
            int x = 0;
            var b = "Insert into TutorCourses values('" + Course + "','" + email + "')";
            var adminQuery = "Insert into [Admin] values('" + Course + "','" + email + "','" + int.Parse(fees) + "','none')";
            SqlCommand cmd = new SqlCommand("select * from TutorCourses Where email= '" + email + "' and coursecode ='" + Course + "'", new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                return new CUD { rowEffected = x, Reason = "Already Subject   exist" };
            }
            else
            {
                cmd.Connection.Close();
                SqlCommand cmd1 = new SqlCommand(b, new SqlConnection(connectionString));

                cmd1.Connection.Open();
                x = cmd1.ExecuteNonQuery();
                cmd1.Connection.Close();

                SqlCommand cmd2 = new SqlCommand(adminQuery, new SqlConnection(connectionString));

                cmd2.Connection.Open();
                x = cmd2.ExecuteNonQuery();
                cmd2.Connection.Close();
                return new CUD { rowEffected = x, Reason = "Successfully Subject Addeds" };
            }

            System.Diagnostics.Debug.WriteLine(x, b);
            sdr.Close();
            cmd.Connection.Close();
        }

        public Tutor TutorLogin(string id, string pass)
        {

            SqlCommand cmd = new SqlCommand("Select * from Tutor where email='" + id.Trim() + "' and password='" + pass + "'", new SqlConnection(connectionString));
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
            List<Days> odddays = new List<Days>();
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(connectionString));
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
                    d.rating = "0";
                    days.Add(d);
                }
            }
            sdr.Close();
            cmd.Connection.Close();
            var query1 = "select * from RequestTutor  where semail='" + email + "'";
            SqlCommand cmd1 = new SqlCommand(query1, new SqlConnection(connectionString));
            cmd1.Connection.Open();
            SqlDataReader sdr1 = cmd1.ExecuteReader();
            while (sdr1.Read())
            {
                Days d = new Days();
                d.day = sdr1["Day"].ToString();
                d.Timming = sdr1["Timming"].ToString();
                d.Email = sdr1["TEmail"].ToString();
                d.tutorName = sdr1["SEmail"].ToString();
                odddays.Add(d);
            }
            sdr1.Close();
            cmd1.Connection.Close();
            foreach (var item in days.ToList())
            {
                foreach (var item1 in odddays.ToList())
                {
                    if (item.day == item1.day && item.Timming == item1.Timming)
                    {
                        days.Remove(item);
                    }
                }
            }
            string ratingQuery = "select COUNT(*)*5 as totalRating ,SUM(Rating)*5 as sumRating ,Temail from discardStu where Subject='" + sub + "' group by Temail";
            SqlCommand cmdrating = new SqlCommand(ratingQuery, new SqlConnection(connectionString));
            cmdrating.Connection.Open();
            List<Days> ratingdays = new List<Days>();
            SqlDataReader sdrrating = cmdrating.ExecuteReader();
            while (sdrrating.Read())
            {
                Days d = new Days();
                d.Email = sdrrating["temail"].ToString();
                float SumRAting = float.Parse(sdrrating["sumRating"].ToString());
                float totalRating = float.Parse(sdrrating["totalRating"].ToString());
                d.rating = (SumRAting / totalRating).ToString();

                ratingdays.Add(d);
            }
            sdrrating.Close();
            cmdrating.Connection.Close();
            foreach (var item in days.ToList())
            {
                foreach (var item1 in ratingdays.ToList())
                {
                    if (item.Email == item1.Email)
                    {
                        item.rating = item1.rating;
                        break;
                    }
                    else
                    {
                        item.rating = "0";
                    }
                }
            }
            return days.OrderByDescending(d => d.rating).ToList();
        }

        public List<Days> Tuesday(string email, string sub)
        {
            //var query = "select distinct sc.Timming,tc.Email,sc.Tuesday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email  ) as [Full Name] from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email where sc.Email='" + email+"' and sc.Tuesday=tc.Tuesday  and tc.Tuesday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode='"+sub+"'";
            var query = "select distinct sc.Timming,tc.Email,sc.Tuesday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email ) as [Full Name],  (select [status] as stats from RequestTutor rt where rt.SEmail='" + email + "'  and rt.TEmail=tc.Email and rt.Timming=sc.Timming  and subject='" + sub + "') as tutorStatus from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email  where sc.Email='" + email + "' and sc.Tuesday=tc.Tuesday  and tc.Tuesday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode='" + sub + "'";
            List<Days> days = new List<Days>();
            List<Days> odddays = new List<Days>();
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(connectionString));
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
                    d.rating = "0";
                    days.Add(d);
                }
            }
            sdr.Close();
            System.Diagnostics.Debug.WriteLine(days);
            cmd.Connection.Close();
            var query1 = "select * from RequestTutor  where semail='" + email + "'";
            SqlCommand cmd1 = new SqlCommand(query1, new SqlConnection(connectionString));
            cmd1.Connection.Open();
            SqlDataReader sdr1 = cmd1.ExecuteReader();
            while (sdr1.Read())
            {
                Days d = new Days();
                d.day = sdr1["Day"].ToString();
                d.Timming = sdr1["Timming"].ToString();
                d.Email = sdr1["TEmail"].ToString();
                d.tutorName = sdr1["SEmail"].ToString();
                odddays.Add(d);
            }
            sdr1.Close();
            cmd1.Connection.Close();
            foreach (var item in days.ToList())
            {
                foreach (var item1 in odddays.ToList())
                {
                    if (item.day == item1.day && item.Timming == item1.Timming)
                    {
                        days.Remove(item);
                    }
                }
            }
            string ratingQuery = "select COUNT(*)*5 as totalRating ,SUM(Rating)*5 as sumRating ,Temail from discardStu where Subject='" + sub + "' group by Temail";
            SqlCommand cmdrating = new SqlCommand(ratingQuery, new SqlConnection(connectionString));
            cmdrating.Connection.Open();
            List<Days> ratingdays = new List<Days>();
            SqlDataReader sdrrating = cmdrating.ExecuteReader();
            while (sdrrating.Read())
            {
                Days d = new Days();
                d.Email = sdrrating["temail"].ToString();
                float SumRAting = float.Parse(sdrrating["sumRating"].ToString());
                float totalRating = float.Parse(sdrrating["totalRating"].ToString());
                d.rating = (SumRAting / totalRating).ToString();

                ratingdays.Add(d);
            }
            sdrrating.Close();
            cmdrating.Connection.Close();
            foreach (var item in days.ToList())
            {
                foreach (var item1 in ratingdays.ToList())
                {
                    if (item.Email == item1.Email)
                    {
                        item.rating = item1.rating;
                        break;
                    }
                    else
                    {
                        item.rating = "0";
                    }
                }
            }
            return days.OrderByDescending(d => d.rating).ToList();
            //return days;
        }

        public List<Days> Wednesday(string email, string sub)
        {

            var query = "select distinct sc.Timming,tc.Email,sc.Wednesday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email ) as [Full Name],  (select [status] as stats from RequestTutor rt where rt.SEmail='" + email + "'  and rt.TEmail=tc.Email and rt.Timming=sc.Timming  and subject='" + sub + "') as tutorStatus from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email  where sc.Email='" + email + "' and sc.Wednesday=tc.Wednesday  and tc.Wednesday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode='" + sub + "'";
            // var query = "select distinct sc.Timming,tc.Email,sc.Wednesday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email  ) as [Full Name] from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email where sc.Email='" + email+ "' and sc.Wednesday=tc.Wednesday  and tc.Wednesday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode='" + sub+"'";
            List<Days> days = new List<Days>();
            List<Days> odddays = new List<Days>();
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(connectionString));
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
                    d.rating = "0";
                    days.Add(d);
                }
            }
            sdr.Close();
            System.Diagnostics.Debug.WriteLine(days);
            cmd.Connection.Close();
            var query1 = "select * from RequestTutor  where semail='" + email + "'";
            SqlCommand cmd1 = new SqlCommand(query1, new SqlConnection(connectionString));
            cmd1.Connection.Open();
            SqlDataReader sdr1 = cmd1.ExecuteReader();
            while (sdr1.Read())
            {
                Days d = new Days();
                d.day = sdr1["Day"].ToString();
                d.Timming = sdr1["Timming"].ToString();
                d.Email = sdr1["TEmail"].ToString();
                d.tutorName = sdr1["SEmail"].ToString();
                odddays.Add(d);
            }
            sdr1.Close();
            cmd1.Connection.Close();
            foreach (var item in days.ToList())
            {
                foreach (var item1 in odddays.ToList())
                {
                    if (item.day == item1.day && item.Timming == item1.Timming)
                    {
                        days.Remove(item);
                    }
                }
            }
            string ratingQuery = "select COUNT(*)*5 as totalRating ,SUM(Rating)*5 as sumRating ,Temail from discardStu where Subject='" + sub + "' group by Temail";
            SqlCommand cmdrating = new SqlCommand(ratingQuery, new SqlConnection(connectionString));
            cmdrating.Connection.Open();
            List<Days> ratingdays = new List<Days>();
            SqlDataReader sdrrating = cmdrating.ExecuteReader();
            while (sdrrating.Read())
            {
                Days d = new Days();
                d.Email = sdrrating["temail"].ToString();
                float SumRAting = float.Parse(sdrrating["sumRating"].ToString());
                float totalRating = float.Parse(sdrrating["totalRating"].ToString());
                d.rating = (SumRAting / totalRating).ToString();

                ratingdays.Add(d);
            }
            sdrrating.Close();
            cmdrating.Connection.Close();
            foreach (var item in days.ToList())
            {
                foreach (var item1 in ratingdays.ToList())
                {
                    if (item.Email == item1.Email)
                    {
                        item.rating = item1.rating;
                        break;
                    }
                    else
                    {
                        item.rating = "0";
                    }
                }
            }
            return days.OrderByDescending(d => d.rating).ToList();
            //return days;
        }

        public List<Days> Thursday(string email, string sub)
        {
            //var query = "select distinct sc.Timming,tc.Email,sc.Thursday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email  ) as [Full Name] from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email where sc.Email='" + email + "' and sc.Thursday=tc.Thursday  and tc.Thursday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode='" + sub + "'";
            var query = "select distinct sc.Timming,tc.Email,sc.Thursday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email ) as [Full Name],  (select [status] as stats from RequestTutor rt where rt.SEmail='" + email + "'  and rt.TEmail=tc.Email and rt.Timming=sc.Timming  and subject='" + sub + "') as tutorStatus from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email  where sc.Email='" + email + "' and sc.Thursday=tc.Thursday  and tc.Thursday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode='" + sub + "'";
            List<Days> days = new List<Days>();
            List<Days> odddays = new List<Days>();
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(connectionString));
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
                    d.rating = "0";
                    days.Add(d);
                }
            }
            sdr.Close();
            System.Diagnostics.Debug.WriteLine(days);
            cmd.Connection.Close();
            var query1 = "select * from RequestTutor  where semail='" + email + "'";
            SqlCommand cmd1 = new SqlCommand(query1, new SqlConnection(connectionString));
            cmd1.Connection.Open();
            SqlDataReader sdr1 = cmd1.ExecuteReader();
            while (sdr1.Read())
            {
                Days d = new Days();
                d.day = sdr1["Day"].ToString();
                d.Timming = sdr1["Timming"].ToString();
                d.Email = sdr1["TEmail"].ToString();
                d.tutorName = sdr1["SEmail"].ToString();
                odddays.Add(d);
            }
            sdr1.Close();
            cmd1.Connection.Close();
            foreach (var item in days.ToList())
            {
                foreach (var item1 in odddays.ToList())
                {
                    if (item.day == item1.day && item.Timming == item1.Timming)
                    {
                        days.Remove(item);
                    }
                }
            }
            string ratingQuery = "select COUNT(*)*5 as totalRating ,SUM(Rating)*5 as sumRating ,Temail from discardStu where Subject='" + sub + "' group by Temail";
            SqlCommand cmdrating = new SqlCommand(ratingQuery, new SqlConnection(connectionString));
            cmdrating.Connection.Open();
            List<Days> ratingdays = new List<Days>();
            SqlDataReader sdrrating = cmdrating.ExecuteReader();
            while (sdrrating.Read())
            {
                Days d = new Days();
                d.Email = sdrrating["temail"].ToString();
                float SumRAting = float.Parse(sdrrating["sumRating"].ToString());
                float totalRating = float.Parse(sdrrating["totalRating"].ToString());
                d.rating = (SumRAting / totalRating).ToString();

                ratingdays.Add(d);
            }
            sdrrating.Close();
            cmdrating.Connection.Close();
            foreach (var item in days.ToList())
            {
                foreach (var item1 in ratingdays.ToList())
                {
                    if (item.Email == item1.Email)
                    {
                        item.rating = item1.rating;
                        break;
                    }
                    else
                    {
                        item.rating = "0";
                    }
                }
            }
            return days.OrderByDescending(d => d.rating).ToList();
            //return days;
        }

        public List<Days> Friday(string email, string sub)
        {
            //var query = "select distinct sc.Timming,tc.Email,sc.Friday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email  ) as [Full Name] from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email where sc.Email='" + email + "' and sc.Friday=tc.Friday  and tc.Friday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode='" + sub + "'";
            var query = "select distinct sc.Timming,tc.Email,sc.Friday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email ) as [Full Name],  (select [status] as stats from RequestTutor rt where rt.SEmail='" + email + "'  and rt.TEmail=tc.Email and rt.Timming=sc.Timming  and subject='" + sub + "') as tutorStatus from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email  where sc.Email='" + email + "' and sc.Friday=tc.Friday  and tc.Friday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode='" + sub + "'";
            List<Days> days = new List<Days>();
            List<Days> odddays = new List<Days>();
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                string data = sdr["tutorStatus"].ToString();
                if (sdr["tutorStatus"].ToString() == "1" || sdr["tutorStatus"].ToString() == DBNull.Value.ToString())
                {
                    Days d = new Days();
                    d.day = "Friday";
                    d.tutorName = sdr["Full Name"].ToString();
                    d.Timming = sdr["Timming"].ToString();
                    d.Email = sdr["Email"].ToString();
                    d.tutorStatus = sdr["tutorStatus"].ToString();
                    d.rating = "0";
                    days.Add(d);
                }
            }
            sdr.Close();
            System.Diagnostics.Debug.WriteLine(days);
            cmd.Connection.Close();
            var query1 = "select * from RequestTutor  where semail='" + email + "'";
            SqlCommand cmd1 = new SqlCommand(query1, new SqlConnection(connectionString));
            cmd1.Connection.Open();
            SqlDataReader sdr1 = cmd1.ExecuteReader();
            while (sdr1.Read())
            {
                Days d = new Days();
                d.day = sdr1["Day"].ToString();
                d.Timming = sdr1["Timming"].ToString();
                d.Email = sdr1["TEmail"].ToString();
                d.tutorName = sdr1["SEmail"].ToString();
                odddays.Add(d);
            }
            sdr1.Close();
            cmd1.Connection.Close();
            foreach (var item in days.ToList())
            {
                foreach (var item1 in odddays.ToList())
                {
                    if (item.day == item1.day && item.Timming == item1.Timming)
                    {
                        days.Remove(item);
                    }
                }
            }
            string ratingQuery = "select COUNT(*)*5 as totalRating ,SUM(Rating)*5 as sumRating ,Temail from discardStu where Subject='" + sub + "' group by Temail";
            SqlCommand cmdrating = new SqlCommand(ratingQuery, new SqlConnection(connectionString));
            cmdrating.Connection.Open();
            List<Days> ratingdays = new List<Days>();
            SqlDataReader sdrrating = cmdrating.ExecuteReader();
            while (sdrrating.Read())
            {
                Days d = new Days();
                d.Email = sdrrating["temail"].ToString();
                float SumRAting = float.Parse(sdrrating["sumRating"].ToString());
                float totalRating = float.Parse(sdrrating["totalRating"].ToString());
                d.rating = (SumRAting / totalRating).ToString();

                ratingdays.Add(d);
            }
            sdrrating.Close();
            cmdrating.Connection.Close();
            foreach (var item in days.ToList())
            {
                foreach (var item1 in ratingdays.ToList())
                {
                    if (item.Email == item1.Email)
                    {
                        item.rating = item1.rating;
                        break;
                    }
                    else
                    {
                        item.rating = "0";
                    }
                }
            }
            return days.OrderByDescending(d => d.rating).ToList();
            //return days;
        }

        public List<Days> Saturday(string email, string sub)
        {
            //var query = "select distinct sc.Timming,tc.Email,sc.Saturday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email  ) as [Full Name] from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email where sc.Email='" + email + "' and sc.Saturday=tc.Saturday  and tc.Saturday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode='" + sub + "'";
            var query = "select distinct sc.Timming,tc.Email,sc.Saturday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email ) as [Full Name],  (select [status] as stats from RequestTutor rt where rt.SEmail='" + email + "'  and rt.TEmail=tc.Email and rt.Timming=sc.Timming  and subject='" + sub + "') as tutorStatus from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email  where sc.Email='" + email + "' and sc.Saturday=tc.Saturday  and tc.Saturday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode='" + sub + "'";
            List<Days> days = new List<Days>();
            List<Days> odddays = new List<Days>();
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(connectionString));
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
                    d.rating = "0";
                    days.Add(d);
                }
            }
            sdr.Close();
            System.Diagnostics.Debug.WriteLine(days);
            cmd.Connection.Close();
            var query1 = "select * from RequestTutor  where semail='" + email + "'";
            SqlCommand cmd1 = new SqlCommand(query1, new SqlConnection(connectionString));
            cmd1.Connection.Open();
            SqlDataReader sdr1 = cmd1.ExecuteReader();
            while (sdr1.Read())
            {
                Days d = new Days();
                d.day = sdr1["Day"].ToString();
                d.Timming = sdr1["Timming"].ToString();
                d.Email = sdr1["TEmail"].ToString();
                d.tutorName = sdr1["SEmail"].ToString();
                odddays.Add(d);
            }
            sdr1.Close();
            cmd1.Connection.Close();
            foreach (var item in days.ToList())
            {
                foreach (var item1 in odddays.ToList())
                {
                    if (item.day == item1.day && item.Timming == item1.Timming)
                    {
                        days.Remove(item);
                    }
                }
            }
            string ratingQuery = "select COUNT(*)*5 as totalRating ,SUM(Rating)*5 as sumRating ,Temail from discardStu where Subject='" + sub + "' group by Temail";
            SqlCommand cmdrating = new SqlCommand(ratingQuery, new SqlConnection(connectionString));
            cmdrating.Connection.Open();
            List<Days> ratingdays = new List<Days>();
            SqlDataReader sdrrating = cmdrating.ExecuteReader();
            while (sdrrating.Read())
            {
                Days d = new Days();
                d.Email = sdrrating["temail"].ToString();
                float SumRAting = float.Parse(sdrrating["sumRating"].ToString());
                float totalRating = float.Parse(sdrrating["totalRating"].ToString());
                d.rating = (SumRAting / totalRating).ToString();

                ratingdays.Add(d);
            }
            sdrrating.Close();
            cmdrating.Connection.Close();
            foreach (var item in days.ToList())
            {
                foreach (var item1 in ratingdays.ToList())
                {
                    if (item.Email == item1.Email)
                    {
                        item.rating = item1.rating;
                        break;
                    }
                    else
                    {
                        item.rating = "0";
                    }
                }
            }
            return days.OrderByDescending(d => d.rating).ToList();
            //return days;
        }

        public List<Days> Sunday(string email, string sub)
        {
            //var query = "select distinct sc.Timming,tc.Email,sc.Sunday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email  ) as [Full Name] from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email where sc.Email='" + email + "' and sc.Sunday=tc.Sunday  and tc.Sunday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode=ttc.CourseCode and ttc.CourseCode='" + sub + "'";
            var query = "select distinct sc.Timming,tc.Email,sc.Sunday,(select [Tutor].First_Name+' '+[Tutor].Last_Name  from Tutor where Tutor.Email=tc.Email ) as [Full Name],  (select [status] as stats from RequestTutor rt where rt.SEmail='" + email + "'  and rt.TEmail=tc.Email and rt.Timming=sc.Timming  and subject='" + sub + "') as tutorStatus from TutorSchedual tc join StudentSchedual sc  on sc.Timming=tc.Timming join StudentCourses cc on cc.Email=sc.Email join TutorCourses ttc on ttc.Email=tc.Email  where sc.Email='" + email + "' and sc.Sunday=tc.Sunday  and tc.Sunday=1 and cc.CourseCode=ttc.CourseCode and ttc.CourseCode='" + sub + "'";
            List<Days> days = new List<Days>();
            List<Days> odddays = new List<Days>();
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(connectionString));
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
                    d.rating = "0";
                    days.Add(d);
                }
            }
            sdr.Close();
            System.Diagnostics.Debug.WriteLine(days);
            cmd.Connection.Close();
            var query1 = "select * from RequestTutor where semail='" + email + "'";
            SqlCommand cmd1 = new SqlCommand(query1, new SqlConnection(connectionString));
            cmd1.Connection.Open();
            SqlDataReader sdr1 = cmd1.ExecuteReader();
            while (sdr1.Read())
            {
                Days d = new Days();
                d.day = sdr1["Day"].ToString();
                d.Timming = sdr1["Timming"].ToString();
                d.Email = sdr1["TEmail"].ToString();
                d.tutorName = sdr1["SEmail"].ToString();
                odddays.Add(d);
            }
            sdr1.Close();
            cmd1.Connection.Close();
            foreach (var item in days.ToList())
            {
                foreach (var item1 in odddays.ToList())
                {
                    if (item.day == item1.day && item.Timming == item1.Timming)
                    {
                        days.Remove(item);
                    }
                }
            }
            string ratingQuery = "select COUNT(*)*5 as totalRating ,SUM(Rating)*5 as sumRating ,Temail from discardStu where Semail='" + email + "' and Subject='" + sub + "' group by Temail";
            SqlCommand cmdrating = new SqlCommand(ratingQuery, new SqlConnection(connectionString));
            cmdrating.Connection.Open();
            List<Days> ratingdays = new List<Days>();
            SqlDataReader sdrrating = cmdrating.ExecuteReader();
            while (sdrrating.Read())
            {
                Days d = new Days();
                d.Email = sdrrating["temail"].ToString();
                float SumRAting = float.Parse(sdrrating["sumRating"].ToString());
                float totalRating = float.Parse(sdrrating["totalRating"].ToString());
                d.rating = (SumRAting / totalRating).ToString();

                ratingdays.Add(d);
            }
            sdrrating.Close();
            cmdrating.Connection.Close();
            foreach (var item in days.ToList())
            {
                foreach (var item1 in ratingdays.ToList())
                {
                    if (item.Email == item1.Email)
                    {
                        item.rating = item1.rating;
                    }
                    else
                    {
                        item.rating = "0";
                    }
                }
            }
            return days.OrderByDescending(d => d.rating).ToList();
            //return days;
        }

        public List<Schedual> GetStudentSche(string email)
        {

            string queryq = "select * from Reschedule where semail='" + email + "' and [RescheStatus]=2";
            List<Schedual> schedual0 = new List<Schedual>();
            SqlCommand cmdd = new SqlCommand(queryq, new SqlConnection(connectionString));
            cmdd.Connection.Open();
            SqlDataReader sdrr = cmdd.ExecuteReader();
            while (sdrr.Read())
            {
                Schedual s = new Schedual();
                s.Timming = sdrr["timmings"].ToString();
                if (sdrr["Day"].ToString() == "Monday")
                {
                    s.M = "1";
                }
                else if (sdrr["Day"].ToString() == "Tuesday")
                {
                    s.T = "1";
                }
                else if (sdrr["Day"].ToString() == "Wednesday")
                {
                    s.W = "1";
                }
                else if (sdrr["Day"].ToString() == "Thursday")
                {
                    s.Th = "1";
                }
                else if (sdrr["Day"].ToString() == "Friday")
                {
                    s.F = "1";
                }
                else if (sdrr["Day"].ToString() == "Saturday")
                {
                    s.S = "1";
                }
                else
                {
                    s.Su = "1";
                }
                schedual0.Add(s);
            }
            sdrr.Close();
            cmdd.Connection.Close();
            string q = "select * from RequestTutor where status=2 and semail='" + email + "'";
            List<Schedual> schedual1 = new List<Schedual>();
            SqlCommand cmd2 = new SqlCommand(q, new SqlConnection(connectionString));
            cmd2.Connection.Open();
            SqlDataReader sdr2 = cmd2.ExecuteReader();
            while (sdr2.Read())
            {
                Schedual sc = new Schedual();

                sc.Timming = sdr2["timming"].ToString();
                if (sdr2["Day"].ToString() == "Monday")
                {
                    sc.M = "1";
                }
                else if (sdr2["Day"].ToString() == "Tuesday")
                {
                    sc.T = "1";
                }
                else if (sdr2["Day"].ToString() == "Wednesday")
                {
                    sc.W = "1";
                }
                else if (sdr2["Day"].ToString() == "Thursday")
                {
                    sc.Th = "1";
                }
                else if (sdr2["Day"].ToString() == "Friday")
                {
                    sc.F = "1";
                }
                else if (sdr2["Day"].ToString() == "Saturday")
                {
                    sc.S = "1";
                }
                else
                {
                    sc.Su = "1";
                }
                schedual1.Add(sc);
            }
            cmd2.Connection.Close();
            sdr2.Close();
            List<Schedual> schedual = new List<Schedual>();
            SqlCommand cmd = new SqlCommand("select * from studentschedual where email='" + email + "' and  countRows < 23 order by countRows asc", new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Schedual sc = new Schedual();
                sc.AridNo = sdr["email"].ToString();
                sc.Timming = sdr["Timming"].ToString();
                if (sdr["monday"].ToString() == "1")
                {
                    foreach (var item in schedual1.ToList())
                    {

                        if (item.M == sdr["Monday"].ToString() && sc.Timming == item.Timming)
                        {

                            if (schedual0.Count == 0)
                            {
                                sc.M = "B";
                                break;
                            }
                            else if (schedual0.ToList().Count > 0)
                            {
                                foreach (var item1 in schedual0.ToList())
                                {
                                    if (item1.M == sdr["Monday"].ToString() && sc.Timming == item1.Timming)
                                    {
                                        sc.M = "R";
                                        break;
                                    }
                                    else
                                    {
                                        sc.M = "B";
                                        //break;
                                    }
                                }
                                break;
                            }
                            else
                            {
                                sc.M = "1";
                            }

                        }
                        else
                        {
                            sc.M = "1";
                        }
                        //else if (schedual0.ToList().Count > 0)
                        //{
                        //    foreach (var item1 in schedual0)
                        //    {
                        //        if (item1.M == sdr["Monday"].ToString() && sc.Timming == item1.Timming)
                        //        {
                        //            sc.M = "R";
                        //            break;
                        //        }
                        //        else
                        //        {
                        //            sc.M = "1";
                        //            //break;
                        //        }
                        //    }

                        //}
                        //else
                        //{
                        //    sc.M = "1";
                        //}
                    }
                    if (schedual1.ToList().Count == 0)
                    {
                        sc.M = "1";
                    }

                }
                else
                {
                    sc.M = "0";
                }
                if (sdr["tuesday"].ToString() == "1")
                {
                    foreach (var item in schedual1.ToList())
                    {
                        if (item.T == sdr["Tuesday"].ToString() && sc.Timming == item.Timming)
                        {
                            if (schedual0.Count == 0)
                            {
                                sc.T = "B";
                                break;
                            }
                            else if (schedual0.ToList().Count > 0)
                            {
                                foreach (var item1 in schedual0)
                                {
                                    if (item1.T == sdr["Tuesday"].ToString() && sc.Timming == item1.Timming)
                                    {
                                        sc.T = "R";
                                        break;
                                    }
                                    else
                                    {
                                        sc.T = "B";
                                    }
                                }
                                break;
                            }
                            else
                            {
                                sc.T = "1";
                            }
                        }
                        else
                        {
                            sc.T = "1";
                        }
                        //else if (schedual0.ToList().Count > 0)
                        //{
                        //    foreach (var item1 in schedual0)
                        //    {
                        //        if (item1.T == sdr["Tuesday"].ToString() && sc.Timming == item1.Timming)
                        //        {
                        //            sc.T = "R";
                        //            break;
                        //        }
                        //        else
                        //        {
                        //            sc.T = "1";
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    sc.T = "1";
                        //    //break;
                        //}
                    }
                    if (schedual1.ToList().Count == 0)
                    {
                        sc.T = "1";
                    }
                }
                else
                {
                    sc.T = "0";
                }
                if (sdr["wednesday"].ToString() == "1")
                {
                    foreach (var item in schedual1.ToList())
                    {
                        if (item.W == sdr["wednesday"].ToString() && sc.Timming == item.Timming)
                        {
                            if (schedual0.Count == 0)
                            {
                                sc.W = "B";
                                break;
                            }
                            else if (schedual0.ToList().Count > 0)
                            {
                                foreach (var item1 in schedual0)
                                {
                                    if (item1.W == sdr["wednesday"].ToString() && sc.Timming == item1.Timming)
                                    {
                                        sc.W = "R";
                                        break;
                                    }
                                    else
                                    {
                                        sc.W = "B";
                                    }
                                }
                                break;
                            }
                            else
                            {
                                sc.W = "1";
                                //break;
                            }
                        }
                        else
                        {
                            sc.W = "1";
                        }
                        //else if (schedual0.ToList().Count > 0)
                        //{
                        //    foreach (var item1 in schedual0)
                        //    {
                        //        if (item1.W == sdr["wednesday"].ToString() && sc.Timming == item1.Timming)
                        //        {
                        //            sc.W = "R";
                        //            break;
                        //        }
                        //        else
                        //        {
                        //            sc.W = "1";
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    sc.W = "1";
                        //    //break;
                        //}
                    }
                    if (schedual1.ToList().Count == 0)
                    {
                        sc.W = "1";
                    }
                }
                else
                {
                    sc.W = "0";
                }
                if (sdr["thursday"].ToString() == "1")
                {
                    foreach (var item in schedual1.ToList())
                    {
                        if (item.Th == sdr["thursday"].ToString() && sc.Timming == item.Timming)
                        {
                            if (schedual0.Count == 0)
                            {
                                sc.Th = "B";
                                break;
                            }
                            else if (schedual0.ToList().Count > 0)
                            {
                                foreach (var item1 in schedual0)
                                {
                                    if (item1.Th == sdr["thursday"].ToString() && sc.Timming == item1.Timming)
                                    {
                                        sc.Th = "R";
                                        break;
                                    }
                                    else
                                    {
                                        sc.Th = "B";
                                    }
                                }
                                break;
                            }
                            else
                            {
                                sc.Th = "1";
                                //break;
                            }
                        }
                        else
                        {
                            sc.Th = "1";
                        }
                        //else if (schedual0.ToList().Count > 0)
                        //{
                        //    foreach (var item1 in schedual0)
                        //    {
                        //        if (item1.Th == sdr["thursday"].ToString() && sc.Timming == item1.Timming)
                        //        {
                        //            sc.Th = "R";
                        //            break;
                        //        }
                        //        else
                        //        {
                        //            sc.Th = "1";
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    sc.Th = "1";
                        //    //break;
                        //}
                    }
                    if (schedual1.ToList().Count == 0)
                    {
                        sc.Th = "1";
                    }
                }
                else
                {
                    sc.Th = "0";
                }
                if (sdr["friday"].ToString() == "1")
                {
                    foreach (var item in schedual1.ToList())
                    {
                        if (item.F == sdr["Friday"].ToString() && sc.Timming == item.Timming)
                        {
                            if (schedual0.Count == 0)
                            {
                                sc.F = "B";
                                break;
                            }
                            else if (schedual0.ToList().Count > 0)
                            {
                                foreach (var item1 in schedual0)
                                {
                                    if (item1.F == sdr["Friday"].ToString() && sc.Timming == item1.Timming)
                                    {
                                        sc.F = "R";
                                        break;
                                    }
                                    else
                                    {
                                        sc.F = "B";
                                    }
                                }
                                break;
                            }
                            else
                            {
                                sc.F = "1";
                                //break;
                            }
                        }
                        else
                        {
                            sc.F = "1";
                        }
                        //else if (schedual0.ToList().Count > 0)
                        //{
                        //    foreach (var item1 in schedual0)
                        //    {
                        //        if (item1.F == sdr["Friday"].ToString() && sc.Timming == item1.Timming)
                        //        {
                        //            sc.F = "R";
                        //            break;
                        //        }
                        //        else
                        //        {
                        //            sc.F = "1";
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    sc.F = "1";
                        //    //break;
                        //}
                    }
                    if (schedual1.ToList().Count == 0)
                    {
                        sc.F = "1";
                    }
                }
                else
                {
                    sc.F = "0";
                }
                if (sdr["saturday"].ToString() == "1")
                {
                    foreach (var item in schedual1.ToList())
                    {
                        if (item.S == sdr["saturday"].ToString() && sc.Timming == item.Timming)
                        {
                            if (schedual0.Count == 0)
                            {
                                sc.S = "B";
                                break;
                            }
                            else if (schedual0.ToList().Count > 0)
                            {
                                foreach (var item1 in schedual0)
                                {
                                    if (item1.S == sdr["saturday"].ToString() && sc.Timming == item1.Timming)
                                    {
                                        sc.S = "R";
                                        break;
                                    }
                                    else
                                    {
                                        sc.S = "B";
                                    }
                                }
                                break;
                            }
                            else
                            {
                                sc.S = "1";
                                //break;
                            }
                        }
                        else
                        {
                            sc.S = "1";
                        }
                        //else if (schedual0.ToList().Count > 0)
                        //{
                        //    foreach (var item1 in schedual0)
                        //    {
                        //        if (item1.S == sdr["saturday"].ToString() && sc.Timming == item1.Timming)
                        //        {
                        //            sc.S = "R";
                        //            break;
                        //        }
                        //        else
                        //        {
                        //            sc.S = "1";
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    sc.S = "1";
                        //    //break;
                        //}
                    }
                    if (schedual1.ToList().Count == 0)
                    {
                        sc.S = "1";
                    }
                }
                else
                {
                    sc.S = "0";
                }
                if (sdr["sunday"].ToString() == "1")
                {
                    foreach (var item in schedual1.ToList())
                    {
                        if (item.Su == sdr["sunday"].ToString() && sc.Timming == item.Timming)
                        {
                            if (schedual0.Count == 0)
                            {
                                sc.Su = "B";
                                break;
                            }
                            else if (schedual0.ToList().Count > 0)
                            {
                                foreach (var item1 in schedual0)
                                {
                                    if (item1.Su == sdr["sunday"].ToString() && sc.Timming == item1.Timming)
                                    {
                                        sc.Su = "R";
                                        break;
                                    }
                                    else
                                    {
                                        sc.Su = "B";
                                    }
                                }
                                break;
                            }
                            else
                            {
                                sc.Su = "1";
                                //break;
                            }
                        }
                        else
                        {
                            sc.Su = "1";
                        }
                        //else if (schedual0.ToList().Count > 0)
                        //{
                        //    foreach (var item1 in schedual0)
                        //    {
                        //        if (item1.Su == sdr["sunday"].ToString() && sc.Timming == item1.Timming)
                        //        {
                        //            sc.Su = "R";
                        //            break;
                        //        }
                        //        else
                        //        {
                        //            sc.Su = "1";
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    sc.Su = "1";
                        //    //break;
                        //}
                    }
                    if (schedual1.ToList().Count == 0)
                    {
                        sc.Su = "1";
                    }
                }
                else
                {
                    sc.Su = "0";
                }
                //sc.M = sdr["Monday"].ToString();
                //sc.T = sdr["Tuesday"].ToString();
                //sc.W = sdr["Wednesday"].ToString();
                //sc.Th = sdr["Thursday"].ToString();
                //sc.F = sdr["Friday"].ToString();
                //sc.S = sdr["Saturday"].ToString();
                //sc.Su = sdr["Sunday"].ToString();
                schedual.Add(sc);
            }
            sdr.Close();
            cmd.Connection.Close();
            return schedual;
        }


        public List<Schedual> GetTutorSche(string email)
        {
            string queryq = "select * from Reschedule where temail='" + email + "' and [RescheStatus]=2";
            List<Schedual> schedual0 = new List<Schedual>();
            SqlCommand cmdd = new SqlCommand(queryq, new SqlConnection(connectionString));
            cmdd.Connection.Open();
            SqlDataReader sdrr = cmdd.ExecuteReader();
            while (sdrr.Read())
            {
                Schedual s = new Schedual();
                s.Timming = sdrr["timmings"].ToString();
                if (sdrr["Day"].ToString() == "Monday")
                {
                    s.M = "1";
                }
                else if (sdrr["Day"].ToString() == "Tuesday")
                {
                    s.T = "1";
                }
                else if (sdrr["Day"].ToString() == "Wednesday")
                {
                    s.W = "1";
                }
                else if (sdrr["Day"].ToString() == "Thursday")
                {
                    s.Th = "1";
                }
                else if (sdrr["Day"].ToString() == "Friday")
                {
                    s.F = "1";
                }
                else if (sdrr["Day"].ToString() == "Saturday")
                {
                    s.S = "1";
                }
                else
                {
                    s.Su = "1";
                }
                schedual0.Add(s);
            }
            sdrr.Close();
            cmdd.Connection.Close();
            string q = "select * from RequestTutor where status =2 and temail='" + email + "'";
            List<Schedual> schedual1 = new List<Schedual>();
            SqlCommand cmd2 = new SqlCommand(q, new SqlConnection(connectionString));
            cmd2.Connection.Open();
            SqlDataReader sdr2 = cmd2.ExecuteReader();
            while (sdr2.Read())
            {
                Schedual sc = new Schedual();

                sc.Timming = sdr2["timming"].ToString();
                if (sdr2["Day"].ToString() == "Monday")
                {
                    sc.M = "1";
                }
                else if (sdr2["Day"].ToString() == "Tuesday")
                {
                    sc.T = "1";
                }
                else if (sdr2["Day"].ToString() == "Wednesday")
                {
                    sc.W = "1";
                }
                else if (sdr2["Day"].ToString() == "Thursday")
                {
                    sc.Th = "1";
                }
                else if (sdr2["Day"].ToString() == "Friday")
                {
                    sc.F = "1";
                }
                else if (sdr2["Day"].ToString() == "Saturday")
                {
                    sc.S = "1";
                }
                else
                {
                    sc.Su = "1";
                }
                schedual1.Add(sc);
            }
            cmd2.Connection.Close();
            sdr2.Close();
            List<Schedual> schedual = new List<Schedual>();
            SqlCommand cmd = new SqlCommand("select * from tutorschedual where email='" + email + "' and countRows < 23 order by countRows asc", new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Schedual sc = new Schedual();
                sc.AridNo = sdr["email"].ToString();
                sc.Timming = sdr["Timming"].ToString();
                //    if (sdr["monday"].ToString() == "1")
                //    {
                //        foreach (var item in schedual1.ToList())
                //        {

                //            if (item.M == sdr["Monday"].ToString() && sc.Timming == item.Timming)
                //            {

                //                if (schedual0.Count == 0)
                //                {
                //                    sc.M = "B";
                //                    break;
                //                }
                //                else
                //                {
                //                    foreach (var item1 in schedual0.ToList())
                //                    {
                //                        if (item1.M == sdr["Monday"].ToString() && sc.Timming == item1.Timming)
                //                        {
                //                            sc.M = "R";
                //                            break;
                //                        }
                //                        else
                //                        {
                //                            sc.M = "B";
                //                            //break;
                //                        }
                //                    }
                //                }

                //            }
                //            else if (schedual0.ToList().Count > 0)
                //            {
                //                foreach (var item1 in schedual0)
                //                {
                //                    if (item1.M == sdr["Monday"].ToString() && sc.Timming == item1.Timming)
                //                    {
                //                        sc.M = "R";
                //                        break;
                //                    }
                //                    else
                //                    {
                //                        sc.M = "1";
                //                        //break;
                //                    }
                //                }

                //            }
                //            else
                //            {
                //                sc.M = "1";
                //            }
                //        }
                //        if (schedual1.ToList().Count == 0)
                //        {
                //            sc.M = "1";
                //        }

                //    }
                //    else
                //    {
                //        sc.M = "0";
                //    }
                //    if (sdr["tuesday"].ToString() == "1")
                //    {
                //        foreach (var item in schedual1.ToList())
                //        {
                //            if (item.T == sdr["Tuesday"].ToString() && sc.Timming == item.Timming)
                //            {
                //                if (schedual0.Count == 0)
                //                {
                //                    sc.T = "B";
                //                    break;
                //                }
                //                else
                //                {
                //                    foreach (var item1 in schedual0)
                //                    {
                //                        if (item1.T == sdr["Tuesday"].ToString() && sc.Timming == item1.Timming)
                //                        {
                //                            sc.T = "R";
                //                            break;
                //                        }
                //                        else
                //                        {
                //                            sc.T = "B";
                //                        }
                //                    }
                //                }
                //            }
                //            else if (schedual0.ToList().Count > 0)
                //            {
                //                foreach (var item1 in schedual0)
                //                {
                //                    if (item1.T == sdr["Tuesday"].ToString() && sc.Timming == item1.Timming)
                //                    {
                //                        sc.T = "R";
                //                        break;
                //                    }
                //                    else
                //                    {
                //                        sc.T = "1";
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                sc.T = "1";
                //                //break;
                //            }
                //        }
                //        if (schedual1.ToList().Count == 0)
                //        {
                //            sc.T = "1";
                //        }
                //    }
                //    else
                //    {
                //        sc.T = "0";
                //    }
                //    if (sdr["wednesday"].ToString() == "1")
                //    {
                //        foreach (var item in schedual1.ToList())
                //        {
                //            if (item.W == sdr["wednesday"].ToString() && sc.Timming == item.Timming)
                //            {
                //                if (schedual0.Count == 0)
                //                {
                //                    sc.W = "B";
                //                    break;
                //                }
                //                else
                //                {
                //                    foreach (var item1 in schedual0)
                //                    {
                //                        if (item1.W == sdr["wednesday"].ToString() && sc.Timming == item1.Timming)
                //                        {
                //                            sc.W = "R";
                //                            break;
                //                        }
                //                        else
                //                        {
                //                            sc.W = "B";
                //                        }
                //                    }
                //                }
                //            }
                //            else if (schedual0.ToList().Count > 0)
                //            {
                //                foreach (var item1 in schedual0)
                //                {
                //                    if (item1.W == sdr["wednesday"].ToString() && sc.Timming == item1.Timming)
                //                    {
                //                        sc.W = "R";
                //                        break;
                //                    }
                //                    else
                //                    {
                //                        sc.W = "1";
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                sc.W = "1";
                //                //break;
                //            }
                //        }
                //        if (schedual1.ToList().Count == 0)
                //        {
                //            sc.W = "1";
                //        }
                //    }
                //    else
                //    {
                //        sc.W = "0";
                //    }
                //    if (sdr["thursday"].ToString() == "1")
                //    {
                //        foreach (var item in schedual1.ToList())
                //        {
                //            if (item.Th == sdr["thursday"].ToString() && sc.Timming == item.Timming)
                //            {
                //                if (schedual0.Count == 0)
                //                {
                //                    sc.Th = "B";
                //                    break;
                //                }
                //                else
                //                {
                //                    foreach (var item1 in schedual0)
                //                    {
                //                        if (item1.Th == sdr["thursday"].ToString() && sc.Timming == item1.Timming)
                //                        {
                //                            sc.Th = "R";
                //                            break;
                //                        }
                //                        else
                //                        {
                //                            sc.Th = "B";
                //                        }
                //                    }
                //                }
                //            }
                //            else if (schedual0.ToList().Count > 0)
                //            {
                //                foreach (var item1 in schedual0)
                //                {
                //                    if (item1.Th == sdr["thursday"].ToString() && sc.Timming == item1.Timming)
                //                    {
                //                        sc.Th = "R";
                //                        break;
                //                    }
                //                    else
                //                    {
                //                        sc.Th = "1";
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                sc.Th = "1";
                //                //break;
                //            }
                //        }
                //        if (schedual1.ToList().Count == 0)
                //        {
                //            sc.Th = "1";
                //        }
                //    }
                //    else
                //    {
                //        sc.Th = "0";
                //    }
                //    if (sdr["friday"].ToString() == "1")
                //    {
                //        foreach (var item in schedual1.ToList())
                //        {
                //            if (item.F == sdr["Friday"].ToString() && sc.Timming == item.Timming)
                //            {
                //                if (schedual0.Count == 0)
                //                {
                //                    sc.F = "B";
                //                    break;
                //                }
                //                else
                //                {
                //                    foreach (var item1 in schedual0)
                //                    {
                //                        if (item1.F == sdr["Friday"].ToString() && sc.Timming == item1.Timming)
                //                        {
                //                            sc.F = "R";
                //                            break;
                //                        }
                //                        else
                //                        {
                //                            sc.F = "B";
                //                        }
                //                    }
                //                }
                //            }
                //            else if (schedual0.ToList().Count > 0)
                //            {
                //                foreach (var item1 in schedual0)
                //                {
                //                    if (item1.F == sdr["Friday"].ToString() && sc.Timming == item1.Timming)
                //                    {
                //                        sc.F = "R";
                //                        break;
                //                    }
                //                    else
                //                    {
                //                        sc.F = "1";
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                sc.F = "1";
                //                //break;
                //            }
                //        }
                //        if (schedual1.ToList().Count == 0)
                //        {
                //            sc.F = "1";
                //        }
                //    }
                //    else
                //    {
                //        sc.F = "0";
                //    }
                //    if (sdr["saturday"].ToString() == "1")
                //    {
                //        foreach (var item in schedual1.ToList())
                //        {
                //            if (item.S == sdr["saturday"].ToString() && sc.Timming == item.Timming)
                //            {
                //                if (schedual0.Count == 0)
                //                {
                //                    sc.S = "B";
                //                    break;
                //                }
                //                else
                //                {
                //                    foreach (var item1 in schedual0)
                //                    {
                //                        if (item1.S == sdr["saturday"].ToString() && sc.Timming == item1.Timming)
                //                        {
                //                            sc.S = "R";
                //                            break;
                //                        }
                //                        else
                //                        {
                //                            sc.S = "B";
                //                        }
                //                    }
                //                }
                //            }
                //            else if (schedual0.ToList().Count > 0)
                //            {
                //                foreach (var item1 in schedual0)
                //                {
                //                    if (item1.S == sdr["saturday"].ToString() && sc.Timming == item1.Timming)
                //                    {
                //                        sc.S = "R";
                //                        break;
                //                    }
                //                    else
                //                    {
                //                        sc.S = "1";
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                sc.S = "1";
                //                //break;
                //            }
                //        }
                //        if (schedual1.ToList().Count == 0)
                //        {
                //            sc.S = "1";
                //        }
                //    }
                //    else
                //    {
                //        sc.S = "0";
                //    }
                //    if (sdr["sunday"].ToString() == "1")
                //    {
                //        foreach (var item in schedual1.ToList())
                //        {
                //            if (item.Su == sdr["sunday"].ToString() && sc.Timming == item.Timming)
                //            {
                //                if (schedual0.Count == 0)
                //                {
                //                    sc.Su = "B";
                //                    break;
                //                }
                //                else
                //                {
                //                    foreach (var item1 in schedual0)
                //                    {
                //                        if (item1.Su == sdr["sunday"].ToString() && sc.Timming == item1.Timming)
                //                        {
                //                            sc.Su = "R";
                //                            break;
                //                        }
                //                        else
                //                        {
                //                            sc.Su = "B";
                //                        }
                //                    }
                //                }
                //            }
                //            else if (schedual0.ToList().Count > 0)
                //            {
                //                foreach (var item1 in schedual0)
                //                {
                //                    if (item1.Su == sdr["sunday"].ToString() && sc.Timming == item1.Timming)
                //                    {
                //                        sc.Su = "R";
                //                        break;
                //                    }
                //                    else
                //                    {
                //                        sc.Su = "1";
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                sc.Su = "1";
                //                //break;
                //            }
                //        }
                //        if (schedual1.ToList().Count == 0)
                //        {
                //            sc.Su = "1";
                //        }
                //    }
                //    else
                //    {
                //        sc.Su = "0";
                //    }
                //    //sc.M = sdr["Monday"].ToString();
                //    //sc.T = sdr["Tuesday"].ToString();
                //    //sc.W = sdr["Wednesday"].ToString();
                //    //sc.Th = sdr["Thursday"].ToString();
                //    //sc.F = sdr["Friday"].ToString();
                //    //sc.S = sdr["Saturday"].ToString();
                //    //sc.Su = sdr["Sunday"].ToString();
                //    schedual.Add(sc);
                //}
                if (sdr["monday"].ToString() == "1")
                {
                    foreach (var item in schedual1.ToList())
                    {

                        if (item.M == sdr["Monday"].ToString() && sc.Timming == item.Timming)
                        {

                            if (schedual0.Count == 0)
                            {
                                sc.M = "B";
                                break;
                            }
                            else if (schedual0.ToList().Count > 0)
                            {
                                foreach (var item1 in schedual0.ToList())
                                {
                                    if (item1.M == sdr["Monday"].ToString() && sc.Timming == item1.Timming)
                                    {
                                        sc.M = "R";
                                        break;
                                    }
                                    else
                                    {
                                        sc.M = "B";
                                        //break;
                                    }
                                }
                                break;
                            }
                            else
                            {
                                sc.M = "1";
                            }

                        }
                        else
                        {
                            sc.M = "1";
                        }
                        //else if (schedual0.ToList().Count > 0)
                        //{
                        //    foreach (var item1 in schedual0)
                        //    {
                        //        if (item1.M == sdr["Monday"].ToString() && sc.Timming == item1.Timming)
                        //        {
                        //            sc.M = "R";
                        //            break;
                        //        }
                        //        else
                        //        {
                        //            sc.M = "1";
                        //            //break;
                        //        }
                        //    }

                        //}
                        //else
                        //{
                        //    sc.M = "1";
                        //}
                    }
                    if (schedual1.ToList().Count == 0)
                    {
                        sc.M = "1";
                    }

                }
                else
                {
                    sc.M = "0";
                }
                if (sdr["tuesday"].ToString() == "1")
                {
                    foreach (var item in schedual1.ToList())
                    {
                        if (item.T == sdr["Tuesday"].ToString() && sc.Timming == item.Timming)
                        {
                            if (schedual0.Count == 0)
                            {
                                sc.T = "B";
                                break;
                            }
                            else if (schedual0.ToList().Count > 0)
                            {
                                foreach (var item1 in schedual0)
                                {
                                    if (item1.T == sdr["Tuesday"].ToString() && sc.Timming == item1.Timming)
                                    {
                                        sc.T = "R";
                                        break;
                                    }
                                    else
                                    {
                                        sc.T = "B";
                                    }
                                }
                                break;
                            }
                            else
                            {
                                sc.T = "1";
                            }
                        }
                        else
                        {
                            sc.T = "1";
                        }
                        //else if (schedual0.ToList().Count > 0)
                        //{
                        //    foreach (var item1 in schedual0)
                        //    {
                        //        if (item1.T == sdr["Tuesday"].ToString() && sc.Timming == item1.Timming)
                        //        {
                        //            sc.T = "R";
                        //            break;
                        //        }
                        //        else
                        //        {
                        //            sc.T = "1";
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    sc.T = "1";
                        //    //break;
                        //}
                    }
                    if (schedual1.ToList().Count == 0)
                    {
                        sc.T = "1";
                    }
                }
                else
                {
                    sc.T = "0";
                }
                if (sdr["wednesday"].ToString() == "1")
                {
                    foreach (var item in schedual1.ToList())
                    {
                        if (item.W == sdr["wednesday"].ToString() && sc.Timming == item.Timming)
                        {
                            if (schedual0.Count == 0)
                            {
                                sc.W = "B";
                                break;
                            }
                            else if (schedual0.ToList().Count > 0)
                            {
                                foreach (var item1 in schedual0)
                                {
                                    if (item1.W == sdr["wednesday"].ToString() && sc.Timming == item1.Timming)
                                    {
                                        sc.W = "R";
                                        break;
                                    }
                                    else
                                    {
                                        sc.W = "B";
                                    }
                                }
                                break;
                            }
                            else
                            {
                                sc.W = "1";
                                //break;
                            }
                        }
                        else
                        {
                            sc.W = "1";
                        }
                        //else if (schedual0.ToList().Count > 0)
                        //{
                        //    foreach (var item1 in schedual0)
                        //    {
                        //        if (item1.W == sdr["wednesday"].ToString() && sc.Timming == item1.Timming)
                        //        {
                        //            sc.W = "R";
                        //            break;
                        //        }
                        //        else
                        //        {
                        //            sc.W = "1";
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    sc.W = "1";
                        //    //break;
                        //}
                    }
                    if (schedual1.ToList().Count == 0)
                    {
                        sc.W = "1";
                    }
                }
                else
                {
                    sc.W = "0";
                }
                if (sdr["thursday"].ToString() == "1")
                {
                    foreach (var item in schedual1.ToList())
                    {
                        if (item.Th == sdr["thursday"].ToString() && sc.Timming == item.Timming)
                        {
                            if (schedual0.Count == 0)
                            {
                                sc.Th = "B";
                                break;
                            }
                            else if (schedual0.ToList().Count > 0)
                            {
                                foreach (var item1 in schedual0)
                                {
                                    if (item1.Th == sdr["thursday"].ToString() && sc.Timming == item1.Timming)
                                    {
                                        sc.Th = "R";
                                        break;
                                    }
                                    else
                                    {
                                        sc.Th = "B";
                                    }
                                }
                                break;
                            }
                            else
                            {
                                sc.Th = "1";
                                //break;
                            }
                        }
                        else
                        {
                            sc.Th = "1";
                        }
                        //else if (schedual0.ToList().Count > 0)
                        //{
                        //    foreach (var item1 in schedual0)
                        //    {
                        //        if (item1.Th == sdr["thursday"].ToString() && sc.Timming == item1.Timming)
                        //        {
                        //            sc.Th = "R";
                        //            break;
                        //        }
                        //        else
                        //        {
                        //            sc.Th = "1";
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    sc.Th = "1";
                        //    //break;
                        //}
                    }
                    if (schedual1.ToList().Count == 0)
                    {
                        sc.Th = "1";
                    }
                }
                else
                {
                    sc.Th = "0";
                }
                if (sdr["friday"].ToString() == "1")
                {
                    foreach (var item in schedual1.ToList())
                    {
                        if (item.F == sdr["Friday"].ToString() && sc.Timming == item.Timming)
                        {
                            if (schedual0.Count == 0)
                            {
                                sc.F = "B";
                                break;
                            }
                            else if (schedual0.ToList().Count > 0)
                            {
                                foreach (var item1 in schedual0)
                                {
                                    if (item1.F == sdr["Friday"].ToString() && sc.Timming == item1.Timming)
                                    {
                                        sc.F = "R";
                                        break;
                                    }
                                    else
                                    {
                                        sc.F = "B";
                                    }
                                }
                                break;
                            }
                            else
                            {
                                sc.F = "1";
                                //break;
                            }
                        }
                        else
                        {
                            sc.F = "1";
                        }
                        //else if (schedual0.ToList().Count > 0)
                        //{
                        //    foreach (var item1 in schedual0)
                        //    {
                        //        if (item1.F == sdr["Friday"].ToString() && sc.Timming == item1.Timming)
                        //        {
                        //            sc.F = "R";
                        //            break;
                        //        }
                        //        else
                        //        {
                        //            sc.F = "1";
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    sc.F = "1";
                        //    //break;
                        //}
                    }
                    if (schedual1.ToList().Count == 0)
                    {
                        sc.F = "1";
                    }
                }
                else
                {
                    sc.F = "0";
                }
                if (sdr["saturday"].ToString() == "1")
                {
                    foreach (var item in schedual1.ToList())
                    {
                        if (item.S == sdr["saturday"].ToString() && sc.Timming == item.Timming)
                        {
                            if (schedual0.Count == 0)
                            {
                                sc.S = "B";
                                break;
                            }
                            else if (schedual0.ToList().Count > 0)
                            {
                                foreach (var item1 in schedual0)
                                {
                                    if (item1.S == sdr["saturday"].ToString() && sc.Timming == item1.Timming)
                                    {
                                        sc.S = "R";
                                        break;
                                    }
                                    else
                                    {
                                        sc.S = "B";
                                    }
                                }
                                break;
                            }
                            else
                            {
                                sc.S = "1";
                                //break;
                            }
                        }
                        else
                        {
                            sc.S = "1";
                        }
                        //else if (schedual0.ToList().Count > 0)
                        //{
                        //    foreach (var item1 in schedual0)
                        //    {
                        //        if (item1.S == sdr["saturday"].ToString() && sc.Timming == item1.Timming)
                        //        {
                        //            sc.S = "R";
                        //            break;
                        //        }
                        //        else
                        //        {
                        //            sc.S = "1";
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    sc.S = "1";
                        //    //break;
                        //}
                    }
                    if (schedual1.ToList().Count == 0)
                    {
                        sc.S = "1";
                    }
                }
                else
                {
                    sc.S = "0";
                }
                if (sdr["sunday"].ToString() == "1")
                {
                    foreach (var item in schedual1.ToList())
                    {
                        if (item.Su == sdr["sunday"].ToString() && sc.Timming == item.Timming)
                        {
                            if (schedual0.Count == 0)
                            {
                                sc.Su = "B";
                                break;
                            }
                            else if (schedual0.ToList().Count > 0)
                            {
                                foreach (var item1 in schedual0)
                                {
                                    if (item1.Su == sdr["sunday"].ToString() && sc.Timming == item1.Timming)
                                    {
                                        sc.Su = "R";
                                        break;
                                    }
                                    else
                                    {
                                        sc.Su = "B";
                                    }
                                }
                                break;
                            }
                            else
                            {
                                sc.Su = "1";
                                //break;
                            }
                        }
                        else
                        {
                            sc.Su = "1";
                        }
                        //else if (schedual0.ToList().Count > 0)
                        //{
                        //    foreach (var item1 in schedual0)
                        //    {
                        //        if (item1.Su == sdr["sunday"].ToString() && sc.Timming == item1.Timming)
                        //        {
                        //            sc.Su = "R";
                        //            break;
                        //        }
                        //        else
                        //        {
                        //            sc.Su = "1";
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    sc.Su = "1";
                        //    //break;
                        //}
                    }
                    if (schedual1.ToList().Count == 0)
                    {
                        sc.Su = "1";
                    }
                }
                else
                {
                    sc.Su = "0";
                }
                //sc.M = sdr["Monday"].ToString();
                //sc.T = sdr["Tuesday"].ToString();
                //sc.W = sdr["Wednesday"].ToString();
                //sc.Th = sdr["Thursday"].ToString();
                //sc.F = sdr["Friday"].ToString();
                //sc.S = sdr["Saturday"].ToString();
                //sc.Su = sdr["Sunday"].ToString();
                schedual.Add(sc);
            }
            sdr.Close();
            cmd.Connection.Close();
            return schedual;
        }
        //is mn phly subject nkhalo agr subject same ha tu request bhj 2 vrna nhi
        public CUD TutorReq(toTutorRequest t)
        {
            int x = 0;
            //string q = "select * from RequestTutor where Timming ='" + t.Timmings + "' and Temail='"+t.TuEmail+"' and Day='"+t.Day+"'";// only day has added if error Remove day only from This function..
            //string q = "select * from RequestTutor where Timming ='" + t.Timmings + "' and Day='" + t.Day + "' and subject='"+t.Subj+"'";//to get all student request which are want to study same subject at same time...
            //string q = "select * from RequestTutor where Timming ='" + t.Timmings + "' and Day='" + t.Day + "' and subject='" + t.Subj + "' and Semail!='"+t.SEmail+"'";
            int limit = 0;
            string limitReq = "select count(*) as totalrequest from RequestTutor where day='" + t.Day + "' and status=1 and semail='" + t.SEmail + "'";
            SqlCommand lcmd = new SqlCommand(limitReq, new SqlConnection(connectionString));
            lcmd.Connection.Open();
            SqlDataReader sdrlim = lcmd.ExecuteReader();
            if (sdrlim.HasRows)
            {
                sdrlim.Read();
                limit = int.Parse(sdrlim["totalrequest"].ToString());
            }
            sdrlim.Close();
            lcmd.Connection.Close();
            if (limit <= 2)
            {
                string q = "select * from RequestTutor where Timming ='" + t.Timmings + "' and Day='" + t.Day + "' and Semail='" + t.SEmail + "'";
                SqlCommand cmd1 = new SqlCommand(q, new SqlConnection(connectionString));
                cmd1.Connection.Open();
                SqlDataReader sdr = cmd1.ExecuteReader();
                if (sdr.HasRows)
                {
                    cmd1.Connection.Close();
                    sdr.Close();
                    return new CUD { rowEffected = x, Reason = "Your Request Already Submitted to another Tutor" };
                }
                else
                {
                    cmd1.Connection.Close();
                    sdr.Close();
                    var query1 = "select * from RequestTutor where Timming ='" + t.Timmings + "' and Day='" + t.Day + "' and Temail='" + t.TuEmail + "' and subject='" + t.Subj + "'";
                    SqlCommand cmd2 = new SqlCommand(query1, new SqlConnection(connectionString));
                    cmd2.Connection.Open();
                    SqlDataReader sdr1 = cmd2.ExecuteReader();
                    if (sdr1.HasRows)
                    {
                        cmd2.Connection.Close();
                        sdr1.Close();
                        string query = "insert into RequestTutor values('" + t.SEmail + "','" + t.TuEmail + "','" + t.Timmings + "','" + t.Subj + "','" + t.Day + "','" + 1 + "')";
                        SqlCommand cmd = new SqlCommand(query, new SqlConnection(connectionString));
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
                    else
                    {
                        cmd2.Connection.Close();
                        sdr1.Close();
                        var query = "select * from RequestTutor where Timming = '" + t.Timmings + "' and Day = '" + t.Day + "' and Temail = '" + t.TuEmail + "'";
                        SqlCommand cmd3 = new SqlCommand(query, new SqlConnection(connectionString));
                        cmd3.Connection.Open();
                        SqlDataReader sdr2 = cmd3.ExecuteReader();
                        if (sdr2.HasRows)
                        {
                            cmd3.Connection.Close();
                            sdr2.Close();
                            return new CUD { rowEffected = x, Reason = "Tutor is Busy" };
                        }
                        else
                        {
                            cmd3.Connection.Close();
                            sdr2.Close();
                            string query3 = "insert into RequestTutor values('" + t.SEmail + "','" + t.TuEmail + "','" + t.Timmings + "','" + t.Subj + "','" + t.Day + "','" + 1 + "')";
                            SqlCommand cmd = new SqlCommand(query3, new SqlConnection(connectionString));
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

                }
            }
            else
            {
                return new CUD { rowEffected = x, Reason = "Request limit Exceeds than 2 in single day" };
            }



        }

        public List<StudentRequest> RequestAcceptOrReject(string sub, string email)
        {
            List<StudentRequest> lstSr = new List<StudentRequest>();
            string q = "select * from RequestTutor where subject='" + sub + "' and temail='" + email + "'";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(connectionString));
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
            string query = "select * from RequestTutor where Semail='" + email + "' and Subject='" + sub + "'";
            string q = "delete  from StudentCourses where CourseCode='" + sub + "' and email='" + email + "'";
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                cmd.Connection.Close();
                sdr.Close();
                return new CUD { Reason = "You Can not Deleted This Subject" };
            }
            else
            {
                cmd.Connection.Close();
                sdr.Close();

                SqlCommand cmd1 = new SqlCommand(q, new SqlConnection(connectionString));
                cmd1.Connection.Open();
                x = cmd1.ExecuteNonQuery();
                cmd1.Connection.Close();
                if (x == 1)
                {
                    return new CUD { Reason = sub + " Succesfully Deleted" };
                }
                else
                {
                    return new CUD { Reason = sub + " Failed to Deleted" };
                }
            }

        }

        public CUD TutorDelCourse(string email, string sub)
        {
            //string q = "delete  from TutorCourses where CourseCode='" + sub + "' and email='" + email + "'";
            int x = 0;
            string query = "select * from RequestTutor where Temail='" + email + "' and Subject='" + sub + "'";
            string q = "delete  from TutorCourses where CourseCode='" + sub + "' and email='" + email + "'";
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                cmd.Connection.Close();
                sdr.Close();
                return new CUD { Reason = "You Can not Deleted This Subject" };
            }
            else
            {
                cmd.Connection.Close();
                sdr.Close();

                SqlCommand cmd1 = new SqlCommand(q, new SqlConnection(connectionString));
                cmd1.Connection.Open();
                x = cmd1.ExecuteNonQuery();
                cmd1.Connection.Close();
                if (x == 1)
                {
                    return new CUD { Reason = sub + " Succesfully Deleted" };
                }
                else
                {
                    return new CUD { Reason = sub + " Failed to Deleted" };
                }
            }
        }

        public List<toTutorRequest> TutorAcceptRequest(string email)
        {
            List<toTutorRequest> tutorReq = new List<toTutorRequest>();
            string query = "select (select Student.First_Name+' '+Student.Last_Name  from Student where Email=RequestTutor.Semail) as [fullname],Temail, (select Tutor.First_Name+' '+Tutor.Last_Name  from Tutor where Email=RequestTutor.TEmail) as [fullname1],SEmail,Temail,Timming,[Day],[Subject] from RequestTutor where status=1 and TEmail='" + email + "' or Semail='" + email + "' and Status=1";
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                while (sdr.Read())
                {
                    toTutorRequest t = new toTutorRequest();
                    if (email == sdr["Semail"].ToString())
                    {
                        t.SEmail = sdr["Semail"].ToString();
                        t.Subj = sdr["subject"].ToString();
                        t.Timmings = sdr["timming"].ToString();
                        t.Day = sdr["Day"].ToString();
                        t.Name = sdr["fullname1"].ToString();
                        t.TuEmail = sdr["temail"].ToString();
                    }
                    else
                    {
                        t.SEmail = sdr["semail"].ToString();
                        t.Subj = sdr["subject"].ToString();
                        t.Timmings = sdr["timming"].ToString();
                        t.Day = sdr["Day"].ToString();
                        t.Name = sdr["fullname"].ToString();
                        t.TuEmail = sdr["temail"].ToString();
                    }
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
            string q = "update RequestTutor set   status='" + 2 + "'  where Temail='" + t.TuEmail + "' and semail='" + t.SEmail + "' and Day='" + t.Day + "' and timming='" + t.Timmings + "' and subject='" + t.Subj + "'  ";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(connectionString));
            cmd.Connection.Open();
            x = cmd.ExecuteNonQuery();

            cmd.Connection.Close();
            if (x == 1)
            {
                string DeleteOtherSub = "delete  from RequestTutor where temail!='" + t.TuEmail + "' and [Status]=1 and subject='" + t.Subj + "' and semail='" + t.SEmail + "'";
                SqlCommand cmd1 = new SqlCommand(DeleteOtherSub, new SqlConnection(connectionString));
                cmd1.Connection.Open();
                cmd1.ExecuteNonQuery();
                cmd1.Connection.Close();
                return new CUD { Reason = t.SEmail + " Request Accept" };
            }
            else
            {
                return new CUD { Reason = t.SEmail + " Failed to Accept" };
            }
        }

        public List<HeldClassess> TodayStudentClasses(string Email, string day, string datetimetoday)
        {

            string quer = "select * from reschedule where endDate!='' and [Read]=0";
            SqlCommand cmdd = new SqlCommand(quer, new SqlConnection(connectionString));
            cmdd.Connection.Open();
            SqlDataReader sdrr = cmdd.ExecuteReader();
            while (sdrr.Read())
            {
                DateTime dbendDate = new DateTime(int.Parse(sdrr["endDate"].ToString().Split('-')[2].ToString()), int.Parse(sdrr["endDate"].ToString().Split('-')[1]), int.Parse(sdrr["endDate"].ToString().Split('-')[0]));
                DateTime nowDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                TimeSpan diffDays = dbendDate.Subtract(nowDate);
                if (diffDays.TotalDays <= 0)
                {
                    //sdrr.Close();
                    cmdd.Connection.Close();

                    string qu = "update requesttutor set timming='" + sdrr["preTimming"] + "',day='" + sdrr["Preday"].ToString() + "' where day='" + sdrr["day"].ToString() + "' and timming='" + sdrr["timmings"].ToString() + "' and temail='" + Email + "' and subject='" + sdrr["subject"].ToString() + "'";
                    SqlCommand cmds = new SqlCommand(qu, new SqlConnection(connectionString));
                    cmds.Connection.Open();
                    int x = cmds.ExecuteNonQuery();
                    cmds.Connection.Close();
                    if (x == 1)
                    {
                        string updateRes = "update reschedule set [Read]=1 where endDate='" + sdrr["endDate"].ToString() + "'";
                        SqlCommand cmd12 = new SqlCommand(updateRes, new SqlConnection(connectionString));
                        cmd12.Connection.Open();
                        cmd12.ExecuteNonQuery();
                        cmd12.Connection.Close();
                    }
                    cmdd.Connection.Open();
                }
                else
                {
                    Console.WriteLine("Greater");
                }

            }
            sdrr.Close();
            cmdd.Connection.Close();
            string q = "select (select Email from Student where Email=rt.SEmail) as StuEmail,(select btnStatus from HeldStuClass hs where hs.Semail=rt.SEmail and hs.Temail=rt.TEmail and hs.DayTimeMonth='" + datetimetoday + "' and hs.Timmings=rt.Timming and hs.[Day]=rt.[Day] and hs.[Subject]=rt.[Subject]) as bitStatus,(select First_Name+' '+Last_Name from Student where Email=rt.SEmail) as [fullname],[Day],[Subject],[Timming] from requesttutor rt where rt.TEmail='" + Email + "' and rt.[Day]='" + day + "' and rt.[Status]=2";
            List<HeldClassess> Hldcls = new List<HeldClassess>();
            List<HeldClassess> Hldclss = new List<HeldClassess>();
            // string query = "select (select Student.First_Name+' '+Student.Last_Name  from Student where Email=RequestTutor.SEmail) as [fullname],SEmail,Timming,[Day],[Subject] from RequestTutor where TEmail='" + email + "' and subject='" + sub + "' and Status=1";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(connectionString));
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
                cmd.Connection.Close();

                string rescueQuery = "select * from HeldStuClass where temail='" + Email + "' and day='" + day + "' and [DayTimeMonth]='" + datetimetoday + "'";
                SqlCommand cmd3 = new SqlCommand(rescueQuery, new SqlConnection(connectionString));
                cmd3.Connection.Open();
                SqlDataReader sr = cmd3.ExecuteReader();
                while (sr.Read())
                {
                    HeldClassess t = new HeldClassess();
                    t.SEmail = sr["SEmail"].ToString();
                    t.TuEmail = Email;
                    t.Subj = sr["subject"].ToString();
                    t.Timmings = sr["timmings"].ToString();
                    t.Day = sr["Day"].ToString();
                    t.DateTimeToday = sr["DayTimeMonth"].ToString();
                    Hldclss.Add(t);
                }
                sr.Close();
                cmd3.Connection.Close();
                var datetime = "";
                if (datetimetoday.Split('-')[0].Length != 2)
                {
                    datetime = "0" + datetimetoday.Split('-')[0] + "-";
                }
                else
                {
                    datetime = datetimetoday.Split('-')[0] + "-";
                }
                if (datetimetoday.Split('-')[1].Length != 2)
                {
                    datetime += "0" + datetimetoday.Split('-')[1] + "-" + datetimetoday.Split('-')[2];
                }
                else
                {
                    datetime += datetimetoday.Split('-')[1] + "-" + datetimetoday.Split('-')[2];
                }
                string resQuery = "select  (select First_Name+' '+Last_Name from Student where Email=SEmail) as fullname,semail,subject,timmings,day from Reschedule where  TEmail='" + Email + "' and day='" + day + "' and StartDate='" + datetime + "' and [Read]!=1";
                SqlCommand cmd2 = new SqlCommand(resQuery, new SqlConnection(connectionString));
                cmd2.Connection.Open();
                SqlDataReader sdr1 = cmd2.ExecuteReader();
                while (sdr1.Read())
                {
                    HeldClassess t = new HeldClassess();
                    t.SEmail = sdr1["SEmail"].ToString();
                    t.Subj = sdr1["subject"].ToString();
                    t.Timmings = sdr1["timmings"].ToString();
                    t.TuEmail = Email;
                    t.Day = sdr1["Day"].ToString();
                    t.Name = sdr1["fullname"].ToString();
                    t.DateTimeToday = datetimetoday;
                    t.status = "cancel";
                    Hldcls.Add(t);
                }
                sdr1.Close();
                cmd2.Connection.Close();
                foreach (var item in Hldcls.ToList())
                {
                    foreach (var item1 in Hldclss.ToList())
                    {
                        if (item.SEmail == item1.SEmail && item.TuEmail == item1.TuEmail && item.Timmings == item1.Timmings && item.DateTimeToday == item1.DateTimeToday && item.Day == item1.Day && item.Subj == item1.Subj)
                        {
                            item.status = "1";
                        }
                    }
                }
                return Hldcls;
            }
            else
            {
                HeldClassess t = new HeldClassess();
                t.Reason = "No Classes Today";
                sdr.Close();
                cmd.Connection.Close();
                return Hldcls;
            }

        }

        public CUD TutorHeldStudentClassses(List<Topics> t)
        {


            if (t[0].status == "cancel")
            {
                var startdate = DateTime.Now.ToShortDateString();
                startdate = startdate.Replace('/', '-');

                string q = "select PreTimming,PreDay,subject from Reschedule where temail='" + t[0].TuEmail + "' and timmings='" + t[0].timming + "' and subject='" + t[0].subject + "' and enddate='' and startDate='" + startdate + "' ";
                SqlCommand cmd = new SqlCommand(q, new SqlConnection(connectionString));
                cmd.Connection.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    sdr.Read();
                    var preday = sdr["PreDay"].ToString();
                    var subject = sdr["subject"].ToString();
                    var PreTimming = sdr["PreTimming"].ToString();
                    sdr.Close();
                    cmd.Connection.Close();
                    string query = "update [HeldStuClass] set [Timmings]='" + t[0].timming + "',day='" + t[0].day + "',[HeldStatus]='held',[DayTimeMonth]='" + t[0].datetimetoday + "' where temail='" + t[0].TuEmail + "' and semail='" + t[0].semail + "' and subject='" + t[0].subject + "' and [Timmings]='" + PreTimming + "' and day='" + preday + "'";
                    SqlCommand cmd1 = new SqlCommand(query, new SqlConnection(connectionString));
                    cmd1.Connection.Open();
                    int x = cmd1.ExecuteNonQuery();

                    cmd1.Connection.Close();
                    if (x == 1)
                    {
                        var datetime = "";
                        if (t[0].datetimetoday.Split('-')[0].Length != 2)
                        {
                            datetime = "0" + t[0].datetimetoday.Split('-')[0] + "-";
                        }
                        else
                        {
                            datetime = t[0].datetimetoday.Split('-')[0] + "-";
                        }
                        if (t[0].datetimetoday.Split('-')[1].Length != 2)
                        {
                            datetime += "0" + t[0].datetimetoday.Split('-')[1] + "-" + t[0].datetimetoday.Split('-')[2];
                        }
                        else
                        {
                            datetime += t[0].datetimetoday.Split('-')[1] + "-" + t[0].datetimetoday.Split('-')[2];
                        }
                        string updateRes = "update reschedule set [Read]=1 where StartDate='" + datetime + "' and enddate=''";
                        SqlCommand cmd12 = new SqlCommand(updateRes, new SqlConnection(connectionString));
                        cmd12.Connection.Open();
                        cmd12.ExecuteNonQuery();
                        cmd12.Connection.Close();
                        foreach (var item in t.ToList())
                        {
                            foreach (var item1 in item.subtopics.ToList())
                            {
                                string LessonInsert = "insert into CompletedLessons values('" + item.maintopics + "','" + item1.semail + "','" + item1.topicstatus + "','" + t[0].semail + "','" + t[0].TuEmail + "','" + t[0].subject + "','" + t[0].timming + "','" + t[0].datetimetoday + "','" + t[0].day + "') ";
                                SqlCommand c = new SqlCommand(LessonInsert, new SqlConnection(connectionString));
                                c.Connection.Open();
                                int x1 = c.ExecuteNonQuery();
                                c.Connection.Close();
                                if (x1 == 1)
                                {
                                    return new CUD { Reason = "Held Class", rowEffected = x };
                                }
                                else
                                {
                                    return new CUD { Reason = "Faild to Held Class", rowEffected = x };
                                }
                            }
                        }
                    }

                }
                else
                {

                }
                cmd.Connection.Close();
                return new CUD { Reason = "" };
            }
            else
            {
                int x = 0;
                string Held = "Held";
                string q = "insert into HeldStuClass values('" + t[0].semail + "','" + t[0].TuEmail + "','" + t[0].timming + "','" + t[0].day + "','" + t[0].subject + "','" + Held.ToString() + "','" + t[0].datetimetoday + "','" + 1 + "')";
                SqlCommand cmd = new SqlCommand(q, new SqlConnection(connectionString));
                cmd.Connection.Open();
                x = cmd.ExecuteNonQuery();

                cmd.Connection.Close();
                if (x == 1)
                {
                    foreach (var item in t.ToList())
                    {
                        foreach (var item1 in item.subtopics.ToList())
                        {
                            string LessonInsert = "insert into CompletedLessons values('" + item.maintopics + "','" + item1.semail + "','" + item1.topicstatus + "','" + t[0].semail + "','" + t[0].TuEmail + "','" + t[0].subject + "','" + t[0].timming + "','" + t[0].datetimetoday + "','" + t[0].day + "') ";
                            SqlCommand c = new SqlCommand(LessonInsert, new SqlConnection(connectionString));
                            c.Connection.Open();
                            int x2 = c.ExecuteNonQuery();
                            c.Connection.Close();
                            //if (x2 == 1)
                            //{
                            //    return new CUD { Reason = "Held Class", rowEffected = x };
                            //}else
                            //{
                            //    return new CUD { Reason = "Faild to Held Class", rowEffected = x };
                            //}
                        }
                    }
                    return new CUD { Reason = "Held Class", rowEffected = x };
                }
                else
                {
                    return new CUD { Reason = "Faild to Held Class", rowEffected = x };
                }
            }
            //return new CUD { Reason = "asda" };
        }
        public CUD TutorCancelStudentClassses(HeldClassess t)
        {
            //if (t.status=="cancel")
            //{
            //    return new CUD { Reason = "" };
            //}
            //else
            //{
            int x = 0;
            string Held = "Cancl";
            string q = "insert into HeldStuClass values('" + t.SEmail + "','" + t.TuEmail + "','" + t.Timmings + "','" + t.Day + "','" + t.Subj + "','" + Held.ToString() + "','" + t.DateTimeToday + "','" + 1 + "')";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(connectionString));
            cmd.Connection.Open();
            x = cmd.ExecuteNonQuery();

            cmd.Connection.Close();
            if (x == 1)
            {
                var datetime = "";
                if (t.DateTimeToday.Split('-')[0].Length != 2)
                {
                    datetime = "0" + t.DateTimeToday.Split('-')[0] + "-";
                }
                else
                {
                    datetime = t.DateTimeToday.Split('-')[0] + "-";
                }
                if (t.DateTimeToday.Split('-')[1].Length != 2)
                {
                    datetime += "0" + t.DateTimeToday.Split('-')[0] + "-" + t.DateTimeToday.Split('-')[2];
                }
                else
                {
                    datetime += t.DateTimeToday.Split('-')[0] + "-" + t.DateTimeToday.Split('-')[2];
                }
                if (t.status == "cancel")
                {
                    string updateRes = "update reschedule set [Read]=1 where StartDate='" + datetime + "' and endDate=''";
                    SqlCommand cmd2 = new SqlCommand(updateRes, new SqlConnection(connectionString));
                    cmd2.Connection.Open();
                    cmd2.ExecuteNonQuery();
                    cmd2.Connection.Close();
                }
                return new CUD { Reason = "Cancel Class", rowEffected = x };
            }
            else
            {
                return new CUD { Reason = "Faild to cancel Class", rowEffected = x };
            }
            //}
        }

        public CUD TutorRejectedRequest(toTutorRequest t)
        {
            int x = 0;
            string val = null;
            string q = "delete from requestTutor  where Temail='" + t.TuEmail + "' and semail='" + t.SEmail + "' and Day='" + t.Day + "' and timming='" + t.Timmings + "' and subject='" + t.Subj + "'  ";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(connectionString));
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

        public Student Login(string Email, string Pass)
        {
            var a = "Select * from Student where email = '" + Email + "' and Password = '" + Pass + "'";
            SqlCommand cmd = new SqlCommand(a, new SqlConnection(connectionString));
            System.Diagnostics.Debug.WriteLine(a);
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();


            if (sdr.HasRows)
            {
                sdr.Read();
                return new Student
                {
                    firstname = sdr["First_Name"].ToString(),
                    lastname = sdr["Last_Name"].ToString(),
                    email = sdr["Email"].ToString().Trim(),
                    gender = Convert.ToChar(sdr["gender"].ToString()),
                    password = sdr["password"].ToString(),
                    Type = sdr["Type"].ToString(),
                    imgsrc = sdr["imgsrc"].ToString(),

                    Error = "Data Exist In Database"
                };
            }
            else
            {
                sdr.Close();
                cmd.Connection.Close();
                var q = "Select * from Tutor where email = '" + Email + "' and Password = '" + Pass + "'";
                SqlCommand cmd1 = new SqlCommand(q, new SqlConnection(connectionString));
                System.Diagnostics.Debug.WriteLine(a);
                cmd1.Connection.Open();
                SqlDataReader sdr1 = cmd1.ExecuteReader();
                if (sdr1.HasRows)
                {
                    sdr1.Read();
                    return new Student
                    {
                        firstname = sdr1["First_Name"].ToString(),
                        lastname = sdr1["Last_Name"].ToString(),
                        email = sdr1["Email"].ToString(),
                        gender = Convert.ToChar(sdr1["gender"].ToString()),
                        password = sdr1["password"].ToString(),
                        Type = sdr1["Type"].ToString(),
                        imgsrc = sdr1["imgsrc"].ToString(),
                        Error = "Data Exist In Database"
                    };
                }
                else
                {
                    sdr1.Close();
                    cmd1.Connection.Close();
                    var query = "Select * from Parent where email = '" + Email + "' and Password = '" + Pass + "'";
                    SqlCommand cmd2 = new SqlCommand(query, new SqlConnection(connectionString));
                    System.Diagnostics.Debug.WriteLine(a);
                    cmd2.Connection.Open();
                    SqlDataReader sdr2 = cmd2.ExecuteReader();
                    if (sdr2.HasRows)
                    {
                        cmd.Connection.Close();
                        sdr2.Read();
                        return new Student
                        {
                            firstname = sdr2["First_Name"].ToString(),
                            lastname = sdr2["Last_Name"].ToString(),
                            email = sdr2["Email"].ToString(),
                            gender = Convert.ToChar(sdr2["gender"].ToString()),
                            password = sdr2["password"].ToString(),
                            Type = sdr2["Type"].ToString(),
                            imgsrc = sdr2["imgsrc"].ToString(),
                            CNIC = sdr2["CNIC"].ToString(),
                            Error = "Data Exist In Database"
                        };

                    }
                    else
                    {
                        sdr2.Close();
                        cmd2.Connection.Close();
                        return new Student { Error = "Email or Password Incorrect" };
                    }
                }
            }
        }

        public List<HeldClassess> HeldClass(string Email)
        {
            List<HeldClassess> obj = new List<HeldClassess>();
            var query = "select (select First_Name+' '+Last_Name  from Student where Email=SEmail) as FullName ,sEmail,Subject,Timmings,Day,DayTimeMonth from HeldStuClass where TEmail='" + Email + "'";
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                HeldClassess hld = new HeldClassess();
                hld.Name = sdr["FullName"].ToString();
                hld.SEmail = sdr["sEmail"].ToString();
                hld.Subj = sdr["Subject"].ToString();
                hld.Timmings = sdr["Timmings"].ToString();
                hld.Day = sdr["Day"].ToString();
                hld.DateTimeToday = sdr["DayTimeMonth"].ToString();
                obj.Add(hld);
            }
            sdr.Close();
            cmd.Connection.Close();
            return obj;
        }

        public List<toTutorRequest> StudentTimeTable(string Email)
        {
            List<toTutorRequest> obj = new List<toTutorRequest>();
            var Query = "select(select First_Name + ' ' + Last_Name from Tutor where Email = Temail) as FullName,(select First_Name + ' ' + Last_Name from Student where Email = semail) as FullName1,Timming,Subject,Day,Temail,semail from RequestTutor where status =2 and  Semail = '" + Email + "' or temail = '" + Email + "'  order by Day desc";
            SqlCommand cmd = new SqlCommand(Query, new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                toTutorRequest tr = new toTutorRequest();
                if (Email == sdr["Temail"].ToString())
                {
                    tr.Name = sdr["FullName1"].ToString();
                    tr.TuEmail = sdr["Temail"].ToString();
                    tr.SEmail = sdr["Temail"].ToString();
                    tr.Day = sdr["Day"].ToString();
                    tr.Timmings = sdr["Timming"].ToString();
                    tr.Subj = sdr["subject"].ToString();
                }
                else
                {
                    tr.Name = sdr["FullName"].ToString();
                    tr.TuEmail = sdr["Temail"].ToString();
                    tr.SEmail = sdr["SEmail"].ToString();
                    tr.Day = sdr["Day"].ToString();
                    tr.Timmings = sdr["Timming"].ToString();
                    tr.Subj = sdr["subject"].ToString();
                }
                obj.Add(tr);
            }
            sdr.Close();
            cmd.Connection.Close();
            return obj;
        }

        public CUD isStudentClassStudy(StudentRequest s)
        {
            var query = "select * from RequestTutor where Timming = '" + s.Timming + "' and Day = '" + s.Day + "' and Semail = '" + s.SEmail + "'";
            SqlCommand cmd3 = new SqlCommand(query, new SqlConnection(connectionString));
            cmd3.Connection.Open();
            SqlDataReader sdr2 = cmd3.ExecuteReader();
            if (sdr2.HasRows)
            {
                cmd3.Connection.Close();
                sdr2.Close();
                return new CUD { Reason = "Yes" };
            }
            else
            {
                cmd3.Connection.Close();
                sdr2.Close();
                return new CUD { Reason = "No" };
            }
        }
        public CUD isTutorClassStudy(StudentRequest s)
        {
            var query = "select * from RequestTutor where Timming = '" + s.Timming + "' and Day = '" + s.Day + "' and Temail = '" + s.SEmail + "'";
            SqlCommand cmd3 = new SqlCommand(query, new SqlConnection(connectionString));
            cmd3.Connection.Open();
            SqlDataReader sdr2 = cmd3.ExecuteReader();
            if (sdr2.HasRows)
            {
                cmd3.Connection.Close();
                sdr2.Close();
                return new CUD { Reason = "Yes" };
            }
            else
            {
                cmd3.Connection.Close();
                sdr2.Close();
                return new CUD { Reason = "No" };
            }
        }

        public List<FinanceData> TutorFinance(string email)
        {

            List<FinanceData> fd = new List<FinanceData>();
            SqlCommand cmd = new SqlCommand("select semail,HELDSTATUS,Temail,(select First_Name+' '+Last_Name from Student where Email=SEmail) as Name,(select First_Name+' '+Last_Name from Tutor where Email=TEmail) as Name1,Subject,COUNT(*)*(select fees from Admin where TutorEmail=HeldStuClass.TEmail and CourseTitle=HeldStuClass.Subject AND HeldStuClass.HELDSTATUS='HELD') as fee  from HeldStuClass   where  [HeldStatus] ='held' and  TEmail='" + email + "' or semail='" + email + "' group by Subject,SEmail,TEmail,HELDSTATUS", new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                FinanceData f = new FinanceData();
                if (sdr["HELDSTATUS"].ToString() != "Cancl")
                {
                    if (sdr["semail"].ToString() == email)
                    {
                        f.Semail = sdr["temail"].ToString();
                        f.Amount = int.Parse(sdr["Fee"].ToString());
                        f.CTile = sdr["Subject"].ToString();
                        f.Name = sdr["name1"].ToString();
                        if (fd.Count > 0)
                        {
                            foreach (var item in fd.ToList())
                            {
                                if (item.Semail == f.Semail && item.CTile != f.CTile)
                                {
                                    item.Amount += f.Amount;
                                    //item.CTile = "";

                                }
                                else
                                {

                                    if (fd.Contains(f))
                                    {

                                    }
                                    else
                                    {
                                        var data = fd.Where(d => d.Semail == f.Semail).ToList();
                                        if (data.Count == 0)
                                        {

                                            fd.Add(f);
                                        }
                                    }

                                }
                            }


                        }
                        else
                        {
                            fd.Add(f);
                        }
                    }
                    else
                    {
                        f.Semail = sdr["semail"].ToString();
                        f.Amount = int.Parse(sdr["Fee"].ToString());
                        f.CTile = sdr["Subject"].ToString();
                        f.Name = sdr["name"].ToString();
                        if (fd.Count > 0)
                        {
                            foreach (var item in fd.ToList())
                            {
                                if (item.Semail == f.Semail && item.CTile != f.CTile)
                                {
                                    item.Amount += f.Amount;
                                    item.CTile = "";
                                }
                                else
                                {
                                    if (fd.Contains(f))
                                    {

                                    }
                                    else
                                    {
                                        var data = fd.Where(d => d.Semail == f.Semail).ToList();
                                        if (data.Count == 0)
                                        {

                                            fd.Add(f);
                                        }
                                    }
                                }
                            }


                        }
                        else
                        {
                            fd.Add(f);
                        }
                    }

                }
            }
            cmd.Connection.Close();
            return fd;
        }

        public List<FinanceData> ViewDeatilsTutorFinance(string temail, string semail, string fname, string lname, string type)
        {
            if (type == "Student")
            {
                string name = fname + " " + lname;
                List<FinanceData> fd = new List<FinanceData>();
                SqlCommand cmd = new SqlCommand("select distinct Subject,(select First_Name+' '+Last_Name from Student where Email=SEmail) as NAme,(select First_Name+' '+Last_Name from Tutor where Email=TEmail) as NAme1 from RequestTutor where TEmail='" + temail + "' and Semail='" + semail + "'", new SqlConnection(connectionString));
                cmd.Connection.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    FinanceData s = new FinanceData();
                    s.CTile = sdr["Subject"].ToString();

                    if (name != sdr["name"].ToString())
                    {
                        s.Name = sdr["name"].ToString();
                        fd.Add(s);
                    }
                    else
                    {
                        s.Name = sdr["name1"].ToString();
                        fd.Add(s);
                    }

                }
                cmd.Connection.Close();

                return fd;
            }
            else
            {
                string name = fname + " " + lname;
                List<FinanceData> fd = new List<FinanceData>();
                SqlCommand cmd = new SqlCommand("select distinct Subject,(select First_Name+' '+Last_Name from Student where Email=SEmail) as NAme,(select First_Name+' '+Last_Name from Tutor where Email=TEmail) as NAme1 from RequestTutor where TEmail='" + semail + "' and Semail='" + temail + "'", new SqlConnection(connectionString));
                cmd.Connection.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    FinanceData s = new FinanceData();
                    s.CTile = sdr["Subject"].ToString();

                    if (name != sdr["name"].ToString())
                    {
                        s.Name = sdr["name"].ToString();
                        fd.Add(s);
                    }
                    else
                    {
                        s.Name = sdr["name1"].ToString();
                        fd.Add(s);
                    }

                }
                cmd.Connection.Close();

                return fd;
            }
        }

        public List<Student> GetChildrenRec(string cnic)
        {
            List<Student> student = new List<Student>();
            SqlCommand cmd = new SqlCommand("select * from student where cnic ='" + cnic + "'", new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Student s = new Student();
                s.email = sdr["email"].ToString();
                s.firstname = sdr["first_name"].ToString();
                s.lastname = sdr["last_name"].ToString();
                s.imgsrc = sdr["imgsrc"].ToString();
                s.phoneNo = sdr["phone_no"].ToString();
                s.gender = Convert.ToChar(sdr["gender"].ToString());
                s.Type = sdr["type"].ToString();
                student.Add(s);
            }
            cmd.Connection.Close();
            sdr.Close();
            return student;
        }

        public List<StudentRequest> CancelClasses(string email, string type)
        {
            if (type == "Student")
            {
                string q = "select (select first_name +' '+last_name from tutor where email=temail) as name, temail,timmings,day,subject from HeldStuClass where heldStatus='cancl' and semail='" + email + "'";
                List<StudentRequest> student = new List<StudentRequest>();
                SqlCommand cmd = new SqlCommand(q, new SqlConnection(connectionString));
                cmd.Connection.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    StudentRequest s = new StudentRequest();
                    s.SEmail = sdr["temail"].ToString();
                    s.Timming = sdr["timmings"].ToString();
                    s.Day = sdr["day"].ToString();
                    s.Subject = sdr["subject"].ToString();
                    s.Name = sdr["name"].ToString();
                    student.Add(s);
                }
                sdr.Close();
                cmd.Connection.Close();
                string query = "select * from Reschedule where endDate=''  and [Read]=0";
                SqlCommand cmd1 = new SqlCommand(query, new SqlConnection(connectionString));
                cmd1.Connection.Open();
                SqlDataReader sdr1 = cmd1.ExecuteReader();
                while (sdr1.Read())
                {
                    StudentRequest sr = new StudentRequest();
                    sr.TEmail = sdr1["temail"].ToString();
                    sr.Subject = sdr1["subject"].ToString();
                    sr.Timming = sdr1["Pretimming"].ToString();
                    foreach (var item in student.ToList())
                    {
                        if (sr.TEmail == item.SEmail && sr.Subject == item.Subject && sr.Timming == item.Timming)
                        {
                            student.Remove(item);
                            break;
                        }
                    }

                }
                sdr1.Close();
                cmd1.Connection.Close();


                return student;
            }
            else
            {
                string q = "select (select first_name +' '+last_name from student where email=semail) as name, semail,timmings,day,subject from HeldStuClass where heldStatus='cancl' and TEmail='" + email + "'";
                List<StudentRequest> student = new List<StudentRequest>();
                SqlCommand cmd = new SqlCommand(q, new SqlConnection(connectionString));
                cmd.Connection.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    StudentRequest s = new StudentRequest();
                    s.SEmail = sdr["semail"].ToString();
                    s.Timming = sdr["timmings"].ToString();
                    s.Day = sdr["day"].ToString();
                    s.Subject = sdr["subject"].ToString();
                    s.Name = sdr["name"].ToString();
                    student.Add(s);
                }
                sdr.Close();
                cmd.Connection.Close();
                string query = "select * from Reschedule where endDate='' and [Read]=0";
                SqlCommand cmd1 = new SqlCommand(query, new SqlConnection(connectionString));
                cmd1.Connection.Open();
                SqlDataReader sdr1 = cmd1.ExecuteReader();
                while (sdr1.Read())
                {
                    StudentRequest sr = new StudentRequest();
                    sr.TEmail = sdr1["semail"].ToString();
                    sr.Subject = sdr1["subject"].ToString();
                    sr.Timming = sdr1["Pretimming"].ToString();
                    foreach (var item in student.ToList())
                    {
                        if (sr.TEmail == item.SEmail && sr.Subject == item.Subject && sr.Timming == item.Timming)
                        {
                            student.Remove(item);
                            break;
                        }
                    }

                }
                sdr1.Close();
                cmd1.Connection.Close();
                return student;
            }
        }

        public List<StudentRequest> FreeSlotes(string temail, string semail, string type)
        {
            if (type == "Tutor")
            {
                string q = "select * from RequestTutor where Semail='" + semail + "' or Temail='" + temail + "' ";
                List<StudentRequest> oddstudent = new List<StudentRequest>();
                SqlCommand cmd0 = new SqlCommand(q, new SqlConnection(connectionString));
                cmd0.Connection.Open();
                SqlDataReader sdr0 = cmd0.ExecuteReader();
                while (sdr0.Read())
                {
                    StudentRequest s = new StudentRequest();
                    s.TEmail = sdr0["temail"].ToString();
                    s.SEmail = sdr0["sEmail"].ToString();
                    s.Day = sdr0["Day"].ToString();
                    s.Timming = sdr0["Timming"].ToString();
                    oddstudent.Add(s);
                }
                sdr0.Close();
                cmd0.Connection.Close();
                string q1 = "select distinct  ss.Monday,(select first_name +' '+last_name from student where Email=ss.Email) as Sname,(select first_name +' '+last_name from tutor where Email=ts.Email) as STname,ss.Email as Semail,ts.Monday,ts.Email as temail,ss.Timming as Timming,ts.Timming from StudentSchedual ss join TutorSchedual ts on ss.Monday=ts.Monday where ts.Monday=ss.Monday and ss.Monday=1 and ss.Timming=ts.Timming and ss.Email='" + semail + "'  and ts.Email='" + temail + "' and  ss.CountRows<23";
                string q2 = "select distinct  ss.tuesday,(select first_name +' '+last_name from student where Email=ss.Email) as Sname,(select first_name +' '+last_name from tutor where Email=ts.Email) as STname,ss.Email as Semail,ts.tuesday,ts.Email as temail,ss.Timming as Timming,ts.Timming from StudentSchedual ss join TutorSchedual ts on ss.Monday=ts.Monday where ts.tuesday=ss.tuesday and ss.tuesday=1 and ss.Timming=ts.Timming and ss.Email='" + semail + "'  and ts.Email='" + temail + "' and  ss.CountRows<23";
                string q3 = "select distinct  ss.wednesday,(select first_name +' '+last_name from student where Email=ss.Email) as Sname,(select first_name +' '+last_name from tutor where Email=ts.Email) as STname,ss.Email as Semail,ts.wednesday,ts.Email as temail,ss.Timming as Timming,ts.Timming from StudentSchedual ss join TutorSchedual ts on ss.Monday=ts.Monday where ts.wednesday=ss.wednesday and ss.wednesday=1 and ss.Timming=ts.Timming and ss.Email='" + semail + "'  and ts.Email='" + temail + "' and  ss.CountRows<23";
                string q4 = "select distinct  ss.thursday,(select first_name +' '+last_name from student where Email=ss.Email) as Sname,(select first_name +' '+last_name from tutor where Email=ts.Email) as STname,ss.Email as Semail,ts.thursday,ts.Email as temail ,ss.Timming as Timming,ts.Timming from StudentSchedual ss join TutorSchedual ts on ss.Monday=ts.Monday where ts.thursday=ss.thursday and ss.thursday=1 and ss.Timming=ts.Timming and ss.Email='" + semail + "'  and ts.Email='" + temail + "' and  ss.CountRows<23";
                string q5 = "select distinct  ss.friday,(select first_name +' '+last_name from student where Email=ss.Email) as Sname,(select first_name +' '+last_name from tutor where Email=ts.Email) as STname,ss.Email as Semail,ts.friday,ts.Email as temail ,ss.Timming as Timming,ts.Timming from StudentSchedual ss join TutorSchedual ts on ss.Monday=ts.Monday where ts.friday=ss.friday and ss.friday=1 and ss.Timming=ts.Timming and ss.Email='" + semail + "'  and ts.Email='" + temail + "' and  ss.CountRows<23";
                string q6 = "select distinct  ss.saturday,(select first_name +' '+last_name from student where Email=ss.Email) as Sname,(select first_name +' '+last_name from tutor where Email=ts.Email) as STname,ss.Email as Semail,ts.saturday,ts.Email as temail,ss.Timming as Timming,ts.Timming from StudentSchedual ss join TutorSchedual ts on ss.Monday=ts.Monday where ts.saturday=ss.saturday and ss.saturday=1 and ss.Timming=ts.Timming and ss.Email='" + semail + "'  and ts.Email='" + temail + "' and  ss.CountRows<23";
                string q7 = "select distinct  ss.sunday,(select first_name +' '+last_name from student where Email=ss.Email) as Sname,(select first_name +' '+last_name from tutor where Email=ts.Email) as STname,ss.Email as Semail,ts.sunday,ts.Email as temail,ss.Timming as Timming,ts.Timming from StudentSchedual ss join TutorSchedual ts on ss.Monday=ts.Monday where ts.sunday=ss.sunday and ss.sunday=1 and ss.Timming=ts.Timming and ss.Email='" + semail + "'  and ts.Email='" + temail + "' and  ss.CountRows<23";
                List<StudentRequest> student = new List<StudentRequest>();
                SqlCommand cmd = new SqlCommand(q1, new SqlConnection(connectionString));
                cmd.Connection.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    StudentRequest s = new StudentRequest();
                    s.Name = sdr["sname"].ToString();
                    s.SEmail = sdr["sEmail"].ToString();
                    s.Day = "Monday";
                    s.Timming = sdr["Timming"].ToString();
                    student.Add(s);
                }
                sdr.Close();
                cmd.Connection.Close();

                SqlCommand cmd1 = new SqlCommand(q2, new SqlConnection(connectionString));
                cmd1.Connection.Open();
                SqlDataReader sdr1 = cmd1.ExecuteReader();
                while (sdr1.Read())
                {
                    StudentRequest s = new StudentRequest();
                    s.Name = sdr1["sname"].ToString();
                    s.SEmail = sdr1["sEmail"].ToString();
                    s.Day = "Tuesday";
                    s.Timming = sdr1["Timming"].ToString();
                    student.Add(s);
                }
                sdr1.Close();
                cmd1.Connection.Close();
                SqlCommand cmd2 = new SqlCommand(q3, new SqlConnection(connectionString));
                cmd2.Connection.Open();
                SqlDataReader sdr2 = cmd2.ExecuteReader();
                while (sdr2.Read())
                {
                    StudentRequest s = new StudentRequest();
                    s.Name = sdr2["sname"].ToString();
                    s.SEmail = sdr2["sEmail"].ToString();
                    s.Day = "Wednesday";
                    s.Timming = sdr2["Timming"].ToString();
                    student.Add(s);
                }
                sdr2.Close();
                cmd2.Connection.Close();
                SqlCommand cmd3 = new SqlCommand(q4, new SqlConnection(connectionString));
                cmd3.Connection.Open();
                SqlDataReader sdr3 = cmd3.ExecuteReader();
                while (sdr3.Read())
                {
                    StudentRequest s = new StudentRequest();
                    s.Name = sdr3["sname"].ToString();
                    s.SEmail = sdr3["sEmail"].ToString();
                    s.Day = "Thursday";
                    s.Timming = sdr3["Timming"].ToString();
                    student.Add(s);
                }
                sdr3.Close();
                cmd3.Connection.Close();
                SqlCommand cmd4 = new SqlCommand(q5, new SqlConnection(connectionString));
                cmd4.Connection.Open();
                SqlDataReader sdr4 = cmd4.ExecuteReader();
                while (sdr4.Read())
                {
                    StudentRequest s = new StudentRequest();
                    s.Name = sdr4["sname"].ToString();
                    s.SEmail = sdr4["sEmail"].ToString();
                    s.Day = "Friday";
                    s.Timming = sdr4["Timming"].ToString();
                    student.Add(s);
                }
                sdr4.Close();
                cmd4.Connection.Close();
                SqlCommand cmd5 = new SqlCommand(q6, new SqlConnection(connectionString));
                cmd5.Connection.Open();
                SqlDataReader sdr5 = cmd5.ExecuteReader();
                while (sdr5.Read())
                {
                    StudentRequest s = new StudentRequest();
                    s.Name = sdr5["sname"].ToString();
                    s.SEmail = sdr5["sEmail"].ToString();
                    s.Day = "Saturday";
                    s.Timming = sdr5["Timming"].ToString();
                    student.Add(s);
                }
                sdr5.Close();
                cmd5.Connection.Close();
                SqlCommand cmd6 = new SqlCommand(q7, new SqlConnection(connectionString));
                cmd6.Connection.Open();
                SqlDataReader sdr6 = cmd6.ExecuteReader();
                while (sdr6.Read())
                {
                    StudentRequest s = new StudentRequest();
                    s.Name = sdr6["sname"].ToString();
                    s.SEmail = sdr6["sEmail"].ToString();
                    s.Day = "Sunday";
                    s.Timming = sdr6["Timming"].ToString();
                    student.Add(s);
                }
                sdr6.Close();
                cmd6.Connection.Close();
                foreach (var item in student.ToList())
                {
                    foreach (var item1 in oddstudent.ToList())
                    {
                        if (item.Timming == item1.Timming && item.Day == item1.Day)
                        {
                            student.Remove(item);
                        }
                    }
                }
                string query12 = "select * from Reschedule ";
                SqlCommand cmd11 = new SqlCommand(query12, new SqlConnection(connectionString));
                cmd11.Connection.Open();
                SqlDataReader sdr11 = cmd11.ExecuteReader();
                while (sdr11.Read())
                {
                    StudentRequest sr = new StudentRequest();
                    sr.TEmail = sdr11["temail"].ToString();
                    sr.Subject = sdr11["subject"].ToString();
                    sr.Timming = sdr11["timmings"].ToString();
                    sr.Day = sdr11["Day"].ToString();
                    foreach (var item in student.ToList())
                    {
                        if (sr.TEmail == item.SEmail && sr.Day == item.Day && sr.Timming == item.Timming)
                        {
                            student.Remove(item);
                            break;
                        }
                    }

                }
                sdr11.Close();
                cmd11.Connection.Close();
                return student;
            }
            else
            {
                string q = "select * from RequestTutor where Semail='" + temail + "' or Temail='" + semail + "' ";
                List<StudentRequest> oddstudent = new List<StudentRequest>();
                SqlCommand cmd0 = new SqlCommand(q, new SqlConnection(connectionString));
                cmd0.Connection.Open();
                SqlDataReader sdr0 = cmd0.ExecuteReader();
                while (sdr0.Read())
                {
                    StudentRequest s = new StudentRequest();
                    s.TEmail = sdr0["temail"].ToString();
                    s.SEmail = sdr0["sEmail"].ToString();
                    s.Day = sdr0["Day"].ToString();
                    s.Timming = sdr0["Timming"].ToString();
                    oddstudent.Add(s);
                }
                sdr0.Close();
                cmd0.Connection.Close();
                string q1 = "select distinct  ss.Monday,(select first_name +' '+last_name from student where Email=ss.Email) as Sname,(select first_name +' '+last_name from tutor where Email=ts.Email) as Tname,ss.Email as Semail,ts.Monday,ts.Email as temail,ss.Timming as Timming,ts.Timming from StudentSchedual ss join TutorSchedual ts on ss.Monday=ts.Monday where ts.Monday=ss.Monday and ss.Monday=1 and ss.Timming=ts.Timming and ss.Email='" + temail + "'  and ts.Email='" + semail + "' and  ss.CountRows<23";
                string q2 = "select distinct  ss.tuesday,(select first_name +' '+last_name from student where Email=ss.Email) as Sname,(select first_name +' '+last_name from tutor where Email=ts.Email) as Tname,ss.Email as Semail,ts.tuesday,ts.Email as temail,ss.Timming as Timming,ts.Timming from StudentSchedual ss join TutorSchedual ts on ss.Monday=ts.Monday where ts.tuesday=ss.tuesday and ss.tuesday=1 and ss.Timming=ts.Timming and ss.Email='" + temail + "'  and ts.Email='" + semail + "' and  ss.CountRows<23";
                string q3 = "select distinct  ss.wednesday,(select first_name +' '+last_name from student where Email=ss.Email) as Sname,(select first_name +' '+last_name from tutor where Email=ts.Email) as Tname,ss.Email as Semail,ts.wednesday,ts.Email as temail,ss.Timming as Timming,ts.Timming from StudentSchedual ss join TutorSchedual ts on ss.Monday=ts.Monday where ts.wednesday=ss.wednesday and ss.wednesday=1 and ss.Timming=ts.Timming and ss.Email='" + temail + "'  and ts.Email='" + semail + "' and  ss.CountRows<23";
                string q4 = "select distinct  ss.thursday,(select first_name +' '+last_name from student where Email=ss.Email) as Sname,(select first_name +' '+last_name from tutor where Email=ts.Email) as Tname,ss.Email as Semail,ts.thursday,ts.Email as temail ,ss.Timming as Timming,ts.Timming from StudentSchedual ss join TutorSchedual ts on ss.Monday=ts.Monday where ts.thursday=ss.thursday and ss.thursday=1 and ss.Timming=ts.Timming and ss.Email='" + temail + "'  and ts.Email='" + semail + "' and  ss.CountRows<23";
                string q5 = "select distinct  ss.friday,(select first_name +' '+last_name from student where Email=ss.Email) as Sname,(select first_name +' '+last_name from tutor where Email=ts.Email) as Tname,ss.Email as Semail,ts.friday,ts.Email as temail ,ss.Timming as Timming,ts.Timming from StudentSchedual ss join TutorSchedual ts on ss.Monday=ts.Monday where ts.friday=ss.friday and ss.friday=1 and ss.Timming=ts.Timming and ss.Email='" + temail + "'  and ts.Email='" + semail + "' and  ss.CountRows<23";
                string q6 = "select distinct  ss.saturday,(select first_name +' '+last_name from student where Email=ss.Email) as Sname,(select first_name +' '+last_name from tutor where Email=ts.Email) as Tname,ss.Email as Semail,ts.saturday,ts.Email as temail,ss.Timming as Timming,ts.Timming from StudentSchedual ss join TutorSchedual ts on ss.Monday=ts.Monday where ts.saturday=ss.saturday and ss.saturday=1 and ss.Timming=ts.Timming and ss.Email='" + temail + "'  and ts.Email='" + semail + "' and  ss.CountRows<23";
                string q7 = "select distinct  ss.sunday,(select first_name +' '+last_name from student where Email=ss.Email) as Sname,(select first_name +' '+last_name from tutor where Email=ts.Email) as Tname,ss.Email as Semail,ts.sunday,ts.Email as temail,ss.Timming as Timming,ts.Timming from StudentSchedual ss join TutorSchedual ts on ss.Monday=ts.Monday where ts.sunday=ss.sunday and ss.sunday=1 and ss.Timming=ts.Timming and ss.Email='" + temail + "'  and ts.Email='" + semail + "' and  ss.CountRows<23";
                List<StudentRequest> student = new List<StudentRequest>();
                SqlCommand cmd = new SqlCommand(q1, new SqlConnection(connectionString));
                cmd.Connection.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    StudentRequest s = new StudentRequest();
                    s.Name = sdr["Tname"].ToString();
                    s.SEmail = sdr["TEmail"].ToString();
                    s.Day = "Monday";
                    s.Timming = sdr["Timming"].ToString();
                    student.Add(s);
                }
                sdr.Close();
                cmd.Connection.Close();

                SqlCommand cmd1 = new SqlCommand(q2, new SqlConnection(connectionString));
                cmd1.Connection.Open();
                SqlDataReader sdr1 = cmd1.ExecuteReader();
                while (sdr1.Read())
                {
                    StudentRequest s = new StudentRequest();
                    s.Name = sdr1["Tname"].ToString();
                    s.SEmail = sdr1["TEmail"].ToString();
                    s.Day = "Tuesday";
                    s.Timming = sdr1["Timming"].ToString();
                    student.Add(s);
                }
                sdr1.Close();
                cmd1.Connection.Close();
                SqlCommand cmd2 = new SqlCommand(q3, new SqlConnection(connectionString));
                cmd2.Connection.Open();
                SqlDataReader sdr2 = cmd2.ExecuteReader();
                while (sdr2.Read())
                {
                    StudentRequest s = new StudentRequest();
                    s.Name = sdr2["Tname"].ToString();
                    s.SEmail = sdr2["TEmail"].ToString();
                    s.Day = "Wednesday";
                    s.Timming = sdr2["Timming"].ToString();
                    student.Add(s);
                }
                sdr2.Close();
                cmd2.Connection.Close();
                SqlCommand cmd3 = new SqlCommand(q4, new SqlConnection(connectionString));
                cmd3.Connection.Open();
                SqlDataReader sdr3 = cmd3.ExecuteReader();
                while (sdr3.Read())
                {
                    StudentRequest s = new StudentRequest();
                    s.Name = sdr3["Tname"].ToString();
                    s.SEmail = sdr3["TEmail"].ToString();
                    s.Day = "Thursday";
                    s.Timming = sdr3["Timming"].ToString();
                    student.Add(s);
                }
                sdr3.Close();
                cmd3.Connection.Close();
                SqlCommand cmd4 = new SqlCommand(q5, new SqlConnection(connectionString));
                cmd4.Connection.Open();
                SqlDataReader sdr4 = cmd4.ExecuteReader();
                while (sdr4.Read())
                {
                    StudentRequest s = new StudentRequest();
                    s.Name = sdr4["Tname"].ToString();
                    s.SEmail = sdr4["TEmail"].ToString();
                    s.Day = "Friday";
                    s.Timming = sdr4["Timming"].ToString();
                    student.Add(s);
                }
                sdr4.Close();
                cmd4.Connection.Close();
                SqlCommand cmd5 = new SqlCommand(q6, new SqlConnection(connectionString));
                cmd5.Connection.Open();
                SqlDataReader sdr5 = cmd5.ExecuteReader();
                while (sdr5.Read())
                {
                    StudentRequest s = new StudentRequest();
                    s.Name = sdr5["Tname"].ToString();
                    s.SEmail = sdr5["TEmail"].ToString();
                    s.Day = "Saturday";
                    s.Timming = sdr5["Timming"].ToString();
                    student.Add(s);
                }
                sdr5.Close();
                cmd5.Connection.Close();
                SqlCommand cmd6 = new SqlCommand(q7, new SqlConnection(connectionString));
                cmd6.Connection.Open();
                SqlDataReader sdr6 = cmd6.ExecuteReader();
                while (sdr6.Read())
                {
                    StudentRequest s = new StudentRequest();
                    s.Name = sdr6["Tname"].ToString();
                    s.SEmail = sdr6["TEmail"].ToString();
                    s.Day = "Sunday";
                    s.Timming = sdr6["Timming"].ToString();
                    student.Add(s);
                }
                sdr6.Close();
                cmd6.Connection.Close();
                foreach (var item in student.ToList())
                {
                    foreach (var item1 in oddstudent.ToList())
                    {
                        if (item.Timming == item1.Timming && item.Day == item1.Day)
                        {
                            student.Remove(item);
                        }
                    }
                }
                string query12 = "select * from Reschedule";
                SqlCommand cmd11 = new SqlCommand(query12, new SqlConnection(connectionString));
                cmd11.Connection.Open();
                SqlDataReader sdr11 = cmd11.ExecuteReader();
                while (sdr11.Read())
                {
                    StudentRequest sr = new StudentRequest();
                    sr.TEmail = sdr11["semail"].ToString();
                    sr.Subject = sdr11["subject"].ToString();
                    sr.Timming = sdr11["timmings"].ToString();
                    sr.Day = sdr11["Day"].ToString();
                    foreach (var item in student.ToList())
                    {
                        if (sr.TEmail == item.SEmail && sr.Day == item.Day && sr.Timming == item.Timming)
                        {
                            student.Remove(item);
                            break;
                        }
                    }

                }
                sdr11.Close();
                cmd11.Connection.Close();
                return student;
            }
        }

        public CUD RescheduledClass(Rescheduled r)
        {
            if (r.Type == "Student")
            {
                var email = r.SEmail;
                r.SEmail = r.TuEmail;
                r.TuEmail = email;
            }
            else
            {
                r.SEmail = r.SEmail;
                r.TuEmail = r.TuEmail;
            }
            //string q = "select * from Reschedule where Timmings='" + r.Timmings + "' and semail='" + r.SEmail + "' and temail='" + r.TuEmail + "' and day!='" + r.Day + "' and Startdate!='" + r.StartDate + "'";
            string q = "select * from Reschedule where Timmings='" + r.Timmings + "' and semail='" + r.SEmail + "' and temail='" + r.TuEmail + "' and day='" + r.Day + "' and Startdate='" + r.StartDate + "'";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                sdr.Close();
                cmd.Connection.Close();
                return new CUD { Reason = "Class Already Scheduled" };
            }
            else
            {
                sdr.Close();
                cmd.Connection.Close();
                if (r.EndDate == null)
                {
                    string checkQuery = "select * from Reschedule where semail='" + r.SEmail + "' and temail='" + r.TuEmail + "' and timmings='" + r.prTimming + "' and subject='" + r.Subj + "' and day='" + r.preDay + "' and startDate='" + DateTime.Now.ToShortDateString().Replace('/', '-') + "'";
                    SqlCommand cmd3 = new SqlCommand(checkQuery, new SqlConnection(connectionString));
                    cmd3.Connection.Open();
                    SqlDataReader sdr2 = cmd3.ExecuteReader();
                    if (sdr2.HasRows)
                    {
                        sdr2.Read();
                        cmd3.Connection.Close();
                        string updateQuery = "update Reschedule set timmings='" + r.Timmings + "', day='" + r.Day + "',subject='" + r.Subj + "',startDate='" + r.StartDate + "' where semail='" + r.SEmail + "' and temail='" + r.TuEmail + "' and timmings='" + r.prTimming + "' and day='" + r.preDay + "' and startDate='" + DateTime.Now.ToShortDateString().Replace('/', '-') + "'and  endDate='' and [Read]=0";
                        SqlCommand cmd2 = new SqlCommand(updateQuery, new SqlConnection(connectionString));
                        cmd2.Connection.Open();
                        int x = cmd2.ExecuteNonQuery();
                        cmd2.Connection.Close();
                        if (x == 1)
                        {
                            return new CUD { Reason = "Class  Scheduled" };
                        }
                        else
                        {
                            return new CUD { Reason = "Class Failed to Scheduled" };
                        }
                    }
                    else
                    {
                        sdr2.Close();
                        cmd3.Connection.Close();
                        string status = "2";
                        string query = "insert into Reschedule values('" + r.SEmail + "','" + r.TuEmail + "','" + r.Timmings + "','" + r.Day + "','" + r.Subj + "','" + status + "','" + r.preDay + "','" + r.prTimming + "','" + r.StartDate + "','" + r.EndDate + "','" + 0 + "')";
                        SqlCommand cmd1 = new SqlCommand(query, new SqlConnection(connectionString));
                        cmd1.Connection.Open();
                        int x = cmd1.ExecuteNonQuery();
                        cmd1.Connection.Close();
                        return new CUD { Reason = "Class  Scheduled" };
                    }

                }
                else

                {
                    string status = "2";
                    string query = "insert into Reschedule values('" + r.SEmail + "','" + r.TuEmail + "','" + r.Timmings + "','" + r.Day + "','" + r.Subj + "','" + status + "','" + r.preDay + "','" + r.prTimming + "','" + r.StartDate + "','" + r.EndDate + "','" + 0 + "')";
                    SqlCommand cmd1 = new SqlCommand(query, new SqlConnection(connectionString));
                    cmd1.Connection.Open();
                    int x = cmd1.ExecuteNonQuery();
                    cmd1.Connection.Close();

                    var q1 = "update requesttutor set timming ='" + r.Timmings + "',day='" + r.Day + "' where semail='" + r.SEmail + "' and temail='" + r.TuEmail + "' and subject='" + r.Subj + "'";
                    SqlCommand cmd2 = new SqlCommand(q1, new SqlConnection(connectionString));
                    cmd2.Connection.Open();
                    cmd2.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return new CUD { Reason = "Class Rescheduled Done", rowEffected = x };
                }
            }
        }

        public List<Rescheduled> Notifications(string Email)
        {
            List<Rescheduled> lstRes = new List<Rescheduled>();
            string q = "select (select first_name+' '+last_name from tutor where email=temail) as name,timmings,temail,day,subject,[read],preday,pretimming,startdate,enddate from Reschedule where semail='" + Email + "' order by [Read] asc";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Rescheduled r = new Rescheduled();
                r.Day = sdr["day"].ToString();
                r.TuEmail = sdr["temail"].ToString();
                r.Subj = sdr["subject"].ToString();
                r.TuEmail = sdr["temail"].ToString();
                r.preDay = sdr["preday"].ToString();
                r.Timmings = sdr["timmings"].ToString();
                r.prTimming = sdr["preTimming"].ToString();
                r.StartDate = sdr["startDate"].ToString();
                if (sdr["endDate"].ToString() == "")
                {
                    r.EndDate = sdr["startDate"].ToString();
                }
                else
                {
                    r.EndDate = sdr["endDate"].ToString();
                }
                r.Reason = sdr["Read"].ToString();
                r.Name = sdr["name"].ToString();
                r.status = "Reschedule";
                lstRes.Add(r);
            }
            sdr.Close();
            cmd.Connection.Close();
            string querydiscar = "select (select first_name+' '+last_name from tutor where email=temail) as name,SEmail,TEmail,Rating,DiscardStatus,subject,DateTimeDay from [discardStu] where semail='" + Email + "'";
            SqlCommand cmd2 = new SqlCommand(querydiscar, new SqlConnection(connectionString));
            cmd2.Connection.Open();
            SqlDataReader sdr2 = cmd2.ExecuteReader();
            while (sdr2.Read())
            {
                Rescheduled r = new Rescheduled();
                r.Name = sdr2["name"].ToString();
                r.TuEmail = sdr2["TEmail"].ToString();
                r.status = "Discard";
                r.Subj = sdr2["subject"].ToString();
                r.DateTimeToday = sdr2["DateTimeDay"].ToString();

                if (sdr2["DiscardStatus"].ToString() == "non")
                {
                    r.Reason = "False";
                }
                else
                {
                    r.Reason = "True";
                }
                lstRes.Add(r);
            }
            sdr2.Close();
            cmd2.Connection.Close();
            return lstRes;
        }
        public List<toTutorRequest> TutororStudents(string Email)
        {
            List<toTutorRequest> tutorReq = new List<toTutorRequest>();
            string query = "select distinct subject ,(select Student.First_Name+' '+Student.Last_Name  from Student where Email=RequestTutor.Semail) as [fullname],Temail, (select Tutor.First_Name+' '+Tutor.Last_Name  from Tutor where Email=RequestTutor.TEmail) as [fullname1],SEmail,Temail from RequestTutor where status=2 and TEmail='" + Email + "' or Semail='" + Email + "'";
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                while (sdr.Read())
                {
                    toTutorRequest t = new toTutorRequest();
                    if (Email == sdr["Semail"].ToString())
                    {
                        t.SEmail = sdr["Semail"].ToString();
                        t.Subj = sdr["subject"].ToString();
                        //t.Timmings = sdr["timming"].ToString();
                        //t.Day = sdr["Day"].ToString();
                        t.Name = sdr["fullname1"].ToString();
                        t.TuEmail = sdr["temail"].ToString();
                    }
                    else
                    {
                        t.SEmail = sdr["semail"].ToString();
                        t.Subj = sdr["subject"].ToString();
                        //t.Timmings = sdr["timming"].ToString();
                        //t.Day = sdr["Day"].ToString();
                        t.Name = sdr["fullname"].ToString();
                        t.TuEmail = sdr["temail"].ToString();
                    }
                    tutorReq.Add(t);
                }

                sdr.Close();
                cmd.Connection.Close();
                //string q = "select * from discardStu where temail ='" + Email + "' or Semail='" + Email + "'";
                //SqlCommand cmd1 = new SqlCommand(q, new SqlConnection(connectionString));
                //cmd1.Connection.Open();
                //SqlDataReader sdr1 = cmd1.ExecuteReader();
                //while (sdr1.Read())
                //{
                //    foreach (var item in tutorReq.ToList())
                //    {
                //        if (item.SEmail == sdr1["semail"].ToString() && item.TuEmail == sdr1["temail"].ToString() && item.Subj == sdr1["Subject"].ToString())
                //        {
                //            tutorReq.Remove(item);
                //        }
                //    }
                //}
                //sdr.Close();
                //cmd1.Connection.Close();
                return tutorReq;
            }
            else
            {
                return new List<toTutorRequest> { new toTutorRequest { Reason = "You Have No Student" } };
            }
        }
        public CUD SubmitRating(string TEmail, string SEmail, string subject)
        {
            float rating = 0f;
            string disstatus = "non";
            var dateTime = DateTime.Now.ToShortDateString().Replace('/', '-');
            string q = "insert into discardStu values('" + SEmail + "','" + TEmail + "','" + rating + "','" + disstatus + "','" + subject + "','" + dateTime + "')";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(connectionString));
            cmd.Connection.Open();
            int x = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            if (x == 1)
            {
                string deleteStudentRequest = "delete from RequestTutor where semail='" + SEmail + "' and temail='" + TEmail + "' and subject='" + subject + "'";
                SqlCommand c = new SqlCommand(deleteStudentRequest, new SqlConnection(connectionString));
                c.Connection.Open();
                int x2 = c.ExecuteNonQuery();
                c.Connection.Close();
                if (x2 == 1)
                {
                    string updateResc = "update reschedule set [Read]=1 where semail='" + SEmail + "' and temail='" + TEmail + "' and subject='" + subject + "'";
                    SqlCommand c1 = new SqlCommand(updateResc, new SqlConnection(connectionString));
                    c1.Connection.Open();
                    int x3 = c1.ExecuteNonQuery();
                    c1.Connection.Close();


                    if (x3 == 1)
                    {
                        return new CUD { Reason = "Discarded Student" };
                    }
                    else
                    {
                        return new CUD { Reason = "Faild to Discard Student" };
                    }
                }
                else
                {
                    return new CUD { Reason = "Faild to Discard Student" };
                }
                //delete student subject from 
            }
            else
            {
                return new CUD { Reason = "Faild to Discard Student" };
            }

        }
        public CUD GetRating(Rescheduled ttr)
        {
            string update = "update discardStu set rating='" + float.Parse(ttr.status) + "' , DiscardStatus='Approved' where subject ='" + ttr.Subj + "' and temail='" + ttr.TuEmail + "' and semail='" + ttr.SEmail + "' and DateTimeDay='" + ttr.DateTimeToday + "'";
            SqlCommand cmd = new SqlCommand(update, new SqlConnection(connectionString));
            cmd.Connection.Open();
            int x = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            if (x == 1)
            {
                return new CUD { Reason = "Rating Submitted" };
            }
            else
            {
                return new CUD { Reason = "Rating Failed" };
            }
        }
        public List<Topics> Lessons(string email, string sub)
        {
            int counter = 0;
            List<Topics> tp = new List<Topics>();
            string q = "select * from CourseTopics where couseTitle='" + sub + "' order by maintopics asc";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                List<Topics> thp = new List<Topics>();
                Topics t = new Topics();
                t.subject = sdr["cousetitle"].ToString();
                t.maintopics = sdr["maintopics"].ToString();
                t.id = int.Parse(sdr["courseid"].ToString());

                var sss = sdr["subtopics"].ToString();


                foreach (var item in sdr["subtopics"].ToString().Split(','))
                {
                    Topics tt = new Topics();
                    tt.semail = item;
                    thp.Add(tt);
                }
                t.subtopics = thp;
                tp.Add(t);
            }
            sdr.Close();
            cmd.Connection.Close();

            List<Topics> stp = new List<Topics>();
            string heldClassTopics = "select * from CompletedLessons where semail='" + email + "' and subj='" + sub + "'";



            //string heldClassTopics = "select * from extra";
            SqlCommand cmd1 = new SqlCommand(heldClassTopics, new SqlConnection(connectionString));

            cmd1.Connection.Open();
            SqlDataReader sdr1 = cmd1.ExecuteReader();
            if (sdr1.HasRows)
            {
                while (sdr1.Read())
                {
                    Topics s = new Topics();
                    s.maintopics = sdr1["LMtopic"].ToString();
                    var sss = sdr1["LSubTopic"].ToString();
                    foreach (var item in tp.ToList())
                    {

                        var x = item.subtopics.Find(d => d.semail == sdr1["LSubTopic"].ToString());
                        if (s.maintopics == item.maintopics && sdr1["TopicStatus"].ToString() == "1" && x.semail == sdr1["LSubTopic"].ToString())
                        {
                            item.subtopics.Remove(x);
                            counter = item.id;
                            if (item.subtopics.Count == 0)
                            {

                                tp.Remove(item);
                            }
                        }
                    }
                }
                counter++;
            }
            else
            {
                counter = 1;
            }

            //string heldClassTopics = "select * from extra";
            //SqlCommand cmd1 = new SqlCommand(heldClassTopics, new SqlConnection(connectionString));

            //cmd1.Connection.Open();
            //SqlDataReader sdr1 = cmd1.ExecuteReader();
            //if (sdr1.HasRows)
            //{
            //    while (sdr1.Read())
            //    {
            //        Topics s = new Topics();
            //        s.maintopics = sdr1["mtp"].ToString();
            //        var sss = sdr1["stp"].ToString();
            //        foreach (var item in tp.ToList())
            //        {

            //            var x = item.subtopics.Find(d => d.semail == sdr1["stp"].ToString());
            //            if (s.maintopics == item.maintopics && sdr1["stps"].ToString() == "1" && x.semail == sdr1["stp"].ToString())
            //            {
            //                item.subtopics.Remove(x);
            //                counter = item.id;
            //                if (item.subtopics.Count == 0)
            //                {

            //                    tp.Remove(item);
            //                }
            //            }
            //        }
            //    }
            //    counter++;
            //}
            //else
            //{
            //    counter = 1;
            //}
            sdr1.Close();
            cmd1.Connection.Close();

            if (counter == 1)
            {
                return tp.Take(1).ToList();
            }
            else
            {
                return tp.Where(d => d.id <= counter).ToList();
            }
        }
        public List<Student> Students()
        {
            List<Student> st = new List<Student>();
            string q = "select * from student";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Student s = new Student();
                s.firstname = sdr["first_name"].ToString();
                s.lastname = sdr["last_name"].ToString();
                s.imgsrc = sdr["imgsrc"].ToString();
                s.Type = sdr["type"].ToString();
                s.gender = Convert.ToChar(sdr["gender"].ToString());
                s.phoneNo = sdr["phone_no"].ToString();
                s.email = sdr["email"].ToString();
                s.CNIC = sdr["cnic"].ToString();
                st.Add(s);
            }
            sdr.Close();
            cmd.Connection.Close();
            return st;
        }

        public List<Tutor> Tutors()
        {
            List<Tutor> st = new List<Tutor>();
            string q = "select * from Tutor";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Tutor s = new Tutor();
                s.firstname = sdr["first_name"].ToString();
                s.lastname = sdr["last_name"].ToString();
                s.imgsrc = sdr["imgsrc"].ToString();
                s.Type = sdr["type"].ToString();
                s.gender = Convert.ToChar(sdr["gender"].ToString());
                s.phoneNo = sdr["phone_no"].ToString();
                s.email = sdr["email"].ToString();
                s.CNIC = sdr["cnic"].ToString();
                st.Add(s);
            }
            sdr.Close();
            cmd.Connection.Close();
            return st;
        }
    }
}
