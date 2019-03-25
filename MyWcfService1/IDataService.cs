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
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/InserStudent/{email}/{firstname}/{lastname}/{phoneNo}/{discipline}/{semester}/{password}/{address}/{gender}"
            )]
        /*int*/CUD InsertStudentData(string email, string firstname, string lastname, string phoneNo, string discipline, string semester, string password, string address, string gender);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/InserTutor/{email}/{firstname}/{lastname}/{phoneNo}/{address}/{password}/{gender}"
            )]
        CUD InsertTutortData(string email, string firstname, string lastname, string phoneNo, string address, string password, string gender);


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
        CUD StudentSchedual(string dataSource1);

        [OperationContract]
        [WebInvoke(
           Method = "GET",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "/GellAllCourses/{email}"
           )]
        List<Courses> Courses(string email);


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
           UriTemplate = "/AddTutorCourses/{Course}/{email}"
           )]
        CUD TutorCourses(string Course,string email);


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

    }
}
