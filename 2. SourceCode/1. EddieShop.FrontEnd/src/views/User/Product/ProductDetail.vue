<template lang="">
  <base-content-area
    class="productdetail"
    :title="ProductDetailData.ProductName"
    pHor="100px"
    bgColor="#fff"
  >
    <template v-slot:header>
      <div class="h-full fx-row jus-c-fend aln-i-center">
        <div @click="updateLike" class="productdetail__heart cur-p scale-1.4">
          <i
            v-if="ProductDetailData._Like"
            class="product-item__liked fas fa-heart"
          ></i>
          <i
            v-if="!ProductDetailData._Like"
            class="product-item__unliked far fa-heart"
          ></i>
        </div>
      </div>
    </template>
    <template v-slot:content>
      <div class="fx-wrap gut-h-12">
        <EdRow>
          <EdCol :colLg="4" :colXl="6" :colXs="12" :colSm="12">
            <div
              @click="showImg"
              class="productdetail__img"
              :style="{
                'background-image': `url(${
                  ProductDetailData.Image ? ProductDetailData.Image : ''
                }`
              }"
            ></div>
          </EdCol>
          <EdCol
            :colLg="8"
            :colXl="6"
            :colXs="12"
            :colSm="12"
            class="fx-col jus-c-sbtn"
          >
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
                :max="ProductDetailData.Quantity"
                :disable="ProductDetailData.Quantity <= 0"
              />
              <div
                @click="addToCart"
                v-tooltip="'Thêm vào giỏ hàng'"
                class="txt-b-2 txt-s-30 cur-p hov-act-d m-l-16"
              >
                <i class="fas fa-cart-plus"></i>
              </div>
            </div>
            <ed-button
              class="m-t-20"
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
          <EdCol class="m-t-20" :colSm="12" :colLg="12" :colXs="12" :colXl="12">
            <EdLabel value="Mô tả:" />
            <div class="productdetail__description">
              {{ ProductDetailData.Description }}
            </div>
          </EdCol>
          <EdCol
            v-if="!ProductDetailData.Star"
            class="m-t-20"
            :colSm="12"
            :colLg="12"
            :colXs="12"
            :colXl="12"
          >
            <EdLabel value="Đánh giá của bạn" />
            <div class="m-v-10 productdetail__rating">
              <i
                v-for="star in 5"
                class="fas fa-star cur-p"
                :class="{
                  'productdetail__star--gold':
                    star <= tempStar,
                  'productdetail__star--normal':
                    star > tempStar
                }"
                @click="tempStar = star"
                :key="star"
              ></i>
            </div>
            <EdTextarea v-model="ProductDetailData.Comment" />
            <EdButton
              :method="sendComment"
              :styleBtn="1"
              label="Gửi đánh giá"
              bgColor="#444"
            />
          </EdCol>
          <EdCol
            v-else
            class="m-t-20"
            :colSm="12"
            :colLg="12"
            :colXs="12"
            :colXl="12"
          >
            <EdLabel value="Đánh giá của bạn" />
            <div class="productdetail__rating">
              <i
                v-for="starFull in ProductDetailData.Star"
                class="productdetail__star--gold fas fa-star"
                :key="starFull"
              ></i>
              <i
                v-for="starEmpty in 5 - ProductDetailData.Star"
                class="productdetail__star--normal fas fa-star"
              ></i>
            </div>
            <div class="productdetail__comment">
              {{ ProductDetailData.Comment }}
            </div>
          </EdCol>
         
        </EdRow>
        <vue-easy-lightbox
          :visible="showPreview"
          :imgs="[
            {
              src: `${ProductDetailData.Image ? ProductDetailData.Image : ''}`,
              title: ProductDetailData.Title
            }
          ]"
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
import ProductDetailAPI from "@/api/components/ProductDetail/ProductDetailAPI.js";
import CartAPI from "@/api/components/Cart/CartAPI.js";
import { AccountType } from "@/models/enum/AccountType.js";

