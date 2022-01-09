<template lang="">
  <form @submit.prevent="handleSubmit">
    <EdPopup
      :title="mode ? 'Chỉnh sửa sản phẩm' : 'Thêm sản phẩm'"
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
              label="Tên sản phẩm"
              :rules="{ required: true }"
              :errMsg="errors.first('ProductName')"
              v-model="value.ProductName"
            />
          </ed-col>
          <ed-col :colLg="6" :colXl="6" :colXs="6" :colSm="12">
            <ed-label required value="Mã sản phẩm" />
            <ed-input
              name="ProductCode"
              :disabled="mode"
              :rules="{ required: true, max: 20 }"
              :errMsg="errors.first('ProductCode')"
              v-model="value.ProductCode"
            />
          </ed-col>
          <ed-col :colLg="4" :colXl="4" :colXs="4" :colSm="12">
            <ed-label required value="Đơn giá" />
            <ed-input
              name="Price"
              :rules="{ required: true, max: 10 }"
              :errMsg="errors.first('Price')"
              type="number"
              v-model="value.Price"
            />
          </ed-col>
          <ed-col :colLg="2" :colXl="2" :colXs="2" :colSm="12">
            <ed-label required value="Số lượng" />
            <ed-input
              name="Quantity"
              :rules="{ required: true, max: 3 }"
              :errMsg="errors.first('Quantity')"
              type="number"
              v-model="value.Quantity"
            />
          </ed-col>
          <ed-col :colLg="6" :colXl="6" :colXs="6" :colSm="12">
            <ed-label value="Tổng tiền" />
            <ed-input
              type="number"
              disabled
              :value="
                Number(
                  value.Quantity * value.Price
                    ? value.Quantity * value.Price
                    : 0
                )
              "
            />
          </ed-col>
          <ed-col :colLg="3" :colXl="3" :colXs="3" :colSm="12">
            <ed-label required value="Hình ảnh" />
            <ed-input-file
              name="Image"
              :rules="{ required: true }"
              :errMsg="errors.first('Image')"
              v-model="value.Image"
            />
          </ed-col>
          <ed-col :colLg="9" :colXl="9" :colXs="8" :colSm="12">
            <ed-label required value="Mô tả" />
            <ed-textarea
              name="Description"
              :errMsg="errors.first('Description')"
              :rules="{ required: true }"
              v-model="value.Description"
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
            :label="mode ? 'Chỉnh sửa' : 'Thêm mới'"
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
  props: {
    mode: {
      type: Boolean,
      default: false
    },
    value: {
      type: Object,
      default: () => {}
    }
  },
  data() {
    return {
      // productInfo: {},
      productInfoClone: {},
      gender: 1    
    };
  },
  created() {
    if (!this.mode) this.getNewProductCode();
    else this.productInfoClone = { ...this.value };
  },
  methods: {
    /**
     * Lấy mã sản phẩm mới
     * CreatedBy: NTDUNG (15/12/2021)
     */
    getNewProductCode() {
      ProductAPI.GetNewCode()
        .then(res => {
          if (res.data.Success) {
            var data = { ...this.value };
            this.$set(data, "ProductCode", res.data.Data);
            this.$emit("input", data);
            this.productInfoClone = { ...data };
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
      if (this.deepEqualObject(this.value, this.productInfoClone)) {
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
          // Bind giá trị bổ sung trước khi submit
          this.preSubmit();
          if (!this.mode)
            if (!this.deepEqualObject(this.value, this.productInfoClone))
              this.$emit("addProduct");
            else this.$emit("close");
          else if (!this.deepEqualObject(this.value, this.productInfoClone))
            this.$emit("editProduct");
          else this.$emit("close");
        } else {
          this.$toast.error(this.errors.items[0].msg);
          document.querySelector(`#${this.errors.items[0].field}`).focus();
        }
      });
    },
    /**
     * Tiền xử lý
     * CreatedBy: NTDUNG (21/12/2021)
     */
    preSubmit() {
      var data = { ...this.value };
      data.TotalPrice = data.Price * data.Quantity;
      this.$emit("input", data);
    }
  }
};
</script>
<style lang=""></style>
