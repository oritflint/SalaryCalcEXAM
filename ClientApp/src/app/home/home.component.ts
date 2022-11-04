import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormBuilder, Validators } from '@angular/forms';
import { ICalcSalary } from '../models/calcSalary.interface';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})

export class HomeComponent {
    private _baseUrl: string

    public calcSalary: ICalcSalary;

    public submitted = false;
    public arrPartTime: Array<string> = ['100', '75', '50'];
    public arrProfLvls: Array<string> = ['מתחיל', 'מנוסה'];
    public arrMngLvls: Array<string> = ['ללא', 'רמת ניהול 1', 'רמת ניהול 2', 'רמת ניהול 3', 'רמת ניהול 4'];
    public arrAddWorkGrp: Array<string> = ['קבוצה א', 'קבוצה ב']

    public propLvl;
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private fb: FormBuilder) {
        this._baseUrl = baseUrl;

    }

    public salaryCalcForm = this.fb.group({
        fcpartTime: [this.arrPartTime[0], Validators.required],
        fcProfLvl: ['1', Validators.required],
        fcMngLvl: ['0', Validators.required],
        fcTotalSnr: [, Validators.required],
        cdIsAddWork: false,
        cbAddWorkGroup: ['1', Validators.required]
    });


    //GETers 
    get cdIsAddWork(): any {
        return this.salaryCalcForm.get('cdIsAddWork');
    }

    get fcpartTime(): any {
        return this.salaryCalcForm.get('fcpartTime');
    }

    get fcProfLvl(): any {
        return this.salaryCalcForm.get('fcProfLvl');
    }

    get fcMngLvl(): any {
        return this.salaryCalcForm.get('fcMngLvl');
    }

    get fcTotalSnr(): any {
        return this.salaryCalcForm.get('fcTotalSnr');
    }

    get cbAddWorkGroup(): any {
        return this.salaryCalcForm.get('cbAddWorkGroup');
    }


    public salaryCalcSubmit(event: any): void {

        this.submitted = true;
        if (this.salaryCalcForm.valid) {
            console.log("our form:", this.salaryCalcForm)

            this.http.get<ICalcSalary[]>(this._baseUrl
                + 'calcSalary?partTime=' + this.fcpartTime.value
                + '&professionalLevel=' + this.fcProfLvl.value
                + '&managerialLevel=' + this.fcMngLvl.value
                + '&totalSeniority=' + this.fcTotalSnr.value
                + '&isAllowedAddWork=' + this.cdIsAddWork.value
                + '&AllowedAddWorkGroup=' + this.cbAddWorkGroup.value
            ).subscribe(result => {
                this.calcSalary = result[0];
                console.log("jobAfter:", this.calcSalary)

            }, error => console.error(error));

            console.log("jobBefore:", this.calcSalary)
        }
        else {
            event.preventDefault();
            event.stopPropagation();
        }

    }
}
