<template>
  <div id="app" class="h-100">
    <!-- <transition name="page" mode="out-in"> -->
    <router-view></router-view>
    <!-- </transition> -->
    <resume-play></resume-play>
  </div>
</template>

<script>
import ResumePlay from "./components/resumeplay.vue";

export default {
  components: {
    ResumePlay
  }
};
</script>
