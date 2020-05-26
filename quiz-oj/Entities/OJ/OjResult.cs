namespace quiz_oj.Entities.OJ
{
    public class OjResult
    {
        //Pending is true, when the last submission is pending;
        public bool Pending { get; set; }
        //
        public double SuccessRate { get; set; }
        
        public bool Failed { get; set; }
        public bool Pass { get; set; }
        //If pass == false, then Info stores Why it fails;
        public string Info { get; set; }
    }
}