import BaseAPIConfig from "@/api/base/BaseAPIConfig.js";
class Upload {
  constructor() {
    this.controller = "Upload";
  }
  /**
   * Tải ảnh lên server
   * @returns 
   * created by ntdung5 26.01.2022
   */
  Image() {
    return BaseAPIConfig.get(`${this.controller}/image`);
  }
}
export default new Upload();