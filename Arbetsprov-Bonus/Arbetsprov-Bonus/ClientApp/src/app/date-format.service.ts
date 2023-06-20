import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DateFormatService {

  //Would be faster to import a package for dealing with dates, such as date-fns.
  //This is a way of showing understanding of Angular services

  constructor() { }

  public formatDate(date: Date | undefined) {
    if (!date) {
      date = new Date();
    }
    return new Date(date).getFullYear() + '-' + (new Date(date).getMonth() < 9 ? '0' + (new Date(date).getMonth() + 1) : (new Date(date).getMonth() + 1)) + '-' + (new Date(date).getDate() < 10 ? '0' + (new Date(date).getDate()) : (new Date(date).getDate()));
  }

  public formatMonth(date: Date | undefined) {
    if (!date) {
      date = new Date();
    }
    return new Date(date).getFullYear() + '-' + (new Date(date).getMonth() < 9 ? '0' + (new Date(date).getMonth() + 1) : (new Date(date).getMonth() + 1));
  }

}
