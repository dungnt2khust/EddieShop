import BaseAPIConfig from "@/api/base/BaseAPIConfig.js";
class Email {
  AccountData = {};
  constructor() {
    this.controller = "email";
  }

  /**
   * Gửi email
   * @param {*} Subject 
   * @param {*} Body 
   * @param {*} To 
   * @param {*} CC 
   * created by ntdung5 21.01.2022
   */
  SendEmail(Subject, Body, To = [], CC = [], From = "iotandapp.eddieonthecode@gmail.com") {
    return BaseAPIConfig.post(`${this.controller}`, {Subject, Body, To, CC, From});
  }
}

export default new Email();
