<template lang="">
  <div class="main">
    <router-view />
  </div>
</template>
<script>

// Library
import {AccountType} from "@/models/enum/AccountType.js"
import {ConnectionState} from "@/models/enum/ConnectionState.js"
import AdminNavbar from '@/router/menu/Admin'
import UserNavbar from '@/router/menu/User'
import GuestNavbar from '@/router/menu/Guest'
import Authen from '@/router/route/Authen'

export default {
  name: "Main",  
  methods: { 
    /**
     * Kiểm tra phiên đăng nhập
     * CreatedBy: NTDUNG (19/11/2021)
     */
    checkSession() {
      console.log("checksession");
      if (this._getLocalStorageNotParse("Session")) {
        this.$account.checkSession()
          .then(res => {
            if (res.data.Data.AccountType) {
              // Gán thông tin tài khoản
              this._setLocalStorage('AccountData', res.data.Data.Data);
              this._setLocalStorage('AccountType', res.data.Data.AccountType);
              // Cập nhật navbar
              this.updateNavbar();
              // Kết nối với serrver
              this.connectServer();
            } else {
              // Cập nhật navbar
              this.updateNavbar();
              // Gán thông tin tài khoản
              this._setLocalStorage('AccountData', {})
              this._setLocalStorage('AccountType', AccountType.UNKNOWN);
              // Chuyển hướng đến login
              this.$router.replace('/login');
            }
          })
          .catch(err => {
            console.log(err);
          })
      } else {
        // Cập nhật navbar
        this.updateNavbar();
        // Gán thông tin tài khoản
        this._setLocalStorage('AccountData', {})
        this._setLocalStorage('AccountType', AccountType.UNKNOWN); 
        // Chuyển hướng đến login
        this.$router.replace('/login');
      }
    },
    /**
     * Kết nối với server
     * CreatedBy: NTDUNG (13/11/2021)
     */
    connectServer() {
      // Kết nối với server
      this.$SignalR
        .start()
        .then(() => {
          // Kết nối thành công => Cập nhật ConnectionID
          this.$SignalR
            .invoke("UpdateConnectionID", this._getLocalStorage('AccountData'))
            .then(res => {
              if (this.$route.path != "/home" && this.$route.path != "/admin/dashboard") {
                switch(this._getLocalStorage('AccountType')) {
                  case AccountType.ADMIN: 
                    this.$router.push("/admin/dashboard");
                    break;
                  case AccountType.USER:
                    this.$router.push("/home");
                }
              }
            });
        })
        .catch(error => {
          // Kết nối thất bại
          console.log(error);
        });
    },
    /**
     * Cập nhật navbar
     * CreatedBy: NTDUNG (23/11/2021)
     */
    updateNavbar() {
      switch(this._getLocalStorage('AccountType')) {
        case AccountType.GUEST: 
          this.$store.state.navBar = GuestNavbar;
          break;
        case AccountType.ADMIN:
          this.$store.state.navBar = AdminNavbar;
          break;
        case AccountType.USER:
          this.$store.state.navBar = UserNavbar;
          break;
      }
    }
  },
  watch: {
    $route: {
      deep: true,
      handler(to, from) {
        // Đặt title
        document.title = this.$t(to.meta.Title); 
        // Kiểm tra session
        if (this.$SignalR.connection.connectionState == ConnectionState.DISCONNECTED && to)
        {
          var isAuthen = Authen.findIndex(route => route.path == to.path);
          if (isAuthen === -1 && this._getLocalStorage('AccountType') !== AccountType.GUEST)
            this.checkSession();
          else 
            this.updateNavbar();
        } 
      }
    }
  }
};
</script>
<style lang="scss">
@import "@/assets/scss/layouts/main/main.scss";
</style>
