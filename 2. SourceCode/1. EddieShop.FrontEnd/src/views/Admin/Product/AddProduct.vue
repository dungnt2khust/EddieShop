<template lang="">
  <EdPopup
    title="Thêm sản phẩm"
    @close="$emit('close')"
    :active="true"
    maxWidth="700px"
  >
    <template v-slot:content>
      <EdFrame :gutH="8" :gutV="8" class="m-t-20">
        <ed-col :colLg="6" :colXl="6" :colXs="6" :colSm="12">
          <EdRadio label="Giới tính" :options="['Nam', 'Nữ', 'Khác']" v-model="gender"/>
          <ed-label required value="Tên sản phẩm" />
          <ed-input
            required
            :autoFocus="true"
            v-model="productInfo.ProductName" 
          />
        </ed-col>
        <ed-col :colLg="6" :colXl="6" :colXs="6" :colSm="12">
          <ed-label required value="Mã sản phẩm" />
          <ed-input required v-model="productInfo.ProductCode" />
        </ed-col>
        <ed-col :colLg="6" :colXl="6" :colXs="6" :colSm="12">
          <ed-label required value="Đơn giá" />
          <ed-input required isNumber v-model="productInfo.Price" />
        </ed-col>
        <ed-col :colLg="6" :colXl="6" :colXs="6" :colSm="12">
          <ed-label required value="Số lượng" />
          <ed-input required isNumber v-model="productInfo.Quantity" />
        </ed-col>
        <ed-col :colLg="3" :colXl="3" :colXs="6" :colSm="12">
          <ed-label required value="Hình ảnh" />
          <ed-input-file required v-model="productInfo.Image" />
        </ed-col>
        <ed-col :colLg="9" :colXl="9" :colXs="6" :colSm="12">
          <ed-label required value="Mô tả" />
          <ed-textarea required v-model="productInfo.Description" />
        </ed-col>
      </EdFrame>
    </template>
    <template v-slot:footer>
      <div class="fx-row jus-c-fend">
        <ed-button label="Huỷ" :type="0" :method="handleClose" />
        <ed-button
          :method="addProduct"
          class="m-l-10"
          label="Thêm mới"
          :type="1"
          bgColor="#5DCE00"
        />
      </div>
    </template>
  </EdPopup>
</template>
<script>
// Plugins
import ProductAPI from "@/api/components/Product/ProductAPI.js";

export default {
  name: "AddProduct",
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
        alert(" Dữ liệu đã thay đổi. Bạn có muốn thêm mới luôn");
      }
    },
    /**
     * Tạo mới sản phẩm
     * CreatedBy: NTDUNG (15/12/2021)
     */
    addProduct() {
      ProductAPI.post(this.productInfo)
        .then(res => {
          console.log(res);
          this.$emit('close');
        })
        .catch(err => {
          console.log(err);
          alert('Thêm mới thất bại');
        });
    }
  }
};
</script>
<style lang=""></style>
