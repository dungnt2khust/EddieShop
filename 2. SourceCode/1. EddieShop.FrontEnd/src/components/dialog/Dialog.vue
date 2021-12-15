<template lang="">
	<div v-show="dialogState" class="dialog" :class="'dialog--' + dialogType">
		<div class="dialog__main">
			<div class="dialog__content">
				<div class="dialog__icon"></div>
				<div class="dialog__message">
					{{ dialogMessage }}
				</div>
			</div>
			<div class="dialog__separate"></div>
			<div class="dialog__control">
				<div
					@click="confirm($enum.DIALOG_RESULT.Cancel)"
					class="dialog__cancel button"
					:class="{'button--green': cancelGreen}"
				>
					{{ cancelLabel }}
				</div>
				<div class="dialog__answer">
					<div
						@click="confirm($enum.DIALOG_RESULT.No)"
						class="dialog__no button"
					>
						Không
					</div>
					<div
						@click="confirm($enum.DIALOG_RESULT.Yes)"
						class="dialog__yes button button--green"
					>
						Có
					</div>
				</div>
			</div>
		</div>
	</div>
</template>
<script>
	export default {
		name: "Dialog",
		data() {
			return {
				dialogType: null,
				dialogState: false,
                dialogMessage: ''
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
			});
		},
		methods: {
			/**
			 * Bắt sự kiện nhấn vào nút không hoặc có
			 * @param {boolean} answer kết quả của dialog (false: không , true: có)
			 * CreatedBy: NTDUNG (31/08/2021)
			 */
			confirm(answer) {
				// Ẩn dialog 
				this.dialogState = false;

				// Gửi kết quả trả về 
				this.$bus.$emit('dialogConfirm', answer);
			}	
		}
	};
</script>
<style lang="scss">
	@import "@/assets/scss/components/dialog/dialog.scss";
</style>
