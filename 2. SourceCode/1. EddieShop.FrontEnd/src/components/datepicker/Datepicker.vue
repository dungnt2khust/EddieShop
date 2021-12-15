<template lang="">
	<div v-if="datePickerState" class="datepicker">
		<div class="datepicker__head">
			<div @click="prevOnClick()" class="datepicker__prev"></div>
			<div @click="yearPicker = true" class="datepicker__currmonth">
				Tháng {{ curr.month }} {{ curr.year }}
			</div>
			<div @click="nextOnClick()" class="datepicker__next"></div>
			<div v-if="yearPicker" class="datepicker__year">
				<div class="datepicker__year-list">
					<div
						v-for="index in 9"
						@click="pickYear((Math.floor(currYear / 3) - 1) * 3 + index)"
						class="datepicker__year-item"
						:class="{
							'year--selected':
								(Math.floor(currYear / 3) - 1) * 3 + index == selected.year,
						}"
						:key="index"
					>
						{{ (Math.floor(currYear / 3) - 1) * 3 + index }}
					</div>
				</div>
				<div class="datepicker__year-control">
					<div
						@click="currYear = currYear - 6"
						class="datepicker__year-prev"
					></div>
					<div @click="yearPicker = false" class="datepicker__year-cancel">
						Huỷ
					</div>
					<div
						@click="currYear = currYear + 6"
						class="datepicker__year-next"
					></div>
				</div>
			</div>
		</div>
		<table class="datepicker__table">
			<thead class="datepicker__thead">
				<tr class="datepikcer__tr">
					<th v-for="(item, index) in days" class="datepicker__th" :key="index">
						{{ item }}
					</th>
				</tr>
			</thead>
			<tbody class="datepicker__tbody">
				<tr
					v-for="idxRow in Math.ceil(getDates.length / 7)"
					class="datepicker__tr"
					:key="idxRow"
				>
					<td
						v-for="idxData in 7"
						class="datepicker__td"
						@click="dateOnClick(getDates[(idxRow - 1) * 7 + idxData - 1])"
						:class="{
							'datepicker--selected':
								getDates[(idxRow - 1) * 7 + idxData - 1] == selected.date &&
								curr.month == selected.month &&
								curr.year == selected.year,
							'datepicker--overtoday':
								overToday(
									getDates[(idxRow - 1) * 7 + idxData - 1],
									curr.month,
									curr.year
								) && !overtoday,
						}"
						:style="{
							visibility: getDates[(idxRow - 1) * 7 + idxData - 1]
								? 'visible'
								: 'hidden',
						}"
						:key="idxData"
					>
						{{ getDates[(idxRow - 1) * 7 + idxData - 1] }}
					</td>
				</tr>
			</tbody>
		</table>
		<div @click="todayOnClick()" class="datepicker__footer fx-center">
			Hôm nay
		</div>
	</div>
