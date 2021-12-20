<template lang="">
  <div class="textarea">
    <textarea
      class="w-full"
      :rows="row"
      :cols="col"
      :name="name"
      :style="customizeStyle(styleCustom)"
      v-on="textareaListeners"
      :value="value"
      :class="{ 'input--invalid': errMsg != '' }"
      :title="errMsg"
      v-validate="rules"
    />
    <div
      v-if="errMsg"
      class="input__error txt-reg-1 txt-s-12 m-t-10"
      style="color: red;"
    >
      {{ errMsg }}
    </div>
  </div>
</template>
<script>
export default {
  name: "Textarea",
  inject: ['parentValidator'],
  props: {
    col: {
      type: Number,
      default: 1
    },
    row: {
      type: Number,
      default: 5
    },
    resize: {
      type: String,
      default: "none"
    },
    width: {
      type: String,
      default: null
    },
    height: {
      type: String,
      default: null
    },
    value: {
      type: [Number, String],
      default: null
    },
    rules: {
      type: [Object, String],
      default: () => {}
    },
    name: {
      type: [Number, String],
      default: ""
    },
    errMsg: {
      type: [Number, String],
      default: ""
    }
  },
  data() {
    return {
      styleCustom: {
        resize: this.resize,
        width: this.width,
        height: this.height
      }
    };
  },
  created() {
    this.$validator = this.parentValidator;
  },
  computed: {
    /**
     * Lắng nghe textarea
     * CreatedBy: NTDUNG (24/11/2021)
     */
    textareaListeners() {
      return Object.assign({}, this.listeners, {
        input: event => {
          this.$emit("input", event.target.value);
        }
      });
    }
  }
};
</script>
<style lang="scss">
@import "@/assets/scss/components/textarea/textarea.scss";
</style>
