<template lang="">
  <base-content-area
    class="jus-c-center"
    title="Giỏ hàng"
    pHor="20px"
    bgColor="#fff"
  >
    <template v-slot:header>
      <div class="fx-row jus-c-fend">
        <ed-button label="Thanh toán" :styleBtn="1" :disabled="listCheck.length <= 0" />
      </div>
    </template>
    <template v-slot:content>
      <EdFrame class="h-full fx-col" autoScroll>
        <EdListGridAdvance
          class="flex-1 w-full"
          :listHeader="listHeader"
          :pagingInfo="pagingInfo"
          :listCheck="listCheck"
          :editRow="false"
          :editAll="true"
          :editAllState="true"
          :totalData="totalData"
          @saveTable="saveTable"
          @dblClick="productDetail"
          @Delete="deleteCart"
          @DeleteMulti="DeleteMulti"
          @changePagingFilter="changePagingFilter"
          @changeListCheck="changeListCheck($event)"
          v-model="listCart"
          itemID="CartID"
        >
          <template v-slot:header>
            <ed-select-box
              class="h-full"
              :options="options"
              v-model="currOption"
              width="200px"
              query="name"
            />
          </template>
        </EdListGridAdvance>
      </EdFrame>
    </template>
  </base-content-area>
</template>
<script>
// Plugins
import CartAPI from "@/api/components/Cart/CartAPI.js";

