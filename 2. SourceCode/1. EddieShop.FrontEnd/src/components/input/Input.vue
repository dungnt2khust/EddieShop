<template lang="">
  <div class="input" :class="{'just-error': justError}" :style="customizeStyle(styleCustom)">
    <div class="w-full">
      <input
        :autocomplete="autoComplete ? 'on' : 'off'"
        :id="rules && Object.getOwnPropertyNames(rules).length ? name : ''"
        :type="typeTemp == 'number' ? 'text' : typeTemp"
        :value="inputValue"
        :placeholder="placeholder"
        :autofocus="autoFocus"
        :title="errMsg"
        :name="rules && Object.getOwnPropertyNames(rules).length ? name : 'DefaultInput'"
        :class="{ 'input--invalid': errMsg != '', 'input--disabled': disabled }"
        :readonly="disabled"
        v-on="inputListeners"
        v-validate="rules"
      />
      <div
        v-if="type == 'password'"
        @click="togglePassword"
        class="show-password"
      >
        <div v-if="showPassword" class="mi-hidden"></div>
        <div v-else class="mi-show"></div>
      </div>
    </div>
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
  name: "Input",
  inject: {
    parentValidator: {
      default: null
    }
  },
  props: {
    width: {
      type: String,
      default: null
    },
    type: {
      type: String,
      default: "text"
    },
    value: {
      type: [Number, String],
      default: null
    },
    placeholder: {
      type: [Number, String],
      default: null
    },
    autoFocus: {
      type: Boolean,
      default: false
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
    },
    duration: {
      type: [Number, String],
      default: 0
    },
    disabled: {
      type: Boolean,
      default: false
    },
    justError: {
      type: Boolean,
      default: false
    },
    label: {
      type: String,
      default: null
    },
    autoComplete: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      styleCustom: {
        width: this.width
      },
      showPassword: false,
      typeTemp: this.type,
      inputTimeout: null
    };
  },
  created() {
    if (this.parentValidator) this.$validator = this.parentValidator;
  },
  mounted() {
    if (this.parentValidator) {
      var error = {
        custom: {
          [this.name]: {}
        }
      };
      if (this.rules && this.rules.required) {
        error.custom[this.name].required = () => this.$t('i18nValidate.Required', {Field: this.label || this.name})
      }
      if (this.rules && this.rules.max) {
        error.custom[this.name].max = (...rest) => this.$t('i18nValidate.Max', {Field: this.label || this.name, Max: rest[1]})
      }
      this.$validator.localize(this.$store.state.Language, error);
    }
  },
  computed: {
    /**
     * Format input
     * CreatedBy: NTDUNG (24/12/2021)
     */
    inputValue() {
      switch (this.type) {
        case "number":
          return this.formatNumberDot(this.value);
        default:
          return this.value;
      }
    },
    /**
     * Lắng nghe sự kiện trên input
     */
    inputListeners() {
      return Object.assign({}, this.$listener, {
        input: event => {
          clearTimeout(this.inputTimeout);
          if (this.duration)
            this.inputTimeout = setTimeout(() => {
              if (this.type == "number") {
                var value = event.target.value.replaceAll(".", "");
                value.replace(/[^0-9.]/g, "").replace(/(\..*?)\..*/g, "$1");
                this.$emit("input", +value);
              } else this.$emit("input", event.target.value);
            }, this.duration);
          else {
            if (this.type == "number") {
              var value = event.target.value.replaceAll(".", "");
              value.replace(/[^0-9.]/g, "").replace(/(\..*?)\..*/g, "$1");
              this.$emit("input", +value);
              // this.$emit("input", +event.target.value);
            } else this.$emit("input", event.target.value);
          }
        }
      });
    }
  },
  methods: {
    /**
     * Bật tắt hiển thị mật khẩu
     * CreatedBy: NTDUNG (20/11/2021)
     */
    togglePassword() {
      this.showPassword = !this.showPassword;
      this.typeTemp = this.typeTemp == "text" ? "password" : "text";
    }
  }
};
</script>
<style lang="scss">
@import "@/assets/scss/components/input/input.scss";
/* Chrome, Safari, Edge, Opera */
input::-webkit-outer-spin-button,
input::-webkit-inner-spin-button {
  -webkit-appearance: none;
  margin: 0;
}

/* Firefox */
input[type="number"] {
  -moz-appearance: textfield;
}
</style>
