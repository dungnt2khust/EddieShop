<template lang="">
	<div v-show="dialogState" class="dialog" :class="dialogType ? 'dialog--' + dialogType : ''">
		<div class="dialog__main">
			<div class="dialog__content">
				<div class="dialog__icon">
					<div v-if="dialogType == 'error'" class="mi-error"></div>
					<div v-if="dialogType == 'warn' || dialogType == 'confirm'" class="mi-warn"></div>
					<div v-if="dialogType == 'confirmCancel'" class="mi-question"></div>
				</div>
				<div class="dialog__message" v-html="dialogMessage">
				</div>
			</div>
			<div class="dialog__separate"></div>
			<div class="dialog__control">
				<div
					@click="confirm(dialogAnswer.CANCEL)"
					class="dialog__cancel button"
					:class="{'button--green': cancelGreen}"
				>
					{{ cancelLabel }}
				</div>
				<div class="dialog__answer">
					<div
						@click="confirm(dialogAnswer.NO)"
						class="dialog__no button"
					>
						Không
					</div>
					<div
						@click="confirm(dialogAnswer.YES)"
						class="dialog__yes button m-l-10 button--green"
					>
						Có
					</div>
				</div>
			</div>
		</div>
	</div>
</template>
<script>
// Plugins 
import {DialogAnswer} from "@/models/enum/DialogAnswer.js"

	export default {
		name: "Dialog",
		data() {
			return {
				dialogType: null,
				dialogState: false,
                dialogMessage: '',
				dialogFunctions: {},
				dialogAnswer: DialogAnswer
			};
		},
		computed: {
			/**
			 * Chỉ ra những trường hợp nút cancel màu xanh
			 * CreatedBy: NTDUNG (04/09/2021)
			 */
			cancelGreen() {
				switch(this.dialogType) {
					case 'error':
					case 'warn':
						return true;
					default: 
						return false;
				}
			},
			/**
			 * Đặt nội dung cho nút cancel
			 * CreatedBy: NTDUNG (04/09/2021)
			 */
			cancelLabel() {
				switch(this.dialogType) {
					case 'error': 
					case 'warn':
						return 'Đồng ý';
					default: 
						return 'Huỷ';
				}
			}
		},
		created() {
			this.$bus.$on("showDialog", (data) => {
				// Bật dialog
				this.dialogState = true;

				// Đặt kiểu cho dialog
				this.dialogType = data.type;

				// Gán thông điệp
				this.dialogMessage= data.message;

				// Thực thi hàm
				this.dialogFunctions = data.functions
			});
		},
		methods: {
			/**
			 * Trả lời khi nhấn phím
			 * CreatedBy: NTDUNG (19/12/2021)
			 */
			confirm(type) {
				switch(type) {
					case DialogAnswer.CANCEL:
						this.dialogState = false;
						break;
					case DialogAnswer.YES:
						if (this.dialogFunctions && this.dialogFunctions.YES) {
							this.dialogFunctions.YES();
						}
						this.dialogState = false;
						break;
					case DialogAnswer.NO:
						if (this.dialogFunctions && this.dialogFunctions.NO) {
							this.dialogFunctions.NO();
						}
						this.dialogState = false;
						break;
				}
			}
		}
	};
</script>
<style lang="scss">
	@import "@/assets/scss/components/dialog/dialog.scss";
</style>
