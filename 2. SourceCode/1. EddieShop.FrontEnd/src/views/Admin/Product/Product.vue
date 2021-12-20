<template lang="">
  <base-content-area
    class="jus-c-center"
    title="Quản lý sản phẩm"
    pHor="20px"
    bgColor="#fff"
  >
    <template v-slot:header>
      <div class="fx-row jus-c-fend">
        <ed-button
          :method="
            () => {
              showAddProduct = true;
            }
          "
          width="fit-content"
          label="Thêm sản phẩm"
          styleBtn="1"
        />
      </div>
    </template>
    <template v-slot:content>
      <EdFrame class="h-full fx-col" autoScroll>
        <EdListGridTable
          class="flex-1"
          itemID="ProductID"
          :listData="listProduct"
          :dblClick="productDetail"
          :deleteItem="deleteProduct"
          :editItem="editProduct"
          :listHeader="listHeader"
          v-model="listCheck"
          query="ProductName"
        >
          <template v-slot:header>
            <ed-select-box
              :options="options"
              v-model="currOption"
              width="200px"
              query="name"
            />
          </template>
        </EdListGridTable>
      </EdFrame>
      <AddProduct
        @addProduct="addProduct"
        @close="showAddProduct = false"
        v-if="showAddProduct"
      />
    </template>
  </base-content-area>
</template>
<script>
// Plugins
import ProductAPI from "@/api/components/Product/ProductAPI.js";
// Components
import AddProduct from "@/views/Admin/Product/AddProduct.vue";

export default {
  name: "Product",
  components: {
    AddProduct
  },
  data() {
    return {
      options: [
        { name: "Tất cả sản phẩm" },
        { name: "Hàng đổi trả" },
        { name: "Hàng tồn kho" },
        { name: "Hàng 2hand" }
      ],
      currOption: 0,
      listProduct: [],
      showAddProduct: false,
      listCheck: [],
      listHeader: [
        {
          title: "Mã sản phẩm",
          field: "ProductCode",
          type: "text",
          width: '100px'
        },
        {
          title: "Tên sản phẩm",
          field: "ProductName",
          type: "text",
          width: '120px'
        },
        { title: " Hình ảnh", field: "Image", type: "image", width: '200px'},
        {
          title: "Giá",
          field: "Price",
          type: "number",
          width: '100px'
        },
        { title: "Số lượng", field: "Quantity", type: "number", width: 'auto' }
      ]
    };
  },
  mounted() {
    this.getProductsFilterPaging(this.currOption);
  },
  methods: {
    /**
     * Đi đến trang chi tiết sản phẩm
     * CreatedBy: NTDUNG (08/12/2021)
     */
    productDetail(product) {
      this.$router.push(`/admin/product/product-detail/${product.ProductID}`);
    },
    /**
     * Lấy dữ liệu danh sách sản phẩm
     * CreatedBy: NTDUNG (08/12/2021)
     */
    getProducts(filterString, pageNum, pageSize, filterData) {
      ProductAPI.getFilterPaging(filterString, pageNum, pageSize, filterData)
        .then(res => {
          if (res.data.Success) {
            this.listProduct = res.data.Data.Records;
          }
        })
        .catch(err => {
          console.log(err);
        });
    },
    /**
     * Tạo ra lọc cho danh sách sản phẩm
     * CreatedBy: NTDUNG (08/12/2021) */
    getProductsFilterPaging() {
      switch (this.currOption) {
        case 0:
          var filterData = {
            TotalFields: ["Price", "OldPrice"]
          };
          this.getProducts("", 1, -1, filterData);
          break;
        case 1:
          var filterData = {
            TotalFields: ["Price", "OldPrice"],
            RangeDates: [
              {
                FieldName: "ModifiedDate",
                FromDate: new Date(2021, 11, 22),
                ToDate: new Date(2021, 11, 24)
              }
            ]
          };
          this.getProducts("", 1, 10, filterData);
          break;
        case 2:
          var filterData = {
            TotalFields: ["Price", "OldPrice"]
          };
          this.getProducts("", 1, 1, filterData);
          break;
        case 3:
          var filterData = {
            TotalFields: ["Price", "OldPrice"]
          };
          this.getProducts("", 1, 2, filterData);
          break;
      }
    },
    /**
     * Xoá sản phẩm
     * CreatedBy: NTDUNG (19/12/2021)
     */
    deleteProduct(product) {
      this.$dialog.confirm(
        `Bạn có muốn xoá <<b>${product.ProductCode} - ${product.ProductName}</b>> không`,
        {
          YES: () => {
            ProductAPI.delete(product.ProductID)
              .then(res => {
                console.log(res);
                this.getProductsFilterPaging();
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
    editProduct(product) {
      console.log(product);
    },
    /**
     * Thêm sản phẩm
     * CreatedBy: NTDUNG (20/12/2021)
     */
    addProduct(product) {
      ProductAPI.post(product)
        .then(res => {
          console.log(res);
          this.$toast.success("Thêm mới sản phẩm thành công");
          this.showAddProduct = false;
          this.getProductsFilterPaging();
        })
        .catch(err => {
          console.log(err);
          this.$toast.error("Thêm mới sản phẩm thất bại");
        });
    }
  },
  watch: {
    currOption: function(value) {
      this.getProductsFilterPaging(value);
    }
  }
};
</script>
<style lang=""></style>