export default {
  name: "ProductDetail",
  data() {
    return {
      ProductID: null,
      ProductDetailData: {},
      ProductNum: 1,
      showPreview: false,
      inCart: false,
      tempStar: 0
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
      ProductAPI.GetByID(this.ProductID, true)
        .then(res => {
          this.ProductDetailData = res.data.Data;
          this.inCart = this.ProductDetailData["InCart"];
        })
        .catch(err => {
          console.log(err);
        });
    },
    /**
     * Thêm vào giỏ hàng
     * CreatedBy: NTDUNG (23/12/2021)
     */
    addToCart() {
      if (this.inCart) {
        this.$toast.warn(" Sản phẩm đã tồn tại trong giỏ hàng");
      } else {
        this.ProductDetailData["UserID"] = this._getLocalStorageNotParse(
          "AccountID"
        );
        this.ProductDetailData["CartQuantity"] = this.ProductNum;
        this.ProductDetailData["Active"] = 0;
        CartAPI.post(this.ProductDetailData)
          .then(res => {
            console.log(res);
            this.$toast.success("Thêm vào giỏ hàng thành công");
            this.inCart = true;
          })
          .catch(err => {
            console.log(err);
            this.$toast.error("Thêm vào giỏ hàng thất bại");
          });
      }
    },
    /**
     * Cap nhat like san pham
     * created by ntdung 20.02.2022
     */
    updateLike() {
      this.$set(this.ProductDetailData, "_Like", !this.ProductDetailData._Like);
      ProductDetailAPI.GetFilterPaging("", -1, 20, {
        Conditions: [
          {
            FieldName: "UserID",
            Value: this._getLocalStorageNotParse("AccountID"),
            Type: "Guid",
            Method: "Equal"
          },
          {
            FieldName: "ProductID",
            Value: this.ProductDetailData.ProductID,
            Type: "Guid",
            Method: "Equal"
          }
        ]
      })
        .then(res => {
          if (!res.data.Data.Records.length) {
            ProductDetailAPI.post({
              ProductID: this.ProductDetailData.ProductID,
              UserID: this._getLocalStorageNotParse("AccountID"),
              Star: 0,
              _Like: this.ProductDetailData._Like ? 1 : 0
            })
              .then(res => {
                console.log(res);
              })
              .catch(err => {
                console.log(err);
              });
          } else {
            ProductDetailAPI.UpdateColumns(
              res.data.Data.Records[0].ProductDetailID,
              {
                _Like: this.ProductDetailData._Like ? 1 : 0
              }
            )
              .then(res => {
                if (this.ProductDetailData._Like)
                  this.$toast.success("Thêm vào sản phẩm yêu thích");
                else this.$toast.success(" Bỏ yêu thích");
              })
              .catch(err => {
                console.log(err);
              });
          }
        })
        .catch(err => {
          console.log(err);
        });
    },
    /**
     * Gui danh gia
     * created by ntdung 20.02.2022
     */
    sendComment() {
      if (!this.ProductDetailData.Comment || !this.tempStar) {
        this.$toast.warn("Vui lòng điền đủ thông tin khi đánh giá.");
      } else {
        ProductDetailAPI.GetFilterPaging("", -1, 20, {
        Conditions: [
          {
            FieldName: "UserID",
            Value: this._getLocalStorageNotParse("AccountID"),
            Type: "Guid",
            Method: "Equal"
          },
          {
            FieldName: "ProductID",
            Value: this.ProductDetailData.ProductID,
            Type: "Guid",
            Method: "Equal"
          }
        ]
      })
        .then(res => {
          if (!res.data.Data.Records.length) {
            ProductDetailAPI.post({
              ProductID: this.ProductDetailData.ProductID,
              UserID: this._getLocalStorageNotParse("AccountID"),
              Star: this.tempStar,
              _Like: this.ProductDetailData._Like ? 1 : 0, 
              Comment: this.ProductDetailData.Comment
            })
              .then(res => {
                console.log(res);
                this.ProductDetailData.Star = this.tempStar;
              })
              .catch(err => {
                console.log(err);
              });
          } else {
            ProductDetailAPI.UpdateColumns(
              res.data.Data.Records[0].ProductDetailID,
              {
                Star: this.tempStar,
              _Like: this.ProductDetailData._Like ? 1 : 0, 
              Comment: this.ProductDetailData.Comment
              }
            )
              .then(res => {
                this.ProductDetailData.Star = this.tempStar;
                this.$toast.success("Gửi đánh giá thành công")
              })
              .catch(err => {
                console.log(err);
              });
          }
        })
        .catch(err => {
          console.log(err);
        });
      }
      
    },
    /**
     * Xử lý đặt hàng
     * CreatedBy: NTDUNG (01/12/2021)
     */
    handleOrder() {
      this.$toast.warn(" Chức năng đang phát triển");
    },
    handleHide() {
      this.showPreview = false;
    },
    showImg() {
      this.showPreview = true;
    },
    prevImage(oldIdx, newIdx) {
      alert(oldIdx + ":" + newIdx);
    },
    nextImage(oldIdx, newIdx) {
      alert(oldIdx + ":" + newIdx);
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
.product-item__liked {
  color: #f53e2d;
}
.productdetail__comment,
.productdetail__description {
  color: #999;
  font-size: 20px;
  font-family: "Arial Narrow", Arial, sans-serif;
}
.productdetail__star--gold {
  color: #ffce3e;
}
.productdetail__star--normal {
  color: #999;
}
</style>
