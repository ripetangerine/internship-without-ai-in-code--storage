<script setup>
import MainRegion from "../molecules/Home/MainRegion.vue";
import RegionCardList from "../molecules/Home/RegionCardList.vue";
</script>

<template>
  <div class="container">
    <div class="region-selected">
      <MainRegion />
    </div>
    <div class="region-card-list">
      <RegionCardList />
    </div>
    <div class="activity-log-list"></div>
  </div>
</template>

<style scoped>
.container {
  display: grid;
  grid-template-areas: "main list" "logs list";
  grid-template-columns: 3fr 2fr;
  grid-template-rows: 3fr 1fr;
  gap: 10px;
  /* min-height: 800px; */
  min-height: 80vh;
  /* place-items: center; */
}
.region-card-list {
  grid-area: list;
}
.region-selected {
  grid-area: main;
}
.activity-log-list {
  grid-area: logs;
}

.container > * {
  border: var(--card-border);
  display: flex;
  justify-content: center;
  align-items: center;
  width: 100%;
  height: 100%;
  min-height: 100px;
}
</style>
