<template lang="">
  <div
    class="cartcontrols pos-relative"
    @click="toggleCart"
    v-click-outside="hideCart"
  >
    <ed-icon class="pos-relative" :size="30" :tooltip="$t('i18nCart.Cart')">
      <span class="txt-b-0 txt-s-24 m-r-20"
        ><i class="fas fa-shopping-cart"></i
      ></span>
      <ed-icon
        v-if="listCart.filter(cart => cart.Active).length"
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
        {{ listCart.filter(cart => cart.Active).length }}
      </ed-icon>
    </ed-icon>
    <BaseContentFrame
      v-show="showCart"
      class="pos-absolute p-10 fx-col aln-i-sbtn"
      :stopPropagation="true"
      tabindex="0"
      width="300px"
      minHeight="200px"
      maxHeight="400px"
      top="100%"
      right="0"
      boxShadow="0px 4px 10px rgba(0,0,0, 0.2)"
      :zIndex="50"
    >
      <template v-slot:content>
        <div class="list-cart default-scrollbar">
          <div
            v-for="(cart, index) in listCart.slice().reverse()"
            class="list-cart__item p-h-10"
            :class="{'list-cart__unread': cart.Active}"
            :key="index"
          >
            <div class="list-cart__title m-r-10">{{ cart.ProductName }}</div>
            <div class="list-cart__content">{{ cart.CartQuantity }}</div>
          </div>
          <div v-if="!listCart.length">
            {{ $t("i18nCart.NotHaveProduct") }}
          </div>
        </div>
      </template>
      <template v-slot:footer>
        <ed-row class="fx-row jus-c-sbtn m-t-20">
          <ed-button
            :label="$t('i18nCart.Checkout')"
            :method="callDialog"
            :styleBtn="3"
          />
          <ed-button
            :method="goToCart"
            :label="$t('i18nCart.Cart')"
            :styleBtn="3"
          />
        </ed-row>
      </template>
    </BaseContentFrame>
  </div>
</template>
<script>
// Plugins
import CartAPI from "@/api/components/Cart/CartAPI.js";
export default {
  name: "CartControls",
  data() {
    return {
      listCart: [],
      showCart: false
    };
  },
  mounted() {
    this.getCartData();
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
    },
    goToCart() {
      this.redirect("/cart");
      this.showCart = false;
    },
    /**
     * Lấy dữ liệu giỏ hàng
     * CreatedBy: NTDUNG (23/12/2021)
     */
    getCartData() {
      CartAPI.GetFilterPaging("", 1, 10)
        .then(res => {
          if (res.data.Success) {
            this.listCart = res.data.Data.Records;
          }
        })
        .catch(err => {
          this.$toast.error("Có lỗi xảy ra");
          console.log(err);
        });
    }
  },
  watch: {
    showCart: function(val) {
      if (val) {
        this.getCartData();
      }
    }
  }
};
</script>
<style lang="scss">
@import "@/assets/scss/components/cart/listcart.scss";
</style>
