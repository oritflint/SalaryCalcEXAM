using System;

namespace SalaryCalcEXAM
{
    public class CalcSalary
    {
        public double BaseSalary { get; set; } = 0;
        public double AddSeniorityRate { get; set; } = 0;
        public double SalSeniorityRate { get; set; } = 0;
        public double AddWorkByLow { get; set; } = 0;
        public double TotalBaseBeforIncrease { get; set; } = 0;
        public double SalaryIncreaseRate { get; set; } = 0;
        public double AddSalaryIncrease { get; set; } = 0;
        public double NewBaseSalary { get; set; } = 0;
    }
}
