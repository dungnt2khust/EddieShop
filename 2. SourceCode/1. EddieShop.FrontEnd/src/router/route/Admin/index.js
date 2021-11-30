var routes = [
  { path: "/", redirect: "/admin/dashboard" },
  {
    path: "/admin/dashboard",
    name: "AdminDashboard",
    component: () => import("@/views/Admin/Dashboard/Dashboard.vue"),
    meta: {
      Title: "i18nMenu.Admin.Dashboard"
    }
  },
  {
    path: "/admin/order",
    name: "AdminOrder",
    component: () => import("@/views/Admin/Order/Order.vue"),
    meta: {
      Title: "i18nMenu.Order"
    }
  },
  {
    path: "/admin/product",
    name: "AdminProduct",
    component: () => import("@/views/Admin/Product/Product.vue"),
    meta: {
      Title: "i18nMenu.Product"
    }
  },
  {
    path: "/admin/push-notify",
    name: "AdminPushNotify",
    component: () => import("@/views/Admin/PushNotify/PushNotify.vue"),
    meta: {
      Title: "i18nMenu.Admin.PushNotify"
    }
  } 
]
export default routes
