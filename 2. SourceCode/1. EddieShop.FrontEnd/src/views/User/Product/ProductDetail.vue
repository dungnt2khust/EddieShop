<template lang="">
  <base-content-area
    class="productdetail"
    :title="ProductDetailData.ProductName"
    pHor="100px"
    bgColor="#fff"
  >
    <template v-slot:content>
      <div class="fx-wrap gut-h-12">
        <EdRow>
          <EdCol :colLg="4" :colXl="6" :colXs="12" :colSm="12">
            <div
              @click="showImg"
              class="productdetail__img"
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
            <div class="productdetail__oldprice">
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
              <ed-number
                v-model="ProductNum"
                :min="0"
                :max="10"
                :disable="ProductDetailData.Quantity <= 0"
              />
              <div
                v-tooltip="'Thêm vào giỏ hàng'"
                class="txt-b-2 txt-s-30 cur-p hov-act-d m-l-16"
              >
                <i class="fas fa-cart-plus"></i>
              </div>
            </div>
            <ed-button
              :tooltip="$t('i18nOrder.Ordering')"
              :method="handleOrder"
              :disable="ProductDetailData.Quantity <= 0"
              :label="
                ProductDetailData.Quantity > 0
                  ? $t('i18nOrder.Ordering')
                  : $t('i18nProduct.Status.OutOfStock')
              "
              :subLabel="
                ProductDetailData.Quantity > 0
                  ? $t('i18nOrder.OrderNow')
                  : $t('i18nProduct.Status.ThisProductOutOfStock')
              "
              :styleBtn="3"
              txtPos="center"
            />
          </EdCol>
        </EdRow>
        <vue-easy-lightbox
          :visible="showPreview"
          :imgs="[{src: `data:image/gif;base64,${ProductDetailData.Image ? ProductDetailData.Image : ''}`, title: ProductDetailData.Title}]"
          :index="0"
          @hide="handleHide"
          @on-prev-click="prevImage"
          @on-next-click="nextImage"
        ></vue-easy-lightbox>
      </div>
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
      showPreview: false
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
      this.showPreview = false;
    },
    showImg() {
      this.showPreview = true;
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
