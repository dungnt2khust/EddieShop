<template lang="">
  <div class="inputfile">
    <input
      id="file"
      type="file"
      v-on="inputListeners"
      class="inputfile__main d-none"
    />
    <div v-if="fileName" class="inputfile__preview p-v-15">
      <img class="w-full h-full " :src="srcPreview" :alt="fileName" />
    </div>
    <label for="file" class="inputfile__label">
      <ed-button :label="fileName ? 'Đổi ảnh' : 'Chọn ảnh'" :type="0" />
    </label>
  </div>
</template>
<script>
export default {
  name: "InputFile",
  data() {
    return {
      files: [],
      srcPreview: "",
      fileName: ""
    };
  },
  computed: {
    inputListeners() {
      return Object.assign({}, this.$listeners, {
        change: e => {
          var vm = this;
          this.files = e.target.files;
          this.fileName = this.files[0].name;

          // Tạo đường dẫn preview
          this.srcPreview = URL.createObjectURL(this.files[0]);
          // Timeout xoá đường dẫn
          setTimeout(() => {
            URL.revokeObjectURL(this.srcPreview);
          }, 100);

          // Convert sang bytearray và truyền ra
          var reader = new FileReader();
          var bytes, data;
          reader.onloadend = function() {
            bytes = reader.result;
            data = btoa(String.fromCharCode(...new Uint8Array(bytes)));
            vm.$emit("input", data);
          };
          reader.readAsArrayBuffer(this.files[0]);
        }
      });
    }
  }
};
</script>
<style lang="scss">
@import "@/assets/scss/components/input/inputfile.scss";
</style>
