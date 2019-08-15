export default (el, property, value) =>
  new Promise(resolve => {
    el.style[property] = value;
    const transitionEnded = e => {
      if (e.propertyName !== property) return;
      el.removeEventListener("transitionend", transitionEnded);
      resolve();
    };
    el.addEventListener("transitionend", transitionEnded);
  });
