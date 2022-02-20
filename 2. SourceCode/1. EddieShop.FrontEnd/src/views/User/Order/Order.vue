<template lang="">
  <base-content-area
    class="jus-c-center"
    title="Đơn hàng của bạn"
    pHor="20px"
    bgColor="#fff"
  >
    <template v-slot:content>
      <EdFrame class="h-full fx-col" autoScroll>
        <EdListGridAdvance
          class="flex-1 w-full"
          :listHeader="listHeader"
          :pagingInfo="pagingInfo"
          :showSearch="false"
          :showCheck="false"
          :showPaging="false"
          :editRow="false"
          :deleteRow="false"
          @changePagingFilter="changePagingFilter"
          @dblClick="dblclick"
          v-model="listOrder"
          itemID="_OrderID"
        >
        <template v-slot:Status="{ row }">
            <div
              class="status-table"
              :class="{
                'status-table--success': row.Status == 3,
                'status-table--error': row.Status == 4,
                'status-table--grey': row.Status == 0,
                'status-table--warn': row.Status == 1,
                'status-table--blue': row.Status == 2
              }"
            >
              <div class="status-table__icon m-r-10 mi-14"></div>
              <div v-if="row.Status == 0" class="status-table__msg">
                Đang chờ duyệt
              </div>
              <div v-if="row.Status == 1" class="status-table__msg">
                Đã được duyệt, chờ vận chuyển
              </div>
              <div v-if="row.Status == 2" class="status-table__msg">
                Đang vận chuyển
              </div>
              <div v-if="row.Status == 3" class="status-table__msg">
                Giao hàng thành công
              </div>

              <div v-if="row.Status == 4" class="status-table__msg">
                Đơn hàng bị huỷ
              </div>
            </div>
          </template>
        </EdListGridAdvance>
        <OrderDetail v-if="showOrderDetail" @close="showOrderDetail = false" :orderInfo="orderInfo"/>
      </EdFrame>
    </template>
  </base-content-area>
</template>
<script>
import OrderDetail from "@/views/User/Order/OrderDetail.vue";
// Plugins
import OrderAPI from "@/api/components/Order/OrderAPI.js";
export default {
  name: "Order",
  components: {
    OrderDetail
  },
  data() {
    return {
      orderInfo: "",
      pagingInfo: {
        filterString: "",
        pageNum: 1,
        pageSize: 10,
        filterData: {
          TotalFields: [],
          Sorts: []
        }
      },
      listHeader: [
        {
          title: "Mã đơn hàng",
          field: "OrderCode",
          type: "text",
          width: 130,
          headerPos: "left",
          dataPos: "left",
          pin: true
        },
        {
          title: "Mô tả",
          field: "Description",
          width: 500,
          type: "text"
        },
        {
            title: "Tổng tiền",
            field: "TotalPrice",
            type: "money",
            width: 200,
            dataPos: "right"
        },
        {
            title: "Số lượng",
            field: "Quantity",
            type: "number",
            width: 100,
            dataPos: "right"
        },
        {
          title: "Ngày tạo",
          field: "CreatedDate", 
          width: 200,
          type: "datetime"
        },
         {
          // 0 - Hết hàng, 1 - Vửa xinh, 2 - Hàng khả dụng có thể chọn thêm, 3 - Không đủ hàng
          title: "Trạng thái",
          field: "Status",
          type: "custom",
          width: 150,
          headerPos: "left",
          dataPos: "left"
        },
      ],
      showOrderDetail: false,
      listOrder: []
    };
  },
  mounted() {
    this.getOrderData();
  },
  methods: {
    /**
     * Lấy dữ liệu đơn hàng
     * CreatedBy: NTDUNG (09/01/2021)
     */
    getOrderData() {
      OrderAPI.GetFilterPaging(
        this.pagingInfo.filterString,
        this.pagingInfo.pageNum,
        this.pagingInfo.pageSize,
        this.pagingInfo.filterData
      )
        .then(res => {
          if (res.data.Success) {
            this.listOrder = res.data.Data.Records;
            this.pagingInfo.totalPage = res.data.Data.TotalPage;
            this.pagingInfo.totalRecord = res.data.Data.TotalRecord;
            this.pagingInfo.totalData = res.data.Data.TotalDatas;
          }
        })
        .catch(err => {
          console.log(err);
          this.$toast.error("Có lỗi xảy ra");
        });
    },
    /**
     * Paging filter thay đổi
     * CreatedBy: NTDUNG (21/12/2021)
     */
    changePagingFilter(pageNum, pageSize, filterString) {
      this.pagingInfo.pageNum = pageNum;
      this.pagingInfo.pageSize = pageSize;
      this.pagingInfo.filterString = filterString;
      this.getOrderData();
    },
    dblclick(item) {
      this.orderInfo = item;
      this.showOrderDetail = true;
    }
  }
};
</script>
<style lang=""></style>
