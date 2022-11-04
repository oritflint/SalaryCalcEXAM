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

        //שירות לחישוב השכר
        //קלט - נתוני משרה
        //פלט - כל רכיבי השכר מחושבים
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

            //הזנת נתוני הקלט של המשתמש - נתוני המשרה
            jobData.PartTime= partTime;
            jobData.ProfessionalLevel= professionalLevel;
            jobData.ManagerialLevel = managerialLevel;
            jobData.TotalSeniority = totalSeniority;
            jobData.IsAllowedAddWork = isAllowedAddWork;
            jobData.AllowedAddWorkGroup = allowedAddWorkGroup;

            //ביצוע חישוב השכר
            calc = GetCalcSalary(jobData);

            return Enumerable.Range(1, 1).Select(index => calc).ToArray();
        }

        //פונקציה לחישוב כל רכיבי השכר
        public CalcSalary GetCalcSalary(JobData jobData)             //נתוני משרת עובד
        {
            CalcSalary calcSalary = new CalcSalary();
            int iTotalHrs = 170 * jobData.PartTime / 100;          //סה"כ שעות בחודש


            /////////////////////חישוב שכר יסוד לפי חלקיות משרה

            int iSalaryForHour = 100;                               //(0/1) סה"כ שכר לשעה לפי דרגה מקצועית
            if (jobData.ProfessionalLevel == 1) iSalaryForHour = 120;

            iSalaryForHour += (jobData.ManagerialLevel * 20);       //(0-4)תוספת לרמה ניהולית

            calcSalary.BaseSalary = iTotalHrs * iSalaryForHour;


            /////////////////////חישוב שיעור תוספת ותק
            calcSalary.AddSeniorityRate = jobData.TotalSeniority * 1.25;


            /////////////////////חישוב תוספת ותק לשכר
            calcSalary.SalSeniorityRate = Math.Round(calcSalary.BaseSalary * calcSalary.AddSeniorityRate / 100,2);


            /////////////////////חישוב תוספת עבודה מתוקף מינוי בחוק
            calcSalary.AddWorkByLow = 0;

            if (jobData.IsAllowedAddWork)
            {
                double AddWork = 1;
                if (jobData.AllowedAddWorkGroup == 1) AddWork = 0.5;

                
                calcSalary.AddWorkByLow = Math.Round(calcSalary.BaseSalary * AddWork / 100,2);
            }


            /////////////////////חישוב סה"כ שכר בסיס לפני העלאה
            calcSalary.TotalBaseBeforIncrease = calcSalary.BaseSalary
                                        + calcSalary.SalSeniorityRate
                                        + calcSalary.AddWorkByLow;


            /////////////////////חישוב שיעור העלאת שכר
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

            calcSalary.SalaryIncreaseRate = Math.Round(calcSalary.SalaryIncreaseRate, 3);

            /////////////////////חישוב תוספת העלאת שכר
            calcSalary.AddSalaryIncrease = Math.Round(calcSalary.TotalBaseBeforIncrease * calcSalary.SalaryIncreaseRate,2);


            /////////////////////חישוב שכר בסיס חדש
            calcSalary.NewBaseSalary = Math.Round(calcSalary.TotalBaseBeforIncrease + calcSalary.AddSalaryIncrease, 2);


            return calcSalary;

        }
    }
}
