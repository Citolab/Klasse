<script>
import Home from "./Home";
export default Home;
</script>

<template>
  <div class="page">
    <cookie-consent>
      <template slot="message">
        <span class="text-dark">Klasse maakt gebruik van cookies</span>
        <a class="btn btn-link text-cito-cyan" href="www.cito.nl">Meer weten?</a>
      </template>
      <template slot="button">
        <button class="btn btn-cito-cyan rounded btn-sm">Ik snap het</button>
      </template>
    </cookie-consent>
    <div class="theme">
      <transition
        appear
        appear-active-class="animated slideInDown"
        leave-active-class="animated slideOutUp"
      >
        <div class="theme-intro-text" v-if="show">
          <h4 class="title" v-title>**GEHACKT**</h4>
          <p>Het schoolsysteem is gehackt!</p>
        </div>
      </transition>
      <div class="theme-intro-button">
        <transition
          appear
          appear-active-class="animated zoomIn"
          leave-active-class="animated zoomOut"
        >
          <button class="button-large" v-if="show" @click="startGame">SPEEL</button>
        </transition>
      </div>
    </div>

    <transition
      appear
      appear-active-class="animated slideInUp"
      leave-active-class="animated slideOutDown"
      @after-leave="afterLeave"
    >
      <div class="footer" v-if="show">
        <img
          src="../themes/basic/images/logo-klasse@2x.png"
          style="position: absolute; transform-origin: bottom left;transform: scale(1.6) translateY(-1.3rem);"
        >
        <div class="text-dark">Meer informatie op <a href="http://cito.nl/klasse" target="_new" class="text-cito-cyan">cito.nl/klasse</a></div>
        <img src="../themes/hacker/images/logo-cito.svg">
      </div>
    </transition>
  </div>
</template>

<style lang="scss">
@import "../bootstrap/custom-bootstrap";
@import "../bootstrap/custom-utility-classes";
div.page {
  @extend .h-100;
  @extend .d-flex;
  @extend .flex-column;
  @extend .align-items-stretch;
  div.theme {
    @extend .flex-grow-1;
    @extend .d-flex;
    @extend .flex-column;
    @extend .p-2;
    @extend .p-sm-3;
    @extend .p-md-4;
    div.theme-intro-text {
      @extend .flex-even;
      h4 {
        @extend .m-0;
      }
      p {
        @extend .mt-3;
      }
    }
    div.theme-intro-button {
      @extend .flex-even;
      @extend .d-flex;
      @extend .align-items-end;
      button.button-large {
        @extend .ml-auto;
      }
    }
  }
  div.footer {
    @extend .d-flex;
    @extend .align-items-center;
    @extend .justify-content-between;
    @extend .bg-white;
    @extend .p-2;
    @extend .p-sm-3;
    @extend .p-md-4;
    height: 5rem;
    img {
      height: 3rem;
    }
  }
}
</style>
