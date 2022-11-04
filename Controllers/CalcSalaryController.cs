using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryCalcEXAM.Controllers
{


    [ApiController]
    [Route("[controller]")]
    public class CalcSalaryController : ControllerBase
    {
        //private readonly CalculatorService _calcatorServic;
        private readonly ILogger<CalcSalaryController> _logger;

        public CalcSalaryController(ILogger<CalcSalaryController> logger
            //, CalculatorService calcService
            )
        {
            _logger = logger;
            //_calcatorServic = calcService;
        }

        [HttpGet]
        public IEnumerable<CalcSalary> Get(int partTime, 
                                            int professionalLevel, 
                                            int managerialLevel,
                                            int totalSeniority,
                                            bool isAllowedAddWork,
                                            int allowedAddWorkGroup)
        {
            JobData jobData = new JobData();
            CalcSalary calc = new CalcSalary();

            jobData.PartTime= partTime;
            jobData.ProfessionalLevel= professionalLevel;
            jobData.ManagerialLevel = managerialLevel;
            jobData.TotalSeniority = totalSeniority;
            jobData.IsAllowedAddWork = isAllowedAddWork;
            jobData.AllowedAddWorkGroup = allowedAddWorkGroup;

            calc = GetCalcSalary(jobData);

            return Enumerable.Range(1, 1).Select(index => calc).ToArray();
        }

        public CalcSalary GetCalcSalary(JobData jobData)             //נתוני משרת עובד
        {
            CalcSalary calcSalary = new CalcSalary();
            int iTotalHrs = 170 * jobData.PartTime / 100;          //סה"כ שעות בחודש

            int iSalaryForHour = 100;                               //(0/1) סה"כ שכר לשעה לפי דרגה מקצועית
            if (jobData.ProfessionalLevel == 1) iSalaryForHour = 120;

            iSalaryForHour += (jobData.ManagerialLevel * 20);    //(0-4)תוספת לרמה ניהולית

            //שכר יסוד לפי חלקיות משרה
            calcSalary.BaseSalary = iTotalHrs * iSalaryForHour;     //חישוב סופי:

            //שיעור תוספת ותק
            calcSalary.AddSeniorityRate = jobData.TotalSeniority * 1.25;
            //תוספת ותק לשכר
            calcSalary.SalSeniorityRate = calcSalary.BaseSalary * calcSalary.AddSeniorityRate / 100;

            calcSalary.AddWorkByLow = 0;

            if (jobData.IsAllowedAddWork)
            {
                double AddWork = 1;
                if (jobData.AllowedAddWorkGroup == 1) AddWork = 0.5;

                //תוספת עבודה מתוקף מינוי בחוק
                calcSalary.AddWorkByLow = calcSalary.BaseSalary * AddWork / 100;
            }

            //סה"כ שכר בסיס לפני העלאה
            calcSalary.TotalBaseBeforIncrease = calcSalary.BaseSalary
                                        + calcSalary.SalSeniorityRate
                                        + calcSalary.AddWorkByLow;


            //שיעור העלאת שכר
            calcSalary.SalaryIncreaseRate = 0;

            if (calcSalary.TotalBaseBeforIncrease > 30000)
            {
                calcSalary.SalaryIncreaseRate = 1 / 100;
            }
            else if (calcSalary.TotalBaseBeforIncrease > 20000)
            {
                calcSalary.SalaryIncreaseRate = 1.25 / 100;
            }
            else
            {
                calcSalary.SalaryIncreaseRate = 1.5 / 100;
            }

            calcSalary.SalaryIncreaseRate += (jobData.ManagerialLevel * 0.1 / 100);    //(0-4)תוספת לרמה ניהולית


            //תוספת העלאת שכר
            calcSalary.AddSalaryIncrease = calcSalary.TotalBaseBeforIncrease * calcSalary.SalaryIncreaseRate;

            //שכר בסיס חדש
            calcSalary.NewBaseSalary = calcSalary.TotalBaseBeforIncrease + calcSalary.AddSalaryIncrease;


            return calcSalary;


        }
    }
}
