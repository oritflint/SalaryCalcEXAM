import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormBuilder, Validators } from '@angular/forms';
import { Ijob } from '../models/job.interface';
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
    public arrAddWorkGrp: Array<string> = ['א', 'ב']

    public propLvl;
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private fb: FormBuilder) {
        this._baseUrl = baseUrl;

    }

    public salaryCalcForm = this.fb.group({
        partTime: [this.arrPartTime[0], Validators.required],
        professionalLevel: ['1', Validators.required],
        managerialLevel: ['0', Validators.required],
        totalSeniority: [, Validators.required],
        AllowedAddWork: false
        , AllowedAddWorkGroup: ['1', Validators.required]
    });


    //GETers 
    get AllowedAddWork(): any {
        return this.salaryCalcForm.get('AllowedAddWork');
    }

    get partTime(): any {
        return this.salaryCalcForm.get('partTime');
    }

    get professionalLevel(): any {
        return this.salaryCalcForm.get('professionalLevel');
    }

    get managerialLevel(): any {
        return this.salaryCalcForm.get('managerialLevel');
    }

    get totalSeniority(): any {
        return this.salaryCalcForm.get('totalSeniority');
    }

    get AllowedAddWorkGroup(): any {
        return this.salaryCalcForm.get('AllowedAddWorkGroup');
    }


    public salaryCalcSubmit(event: any): void {

        this.submitted = true;
        if (this.salaryCalcForm.valid) {
            console.log("our form:", this.salaryCalcForm)

            this.http.get<ICalcSalary[]>(this._baseUrl
                + 'calcSalary?partTime=' + this.partTime.value
                + '&professionalLevel=' + this.managerialLevel.value
                + '&managerialLevel=' + this.managerialLevel.value
                + '&totalSeniority=' + this.totalSeniority.value
                + '&isAllowedAddWork=' + this.AllowedAddWork.value
                + '&AllowedAddWorkGroup=' + this.AllowedAddWorkGroup.value
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
