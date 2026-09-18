import { createRouter, createWebHistory } from "vue-router";
import HomePage from "../components/pages/HomePage.vue";
import DetailPage from "../components/pages/DetailPage.vue";
import ImageUploadPage from "../components/imageUploadPage.vue";

const routes = [
  {
    path: "/",
    name: "homePage",
    component: HomePage,
  },
  {
    path: "/detail", // TODO: {:id}를 리전이름 으로 고치기
    name: "region detailedPage",
    component: DetailPage,
  },
  {
    path: "/test/image",
    name: "image upload test",
    componet: ImageUploadPage,
  },
];

const router = createRouter({
  history: createWebHistory("/"),
  routes,
});

export default router;
