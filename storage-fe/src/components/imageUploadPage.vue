<script setup>
import { ref } from "vue";

import api from "../api";

const uploadFile = ref(null);
const uploadURL = ref(null);

// TODO : 새로고침시 추가 로직 필요한지 확인
onMounted(async () => {
  try {
    const res = await api.get("/image/upload");
    uploadURL.value = res.uploadURL; // TODO : 추후 이곳에서 값이 없을 시 상세 오류가 나도록 설계
  } catch (error) {
    console.error(`error occurred: ${error}`);
  }
});

function handleImageUpload(e) {
  //   this.input.image = this.$refs.images.files;
  //   console.log(this.input.image);
  const inputImage = e.target;

  if (inputImage && inputImage.files) {
    uploadFile.value = inputImage.files[0];
    const formData = new FormData();
    formData.append("uploadedFile", uploadFile.value);
  }
}

async function saveUploadImage() {}
</script>

/** 뇌를 활성화 그러니까 - presigned 클라이언트 로딩 시간에 >> 백엔드에게
요청해서 업로드 가능한 url을 받고 만약 업로드가 되었다면 >> 해당 url로 이미지를
업로드하고, 백엔드에는 그 url과 메타데이터를 주는거임 1. be url을 줘서 진행될
인증의 방식 (sas)의 요청을 구현 2. fe url로 이미지를 보냄 3. fe 이미지 보낸게
성공 했다면 be에게 이미지의 메타 데이터 전달 이후 저장 */

<template>
  <div>
    <form method="get" @submit.prevent="handleImageUpload">
      <label>image upload</label>
      <input
        @change="handleImageUpload"
        ref="uploadFile"
        type="file"
        accept="image/*"
        id="image_upload"
        name="image_upload"
        capture
      />
      <button>do it NOW!</button>
    </form>

    <div ref="preview" class="preview">
      <p>{{ NO_DATA_TEXT }}</p>
    </div>
  </div>

  <div class="uploaded_image_view">
    <a :href="`/images/${image}`" target="_blank">{{ image }}</a>
  </div>
</template>

<style scoped></style>
