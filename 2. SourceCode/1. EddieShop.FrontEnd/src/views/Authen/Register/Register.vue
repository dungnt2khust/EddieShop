<template lang="">
  <div class="login">
    <ed-popup title="Đăng kí">
      <template v-slot:header>
        <ed-logo txtColor="#fff" txtSize="24px" bgColor="#12007B" />
      </template>
      <template v-slot:content>
        <div class="fx-wrap p-t-20 p-b-20 gut-8">
          <ed-row class="fx-col">
            <ed-col>
              <ed-label value="Tài khoản" />
              <ed-input v-model="accountName" />
            </ed-col>
          </ed-row>
          <ed-row>
            <ed-col :colW="6">
              <ed-label value="Mật khẩu" />
              <ed-input v-model="password" type="password" />
            </ed-col>
            <ed-col :colW="6">
              <ed-label value="Nhập lại mật khẩu" />
              <ed-input v-model="retypePassword" type="password" />
            </ed-col>
          </ed-row>
          <ed-row class="m-t-20">
            <ed-col class="fx-row">
              <ed-button
                class="m-r-10"
                label="Đăng nhập"
                txtPos="center"
                @click.native="register"
                :type="2"
              >
              </ed-button>
              <ed-button
                class="m-r-10"
                label="Đăng kí"
                @click.native="register"
                :type="1"
              >
              </ed-button>
              <ed-button label="Khách" @click.native="guestMode" :type="0">
              </ed-button>
            </ed-col>
          </ed-row>
        </div>
      </template>
    </ed-popup>
  </div>
</template>
<script>
// Library
import { AccountType } from "@/models/enum/AccountType.js";
import { ConnectionState } from "@/models/enum/ConnectionState.js";

export default {
  name: "Register",
  data() {
    return {
      showPassword: false,
      password: "",
      retypePassword: "",
      accountName: ""
    };
  },
  mounted() {
    // Đóng kết nối cũ nếu có
    if (this.$SignalR.connection.connectionState == ConnectionState.CONNECTED) {
      this.$SignalR.stop();
    }
    // Reset lại thông tin tài khoản
    this._setLocalStorage("AccountData", {});
    this._setLocalStorage("AccountType", AccountType.UNKNOWN);
    this._removeLocalStorage("Session");
  },
  methods: {
    /**
     * Đăng nhập
     * CreatedBy: NTDUNG (22/11/2021)
     */
    register() {
      if (this.password != this.retypePassword)
        alert("Mật khẩu nhập lại không đúng. Vui lòng thử lại !!!");
      else {
      }
    },
    /**
     * Chế độ khách
     * CreatedBy: NTDUNG (30/11/2021)
     */
    guestMode() {
      this.$router.push("/home");
      this._setLocalStorage("AccountData", {});
      this._setLocalStorage("AccountType", AccountType.GUEST);
    }
  }
};
</script>
<style lang="scss">
.test {
  height: 50px;
  width: 100%;
  background: red;
}
</style>
