// function secondTick() {
//   self.postMessage(1);
//   setTimeout("secondTick()", 1000);
// }

// secondTick();

let interval = setInterval(() => {
  postMessage(1);
}, 1000);

onmessage = function(e) {
  if (e.data.intervalms) {
    this.clearInterval(interval);
    interval = setInterval(() => {
      postMessage(1);
    }, e.data.intervalms);
  }
};
