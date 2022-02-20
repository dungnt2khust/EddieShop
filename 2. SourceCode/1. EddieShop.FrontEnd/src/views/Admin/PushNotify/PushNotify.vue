<template lang="">
  <BaseContentArea>
    <template v-slot:content>
      <EdFrame :gutH="12" :gutV="12" height="100%" class="jus-c-center">
        <EdCol :colLg="4" :colXl="4" :colXs="6" :colSm="12">
          <BaseContentFrame bgColor="#fff">
            <template v-slot:content>
              <div class="fx-row wrap">
                <ed-row class="fx-col">
                  <ed-label value="Tiêu đề" />
                  <ed-input v-model="title" />
                </ed-row>
                <ed-row class="fx-col">
                  <ed-label value="Nội dung" />
                  <ed-textarea v-model="content" :col="10" :row="5" />
                </ed-row>
                <ed-row>
                  <ed-button
                    label="Send"
                    @click.native="sendNotify"
                    :styleBtn="1"
                  />
                </ed-row>
              </div>
            </template>
          </BaseContentFrame>
        </EdCol>
        <EdCol :colLg="4" :colXl="4" :colXs="6" :colSm="12" class="h-full">
          <ListUser v-model="listUsers" />
        </EdCol>
      </EdFrame>
    </template>
  </BaseContentArea>
</template>
<script>
// Components
import ListUser from "@/layouts/listuser/ListUser.vue";
// Library
import UserAPI from "@/api/components/User/UserAPI.js";

export default {
  name: "PushNotify",
  components: {
    ListUser
  },
  data() {
    return {
      listUsers: [],
      title: "Thông báo",
      content: "Nội dung"
    };
  },
  mounted() {
    // Khởi tạo hàm nhận thông báo từ server
  },
  methods: {
    /**
     * Gửi tin nhắn đến người dùng khác
     * CreatedBy: NTDUNG (14/11/2021)
     */
    sendNotify() {
      var userSent = this._getLocalStorage("AccountData");
      var userReceiveIDs = this.listUsers
        .filter(user => {
          return user.checked == true;
        })
        .map(user => user.UserID);
        console.log(userReceiveIDs)

      if (userReceiveIDs.length)
        this.$SignalR
          .invoke("SendNotifyToUsers", userSent, userReceiveIDs, {
            Title: this.title,
            Content: this.content
          })
          .then(res => {
            this.title = "Thông báo";
            this.content = "Nội dung";
          })
          .catch(error => {
            console.log(error);
          });
      else alert("Bạn phải chọn một người dùng trước khi gửi !!!");
    }
  }
};
</script>
<style lang="scss">
@import "@/assets/scss/views/admin/pushnotify/pushnotify.scss";
</style>
