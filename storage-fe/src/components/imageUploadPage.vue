<script setup>
import { onMounted, onUnmounted, reactive, ref } from "vue";

import api from "../api";

const ProfileImage = ref(null);
const previewURL = ref(null);
const formData = reactive(new FormData());
/** 파일 선택, 미리보기, 업로드 구현 */

function handleInputChange(e) {
  const targetFile = e.target.files[0];
  if (!targetFile) {
    return;
  }
  ProfileImage.value = targetFile;
  previewURL.value = URL.createObjectURL(targetFile);
}

async function uploadImage() {
  if (ProfileImage.value) {
    formData.append("ProfileImage", ProfileImage.value);
    console.log(ProfileImage);

    try {
      // 별도 url 업로드
      // TODO : storage에는 blob를 업로드, db에는 url 메타 데이터를 업로드 이후 결정 (BE 로직 짜고)
      const rtnImages = await api.post("/api/files/images", formData);
      console.log("[1]: ", rtnImages);
      alert("업로드 완료");
      console.log("[2]: ", rtnImages);
    } catch (error) {
      console.error("업로드중 에러 : ", error);
      alert("업로드 중 에러");
    }
  } else {
    alert("파일을 선택하세요");
  }
}
</script>

/** 뇌를 활성화 그러니까 - presigned 클라이언트 로딩 시간에 >> 백엔드에게
요청해서 업로드 가능한 url을 받고 만약 업로드가 되었다면 >> 해당 url로 이미지를
업로드하고, 백엔드에는 그 url과 메타데이터를 주는거임 1. be url을 줘서 진행될
인증의 방식 (sas)의 요청을 구현 2. fe url로 이미지를 보냄 3. fe 이미지 보낸게
성공 했다면 be에게 이미지의 메타 데이터 전달 이후 저장 */

<template>
  <div class="container">
    <label>storage blob upload</label>
    <form @submit.prevent="uploadImage">
      <input
        @change="handleInputChange"
        ref="ProfileImage"
        type="file"
        accept="image/*"
      />
      <button type="submit">upload</button>
    </form>
    <div ref="preview" class="preview">
      <!-- <p>{{ NO_DATA_TEXT }}</p> -->
    </div>
    <div class="uploaded_image_view">
      <img v-if="previewURL" :src="previewURL" alt="이미지 미리보기" />
      <p v-else>이미지 미리보기</p>
    </div>
  </div>
</template>

<style scoped>
.container {
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
  height: 100vh;
  width: 50%;
  margin: 0 auto;
}
</style>