export default {
  name: "Cart",
  data() {
    return {
      options: [
        { name: "Tất cả sản phẩm" },
        { name: "Hàng đổi trả" },
        { name: "Hàng tồn kho" },
        { name: "Hàng 2hand" }
      ],
      currOption: 0,
      listCart: [],
      listCartClone: [],
      showAddCart: false,
      modePopup: false,
      listCheck: [],
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
          width: 300,
          height: "100px",
          pin: true
        },
        {
          title: "Giá",
          field: "Price",
          type: "money",
          width: 100,
          headerPos: "right",
          dataPos: "right"
        },
        {
          title: "Số lượng",
          field: "CartQuantity",
          type: "number",
          width: 100,
          min: 1,
          headerPos: "right",
          dataPos: "right",
          editable: true, 
          total: true
        },
        {
          title: "Tổng tiền",
          field: "CartTotalPrice",
          type: "money",
          width: 100,
          headerPos: "right",
          dataPos: "right",
          total: true
        }
      ],
      pagingInfo: {
        filterString: "",
        pageNum: 1,
        pageSize: 10,
        filterData: {}
      },
      cartInfo: {},
      totalData: {}
    };
  },
  mounted() {
    this.getCartsFilterPaging(this.currOption);
  },
  methods: {
    /**
     * Đi đến trang chi tiết sản phẩm
     * CreatedBy: NTDUNG (08/12/2021)
     */
    productDetail(cart) {
      console.log(cart);
      this.$router.push(`/product/product-detail/${cart.ProductID}`);
    },
    /**
     * Lấy dữ liệu danh sách sản phẩm
     * CreatedBy: NTDUNG (08/12/2021)
     */
    getCarts() {
      CartAPI.GetFilterPaging(
        this.pagingInfo.filterString,
        this.pagingInfo.pageNum,
        this.pagingInfo.pageSize,
        this.pagingInfo.filterData
      )
        .then(res => {
          if (res.data.Success) {
            this.listCart = res.data.Data.Records;
            this.listCartClone =  JSON.parse(JSON.stringify(this.listCart));
            this.pagingInfo.totalPage = res.data.Data.TotalPage;
            this.pagingInfo.totalRecord = res.data.Data.TotalRecord;
            this.listCheck = [];
            this.totalData = res.data.Data.TotalDatas;
          }
        })
        .catch(err => {
          console.log(err);
        });
    },
    /**
     * Tạo ra lọc cho danh sách sản phẩm
     * CreatedBy: NTDUNG (08/12/2021) */
    getCartsFilterPaging() {
      switch (this.currOption) {
        case 0:
          this.pagingInfo.filterData = {
            TotalFields: ["CartTotalPrice", "CartQuantity"]
          };
          this.getCarts();
          break;
        case 1:
          this.pagingInfo.filterData = {
            TotalFields: ["Price", "OldPrice"],
            RangeDates: [
              {
                FieldName: "ModifiedDate",
                FromDate: new Date(2021, 11, 22),
                ToDate: new Date(2021, 11, 24)
              }
            ]
          };
          this.getCarts();
          break;
        case 2:
          this.pagingInfo.filterData = {
            TotalFields: ["Price", "OldPrice"]
          };
          this.getCarts();
          break;
        case 3:
          this.pagingInfo.filterData = {
            TotalFields: ["Price", "OldPrice"]
          };
          this.getCarts();
          break;
      }
    },
    /**
     * Xoá sản phẩm
     * CreatedBy: NTDUNG (19/12/2021)
     */
    deleteCart(cart) {
      this.$dialog.confirm(
        `Bạn có muốn xoá <<b>${cart.ProductCode} - ${cart.ProductName}</b>> khỏi giỏ hàng không ?`,
        {
          YES: () => {
            CartAPI.Delete(cart.CartID)
              .then(res => {
                console.log(res);
                this.getCartsFilterPaging();
                this.$toast.success("Xoá thành công");
              })
              .catch(err => {
                console.log(err);
              });
          },
          NO: () => {}
        }
      );
    },
    /**
     * Chỉnh sửa sản phẩm
     * CreatedBy: NTDUNG (19/12/2021)
     */
    showEditCart(cart) {
      CartAPI.GetByID(cart.CartID)
        .then(res => {
          if (res.data.Success) {
            this.cartInfo = res.data.Data;
            this.modePopup = true;
            this.showAddCart = true;
          }
        })
        .catch(err => {
          this.$toast.error("Có lỗi xảy ra");
          console.log(err);
        });
    },
    /**
     * Thêm sản phẩm
     * CreatedBy: NTDUNG (20/12/2021)
     */
    addCart() {
      CartAPI.post(this.cartInfo)
        .then(res => {
          console.log(res);
          this.$toast.success("Thêm mới sản phẩm thành công");
          this.showAddCart = false;
          this.getCartsFilterPaging();
        })
        .catch(err => {
          this.$toast.error(err.response.data.Msg);
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
      this.getCartsFilterPaging();
    },
    /**
     * Xoá nhiều
     * CreatedBy: NTDUNG (21/12/2021)
     */
    DeleteMulti() {
      this.$dialog.confirm("Bạn có muốn xoá tất cả những sản phẩm được chọn khỏi giỏ hàng ?", {
        YES: () => {
          CartAPI.DeleteMulti(this.listCheck)
            .then(res => {
              this.$toast.success("Xoá thành công");
              this.getCartsFilterPaging();
              console.log(res);
            })
            .catch(err => {
              this.$toast.error("Xoá thất bại");
              console.log(res);
            });
        }
      });
    },
    /**
     * Chỉnh sửa sản phẩm
     * CreatedBy: NTDUNG (23/12/2021)
     */
    editCart() {
      CartAPI.Update(this.cartInfo.CartID, this.cartInfo)
        .then(res => {
          this.$toast.success("Chỉnh sửa sản phẩm thành công");
          this.showAddCart = false;
          this.getCartsFilterPaging();
        })
        .catch(err => {
          this.$toast.error(err.response.data.Msg);
        });
    },
    /**
     * Thay đổi danh sách được check
     * CreatedBy: NTDUNG (23/12/2021)
     */
    changeListCheck(newListCheck) {
      this.listCheck = newListCheck;
    },
    /**
     * Lưu bảng
     * CreatedBy: NTDUNG (23/12/2021)
     */
    saveTable() {
        var listChange = this.listCart.filter((cart, index) => {
            return !this.deepEqualObject(cart, this.listCartClone[index]);
        });
        CartAPI.UpdateMulti(listChange)
            .then(res => {
                console.log(res);
                this.$toast.success("Cập nhật thành công");
                this.getCartsFilterPaging();
            })
            .catch(err => {
                console.log(err);
                this.$toast.error("Cập nhật thất bại");
            })
    }
  },
  watch: {
    currOption: function(value) {
      this.getCartsFilterPaging(value);
    }
  }
};
</script>
<style lang=""></style>
