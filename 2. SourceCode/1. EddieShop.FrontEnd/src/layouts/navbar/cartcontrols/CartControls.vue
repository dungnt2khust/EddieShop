<template lang="">
  <div class="cartcontrols pos-relative" @click="toggleCart" v-click-outside="hideCart">
    <ed-icon
      class="pos-relative"
      :size="30"
      :tooltip="$t('i18nCart.Cart')"
    > 
      <span class="txt-b-0 txt-s-24 m-r-20"
        ><i class="fas fa-shopping-cart"></i
      ></span>
      <ed-icon
        v-if="listCart.length"
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
        {{ listCart.length }}
      </ed-icon>
    </ed-icon>
    <BaseContentFrame
      v-show="showCart"
      class="pos-absolute p-10 fx-col aln-i-sbtn"
      :stopPropagation="true"
      tabindex="0"
      width="300px"
      minHeight="200px"
      top="100%"
      right="0"
      boxShadow="0px 4px 10px rgba(0,0,0, 0.2)"
      :zIndex="50"
    >
      <template v-slot:content>
        <div class="list-notify default-scrollbar">
          <div
            v-for="(cart, index) in listCart.slice().reverse()"
            class="list-notify__item m-r-10 p-l-10"
            :key="index"
          >
            <div class="list-notify__title m-r-10">{{ cart.Title }}</div>
            <div class="list-notify__content">{{ cart.Content }}</div>
          </div>
          <div v-if="!listCart.length">
            {{ $t("i18nCart.NotHaveProduct") }}
          </div>
        </div>
      </template>
      <template v-slot:footer>
        <ed-row class="fx-row jus-c-sbtn">
          <ed-button :label="$t('i18nCart.Checkout')" :method="callDialog" :styleBtn="3" />
          <ed-button :label="$t('i18nCart.Cart')" :styleBtn="3" />
        </ed-row>
      </template>
    </BaseContentFrame>
  </div>
</template>
<script>
export default {
  name: "CartControls",
  data() {
    return {
      listCart: [],
      showCart: false
    };
  },
  methods: {
    /**
     * Bật tắt giỏ hàng
     * CreatedBy: NTDUNG (13/12/2021)
     */
    toggleCart() {
      this.showCart = !this.showCart;
    },
    hideCart() {
      this.showCart = false;
    },
    callDialog() {
      this.$toast.warn(" Chức năng đang phát triển");
    }
  }
};
</script>
<style lang="scss"></style>
