import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DateFormatService } from '../date-format.service';
import { Consultant } from '../shared/models';

@Component({
  selector: 'app-consultants',
  templateUrl: './consultants.component.html'
})
export class ConsultantsComponent {
  public consultants: Consultant[] = [];
  public chosenEditRow = -1;
  public consultantToEdit: Consultant | null = null;
  public lastNameEdit: string = "";
  public isLoading = true;

  constructor(public http: HttpClient, @Inject('BASE_URL') public baseUrl: string, public dateFormatService: DateFormatService) {
    http.get<Consultant[]>(baseUrl + 'api/consultant').subscribe(result => {
      this.consultants = result;
      this.isLoading = false;
    }, error => {
      console.error(error)
      this.isLoading = false;
    });
  }

  public newRow() {
    if (this.consultantToEdit) {
      return;
    }
    let newConsultant: Consultant = { firstName: '', lastName: '', startDate: new Date() };
    this.consultants.unshift(newConsultant);
    this.editRow(0);
  }

  public editRow(tableBodyIndex: number) {
    //Deep clone the consultant object
    this.consultantToEdit = JSON.parse(JSON.stringify(this.consultants[tableBodyIndex]));
    console.log(this.consultantToEdit);
    this.chosenEditRow = tableBodyIndex;
  }

  public async createConsultant(): Promise<Consultant | null> {
    try {
      this.isLoading = true;
      let returnData: any = await this.http.post(this.baseUrl + 'api/consultant', this.consultantToEdit).toPromise();
      this.isLoading = false;
      if (returnData) {
        return returnData;
      }
    }
    catch (error) {
      console.error(error);
    }
    this.isLoading = false;
    return null;
  }

  public async updateConsultant(): Promise<boolean> {
    let success: any = false;
    try {
      this.isLoading = true;
      success = await this.http.put(this.baseUrl + 'api/consultant/' + this.consultantToEdit?.id, this.consultantToEdit).toPromise();
      this.isLoading = false;
      return success;
    }
    catch (error) {
      console.error(error);
    }
    this.isLoading = false;
    return false;
  }

  public async removeConsultant(tableBodyIndex: number) : Promise<boolean> {
    try {
      this.isLoading = true;
      let success: any = await this.http.delete(this.baseUrl + 'api/consultant/' + this.consultants[tableBodyIndex].id).toPromise();
      this.isLoading = false;
      return success;
    }
    catch (error) {
      console.error(error);
    }
    this.isLoading = false;
    return false;
  }

  public async saveRowData(createNew: boolean = false) {
    const dateControl = document.querySelector('input[type="date"]') as any;
    if (createNew || (dateControl?.value != this.dateFormatService.formatDate(this.consultantToEdit?.startDate))) {
      this.consultantToEdit!.startDate = dateControl.value;
    }

    if (createNew) {
      let newConsult = await this.createConsultant();
      if (newConsult) {
        this.consultants.push(newConsult);
      }
      else {
        this.actionFailed();
      }
    }
    else {
      let success = await this.updateConsultant();
      if (success) {
        let index = this.consultants.findIndex(x => x.id == this.consultantToEdit?.id);
        if (index > -1 && this.consultantToEdit) {
          this.consultants[index] = this.consultantToEdit;
        }
      }
      else {
        this.actionFailed();
      }
    }

    this.stopEditing();
  }

  public async deleteRow(tableBodyIndex: number) {
    if (confirm("Är du säker på att du vill ta bort konsulten?")) {
      let success = await this.removeConsultant(tableBodyIndex);
      if (success) {
        this.consultants.splice(tableBodyIndex, 1);
      }
      else {
        this.actionFailed();
      }
    }
  }

  public actionFailed() {
    alert("Åtgärden kunde inte utföras. Var god försök igen. Kontakta support om problemet kvartstår.");
  }


  public stopEditing() {
    //If creating a new consultant
    if (!this.consultantToEdit?.id) {
      this.consultants.splice(0, 1);
    }

    this.chosenEditRow = -1;
    this.consultantToEdit = null;
  }

}
