import BaseAPIConfig from "@/api/base/BaseAPIConfig.js";
import BaseAPI from '@/api/base/BaseAPI.js';
class Order extends BaseAPI {
  constructor() {
    super();
    this.controller = "Order";
  }
  /**
   * Thêm mới đơn hàng
   * CreatedBy: NTDUNG (05/01/2021)
   */
  add(cartIds) {
    return BaseAPIConfig.post(
      `${this.controller}?sessionID=${(localStorage.getItem("Session"))}`,
      cartIds
    );
  }
  /**
   * Thay đổi trạng thái
   * CreatedBy: NTDUNG (05/01/2021)
   */
  changeState() {
    return BaseAPIConfig.post(
      `${this.controller}?sessionID=${(localStorage.getItem("Session"))}`
    );
  }
}

export default new Order();
