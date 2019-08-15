<template>
  <b-modal
    ref="myModalRef"
    hide-footer
    title="Er is nog een spel bezig"
    bodyClass="bg-dark"
    headerClass="bg-dark"
    hideHeaderClose
  >
    <button
      class="btn btn-outline-light border-2"
      variant="outline-danger"
      block
      @click="stopGame"
    >Stoppen</button>
    <button
      class="btn btn-light border-2 ml-3"
      variant="outline-danger"
      block
      @click="resumeGame"
    >Voortzetten</button>
  </b-modal>
</template>
<script>
import bModal from "bootstrap-vue/es/components/modal/modal";
import bModalDirective from "bootstrap-vue/es/directives/modal/modal";
import { HTTP } from "../plugins/http-common";

export default {
  components: {
    "b-modal": bModal
  },
  directives: {
    "b-modal": bModalDirective
  },
  created: function() {
    HTTP.get("/game/gamerunning/1")
      .then(response => {
        // If the game is still on, or even continued, show the option to resume
        if (response.data == true) {
          this.showModal();
        } else {
          this.stopGame();
        }
      })
      .catch(error => {
        console.log("no game" + error);
      });
  },
  methods: {
    showModal() {
      this.$refs.myModalRef.show();
    },
    hideModal() {
      this.$refs.myModalRef.hide();
    },
    stopGame() {
      HTTP.post("/game/stop")
        .then(() => {
          this.hideModal();
        })
        .catch(error => {
          this.errorMessage = "Kan het spel niet stoppen";
          console.log(error);
        });
    },
    resumeGame() {
      this.$router.push({ path: "/countdown" });
      this.hideModal();
    }
  }
};
</script>
<style>
</style>