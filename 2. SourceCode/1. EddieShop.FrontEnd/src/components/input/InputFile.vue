<template lang="">
  <div class="inputfile fx-col">
    <div class="w-full">
      <input
        ref="inputFile"
        type="file"
        :name="name"
        @change="changeFile($event)"
        v-validate="realRules"
        class="inputfile__main d-none"
      />
      <input
        type="text"
        v-validate="{ required: 'required' in rules }"
        :name="name"
        :value="value"
        style="display: none"
      />
      <div v-if="value" class="inputfile__preview flex-1 p-v-15">
        <img
          @click="showPreview = true"
          class="w-full h-full cur-p"
          :src="
            typeof value == 'string' ? 'data:image/gif;base64,' + value : ''
          "
          :alt="fileName"
        />
      </div>
      <ed-button
        :method="chooseFile"
        :label="value ? 'Đổi ảnh' : 'Chọn ảnh'"
        :styleBtn="0"
      />
      <vue-easy-lightbox
        :visible="showPreview"
        :imgs="[
          {
            src:
              typeof value == 'string' ? 'data:image/gif;base64,' + value : '',
            title: fileName
          }
        ]"
        :index="0"
        @hide="handleHide"
      ></vue-easy-lightbox>
    </div>
    <div
      v-if="errMsg"
      class="input__error txt-reg-1 txt-s-12 m-t-10"
      style="color: red;"
    >
      {{ errMsg}}
    </div>
  </div>
</template>
<script>
export default {
  name: "InputFile",
  inject: ["parentValidator"],
  props: {
    rules: {
      type: [Object, String],
      default: () => {}
    },
    errMsg: {
      type: [Number, String],
      default: ""
    },
    value: {
      type: [String, Event],
      default: null
    },
    name: {
      type: [Number, String],
      default: "DefaultInputFile"
    }
  },
  data() {
    return {
      files: [],
      fileName: "",
      reader: null,
      showPreview: false
    };
  },
  created() {
    this.$validator = this.parentValidator;
  },
  computed: {
    realRules() {
      var rules = {...this.rules};
      delete rules.required;
      return rules;
    },
    /**
     * Tệp mới
     * CreatedBy: NTDUNG (23/12/2021)
     */
    newBlob() {
      return new Blob([this.value], { type: "image" });
    },
    /**
     * Lắng nghe sự kiện input
     * CreatedBy: NTDUNG (19/12/2021)
     */
    inputListeners: function() {
      return Object.assign({}, this.$listeners, {
        change: e => {
          console.log(this.$refs.inputFile);
          // var vm = this;
          // this.files = e.target.files;
          // this.fileName = this.files[0].name;

          // // Tạo đường dẫn preview
          // this.srcPreview = URL.createObjectURL(this.files[0]);
          // // Timeout xoá đường dẫn
          // setTimeout(() => {
          //   URL.revokeObjectURL(this.srcPreview);
          // }, 100);

          // Convert sang bytearray và truyền ra
          // console.log(this.files)
          // this.reader.readAsArrayBuffer(this.files[0]);
        }
      });
    }
  },
  methods: {
    changeFile(e) {
      this.files = e.target.files;
      this.fileName = this.files[0].name;

      // // Tạo đường dẫn preview
      // this.srcPreview = URL.createObjectURL(this.files[0]);
      // // Timeout xoá đường dẫn
      // setTimeout(() => {
      //   URL.revokeObjectURL(this.srcPreview);
      // }, 100);

      // Convert sang bytearray và truyền ra
      this.reader = new FileReader();
      this.reader.onload = () => {
        this.readerImage();
      };
      this.reader.readAsArrayBuffer(this.files[0]);
    },
    /**
     * Chuyển sang mảng ký tự
     * CreatedBy: NTDUNG (19/12/2021)
     */
    readerImage() {
      // var binaryString2 = btoa(String.fromCharCode(...(new Uint8Array(this.reader.result))))
      var binaryString = [],
        bytes = new Uint8Array(this.reader.result),
        length = bytes.length;
      for (var i = 0; i < length; i++) {
        binaryString[i] = String.fromCharCode(bytes[i]);
      }
      var data = btoa(binaryString.join(""));
      this.$emit("input", data);
    },
    /**
     * Chọn file
     * CreatedBy: NTDUNG (20/12/2021)
     */
    chooseFile() {
      this.$refs.inputFile.click();
    },
    /**
     * Tắt preview
     * CreatedBy: NTDUNG (19/12/2021)
     */
    handleHide() {
      this.showPreview = false;
    }
  }
};
</script>
<style lang="scss">
@import "@/assets/scss/components/input/inputfile.scss";
</style>
