const routes = [
  {
    path: "/home",
    name: "Home",
    component: () => import("@/views/User/HomePage/HomePage.vue"),
    meta: {
      Title: "i18nMenu.Home"
    }
  },
  {
    path: "/order",
    name: "Order",
    component: () => import("@/views/User/Order/Order.vue"),
    meta: {
      Title: "i18nMenu.Order"
    }
  },
  {
    path: "/cart",
    name: "Cart",
    component: () => import("@/views/User/Cart/Cart.vue"),
    meta: {
      Title: "i18nMenu.Cart"
    }
  },
  {
    path: "/messenger",
    name: "Messenger",
    component: () => import("@/views/User/Messenger/Messenger.vue"),
    meta: {
      Title: "i18nMenu.Messenger"
    }
  }
]
export default routes
