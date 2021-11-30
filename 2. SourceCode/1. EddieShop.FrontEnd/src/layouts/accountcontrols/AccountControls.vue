<template lang="">
  <div class="accountcontrols pos-relative">
    <div class="accountcontrols__icon fx-center cur-p">
      <i class="fas fa-user-circle"></i>
      <ed-blur id="accountcontrols" v-model="showAccountControls"/>
    </div>
    <BaseContentFrame 
      v-if="showAccountControls"
      tabindex="0"
      class="blurable pos-absolute p-10"
      width="220px"
      top="100%"
      right="0"
      border="1px solid #ccc"
    >
      <ed-row class="fx-center">
        <div class="accountcontrols__avatar">
          <i class="fas fa-user-circle"></i>
        </div>
      </ed-row>
      <ed-row class="fx-center">
        <div class="accountcontrols__name">
          <span>{{ _getLocalStorage('AccountData').DisplayName }}</span>
        </div>
      </ed-row>
      <ed-row class="m-t-30 fx-center">
        <ed-button
          v-if="_getLocalStorage('AccountData').DisplayName"
          label="Đăng xuất"
          @click.native="logout"
          :type="1"
        />
        <div v-else>
          <ed-button 
              label="Đăng nhập" 
              @click.native="login"
              :type="2" />
          <ed-button 
              label="Đăng kí" 
              @click.native="register"
              :type="1" />
        </div>
      </ed-row>
    </BaseContentFrame>
  </div>
</template>
<script>
// Plugins
import {AccountType} from "@/models/enum/AccountType.js"

export default {
  name: "Account",
  data() {
    return {
      showAccountControls: false
    };
  },
  methods: {
    /**
     * Đăng xuất
     * CreatedBy: NTDUNG (23/11/2021)
     */
    logout() {
      this.$router.push("/login");
    },
    /**
     * Đăng nhập
     * CreatedBy: NTDUNG (30/11/2021)
     */
    login() {
      this.$router.push("/login");
    },
    /**
     * Đăng kí
     * CreatedBy: NTDUNG (30/11/2021)
     */
    register() {
      this.$router.push("/register");
    }
  }
};
</script>
<style lang="scss">
@import "@/assets/scss/layouts/accountcontrols/accountcontrols.scss";
</style>