</template>
<script>
	export default {
		name: "Datepicker",
		props: {
			datePickerState: {
				type: Boolean,
				default: false,
			},
			value: {
				type: String,
				default: null,
			},
			overtoday: {
				type: Boolean,
				default: false,
			},
		},
		data() {
			return {
				days: ["T2", "T3", "T4", "T5", "T6", "T7", "CN"],
				curr: {
					year: new Date().getFullYear(),
					month: new Date().getMonth() + 1,
				},
				selected: {
					year: new Date().getFullYear(),
					month: new Date().getMonth() + 1,
					date: new Date().getDate(),
				},
				yearPicker: false,
				currYear: new Date().getFullYear(),
			};
		},
		computed: {
			/**
			 * Lấy thông tin cần thiết để in ra ngày
			 * @returns {object}
			 * CreatedBy: NTDUNG (30/08/2021)
			 */
			getInfos() {
				// Số ngày trong một tháng
				var daysInMonth = new Date(
					this.curr.year,
					this.curr.month,
					0
				).getDate();

				// Nyày đầu tiên là thứ mấy
				var dayBegin = new Date(
					`${this.curr.year}-${this.curr.month}-1`
				).getDay();
				dayBegin = dayBegin == 0 ? 7 : dayBegin;

				return { dayBegin: dayBegin, daysInMonth: daysInMonth };
			},
			/**
			 * Tạo ra mảng dữ liệu để in lên bảng
			 * @returns {array}
			 * CreatedBy: NTDUNG (30/08/2021)
			 */
			getDates() {
				var dates = [];
				var dayBegin = this.getInfos["dayBegin"];
				var daýsInMonth = this.getInfos["daysInMonth"];

				// Thêm những phần tử rỗng
				for (var i = 0; i < dayBegin - 1; i++) {
					dates.push("");
				}

				// Thêm những phần tử chính
				for (var j = 1; j <= daýsInMonth; j++) {
					dates.push(j);
				}
				return dates;
			},
		},
		methods: {
			/**
			 * Bắt sự kiện nút next
			 * CreatedBy: NTDUNG (30/08/2021)
			 */
			nextOnClick() {
				if (this.curr.month == 12) {
					this.curr.year++;
					this.curr.month = 1;
				} else {
					this.curr.month++;
				}
			},
			/**
			 * Bắt sự kiện nút prev
			 * CreatedBy: NTDUNG (30/08/2021)
			 */
			prevOnClick() {
				if (this.curr.month == 1) {
					this.curr.year--;
					this.curr.month = 12;
				} else {
					this.curr.month--;
				}
			},
			/**
			 * Bắt sự kiện nút hôm nay
			 * CreatedBy: NTDUNG (30/08/2021)
			 */
			todayOnClick() {
				var currDate = new Date();
				this.curr.month = currDate.getMonth() + 1;
				this.curr.year = currDate.getFullYear();

				this.selected.date = currDate.getDate();
				this.selected.month = currDate.getMonth() + 1;
				this.selected.year = currDate.getFullYear();
				this.dateOnClick(this.selected.date);
			},
			/**
			 * Sự kiện nhấn vào một ngày
			 * @param {number} pickedDate
			 * CreatedBy: NTDUNG (30/08/2021)
			 * ModifiedBy: NTDUNG (01/09/2021)
			 */
			dateOnClick(pickedDate) {
				var date = pickedDate;
				date = date < 10 ? "0" + date : date;
				var month = this.curr["month"];
				month = month < 10 ? "0" + month : month;
				var year = this.curr["year"];

				var newDate = `${year}-${month}-${date}`;

				var datePicked = new Date(newDate);

				if (datePicked && !this.overToday(date, month, year)) {
					this.$emit("input", newDate);
				} else {
					if (this.overtoday) this.$emit("input", newDate);
					// else
					// 	this.callDialog('warn', this.$resourcesVN.OverCurrentDate);
				}
				this.$emit("hideDatepicker");
			},
			/**
			 * Pick year
			 * @param {Number} year
			 * CreatedBy: NTDUNG (03/09/2021)
			 */
			pickYear(year) {
				(this.currYear = year), this.$set(this.selected, "year", year);
				this.$set(this.curr, "year", year);
				this.yearPicker = false;
				this.dateOnClick(this.selected.date);
			},
			/**
			 * Kiểm tra có vượt quá ngày hiện tại
			 * @param {Number} date
			 * @param {Number} date
			 * @param {Number} date
			 * @return {Boolean}
			 * CreatedBy: NTDUNG (01/10/2021)
			 */
			overToday(date, month, year) {
				var currDate = new Date();
				var dateChecked = new Date(`${year}-${month}-${date}`);
				currDate.setHours(0);
				currDate.setMilliseconds(0);
				currDate.setMinutes(0);
				dateChecked.setHours(0);
				dateChecked.setMilliseconds(0);
				dateChecked.setMinutes(0);

				return dateChecked > currDate;
			},
		},
		watch: {
			/**
			 * Value truyền vào thay đổi thì cập nhật ngày hiện tại
			 * @param {String} newValue
			 * CreatedBy: NTDUNG (01/09/2021)
			 */
			value: {
				handler(newValue) {
					if (newValue) {
						// Chuyển về dạng ngày
						var newDate = new Date(newValue);
						var date = newDate.getDate();
						var month = newDate.getMonth() + 1;
						var year = newDate.getFullYear();

						// Gán vào ngày hiện tại
						this.$set(this.curr, "month", month);
						this.$set(this.curr, "year", year);
						this.currYear = year;

						this.$set(this.selected, "date", date);
						this.$set(this.selected, "month", month);
						this.$set(this.selected, "year", year);
					} 
					// else {
					// 		this.todayOnClick();
					// }
				},
				immediate: true,
			},
		},
	};
</script>
<style lang="scss">
	@import '@/assets/scss/components/datepicker/datepicker.scss';
</style>
