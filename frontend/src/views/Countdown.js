import TimerWebWorker from "worker-loader!../timer.worker.js";
import { HTTP } from "../plugins/http-common";
import transitionToPromise from "../transitionpromise";
import { ThemeAudio } from "../themes/hacker/theme";

export default {
  data: function() {
    return {
      secondWorker: undefined, // Web Worker that sends 'tick' every second.
      timer: null, // Timer instance, is aan het aftellen als timer != null is.
      maxTimeInMinutes: 0, //
      remainingSeconds: 0, // Totale tijd = 30 minuten
      solutionLength: 0, // Gevuld door server
      code: "", // Code die ingevoerd is
      penaltySeconds: 30, // penalty seconds
      addSeconds: 0, // Tijd die er bij of eraf gaat
      timeout: false, // Tijd is voorbij, ga optellen
      passed: false, // Flag juiste antwoord
      failed: false,
      gameState: 0, // 0=Started, 1=Passed, 2=Continued, 3=Stopped
      errorMessage: "",
      emptyanswer: false, // Just a check to shake if input is empty
      audio: ThemeAudio,
      show: false
    };
  },
  created: function() {
    HTTP.post("/game/start/1")
      .then(response => {
        this.remainingSeconds = response.data.remainingSeconds;
        this.timeout = this.remainingSeconds < 0;
        this.penaltySeconds = response.data.penaltySeconds;
        this.solutionLength = response.data.solutionLength;
        this.maxTimeInMinutes = response.data.maxTimeInMinutes;
        this.gameState = response.data.state;

        // Do not say game started on refresh
        if (this.remainingSeconds == this.maxTimeInMinutes * 60) {
          this.audio.game_started.play();
        }
        this.show = true;
        this.startTimer();
      })
      .catch(error => {
        this.errorMessage = "Kan spel niet starten";
        console.log(error);
      });
  },
  destroyed: function() {
    this.audio.alert.pause();
    this.audio.beep.pause();
    this.resetTimer();
  },
  methods: {
    startTimer: function() {
      if (typeof this.secondWorker == "undefined") {
        this.secondWorker = new TimerWebWorker();
        const that = this;
        this.secondWorker.onmessage = function() {
          that.countdown();
        };
      }
    },
    stopTimer: function() {
      if (this.secondWorker !== undefined) {
        this.secondWorker.terminate();
        this.secondWorker = undefined;
      }
    },
    resetTimer: function() {
      this.remainingSeconds = 30 * 60;
      if (this.secondWorker !== undefined) {
        this.secondWorker.terminate();
        this.secondWorker = undefined;
      }
    },
    setTimeTo: function(seconds) {
      this.remainingSeconds = seconds;
      clearInterval(this.timer);
      this.countdown();
    },
    padTime: function(time) {
      return (time < 10 ? "0" : "") + time;
    },
    setOutcome: function(passed) {
      // gamestate: 0=Started,1=Passed,2=Continued,3=Stopped
      this.passed = passed;
      this.failed = !passed;
    },
    doPenalty: function() {
      if (this.addSeconds > 0) {
        this.addSeconds--;
        this.timeout ? this.remainingSeconds++ : this.remainingSeconds--;
        setTimeout(() => {
          this.doPenalty();
        }, 10 * this.penaltySeconds - (this.penaltySeconds - this.addSeconds) * 8);
      }
    },
    countdown: function() {
      if (this.remainingSeconds == 0 && !this.timeout) {
        this.audio.game_over.play();
        this.timeout = true;
        this.setOutcome(false);
      }

      this.timeout ? this.remainingSeconds++ : this.remainingSeconds--;

      if (this.remainingSeconds <= 10 && !this.timeout) {
        this.audio.beep.play();
      }

      if (
        this.remainingSeconds < 63 &&
        this.remainingSeconds > 60 &&
        !this.timeout
      ) {
        this.audio.beep.play();
      }

      if (this.remainingSeconds == 60 && !this.timeout) {
        this.audio.one_minute_remaining.play();
      }
    },
    continueGame: function() {
      HTTP.post("/game/continue");
      this.gameState = 2; //Continued
    },
    checkCode: async function() {
      if (this.code.length < this.solutionLength) {
        this.emptyanswer = true;
        return;
      }
      const enter = document.getElementById("enter");
      var finishedPromise = transitionToPromise(enter, "opacity", "0");
      await finishedPromise;
      HTTP.post("/game/checkanswer", `"${this.code}"`, {
        headers: { "Content-Type": "application/json" }
      })
        .then(response => {
          if (response.data.code === 42) {
            this.setOutcome(true);
            this.stopTimer();
          } else {
            this.addSeconds += this.penaltySeconds;
            this.doPenalty();
            this.audio.alert.play();
            this.code = ""; // empty code
          }
          transitionToPromise(enter, "opacity", "1");
        })
        .catch(error => console.log(error));
    },
    afterLeave: function() {
      HTTP.post("/game/stop")
        .then(() => {
          this.$router.push({ path: "/" });
        })
        .catch(error => {
          this.errorMessage = "Kan het spel niet stoppen";
          // this.$router.push({ path: "/" });
          console.log(error);
        });
    },
    stopGame: function() {
      this.show = false;
      
    },
    continuePlaying: function() {
      this.failed = false;
    }
  },
  computed: {
    minutes: function() {
      if (this.remainingSeconds == 0) {
        return "--";
      }
      const minutes = Math.floor(Math.abs(this.remainingSeconds) / 60);
      return this.padTime(minutes);
    },
    seconds: function() {
      if (this.remainingSeconds == 0) {
        return "--";
      }
      const seconds =
        Math.abs(this.remainingSeconds) - Math.abs(this.minutes) * 60;
      return this.padTime(seconds);
    },
    isDebug: function() {
      return process.env.VUE_APP_DEBUG == "true";
    }
  }
};
