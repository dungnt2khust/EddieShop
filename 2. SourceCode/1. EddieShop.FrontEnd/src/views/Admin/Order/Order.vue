<template lang="">
  <base-content-area
    class="jus-c-center"
    title="Đơn hàng"
    pHor="20px"
    bgColor="#fff"
  >
    <template v-slot:content>
      <EdFrame class="h-full fx-col" autoScroll>
        <EdListGridAdvance
          class="flex-1 w-full"
          :listHeader="listHeader"
          :pagingInfo="pagingInfo"
          :editRow="false"
          :editAll="false"
          :deleteRow="false"
          :showCheck="false"
          :haveFooter="false"
          @dblClick="productDetail"
          @changeListCheck="changeListCheck($event)"
          v-model="listOrder"
          itemID="OrderID"
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
          <template v-slot:CreatedBy="{ row }">
            <EdSelectBox
              :default="row.Status"
              :options="listStatus"
              :listField="statusFields"
              bgColor="#5DCE00"
              v-model="row.Status"
              @changeOption="changeStatus($event, row)"
            />
          </template>
        </EdListGridAdvance>
      </EdFrame>
    </template>
  </base-content-area>
</template>
<script>
// Plugins
import OrderAPI from "@/api/components/Order/OrderAPI.js";

export default {
  name: "Order",
  data() {
    return {
      statusFields: [{ field: "name", type: "text" }],
      listStatus: [
        { name: "Đang chờ duyệt", data: 0 },
        { name: "Đã được duyệt, chờ vận chuyển", data: 1 },
        { name: "Đang vận chuyển", data: 2 },
        { name: "Giao hàng thành công", data: 3 },
        { name: "Đơn bị huỷ", data: 4 }
      ],
      listOrder: [],
      listOrderClone: [],
      showAddOrder: false,
      modePopup: false,
      listCheck: [],
      listHeader: [
        {
          title: "Mã đơn hàng",
          field: "OrderCode",
          type: "text",
          width: 150,
          headerPos: "left",
          dataPos: "left",
          pin: true
        },
        {
          title: "Mô tả",
          field: "Description",
          type: "text",
          width: 300,
          height: "100px",
          dataPos: "left",
          pin: true
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
          type: "datetime",
          width: 180,
          headerPos: "right",
          dataPos: "right",
        },
        {
          title: "Người tạo",
          field: "CreatedBy",
          type: "text",
          width: 150,
          headerPos: "right",
          dataPos: "right",
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
        {
          title: "Chuyển trạng thái",
          field: "CreatedBy",
          type: "custom",
          width: 100,
          dataPos: "center"
        }
      ],
      pagingInfo: {
        filterString: "",
        pageNum: 1,
        pageSize: 10,
        filterData: {
          Sorts: [{ Field: "CreatedDate", Desc: true }]
        }
      }
    };
  },
  mounted() {
    this.getOrders();
  },
  methods: {
    /**
     * Đi đến trang chi tiết sản phẩm
     * CreatedBy: NTDUNG (08/12/2021)
     */
    productDetail(cart) {
      this.$router.push(`/product/product-detail/${cart.ProductID}`);
    },
    /**
     * Lấy dữ liệu danh sách sản phẩm
     * CreatedBy: NTDUNG (08/12/2021)
     */
    getOrders() {
      OrderAPI.GetFilterPaging(
        this.pagingInfo.filterString,
        this.pagingInfo.pageNum,
        this.pagingInfo.pageSize,
        this.pagingInfo.filterData,
        "Proc_GetAllOrder"
      )
        .then(res => {
          if (res.data.Success) {
            this.listOrder = res.data.Data.Records;
            this.listOrderClone = JSON.parse(JSON.stringify(this.listOrder));
            this.pagingInfo.totalPage = res.data.Data.TotalPage;
            this.pagingInfo.totalRecord = res.data.Data.TotalRecord;
            this.pagingInfo.totalData = res.data.Data.TotalDatas;
          }
        })
        .catch(err => {
          console.log(err);
        });
    },
    changeStatus(newStatus, row) {
        OrderAPI.UpdateColumns(row._OrderID, {Status: newStatus})
        .then(res => {
            var userSent = this._getLocalStorage("AccountData");
            console.log(this.listStatus.find(i => i.data == newStatus).name)
            this.$toast.success("Cập nhật trạng thái đơn hàng thành công");
            // Gui thong bao cho nguoi dung la da doi trang thai
            this.$SignalR
          .invoke("SendNotifyToUsers", userSent, [row.UserID], {
            Title: "Đơn hàng " + row.OrderCode + " chuyển trạng thái",
            Content: "Đơn hàng đã chuyển sang trạng thái " + this.listStatus.find(i => i.data == newStatus).name
          })
          .then(res => {
          })
          .catch(error => {
            console.log(error);
          });
        }) 
        .catch(err => {
            this.$toast.warn("Cập nhật trạng thái đơn hàng thất bại");
        })
    },
    /**
     * Paging filter thay đổi
     * CreatedBy: NTDUNG (21/12/2021)
     */
    changePagingFilter(pageNum, pageSize, filterString) {
      this.pagingInfo.pageNum = pageNum;
      this.pagingInfo.pageSize = pageSize;
      this.pagingInfo.filterString = filterString;
      this.getOrdersFilterPaging();
    }
  }
};
</script>
<style lang=""></style>
