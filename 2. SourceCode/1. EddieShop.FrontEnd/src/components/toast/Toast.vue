<template lang="">
    <div v-show="toastMessageState" class="toast-message" :class="{'toast-message--error': true}">
        <div class="toast-message__main">
            <div class="toast-message__check"></div>
            <span class="toast-message__content">{{message }}</span>
        </div> 
        <div @click="toastMessageState = false" class="toast-message__cancel"></div>   
    </div>
</template>
<script>
export default {
    name: 'Toast',
    data() {
        return {
            message: 'Chỉnh sửa thành công',
            toastMessageState: false
        }
    },
    created() {
        this.$bus.$on('showToastMessage', (data) => {
            this.message = data.message;
            this.toastMessageState = true;
            setTimeout(() => {
                this.toastMessageState = false;
            }, data.duration)
        })
    }
}
</script>
<style lang="scss">
    @import '@/assets/scss/components/toast/toast.scss'; 
</style>