import App from "@/main.js";
import {ToastType} from '@/models/enum/ToastType.js';

class Toast {
  constructor() {}

  /**
   * Thành công
   * CreatedBy: NTDUNG (20/12/2021)
   */
  success(message) {
    App.$bus.$emit('showToastMessage', {message, type: ToastType.SUCCESS, duration: 2000});
  }

  /**
   * Lỗi
   * CreatedBy: NTDUNG (20/12/2021)
   */
  error(message) {
    App.$bus.$emit('showToastMessage', {message, type: ToastType.ERROR, duration: 2000});
  }

  /**
   * Cảnh báo
   * CreatedBy: NTDUNG (20/12/2021)
   */
  warn(message) {
    App.$bus.$emit('showToastMessage', {message, type: ToastType.WARN, duration: 2000});
  }

  /**
   * Thông tin
   * CreatedBy: NTDUNG (20/12/2021)
   */
  info(message) {
    App.$bus.$emit('showToastMessage', {message, type: ToastType.INFO, duration: 2000});
  }
}
export default new Toast();
