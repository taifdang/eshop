export const profileStorage = {
  get: () => {
    const object = localStorage.getItem("profile");
    return JSON.parse(object);
  },
  set: (profile) => {
    localStorage.setItem("profile", JSON.stringify(profile));
  },
  clear: () => {
    localStorage.removeItem("profile");
  },
};
