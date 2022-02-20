<template lang="">
  <div class="notify pos-relative" @click="showNotify = !showNotify" v-click-outside="hideNotify">
    <ed-icon
      :size="30"
      iconCls="mi-notify-white"
      :tooltip="$t('i18nNotify.Notify')"
    >
      <ed-icon
        v-if="listNotify.length"
        style="z-index: 0"
        class="pos-absolute"
        bgColor="red"
        top="0"
        right="0"
        translate="10%, -10%"
        borderRad="8px"
        txtSize="12px"
        txtColor="#fff"
        :size="18"
      >
        {{ listNotify.length }}
      </ed-icon>
    </ed-icon>
    <BaseContentFrame
      v-show="showNotify"
      class="pos-absolute p-10"
      :stopPropagation="true"
      tabindex="0"
      width="300px"
      height="400px"
      top="100%"
      right="0"
      boxShadow="0px 4px 10px rgba(0,0,0, 0.2)"
      :zIndex="50"
    >
      <template v-slot:content>
        <div class="list-notify default-scrollbar">
          <div
            v-for="(notify, index) in listNotify.slice().reverse()"
            class="list-notify__item m-r-10 p-l-10"
            :key="index"
            @click="clickToNotify"
          >
            <div class="list-notify__title m-r-10">{{ notify.Title }}</div>
            <div class="list-notify__content m-t-10">{{ notify.Content }}</div>
          </div>
          <div v-if="!listNotify.length">
            Không có thông báo
          </div>
        </div>
      </template>
    </BaseContentFrame>
  </div>
</template>
<script>
export default {
  name: "NotifyControls",
  data() {
    return {
      showNotify: false,
      listNotify: []
    };
  },
  mounted() {
    // KhởI tạo hàm nhận thông báo
    this.receiveNotify();
  },
  methods: {
    /**
     * Nhận thông báo
     * CreatedBy: NTDUNG (24/11/2021)
     */
    receiveNotify() {
      this.$SignalR.on("ReceiveNotify", (user, notify) => {
        if (notify.Code == "NEWORDER")
          this.listNotify.push({
            User: user,
            Title: `Đơn hàng mới từ ${user.DisplayName} - ${user.Code}`,
            Content: `Mã đơn hàng: ${notify.Data.OrderCode}` 
          });
        else {
          this.listNotify.push({
            User: user,
            Title: notify.Title,
            Content: notify.Content
          })
        }
      });
    },
    hideNotify() {
      this.showNotify = false;
    },
    clickToNotify() {
      if (this.$route.path.includes('order')) {
        this.$router.go(0)
      } else {
        this.$router.push('/order')
      }
    }
  }
};
</script>
<style lang="scss">
@import "@/assets/scss/layouts/navbar/notify/notify.scss";
</style>
