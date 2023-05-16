import axios from "axios";
import * as helper from "../services/serviceHelpers";

const endpoint = `${helper.API_HOST_PREFIX}/api/users`;

const registerUser = (payload) => {
  const config = {
    method: "POST",
    url: `${endpoint}`,
    data: payload,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};

const signIn = (payload) => {
  const config = {
    method: "POST",
    url: `${endpoint}/login`,
    data: payload,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};

const signOut = () => {
  const config = {
    method: "GET",
    url: `${endpoint}/logout`,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};

const getCurrent = () => {
  const config = {
    method: "GET",
    url: `${endpoint}/current`,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};

const getCurrentUser = () => {
  const config = {
    method: "GET",
    url: `${endpoint}/current/user`,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};

const getCurrentById = (id) => {
  const config = {
    method: "GET",
    url: `${endpoint}/${id}`,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};

const updateStatus = (userId, statusId) => {
  const config = {
    method: "PUT",
    url: `${endpoint}/status?userId=${userId}&statusId=${statusId}`,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};

const updateById = (userId, payload) => {
  const config = {
    method: "PUT",
    url: `${endpoint}/${userId}`,
    data: payload,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};

const userService = {
  registerUser,
  signIn,
  signOut,
  getCurrent,
  getCurrentUser,
  getCurrentById,
  updateStatus,
  updateById,
};
export default userService;
