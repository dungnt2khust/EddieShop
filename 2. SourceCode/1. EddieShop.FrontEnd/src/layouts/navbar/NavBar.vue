<template lang="">
  <div class="navbar">
    <div class="navbar__main w-full fx-row jus-c-sbtn">
      <div class="navbar__left fx-row">
        <div
          v-for="(navbar, index) in $store.state.navBar"
          @click="navBarItemClick(index)"
          class="navbar__item"
          :class="{ selected: $route.path.includes(navbar.Path)}"
          :to="navbar.Path"
          :key="index"
        >
          {{ $t(navbar.Title) }}
        </div>
      </div>
      <div class="navbar__right fx-row aln-i-center cur-p">
        <CartControls />
        <NotifyControls class="m-r-10" />
        <AccountControls />
      </div>
    </div>
    <div class="navbar__mobile">
      <div @click="navbarState = !navbarState" class="navbar__icon scale-2.0 mi-34">
        <i class="fas fa-bars"></i>
      </div>
      <div v-if="navbarState" class="navbar__menu-overlay"></div>
      <div v-if="navbarState" class="navbar__menu">
        <div
          v-for="(navbar, index) in $store.state.navBar"
          @click="navBarItemClick(index)"
          class="navbar__item"
          :class="{ selected: $route.path == navbar.Path }"
          :to="navbar.Path"
          :key="index"
        >
          {{ $t(navbar.Title) }}
        </div>
      </div>
    </div>
  </div>
</template>
<script>
// Components
import NotifyControls from "@/layouts/navbar/notifycontrols/NotifyControls.vue";
import AccountControls from "@/layouts/navbar/accountcontrols/AccountControls.vue";
import CartControls from "@/layouts/navbar/cartcontrols/CartControls.vue";

export default {
  name: "NavBar",
  components: {
    NotifyControls,
    AccountControls,
    CartControls
  },
  data() {
    return {
      navbarState: false
    }
  },
  methods: {
    /**
     * Xử lý click vào item trên navbar
     * @param {Number} index
     * CreatedBy: NTDUNG(02/11/2021)
     * ModifiedBy: NTDUNG (23/11/2021)
     */
    navBarItemClick(index) {
      if (this.$store.state.navBar[index].Path != this.$route.path)
        this.$router.push(this.$store.state.navBar[index].Path);
      this.navbarState = false;
    }
  }
};
</script>
<style lang="scss" scoped>
@import "@/assets/scss/layouts/navbar/navbar.scss";

/* PC medium resolution > */
@media (min-width: 1113px) {
}
/* <= Tablet */
@media (max-width: 740px) {
  .navbar {
    height: 0;
    .navbar__mobile {
      .navbar__icon {
        position: absolute;
        top: 18px;
        left: 10px;
        color: #fff;
        &:hover {
          cursor: pointer;
          color: #5dce00;
          background-color: #fff;
        }
      }
      .navbar__menu {
        position: fixed;
        background-color: #5dce00;
        height: 100vh;
        width: 90%;
        z-index: 200;
        animation: fadeInFromLeft 0.3s ease;
      }
      .navbar__menu-overlay {
        position: fixed;
        background-color: rgba(0, 0, 0, 0.4);
        top: 67px;
        left: 0;
        bottom: 0;
        right: 0;
        z-index: 200;
      }
    }
    .navbar__main {
      display: none;
    }
  }
}
@keyframes fadeInFromLeft {
  0% {
    transform: translateX(-100%);
  }
  100% {
    transform: translateX(0);
  }
}
/* Tablet - PC low resolution */
@media (min-width: 740px) and (max-width: 1023px) {
  .navbar {
    padding: 0 60px;
    .navbar__mobile {
      display: none;
    }
  }
}

/* > PC low resolution */
@media (min-width: 1024px) and (max-width: 1239px) {
  .navbar {
    padding: 0 60px;
    .navbar__mobile {
      display: none;
    }
  }
}

/* PC hight resolution */
@media (min-width: 1240px) {
  .navbar {
    padding: 0 100px;
    .navbar__mobile {
      display: none;
    }
  }
}
</style>
