using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace MyWcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDataService" in both code and config file together.
    [ServiceContract]
    public interface IDataService
    {
        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/GetAllDummyData"
            )]
        List<StudentData> AllStudents();


        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/InsertData"
            )]
        int InsertNewRecord(string data);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/InserStudent"///{email}/{firstname}/{lastname}/{phoneNo}/{discipline}/{semester}/{password}/{address}/{gender}"
            )]
        /*int*/CUD InsertStudentData(Student s);//string email, string firstname, string lastname, string phoneNo, string discipline, string semester, string password, string address, string gender

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/InserTutor"///{email}/{firstname}/{lastname}/{phoneNo}/{address}/{password}/{gender}"
            )]
        CUD InsertTutortData(Tutor t);


        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/LoginStudent/{email}/{pass}"
            )]
        Student StudentLogin(string email,string pass);


        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/LoginTutor/{id}/{pass}"
            )]
        Tutor TutorLogin(string id, string pass);


        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/StudentSche"
            )]
        CUD StudentSchedual(List<Schedual> Sc);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/TutorSche"
            )]
        CUD TutorSchedual(List<Schedual> Sc);

        [OperationContract]
        [WebInvoke(
           Method = "GET",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "/SGellAllCourses/{email}"
           )]
        List<Courses> SCourses(string email);

        [OperationContract]
        [WebInvoke(
           Method = "GET",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "/TGellAllCourses/{email}"
           )]
        List<Courses> TCourses(string email);


        [OperationContract]
        [WebInvoke(
           Method = "GET",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "/AddStudentCourses/{Course}/{email}"
           )]
        CUD StudentCourses(string Course,string email);

        [OperationContract]
        [WebInvoke(
           Method = "GET",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "/AddTutorCourses/{Course}/{email}/{fees}"
           )]
        CUD TutorCourses(string Course,string email,string fees);


        [OperationContract]
        [WebInvoke(
           Method = "GET",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "/StudentEnrollSub/{email}"
           )]
        List<Courses> StudentEnrollCourses(string email);
        [OperationContract]
        [WebInvoke(
           Method = "GET",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "/TutorEnrollSub/{email}"
           )]
        List<Courses> TutorEnrollCourses(string email);



        //StudentData DoWork();


        //-------------------------------------------

        [OperationContract]
        [WebInvoke(
          Method = "GET",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          UriTemplate = "/Monday/{email}/{sub}"
          )]
        List<Days> Monday(string email,string sub);

        [OperationContract]
        [WebInvoke(
         Method = "GET",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         UriTemplate = "/Tuesday/{email}/{sub}"
         )]
        List<Days> Tuesday(string email, string sub);

        [OperationContract]
        [WebInvoke(
         Method = "GET",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         UriTemplate = "/Wednesday/{email}/{sub}"
         )]
        List<Days> Wednesday(string email, string sub);

        [OperationContract]
        [WebInvoke(
         Method = "GET",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         UriTemplate = "/Thursday/{email}/{sub}"
         )]
        List<Days> Thursday(string email, string sub);

        [OperationContract]
        [WebInvoke(
         Method = "GET",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         UriTemplate = "/Friday/{email}/{sub}"
         )]
        List<Days> Friday(string email, string sub);

        [OperationContract]
        [WebInvoke(
         Method = "GET",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         UriTemplate = "/Saturday/{email}/{sub}"
         )]
        List<Days> Saturday(string email, string sub);

        [OperationContract]
        [WebInvoke(
         Method = "GET",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         UriTemplate = "/Sunday/{email}/{sub}"
         )]
        List<Days> Sunday(string email, string sub);


        [OperationContract]
        [WebInvoke(
        Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/GetAllStudentSchedual/{email}"
        )]
        List<Schedual> GetStudentSche(string email);


        //GetAllTutorSchedual

        [OperationContract]
        [WebInvoke(
        Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/GetAllTutorSchedual/{email}"
        )]
        List<Schedual> GetTutorSche(string email);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/TutorRequest"
        )]
        CUD TutorReq(toTutorRequest t);

        [OperationContract]
        [WebInvoke(
        Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/RequestManaging/{sub}/{email}"
        )]
        List<StudentRequest> RequestAcceptOrReject(string sub,string email);


        [OperationContract]
        [WebInvoke(
        Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/DeleteStudentCourse/{email}/{sub}"
        )]
        CUD StdDelCourse(string email,string sub);


        [OperationContract]
        [WebInvoke(
        Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/DeleteTutorCourse/{email}/{sub}"
        )]
        CUD TutorDelCourse(string email, string sub);



        [OperationContract]
        [WebInvoke(
        Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/AcceptRequest/{email}"
        )]
        List<toTutorRequest> TutorAcceptRequest(string email);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/AcceptedRequest"
        )]
        CUD TutorAcceptedRequest(toTutorRequest t);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/RejectedRequest"
        )]
        CUD TutorRejectedRequest(toTutorRequest t);

        [OperationContract]
        [WebInvoke(
        Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/TodayClasses/{Email}/{day}/{datetimetoday}"
        )]
        List<HeldClassess> TodayStudentClasses(string Email,string day,string datetimetoday);


        [OperationContract]
        [WebInvoke(
        Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/HeldStudentClass"
        )]
        CUD TutorHeldStudentClassses(HeldClassess t);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/CancelStudentClass"
        )]
        CUD TutorCancelStudentClassses(HeldClassess t);


        [OperationContract]
        [WebInvoke(
        Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/Login/{Email}/{Pass}"
        )]
        Student Login(string Email,string Pass);

        [OperationContract]
        [WebInvoke(
        Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/Held/{Email}"
        )]
        List<HeldClassess> HeldClass(string Email);


        [OperationContract]
        [WebInvoke(
        Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/SubjectTimeTable/{Email}"
        )]
        List<toTutorRequest> StudentTimeTable(string Email);


        [OperationContract]
        [WebInvoke(
        Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/IsSClassExist"
        )]
        CUD isStudentClassStudy(StudentRequest s);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/IsTClassExist"
        )]
        CUD isTutorClassStudy(StudentRequest s);


        [OperationContract]
        [WebInvoke(
        Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/TutorFinance/{email}"
        )]
        List<FinanceData> TutorFinance(string email);

        [OperationContract]
        [WebInvoke(
        Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/ViewDetailsTutorFinance/{temail}/{semail}/{fname}/{lname}/{type}"
        )]
        List<FinanceData> ViewDeatilsTutorFinance(string temail,string semail, string fname, string lname,string type);

        //HeldStudentClass


    }
}

//select (select Email from Student where Email=rt.SEmail) as StuEmail,(select First_Name+' '+Last_Name from Student where Email=rt.SEmail) as [fullname],[Day],[Subject],[Timming] from requesttutor rt where rt.TEmail='awaisali@gmail.com' and rt.[Day]='thursday'