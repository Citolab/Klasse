<script>
import Intro from "./Intro";
export default Intro;
</script>

<template>
  <div class="h-100 d-flex flex-column p-2 p-sm-3 p-md-4">
    <transition appear appear-active-class="animated zoomIn" leave-active-class="animated zoomOut">
      <button
        v-if="show"
        class="position-absolute right-0 top-0 m-2 m-sm-3 m-md-4 button-small"
        @click="stopGame"
      >STOP</button>
    </transition>

    <transition
      appear
      appear-active-class="animated zoomIn"
      leave-active-class="animated zoomOut"
      @after-leave="afterLeave"
    >
      <div class="window-frame" style="min-height:14rem;" v-if="show">
        <h4 v-title class="title m-0 mb-2">**GEHACKT**</h4>

        <div class="introtext" v-introtext @my-event="stopAudio(); showStart=true">
          Dit systeem is gehackt en alle persoonlijke documenten zijn versleuteld.
          Om ze terug te krijgen moet er een codewoord worden ingevoerd.
          De letters van dat codewoord moeten eerst verdiend worden met opdrachten.
          Als het codewoord op tijd ingevoerd wordt, worden alle persoonlijke documenten weer vrijgegeven.
          Maar pas op: met elke foute invoer gaat er tijd vanaf..
        </div>
        <div v-title="{ time: 500 }" v-if="showStart" class="mt-2">-H4ck3r-</div>
      </div>
    </transition>
    <div class="flex-even d-flex align-items-end">
      <transition
        enter-active-class="animated zoomIn"
        leave-active-class="animated zoomOut"
      >
        <button class="button-large ml-auto" v-if="showStart" @click="startGame">Start</button>
      </transition>
    </div>
  </div>
</template>

