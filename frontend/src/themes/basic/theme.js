import "./theme.scss";
import Vue from "vue";

// Had some problems with emitting evenst from directives, solution here:
// https://stackoverflow.com/questions/49264426/vuejs-custom-directive-emit-event
export const IntroText = {};
export const Title = {};

Vue.directive("introtext", IntroText);
Vue.directive("title", Title);
