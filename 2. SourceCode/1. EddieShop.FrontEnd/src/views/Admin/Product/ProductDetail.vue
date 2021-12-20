<template lang="">
  <base-content-area
    class="productdetail"
    :title="ProductDetailData.ProductName"
    pHor="100px"
    bgColor="#fff"
  >
    <template v-slot:content>
      <EdFrame :gutH="12">
        <EdRow>
          <EdCol :colLg="4" :colXl="6" :colXs="12" :colSm="12">
            <div
              @click="showImg(0)"
              class="productdetail__img cur-p"
              :style="{
                'background-image': `url(data:image/gif;base64,${
                  ProductDetailData.Image ? ProductDetailData.Image : ''
                }`
              }"
            ></div>
          </EdCol>
          <EdCol :colLg="8" :colXl="6" :colXs="12" :colSm="12" class="fx-col jus-c-sbtn">
            <div class="productdetail__price">
              {{ formatMoney(ProductDetailData.Price) }}
            </div>
            <div v-if="ProductDetailData.OldPrice" class="productdetail__oldprice">
              {{ formatMoney(ProductDetailData.OldPrice) }}
            </div>
            <div class="productdetail__status fx-row aln-i-base">
              <ed-label :value="$t('i18nProduct.Status.Status') + ' :'" />
              <span class="txt-smb-2 txt-s-22 m-l-10">{{
                ProductDetailData.Quantity > 0
                  ? $t("i18nProduct.Status.Stocking")
                  : $t("i18nProduct.Status.OutOfStock")
              }}</span>
            </div>
            <div class="fx-row aln-i-fend">
              <ed-label value="Số lượng :" class="m-r-16" />
              <span class="txt-smb-1 txt-s-20">{{ProductDetailData.Quantity}}</span>  
            </div>
          </EdCol>
        </EdRow>
        <vue-easy-lightbox
          :visible="visible"
          :imgs="[{src: `data:image/gif;base64,${ProductDetailData.Image ? ProductDetailData.Image : ''}`, title: ProductDetailData.Title}]"
          :index="0"
          @hide="handleHide"
          @on-prev-click="prevImage"
          @on-next-click="nextImage"
        ></vue-easy-lightbox>
      </EdFrame>
    </template>
  </base-content-area>
</template>
<script>
// Plugins
import ProductAPI from "@/api/components/Product/ProductAPI.js";
import {AccountType} from "@/models/enum/AccountType.js"

export default {
  name: "ProductDetail",
  data() {
    return {
      ProductID: null,
      ProductDetailData: {},
      ProductNum: 1,
      visible: false
    };
  },
  mounted() {
    this.ProductID = this.$route.params.ProductID;
    this.getProductDetail();
  },
  methods: {
    /**
     * Lấy thông tin chi tiết sản phẩm
     * CreatedBy: NTDUNG (01/12/2021)
     */
    getProductDetail() {
      ProductAPI.getById(this.ProductID)
        .then(res => {
          this.ProductDetailData = res.data.Data;
        })
        .catch(err => {
          console.log(err); 
        });
    },
    /**
     * Xử lý đặt hàng
     * CreatedBy: NTDUNG (01/12/2021)
     */
    handleOrder() {
      alert("Order");
    },
    handleHide() {
      this.visible = false;
    },
    showImg() {
      this.visible = true;
    }, 
    prevImage(oldIdx, newIdx) {
      alert(oldIdx + ':' + newIdx);
    },
    nextImage(oldIdx, newIdx) {
      alert(oldIdx + ':' + newIdx);
    }
  }
};
</script>
<style lang="scss">
@import "@/assets/scss/views/user/product/productdetail.scss";
/* >= Tablet */
@media (max-width: 740px) {
  .productdetail {
    padding: 0 10px !important;
  }
}
/* PC medium resolution > */
@media (min-width: 1113px) {
}
/* Tablet - PC low resolution */
@media (min-width: 740px) and (max-width: 1023px) {
  .productdetail {
    padding: 0 50px !important;
  }
}
/* > PC low resolution */
@media (min-width: 1024px) and (max-width: 1239px) {
}
/* PC height resolution */
@media (min-width: 1240px) {
}
</style>
