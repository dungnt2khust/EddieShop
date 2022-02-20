export default {
  methods: {
    /**
     * Định dạng tiền tệ
     * @param {String, Number} money
     * CreatedBy: NTDUNG (01/12/2021)
     */
    formatMoney(money) {
      if (money && !Number.isNaN(+money))
        return money.toLocaleString("vi", {
          style: "currency",
          currency: "VND"
        });
      else return null;
    },
    formatDate(date) {
      if (date) {
        var newDate = new Date(date);
        var day = newDate.getDate();
        day = day < 10 ? "0" + day : day;
        var month = newDate.getMonth() + 1;
        month = month < 10 ? "0" + month : month;
        var year = newDate.getFullYear();
        return `${day}/${month}/${year}`;
      }
      return null;
    },
    formatDateTime(date) {
      if (date) {
        var newDate = new Date(date);
        var day = newDate.getDate();
        day = day < 10 ? "0" + day : day;
        var month = newDate.getMonth() + 1;
        month = month < 10 ? "0" + month : month;
        var year = newDate.getFullYear();
        var hour = newDate.getHours();
        hour = hour < 10 ? "0" + hour : hour;
        var minute = newDate.getMinutes();
        minute = minute < 10 ? "0" + minute : minute;
        return `${hour}:${minute} ${day}/${month}/${year}`;
      }
      return null;
    },
    /**
     * Định dạng số
     * CreatedBy: NTDUNG (20/11/2021)
     */
    formatNumber(number) {
      if (!number) return new Intl.NumberFormat().format(0);
      if (number && !Number.isNaN(+number))
        return new Intl.NumberFormat().format(number);
      else return null;
    },
    formatNumberDot(number) {
      if (!number) {
        number = 0;
        return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
      }
      if (number && !Number.isNaN(+number))
        return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
      else return null;
    }
  }
};
