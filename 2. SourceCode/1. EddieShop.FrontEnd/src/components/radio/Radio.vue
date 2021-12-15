<template lang="">
	<div class="radio">
		<div v-if="label != ''" class="label">{{ label }}</div>
		<div class="radio__list fx-row">
			<div
				v-for="(item, index) in options"
				class="radio__item fx-row"
				:class="{ 'radio__item--selected': index == currIdx }"
                @click="changeRadio(index)"
				@keydown="radioItemOnKeydown($event, index)"
				:key="index"
				tabindex="0"
				:ref="'radio' + index"
			>
				<div class="radio__border">
					<div class="radio__main">
						<div class="radio__icon"></div>
					</div>
				</div>
				<div class="radio__value">{{ item }}</div>
			</div>
		</div>
	</div>
</template>
<script>
	export default {
		name: "Radio",
		props: {
			options: {
				type: Array,
				default: function() {
					return [];
				},
			},
			label: {
				type: String,
				default: "",
			},
			value: {
				type: Number,
				default: null
			},
			tabIdx: {
				type: Number,
				default: -1
			}
		},
		data() {
			return {
				currIdx: -1
			};
		},
		methods: {
			/**
			 * Bắt sự kiện nhấn phím mũi tên để di chuyển
			 * @param {event} event 
			 * @param {number} index
			 * CreatedBy: NTDUNG (30/08/2021)
			 */
			radioItemOnKeydown(event, index) {
				switch(event.key) {
					case 'ArrowRight':
						if (this.currIdx != this.options.length - 1) 
							this.currIdx++;
						else 
							this.currIdx = 0;
						this.$refs['radio' + this.currIdx][0].focus();
						break;
					case 'ArrowLeft':
						if (this.currIdx != 0)
							this.currIdx--;
						else 
							this.currIdx = this.options.length - 1;	
						this.$refs['radio' + this.currIdx][0].focus();
						break;
					case 'Enter':
						this.currIdx = index;
						break;
				}
			},
			changeRadio(index) {
				this.currIdx = index;
			}
		},
		watch: {
			/**
			 * Khi thay đổi giá trị hiện tại thì emit ra ngoài component cha
			 * @param {Number} option lựa chọn hiện tại
			 * CreatedBy: NTDUNG (02/09/2021)
			 */	
			currIdx: function(option) {
				this.$emit('input', option);
			},
			/**
			 * Bắt sự kiện thay đổi giá trị truyền vào
			 * @param {number} value
			 * CreatedBy: NTDUNG (31/08/2021)
			 */
			value: {
				handler(value) {
					if (value !== null)
						this.currIdx = value;
				},
				immediate: true
			}
			
		}
	};
</script>
<style lang="scss">
	@import '@/assets/scss/components/radio/radio.scss';
</style>

