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
    /**
     * Định dạng số
     * CreatedBy: NTDUNG (20/11/2021)
     */
    formatNumber(number) {
      if (!number)
        return new Intl.NumberFormat().format(0);
      if (number && !Number.isNaN(+number))
        return new Intl.NumberFormat().format(number);
      else return null;
    },
    formatNumberDot(number) {
      if (!number)
        return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
      if (number && !Number.isNaN(+number))
        return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
      else return null;
    }
  }
};
