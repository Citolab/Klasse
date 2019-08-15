import ScrambleText from "scramble-text";

// Had some problems with emitting evenst from directives, solution here:
// https://stackoverflow.com/questions/49264426/vuejs-custom-directive-emit-event
export const IntroText = {
  inserted: function(el, binding) {
    var eventName = "my-event";
    var eventData = { myData: "stuff - " + binding.expression };

    var i = 0;
    const txt = el.innerText;
    el.innerText = "";
    function typeWriter() {
      if (i < txt.length) {
        el.innerText = txt.substring(0, i);
        i++;
        setTimeout(typeWriter, txt.charAt(i - 2) == "." ? 800 : 50);
      } else {
        el.dispatchEvent(new CustomEvent(eventName, { detail: eventData }));
      }
    }
    typeWriter();
  }
};

export const Title = {
  inserted: function(el, binding) {
    new ScrambleText(el, {
      timeOffset: binding.value ? binding.value.time : 100
    })
      .start()
      .play();
  }
};

export const Focus = {
  inserted: function(el) {
    el.focus();
  }
};

export const ThemeAudio = {
  game_started: new Audio(require("@/themes/hacker/sounds/game-started.mp3")),
  game_over: new Audio(require("@/themes/hacker/sounds/game-over.mp3")),
  beep: new Audio(require("@/themes/hacker/sounds/Ticking.mp3")),
  alert: new Audio(require("@/themes/hacker/sounds/wrong.mp3")),
  one_minute_remaining: new Audio(
    require("@/themes/hacker/sounds/minute-remaining.mp3")
  ),
  intro_text: new Audio(require("@/themes/hacker/sounds/keyboard.mp3")),
  intro_music: new Audio(
    require("@/themes/hacker/sounds/Ticking.mp3")
  )
};
