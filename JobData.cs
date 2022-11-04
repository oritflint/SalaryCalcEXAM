using System;


namespace SalaryCalcEXAM
{
    //:נתוני הקלט מהמשתמש

    public class JobData
    {
        public int PartTime { get; set; } = 0;
        public int ProfessionalLevel { get; set; } = 0;
        public int ManagerialLevel { get; set; } = 0;
        public double TotalSeniority { get; set; } = 0;
        public bool IsAllowedAddWork { get; set; } = false;
        public int AllowedAddWorkGroup { get; set; } = 0;
    }
}
