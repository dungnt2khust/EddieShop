import BaseAPI from '@/api/base/BaseAPI.js';

class ProductDetailAPI extends BaseAPI {
    constructor() {
        super();
        this.controller = 'ProductDetail';
    }
}

export default new ProductDetailAPI();