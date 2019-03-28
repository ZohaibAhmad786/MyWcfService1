namespace MyWcfService1
{
    public class Schedual
    {
        public string AridNo { get; set; }
        public string Timming { get; set; }
        public string M { get; set; }
        public string T { get; set; }
        public string W { get; set; }
        public string Th{ get; set; }
        public string F { get; set; }
        public string S { get; set; }
        public string Su { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}