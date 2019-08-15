"use strict";

import axios from "axios";

export const HTTP = axios.create({
  baseURL:
    process.env.VUE_APP_DEBUG == "true"
      ? "http://" + window.location.hostname + process.env.VUE_APP_apiUrl
      : process.env.VUE_APP_apiUrl,
  withCredentials: true
});
