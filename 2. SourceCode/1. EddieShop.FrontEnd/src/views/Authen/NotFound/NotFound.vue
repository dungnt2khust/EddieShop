<template lang="">
  <div class="not-found">
    <FullScreen class="jus-c-center aln-i-center">
      <BaseContentFrame :title="$t('i18nMenu.Authen.NotFound')" height="fit-content" width="70%">
        <template v-slot:header>
          <div class="fx-row jus-c-fend">
            <ed-logo txtColor="#fff" txtSize="24px" bgColor="#12007B" />
          </div>
        </template>
        <template v-slot:content>
          <ed-row class="m-t-20">
            <span>
              {{ $t("i18nNotify.NotFound") }}
            </span>
          </ed-row>
          <ed-row class="m-t-20">
            <ed-button
              :label="$t('i18nNotify.BackToHome')"
              @click.native="back"
              :styleBtn="2"
            />
          </ed-row>
        </template>
      </BaseContentFrame>
    </FullScreen>
  </div>
</template>
<script>
// Plugins
import { AccountType } from "@/models/enum/AccountType.js";

export default {
  name: "NotFound",
  methods: {
    /**
     * Trở lại
     * CreatedBy: NTDUNG (30/11/2021)
     */
    back() {
      switch (this._getLocalStorage("AccountType")) {
        case AccountType.ADMIN:
          this.$router.push("/admin/dashboard");
          break;
        case AccountType.USER:
        case AccountType.GUEST:
        case AccountType.UNKNOWN:
          this.$router.push("/home");
          break;
      }
    }
  }
};
</script>
<style lang=""></style>
