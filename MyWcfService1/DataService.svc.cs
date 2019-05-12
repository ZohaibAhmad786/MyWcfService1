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
            //string q = "select  distinct Title,Course.CourseCode from course LEft join studentcourses on studentcourses.coursecode = course.title where StudentCourses.CourseCode is null or StudentCourses.Email!='" + email + "'";
            //SqlCommand cmd = new SqlCommand(q, new SqlConnection(connectionString));
            //cmd.Connection.Open();
            //SqlDataReader sdr = cmd.ExecuteReader();
            //while (sdr.Read())
            //{
            //    Courses c = new Courses();
            //    c.CourseCode = sdr["CourseCode"].ToString();
            //    c.Title = sdr["Title"].ToString();
            //    course.Add(c);
            //}
            //sdr.Close();
            //cmd.Connection.Close();
            //return course;
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
            //List<Courses> course = new List<Courses>();
            //string q = "select  distinct Title,Course.CourseCode from course LEft join TutorCourses on TutorCourses.coursecode = course.title where TutorCourses.CourseCode is null or TutorCourses.Email!='" + email + "'";
            //SqlCommand cmd = new SqlCommand(q, new SqlConnection(connectionString));
            //cmd.Connection.Open();
            //SqlDataReader sdr = cmd.ExecuteReader();
            //while (sdr.Read())
            //{
            //    Courses c = new Courses();
            //    c.CourseCode = sdr["CourseCode"].ToString();
            //    c.Title = sdr["Title"].ToString();
            //    course.Add(c);
            //}
            //sdr.Close();
            //cmd.Connection.Close();
            //return course;
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
                return new CUD { rowEffected = x, Reason = "Email Already data exist" };
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
                    return new CUD { rowEffected = x, Reason = " Email Already data exist" };
                }
                else
                {
                    cmd1.Connection.Close();
                    sdr1.Close();
                    SqlCommand cmd2 = new SqlCommand(query, new SqlConnection(connectionString));

                    cmd2.Connection.Open();
                    x = cmd2.ExecuteNonQuery();
                    cmd2.Connection.Close();
                    return new CUD { rowEffected = x, Reason = "Successfully Registered" };
                }
            }

            System.Diagnostics.Debug.WriteLine(x, query);




        }

        public /*int*/ CUD InsertTutortData(Tutor t)
        {
            //var td = JsonConvert.DeserializeObject<Tutor>(data);
            //
            int x = 0;
            var b = "Insert into Tutor values('" + t.email + "','" + t.firstname + "','" + t.lastname + "','" + t.phoneNo + "','" + t.address + "','" + t.password + "','" + Convert.ToChar(t.gender) + "','" + t.imgsrc + "','" + t.Type + "')";
            SqlCommand cmd = new SqlCommand("select * from Tutor Where email= '" + t.email + "'", new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                cmd.Connection.Close();
                sdr.Close();
                return new CUD { rowEffected = x, Reason = "Email Already data exist" };
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
                    return new CUD { rowEffected = x, Reason = "Email Already data exist" };
                }
                else
                {
                    cmd2.Connection.Close();
                    sdr1.Close();
                    SqlCommand cmd1 = new SqlCommand(b, new SqlConnection(connectionString));
                    cmd1.Connection.Open();
                    x = cmd1.ExecuteNonQuery();
                    cmd1.Connection.Close();
                    return new CUD { rowEffected = x, Reason = "Successfully Registered" };
                }
            }

            System.Diagnostics.Debug.WriteLine(x, b);
            //sdr.Close();
            //cmd.Connection.Close();


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
            return days;
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
            return days;
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
            return days;
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
            return days;
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
            return days;
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
            return days;
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
            return days;
        }

        public List<Schedual> GetStudentSche(string email)
        {
            List<Schedual> schedual = new List<Schedual>();
            SqlCommand cmd = new SqlCommand("select * from studentschedual where email='" + email + "' and  countRows < 23 order by countRows asc", new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Schedual sc = new Schedual();
                sc.AridNo = sdr["email"].ToString();
                sc.Timming = sdr["Timming"].ToString();
                if (sdr["monday"].ToString() == "True")
                {
                    sc.M = "1";
                }
                else
                {
                    sc.M = "0";
                }
                if (sdr["tuesday"].ToString() == "True")
                {
                    sc.T = "1";
                }
                else
                {
                    sc.T = "0";
                }
                if (sdr["wednesday"].ToString() == "True")
                {
                    sc.W = "1";
                }
                else
                {
                    sc.W = "0";
                }
                if (sdr["thursday"].ToString() == "True")
                {
                    sc.Th = "1";
                }
                else
                {
                    sc.Th = "0";
                }
                if (sdr["friday"].ToString() == "True")
                {
                    sc.F = "1";
                }
                else
                {
                    sc.F = "0";
                }
                if (sdr["saturday"].ToString() == "True")
                {
                    sc.S = "1";
                }
                else
                {
                    sc.S = "0";
                }
                if (sdr["sunday"].ToString() == "True")
                {
                    sc.Su = "1";
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
            cmd.Connection.Close();
            return schedual;
        }


        public List<Schedual> GetTutorSche(string email)
        {
            List<Schedual> schedual = new List<Schedual>();
            SqlCommand cmd = new SqlCommand("select * from tutorschedual where email='" + email + "' and countRows < 23 order by countRows asc", new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Schedual sc = new Schedual();
                sc.AridNo = sdr["email"].ToString();
                sc.Timming = sdr["Timming"].ToString();
                if (sdr["monday"].ToString() == "True")
                {
                    sc.M = "1";
                }
                else
                {
                    sc.M = "0";
                }
                if (sdr["tuesday"].ToString() == "True")
                {
                    sc.T = "1";
                }
                else
                {
                    sc.T = "0";
                }
                if (sdr["wednesday"].ToString() == "True")
                {
                    sc.W = "1";
                }
                else
                {
                    sc.W = "0";
                }
                if (sdr["thursday"].ToString() == "True")
                {
                    sc.Th = "1";
                }
                else
                {
                    sc.Th = "0";
                }
                if (sdr["friday"].ToString() == "True")
                {
                    sc.F = "1";
                }
                else
                {
                    sc.F = "0";
                }
                if (sdr["saturday"].ToString() == "True")
                {
                    sc.S = "1";
                }
                else
                {
                    sc.S = "0";
                }
                if (sdr["sunday"].ToString() == "True")
                {
                    sc.Su = "1";
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
            string query = "select (select Student.First_Name+' '+Student.Last_Name  from Student where Email=RequestTutor.Semail) as [fullname],Temail, (select Tutor.First_Name+' '+Tutor.Last_Name  from Tutor where Email=RequestTutor.TEmail) as [fullname1],SEmail,Timming,[Day],[Subject] from RequestTutor where TEmail='" + email + "' or Semail='" + email + "' and Status=1";
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
                        t.SEmail = sdr["Semail"].ToString();
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
                return new CUD { Reason = t.SEmail + " Request Accept" };
            }
            else
            {
                return new CUD { Reason = t.SEmail + " Failed to Accept" };
            }
        }

        public List<HeldClassess> TodayStudentClasses(string Email, string day, string datetimetoday)
        {
            string q = "select (select Email from Student where Email=rt.SEmail) as StuEmail,(select btnStatus from HeldStuClass hs where hs.Semail=rt.SEmail and hs.Temail=rt.TEmail and hs.DayTimeMonth='" + datetimetoday + "' and hs.Timmings=rt.Timming and hs.[Day]=rt.[Day] and hs.[Subject]=rt.[Subject]) as bitStatus,(select First_Name+' '+Last_Name from Student where Email=rt.SEmail) as [fullname],[Day],[Subject],[Timming] from requesttutor rt where rt.TEmail='" + Email + "' and rt.[Day]='" + day + "' and rt.[Status]=2";
            List<HeldClassess> Hldcls = new List<HeldClassess>();
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
            string q = "insert into HeldStuClass values('" + t.SEmail + "','" + t.TuEmail + "','" + t.Timmings + "','" + t.Day + "','" + t.Subj + "','" + Held.ToString() + "','" + t.DateTimeToday + "','" + 1 + "')";
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(connectionString));
            cmd.Connection.Open();
            x = cmd.ExecuteNonQuery();

            cmd.Connection.Close();
            if (x == 1)
            {
                return new CUD { Reason = "Held Class", rowEffected = x };
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
            SqlCommand cmd = new SqlCommand(q, new SqlConnection(connectionString));
            cmd.Connection.Open();
            x = cmd.ExecuteNonQuery();

            cmd.Connection.Close();
            if (x == 1)
            {
                return new CUD { Reason = "Cancel Class", rowEffected = x };
            }
            else
            {
                return new CUD { Reason = "Faild to cancel Class", rowEffected = x };
            }
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
                    return new Student { Error = "Email or Password Incorrect" };
                }
                sdr1.Close();
                cmd1.Connection.Close();
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
            var Query = "select(select First_Name + ' ' + Last_Name from Tutor where Email = Temail) as FullName,(select First_Name + ' ' + Last_Name from Student where Email = semail) as FullName1,Timming,Subject,Day,Temail,semail from RequestTutor where Semail = '" + Email + "' or temail = '" + Email + "' order by Day desc";
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
            SqlCommand cmd = new SqlCommand("select semail,Temail,(select First_Name+' '+Last_Name from Student where Email=SEmail) as Name,(select First_Name+' '+Last_Name from Tutor where Email=TEmail) as Name1,Subject,COUNT(*)*(select fees from Admin where TutorEmail=HeldStuClass.TEmail and CourseTitle=HeldStuClass.Subject) as fee  from HeldStuClass   where  TEmail='" + email + "' or semail='" + email + "' group by Subject,SEmail,TEmail", new SqlConnection(connectionString));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                FinanceData f = new FinanceData();

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
            cmd.Connection.Close();


            return fd;
        }

        public List<FinanceData> ViewDeatilsTutorFinance(string temail, string semail,string fname,string lname,string type)
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
            }else
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
    }
}
