import App from "@/main.js";

class Dialog {
  constructor() {}

  /**
   * Dialog xác nhận
   * @param {string} message
   * @param {object} functions
   * CreatedBy: NTDUNG (18/12/2021)
   */

  confirm(message, functions = { YES: () => {}, NO: () => {} }) {
    App.$bus.$emit("showDialog", {
      message,
      type: "confirm",
      functions
    });
  }

  /**
   * Dialog xác nhận & huỷ
   * @param {string} message
   * @param {object} functions
   * CreatedBy: NTDUNG (18/12/2021)
   */

  confirmCancel(
    message,
    functions = { YES: () => {}, NO: () => {}, CANCEL: () => {} }
  ) {
    App.$bus.$emit("showDialog", {
      message,
      type: "confirmCancel",
      functions
    });
  }

  /**
   * Dialog cảnh báo
   * @param {string} message
   * CreatedBy: NTDUNG (18/12/2021)
   */

  warn(message, functions = { CANCEL: () => {} }) {
    App.$bus.$emit("showDialog", { message, type: "warn", functions });
  }

  /**
   * Dialog lỗi
   * @param {string} message
   * CreatedBy: NTDUNG (18/12/2021)
   */

  error(message, functions = { CANCEL: () => {} }) {
    App.$bus.$emit("showDialog", { message, type: "error", functions });
  }
}
export default new Dialog();
