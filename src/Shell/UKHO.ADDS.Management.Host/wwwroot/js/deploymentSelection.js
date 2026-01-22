window.adds = window.adds || {};
window.adds.deploymentSelection = {
  get: function (moduleId) {
    try {
      return window.localStorage.getItem(`adds:module:${moduleId}:deployment`);
    } catch (e) {
      return null;
    }
  },
  set: function (moduleId, deploymentId) {
    try {
      window.localStorage.setItem(`adds:module:${moduleId}:deployment`, deploymentId);
      return true;
    } catch (e) {
      return false;
    }
  }
};
