import BaseAPIConfig from './BaseAPIConfig.js'

export default class BaseAPI {
	constructor() {
	}

	controller = "";

	/**
	 * Lấy ra sessionID
	 * @returns {String}
	 */
	GetSession() {
		var sessionID = localStorage.getItem('Session');
		var sessionRoute = sessionID ? `sessionID=${sessionID}` : '';
		return sessionRoute;
	}

	/**
	 * Lấy tất cả thông tin
	 * @returns promise
	 * CreatedBy: NTDUNG (01/09/2021)
	 */
	GetAll(route) {	
		route = route ? route : '';
		return BaseAPIConfig.get(this.controller + route + `?${this.GetSession()}`);
	}

	/**
	 * Lấy theo id
	 * @param {string} id
	 * @returns promise
	 * CreatedBy: NTDUNG (01/09/2021)
	 */
	GetByID(id, mode) {
		var append = mode ? `&&mode=${mode}` : '';
		return BaseAPIConfig.get(this.controller + "/" + id + `?${this.GetSession()}${append}`);
	}

	/**
	 * thêm mới
	 * @param {object} body thông tin cần thêm
	 * @returns promise
	 * CreatedBy: NTDUNG (01/09/2021)
	 */
	async post(body) {
		return await BaseAPIConfig.post(this.controller + `?${this.GetSession()}`, body);
	}

	/**
	 * Chỉnh sửa theo Id
	 * @param {string} id
	 * @param {object} body thông tin chỉnh sửa
	 * @returns promise
	 * CreatedBy: NTDUNG (01/09/2021)
	 */
	async Update(id, body) {
		return await BaseAPIConfig.put(this.controller + "/" + id + `?${this.GetSession()}` , body);
	}

	/**
	 * Xóa nhiều theo Id
	 * @param {Array} listData mảng chứa các id
	 * @returns promise get từ call axios api
	 * CreatedBy: NTDUNG (01/09/2021)
	 */
	async DeleteMulti(body) {
		return await BaseAPIConfig.delete(this.controller + `?${this.GetSession()}`, { data: body });
	}
	/**
	 * Xóa theo id
	 * @param {string} id  id của đối tượng
	 * @returns promise
	 * CreatedBy: NTDUNG (01/09/2021)
	 */
	Delete(id) {
		return BaseAPIConfig.delete(this.controller + "/" + id + `?${this.GetSession()}`);
	}

	/**
	 * Lấy mã mới
	 * @returns {Promise}
	 * CreatedBy: NTDUNG (07/10/2021)
	 */
	GetNewCode() {
		return BaseAPIConfig.get(this.controller + "/NewCode" + `?${this.GetSession()}`);
	}

    /**
     * Lọc và phân trang
     * @param {String} filterString 
     * @param {Number} pageNumber 
     * @param {Number} pageSize 
     * @param {Object} filterData
     * @returns {Promise}
     * CreatedBy: NTDUNG(29/10/2021)
	 * ModifiedBy: NTDUNG (10/12/2021)
     */
    GetFilterPaging(filterString, pageNumber, pageSize, filterData = {}, storeCustom = "") {
		filterData.TotalFields = filterData.TotalFields ? filterData.TotalFields : [];
		filterData.Sorts = filterData.Sorts ? filterData.Sorts : [];
		filterData.RangeDates = filterData.RangeDates ? filterData.RangeDates : [];
		filterData.Conditions = filterData.Conditions ? filterData.Conditions : [];
		let api;
		if (filterString)
			api = this.controller + `/Paging?filterString=${filterString}&pageNumber=${pageNumber}&pageSize=${pageSize}` + `&${this.GetSession()}`;
		else	
			api = this.controller + `/Paging?pageNumber=${pageNumber}&pageSize=${pageSize}&storeCustom=${storeCustom}` + `&${this.GetSession()}`;
        return BaseAPIConfig.post(api, filterData);
    }

	/**
	 * Cập nhật thông tin theo cột
	 * CreatedBy: NTDUNG (08/12/2021)
	 */
	UpdateColumns(id, newInfo) {
		var columns = [];
		for (var i in newInfo) {
			columns.push(i);
		}
		return BaseAPIConfig.put(`${this.controller}/UpdateColumns/${id}` + `?${this.GetSession()}`, {
			Entity: newInfo,
			Columns: columns
		});
	}

	/**
	 * Cập nhật nhiều
	 * CreatedBy: NTDUNG(23/12/2021)
	 */
	UpdateMulti(listEntity) {
		return BaseAPIConfig.put(`${this.controller}/UpdateMulti` + `?${this.GetSession()}`, listEntity);
	}
}
