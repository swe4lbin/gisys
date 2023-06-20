import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { DateFormatService } from '../date-format.service';
import { Consultant } from '../shared/models';

interface ConsultantAndCalculation {
  consultant: Consultant,
  billedHours: number,
  points: number,
  bonus: number
}

interface BonusPeriod {
  id?: number,
  netProfit: number,
  totalPoints: number,
  startDate?: Date,
  endDate?: Date,
  status: string
}

interface BonusPeriodAndConsultants {
  bonusPeriod: BonusPeriod
  consultantsAndCalculations: ConsultantAndCalculation[]
}

@Component({
  selector: 'app-bonus',
  templateUrl: './bonus.component.html',
})
export class BonusComponent {

  public netProfit = 0;
  public bonusPeriods: BonusPeriod[] = [];
  public chosenBonusPeriod: BonusPeriodAndConsultants | null = null;
  public consultants: Consultant[] = [];
  public isLoading: boolean = true;

  constructor(public http: HttpClient, @Inject('BASE_URL') public baseUrl: string, public dateFormatService: DateFormatService) {
    this.getBonusPeriods();
  }

  public async getBonusPeriods() {
    try {
      this.isLoading = true;
      this.bonusPeriods = await this.http.get<BonusPeriod[]>(this.baseUrl + 'api/Bonus').toPromise();
    }
    catch (e) {

    }
    this.isLoading = false;
  }

  public async selectBonusPeriod(id: number = 0) {
    if (id == 0) {
      this.chosenBonusPeriod = null;
      this.getBonusPeriods();
      return;
    }
    this.chosenBonusPeriod = await this.http.get<BonusPeriodAndConsultants>(this.baseUrl + 'api/Bonus/' + id).toPromise();
  }

  public async newBonusPeriod() {
    this.chosenBonusPeriod = {
      bonusPeriod: {
        netProfit: 0,
        totalPoints: 0,
        status: 'new',
      },
      consultantsAndCalculations: []
    };
;
    try {
      this.isLoading = true;
      this.consultants = await this.http.get<Consultant[]>(this.baseUrl + 'api/consultant').toPromise();
    }
    catch (e) {

    }
    this.isLoading = false;

    this.chosenBonusPeriod.consultantsAndCalculations = [];
    for (let consultant of this.consultants) {
      let consAndC: ConsultantAndCalculation = {
        consultant: consultant,
        bonus: 0,
        billedHours: 0,
        points: 0
      }
      this.chosenBonusPeriod.consultantsAndCalculations.push(consAndC);
    }
  }

  public async deleteBonusPeriod() {
    if (!this.chosenBonusPeriod?.bonusPeriod.id) {
      this.selectBonusPeriod(0);
      return;
    }

    try {
      this.isLoading = true;
      await this.http.delete<boolean>(this.baseUrl + 'api/Bonus/' + this.chosenBonusPeriod.bonusPeriod.id).toPromise();
    }
    catch (e) {

    }
    this.isLoading = false;
    this.selectBonusPeriod(0);
  }

  public async calculateAndSaveBonuses(newStatus: string) {
    if (newStatus == 'completed') {
      if (!confirm("När du sparar underlaget går det inte att ångra. Kontrollera att alla uppgifter stämmer. Vill du fortsätta?")) {
        return;
      }
    }
    this.chosenBonusPeriod!.bonusPeriod.status = newStatus;

    const dateControl = document.querySelector('input[type="month"]') as any;
    if (dateControl && (dateControl?.value != this.dateFormatService.formatDate(this.chosenBonusPeriod?.bonusPeriod.startDate))) {
      console.log(dateControl.value.split('-'));
      this.chosenBonusPeriod!.bonusPeriod.startDate = new Date(dateControl.value + '-01');
      this.chosenBonusPeriod!.bonusPeriod.endDate = new Date(new Date(this.chosenBonusPeriod!.bonusPeriod.startDate!.toISOString()).setMonth(this.chosenBonusPeriod!.bonusPeriod.startDate!.getMonth() + 1));
    }

    try {
      this.isLoading = true;
      if (this.chosenBonusPeriod?.bonusPeriod.id) {
        this.chosenBonusPeriod = await this.http.put<BonusPeriodAndConsultants>(this.baseUrl + 'api/Bonus/' + this.chosenBonusPeriod?.bonusPeriod.id, this.chosenBonusPeriod).toPromise();
      }
      else {
        this.chosenBonusPeriod = await this.http.post<BonusPeriodAndConsultants>(this.baseUrl + 'api/Bonus', this.chosenBonusPeriod).toPromise();
      }
    }
    catch (e) {

    }
    this.isLoading = false;
  }

}
