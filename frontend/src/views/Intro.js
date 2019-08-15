import { ThemeAudio } from "../themes/hacker/theme";

export default {
  data: function() {
    return {
      audio: ThemeAudio,
      show: true,
      showStart: false,
      routeTo: ""
    };
  },
  created: function() {
    // this.audio.intro_music.play();
    this.audio.intro_text.play();
  },
  destroyed: function() {
    // this.audio.intro_music.pause();
    this.audio.intro_text.pause();
  },
  methods: {
    stopGame: function() {
      this.routeTo = "/"; // store the route for after the animation
      this.show = false;
      this.showStart = false;
    },
    stopAudio: function() {
      this.audio.intro_text.pause();
    },
    afterLeave: function() {
      this.$router.push({ path: this.routeTo }); // route
    },
    startGame: function() {
      this.routeTo = "/countdown"; // store the route for after the animation
      this.show = false;
      this.showStart = false;
    },
  }
};
