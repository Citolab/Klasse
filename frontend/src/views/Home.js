// import transitionToPromise from "../transitionpromise";

export default {
  data: function () {
    return {
      show: true
    };
  },
  methods: {
    startGame: function() {
      this.show = false;
      // this.$router.push({ path: "intro" });
    },
    afterLeave: function() {
      this.$router.push({ path: "intro" });
    }
  }
};

// https://codepen.io/cythilya/pen/GmVNGB
