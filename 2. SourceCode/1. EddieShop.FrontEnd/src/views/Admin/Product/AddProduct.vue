<template lang="">
  <form @submit.prevent="handleSubmit">
    <EdPopup
      title="Thêm sản phẩm"
      @close="$emit('close')"
      :active="true"
      maxWidth="700px"
    >
      <template v-slot:content>
        <EdFrame :gutH="8" :gutV="8" class="m-t-20">
          <ed-col :colLg="6" :colXl="6" :colXs="6" :colSm="12">
            <ed-label required value="Tên sản phẩm" />
            <ed-input
              name="ProductName"
              :rules="{required: true}"
              :errMsg="
                errors.has('ProductName') ? 'Trường không được để trống' : ''
              "
              v-model="productInfo.ProductName"
            />
          </ed-col>
          <ed-col :colLg="6" :colXl="6" :colXs="6" :colSm="12">
            <ed-label required value="Mã sản phẩm" />
            <ed-input
              name="ProductCode"
              :rules="{ required: true, max: 20 }"
              :errMsg="
                errors.has('ProductCode') ? 'Trường không được để trống' : ''
              "
              v-model="productInfo.ProductCode"
            />
          </ed-col>
          <ed-col :colLg="6" :colXl="6" :colXs="6" :colSm="12">
            <ed-label required value="Đơn giá" />
            <ed-input
              name="Price"
              :rules="{ required: true, max: 20 }"
              :errMsg="errors.has('Price') ? 'Trường không được để trống' : ''"
              type="number"
              v-model="productInfo.Price"
            />
          </ed-col>
          <ed-col :colLg="6" :colXl="6" :colXs="6" :colSm="12">
            <ed-label required value="Số lượng" />
            <ed-input
              name="Quantity"
              :rules="{ required: true, max: 20 }"
              :errMsg="
                errors.has('Quantity') ? 'Trường không được để trống' : ''
              "
              type="number"
              v-model="productInfo.Quantity"
            />
          </ed-col>
          <ed-col :colLg="3" :colXl="3" :colXs="4" :colSm="12">
            <ed-label required value="Hình ảnh" />
            <ed-input-file
              name="Image"
              :rules="{ required: true }"
              :errMsg="errors.has('Image') ? 'Trường không được để trống' : ''"
              v-model="productInfo.Image"
            />
          </ed-col>
          <ed-col :colLg="9" :colXl="9" :colXs="8" :colSm="12">
            <ed-label required value="Mô tả" />
            <ed-textarea
              name="Description"
              :errMsg="
                errors.has('Description') ? 'Trường không được để trống' : ''
              "
              :rules="{ required: true, max: 20 }"
              v-model="productInfo.Description"
            />
          </ed-col>
        </EdFrame>
      </template>
      <template v-slot:footer>
        <div class="fx-row jus-c-fend">
          <ed-button label="Huỷ" :styleBtn="0" :method="handleClose" />
          <ed-button
            type="submit"
            class="m-l-10"
            label="Thêm mới"
            :styleBtn="1"
            bgColor="#5DCE00"
          />
        </div>
      </template>
    </EdPopup>
  </form>
</template>
<script>
// Plugins
import ProductAPI from "@/api/components/Product/ProductAPI.js";

export default {
  name: "AddProduct",
  provide() {
    return {
      parentValidator: this.$validator
    };
  },
  data() {
    return {
      productInfo: {},
      productInfoClone: {},
      gender: 1
    };
  },
  created() {
    this.getNewProductCode();
  },
  methods: {
    /**
     * Lấy mã sản phẩm mới
     * CreatedBy: NTDUNG (15/12/2021)
     */
    getNewProductCode() {
      ProductAPI.getNewCode()
        .then(res => {
          if (res.data.Success) {
            this.$set(this.productInfo, "ProductCode", res.data.Data);
            this.productInfoClone = { ...this.productInfo };
          }
        })
        .catch(err => {
          console.log(err);
        });
    },
    /**
     * Xử lý khi đóng
     * CreatedBy: NTDUNG (15/12/2021)
     */
    handleClose() {
      if (this.deepEqualObject(this.productInfo, this.productInfoClone)) {
        this.$emit("close");
      } else {
        this.$dialog.confirmCancel(
          " Dữ liệu đã thay đổi. Bạn có muốn thêm mới luôn không?",
          {
            YES: () => {
              this.addProduct();
            },
            NO: () => {
              this.$emit("close");
            }
          }
        );
      }
    },
    /**
     * Xử lý validate
     * CreatedBy: NTDUNG (20/12/2021)
     */
    handleSubmit(e) {
      this.$validator.validate().then(valid => {
        if (valid) {
          this.$emit("addProduct", this.productInfo);
        } else {
          this.$toast.error("invalid form");
        }
      });
    }
  }
};
</script>
<style lang=""></style>
