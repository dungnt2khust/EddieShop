<template lang="">
  <div class="add-order">
    <EdPopup
      :title="`Đơn hàng ${orderInfo.OrderCode}`"
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
          :deleteRow="false"
          :listHeader="listHeader"
          v-model="products"
        >
          <template v-slot:header>
            <div class="w-full m-t-20 text">
              Ngày tạo: {{ formatDateTime(orderInfo.CreatedDate) }}<br />
              Số sản phẩm trong đơn hàng: {{ products.length }}
              <div class="m-t-20">
                <EdLabel value="Mô tả:" />
                <div class="description">{{ orderInfo.Description }}</div>
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
    </EdPopup>
  </div>
</template>
<script>
// Plugins
import OrderAPI from "@/api/components/Order/OrderAPI.js";
import CartAPI from "@/api/components/Cart/CartAPI.js";
export default {
  name: "OrderDetail",
  props: {
    orderInfo: {
      type: Object,
      default: () => ({})
    }
  },
  data() {
    return {
      products: [],
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
        }
      ]
    };
  },
  mounted() {
    this.getOrderData();
  },
  methods: {
    getOrderData() {
      console.log(this.orderInfo)
      CartAPI.GetFilterPaging("", -1, 20, {
        Conditions: [
          {
            FieldName: "OrderID",
            Value: this.orderInfo._OrderID,
            Type: "Guid",
            Method: "Equal"
          }
        ],
        
      }, "Proc_GetCartFilter2")
        .then(res => {
          if (res.data.Success) {
            this.products = res.data.Data.Records;
          }
        })
        .catch(err => {
          console.log(err);
        });
    }
  }
};
</script>
<style lang="scss" scoped>
.text {
  font-family: "Lucida Sans", "Lucida Sans Regular", "Lucida Grande",
    "Lucida Sans Unicode", Geneva, Verdana, sans-serif;
  font-size: 16px;
}
</style>
