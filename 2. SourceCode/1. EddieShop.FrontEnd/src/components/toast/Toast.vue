<template lang="">
    <div v-show="toastMessageState" class="toast-message" :class="type ? 'toast-message--' + type : ''">
        <div class="toast-message__main">
            <div class="toast-message__icon">
                <div v-if="type == ToastType.SUCCESS" class="mi-success scale-0.6"></div>
                <div v-if="type == ToastType.WARN" class="mi-warn scale-0.6"></div>
                <div v-if="type == ToastType.INFO" class="mi-info scale-0.6"></div>
                <div v-if="type == ToastType.ERROR" class="mi-error scale-0.6"></div>
            </div>
            <span class="toast-message__content">{{message}}</span>
        </div> 
        <div @click="toastMessageState = false" class="toast-message__cancel"></div>   
    </div>
</template>
<script>
// Plugins 
import {ToastType} from "@/models/enum/ToastType.js"

export default {
    name: 'Toast',
    data() {
        return {
            message: '',
            toastMessageState: false,
            type: '',
            ToastType: ToastType
        }
    },
    created() {
        this.$bus.$on('showToastMessage', (data) => {
            this.message = data.message;
            this.type = data.type;
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