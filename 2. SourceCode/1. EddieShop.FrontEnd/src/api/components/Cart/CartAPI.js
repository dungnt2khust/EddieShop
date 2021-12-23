import BaseAPI from '@/api/base/BaseAPI.js';

class CartAPI extends BaseAPI {
    constructor() {
        super();
        this.controller = 'Cart';
    }
}

export default new CartAPI();