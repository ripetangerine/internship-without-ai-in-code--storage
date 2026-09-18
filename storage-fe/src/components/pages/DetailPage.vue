<script setup>
import MetricsLineChart from "../molecules/Detail/MetricsLineChart.vue";
import WaffleChart from "../molecules/Detail/WaffleChart.vue";
</script>

<template>
  <div class="container">
    <div class="waffle-chart">
      <WaffleChart />
    </div>
    <div class="metrics-chart">
      <MetricsLineChart />
    </div>
    <!-- TODO : 바닥까지 스크롤 다운을 한 이후에 더 하면 나오는 형식 (추가페이지X) -->
    <!-- <div class="alert-rule"></div>  -->
  </div>
</template>

<style scoped>
.container {
  display: grid;
  grid-template-areas: "waffle metrics-chart";
  grid-template-columns: 2fr 3fr;
  gap: 10px;
  min-height: 100vh;
  align-items: center;
}
.waffle-chart {
  grid-area: waffle;
}
.metrics-chart {
  grid-area: metrics-chart;
}

.container > * {
  border: var(--card-border);
  min-height: 80vh;
}
</style>
