<script>
import Countdown from "./Countdown";
export default Countdown;
</script>
  
<template>
  <div class="h-100 d-flex flex-column">
    <div class="position-absolute" v-if="isDebug">
      <button v-if="!timeout" @click="()=>setTimeTo(64)">64s</button>
      <button v-if="!timeout" @click="()=>setTimeTo(12)">12s</button>
      <button v-if="!timeout" @click="()=>setTimeTo(2)">2s</button>
    </div>

    <div class="position-absolute success" v-if="passed">
      <div v-bind:class="{ 'particle' : passed}" v-for="n in 100" :key="n"></div>
    </div>

    <transition appear appear-active-class="animated zoomIn" leave-active-class="animated zoomOut">
      <button
        v-if="show"
        class="position-absolute right-0 top-0 m-2 m-sm-3 m-md-4 button-small"
        @click="stopGame"
      >STOP</button>
    </transition>

    <div class="flex-even d-flex flex-column align-items-center justify-content-end">
      <transition
        enter-active-class="animated slideInDown"
        leave-active-class="animated slideOutUp"
      >
        <div v-if="show">
          <h4 v-if="timeout" class="title text-center">*** GAME OVER ***</h4>
          <h6 v-if="timeout" class="title text-center">{{ maxTimeInMinutes }}:00+</h6>
        </div>
      </transition>
      <transition
        enter-active-class="animated zoomIn"
        leave-active-class="animated zoomOut"
        @after-leave="afterLeave"
      >
        <!-- <transition name="timer" mode="out-in"> -->
        <div
          v-if="show"
          class="timer mt-md-2 mb-md-3 display-3 w-100 d-flex justify-content-center text-monospace"
          v-bind:class="{ 'clock-danger pulse' : (this.addSeconds > 0), 'clock-warning pulse' : (this.remainingSeconds < 60 && !this.timeout), 'pulse' : (passed) }"
        >
          <transition name="seconds" mode="out-in">
            <div>{{ minutes }}</div>
          </transition>
          <div>:</div>
          <transition name="timer" mode="out-in">
            <div :key="seconds">{{ seconds }}</div>
          </transition>
        </div>
        <!-- </transition> -->
      </transition>
    </div>

    <div class="flex-even d-flex flex-column align-items-center">
      <transition enter-active-class="animated fadeIn" leave-active-class="animated fadeOut">
        <div
          v-if="show"
          class="masked"
          :class="{'shake animated': emptyanswer}"
          @animationend="emptyanswer = false"
        >
          <span class="placeholders" v-for="n in solutionLength" :key="n">&nbsp;</span>
          <!-- <div class="code" style="position:absolute;left:0;top:0;">
            <transition-group name="list" tag="div">
              <span class="characters list-item" v-for="n in code.length" :key="n">a</span>
            </transition-group>
          </div>-->
          <input
            v-if="!(timeout && (gameState!==2))"
            class="enter"
            id="enter"
            v-model="code"
            spellcheck="false"
            autocomplete="off"
            :disabled="passed"
            v-bind:style="{ width: 'calc( ' + (solutionLength  + 'ch') + ' + '  + ((solutionLength) * 10) + 60 + 'px )' }"
            :maxlength="solutionLength"
          >
        </div>
      </transition>
      <div
        v-if="!passed && !(timeout && (gameState!==2))"
        class="d-flex justify-content-center w-100"
      >
        <transition enter-active-class="animated zoomIn" leave-active-class="animated zoomOut">
          <button
            v-if="show"
            :class="{'pulseon':code.length == solutionLength}"
            class="button-password p-3 mt-3"
            @click="checkCode"
          >
            <img
              src="../themes/hacker/images/lock.svg"
              style="width:1.7rem; height:1.7rem; margin-top:-0.3rem"
            >
          </button>
        </transition>
      </div>
      <transition enter-active-class="animated zoomIn" leave-active-class="animated zoomOut">
        <div v-if="timeout && (gameState!==2)" class="d-flex justify-content-center w-100">
          <button class="button-large m-2" @click="continueGame">doorgaan</button>
          <button class="button-large m-2" @click="stopGame">stoppen</button>
        </div>
      </transition>
    </div>
  </div>
</template>

<style>
</style>
