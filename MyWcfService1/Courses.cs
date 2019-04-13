namespace MyWcfService1
{
    public class Courses
    {
        public string CourseCode { get; set; }
        public string Title { get; set; }

       
        public override string ToString()
        {
            return Title;
        }

    }
    
}