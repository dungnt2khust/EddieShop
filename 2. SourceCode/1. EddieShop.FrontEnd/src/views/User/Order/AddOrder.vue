<template lang="">
  <div class="add-order">
    <EdPopup
      title="Đặt hàng"
      @close="$emit('close')"
      :active="true"
      :contentFix="true"
      minWidth="700px"
      minHeight="500px"
      maxHeight="80%"
    >
      <template v-slot:content>
        <EdListGridAdvance
          :showSearch="false"
          :showCheck="false"
          :showPaging="false"
          :editRow="false"
          :listHeader="listHeader"
          @Delete="deleteCart"
          v-model="products"
        >
          <template v-slot:header>
            <div class="w-full txt-smb-1 txt-s-18 m-t-10">
              Số sản phẩm trong đơn hàng: {{ products.length }}
              <div class="m-t-20">
                <EdLabel value="Mô tả" />
                <EdTextarea v-model="description" :row="2"/>
              </div>
            </div>
          </template>
          <template v-slot:Status="{ row }">
            <div
              class="status-table"
              :class="{
                'status-table--success': row.Status == 2,
                'status-table--error': row.Status == 0 || row.Status == 3,
                'status-table--warn': row.Status == 1
              }"
            >
              <div class="status-table__icon m-r-10 mi-14"></div>
              <div v-if="row.Status == 0" class="status-table__msg">
                Hết hàng
              </div>
              <div v-if="row.Status == 1" class="status-table__msg">
                Hàng vừa đủ, hãy mua ngay
              </div>
              <div v-if="row.Status == 2" class="status-table__msg">
                Hàng đang còn
              </div>
              <div v-if="row.Status == 3" class="status-table__msg">
                Hàng không đủ
              </div>
            </div>
          </template>
        </EdListGridAdvance>
      </template>
      <template v-slot:footer>
        <div class="fx-row jus-c-fend">
          <ed-button :method="closeAddProduct" class="m-r-10" label="Huỷ" />
          <ed-button
            :method="handleOrder"
            label="Đặt hàng"
            :styleBtn="1"
            bgColor="#5DCE00"
          />
        </div>
      </template>
    </EdPopup>
  </div>
</template>
<script>
// Plugins
import OrderAPI from "@/api/components/Order/OrderAPI.js";
export default {
  name: "AddOrder",
  props: {
    products: {
      type: Array,
      default: () => [{}]
    }
  },
  data() {
    return {
      totalPrice: 0,
      quantity: 0,
      description: "",
      listHeader: [
        {
          title: "Tên sản phẩm",
          field: "ProductName",
          type: "text",
          width: 150,
          headerPos: "left",
          dataPos: "left",
          pin: true
        },
        {
          title: " Hình ảnh",
          field: "Image",
          type: "image",
          width: 100,
          height: "100px",
          pin: true
        },
        {
          title: "Giá",
          field: "Price",
          type: "money",
          width: 120,
          headerPos: "right",
          dataPos: "right",
          pin: true
        },
        {
          title: "Số lượng",
          field: "CartQuantity",
          type: "number",
          width: 100,
          min: 1,
          headerPos: "right",
          dataPos: "right",
          total: true
        },
        {
          title: "Tổng tiền",
          field: "CartTotalPrice",
          type: "money",
          width: 120,
          headerPos: "right",
          dataPos: "right",
          total: true
        },
        {
          // 0 - Hết hàng, 1 - Vửa xinh, 2 - Hàng khả dụng có thể chọn thêm, 3 - Không đủ hàng
          title: "Trạng thái",
          field: "Status",
          type: "custom",
          width: 150,
          headerPos: "left",
          dataPos: "left"
        }
      ]
    };
  },
  methods: {
    /**
     * Tắt form đặt hàng
     * CreatedBy: NTDUNG (04/01/2021)
     */
    closeAddProduct() {
      this.$dialog.confirm("Bạn có muốn huỷ đặt hàng không?", {
        YES: () => {
          this.$emit("close");
        }
      });
    },
    /**
     * Xoá sản phẩm trong giỏ khỏi đơn hàng
     * CreatedBy: NTDUNG (04/01/2021)
     */
    deleteCart(cart) {
      this.$dialog.confirm("Bạn có chắc muốn xoá sản phẩm khỏi đơn hàng?", {
        YES: () => {
          this.$emit("deleteItem", cart.CartID);
        }
      });
    },
    /**
     * Xử lý đặt hàng
     * CreatedBy: NTDUNG (07/01/2021)
     */
    handleOrder() {
      var stokedProduct = this.products.filter(product => product.Status == 0);
      var notEnoughProduct = this.products.filter(
        product => product.Status == 3
      );

      if (stokedProduct.length) {
        this.$toast.error(
          "Tồn tại sản phẩm đã hết hàng. Vui lòng kiểm tra lại."
        );
      } else if (notEnoughProduct.length) {
        this.$toast.error(
          "Tồn tại sản phẩm không đủ hàng. Vui lòng kiểm tra lại."
        );
      } else
        this.$dialog.confirm("Bạn đã chắc chắn muốn đặt hàng?", {
          YES: () => {
            var order = {
              UserID: this._getLocalStorageNotParse("AccountID"),
              Description: this.description,
              Status: 0,
              Carts: this.products,
              CreatedBy: this._getLocalStorage("AccountData").DisplayName,
              Quantity: this.quantity,
              TotalPrice: this.totalPrice
            }
            OrderAPI.add(order)
              .then(res => {
                console.log(res);
                this.$toast.success("Đặt hàng thành công");
                this.$emit("close")
              })
              .catch(err => {
                console.log(err);
                this.$toast.error("Đặt hàng thất bại");
              });
          }
        });
    }
  },
  watch: {
    products: {
      handler(val) {
        this.totalPrice = 0;
        this.quantity = 0;
        val.forEach(row => {
          if (row.CartQuantity)
            this.quantity += row.CartQuantity
          if (row.CartTotalPrice)
            this.totalPrice = row.CartTotalPrice;
        })
      },
      immediate: true,
      deep: true
    }
  }
};
</script>
<style lang=""></style>
