const auth = {
  get isAuthenticated() {
    return !!this.accessToken;
  },
  get header() {
    if (!this.isAuthenticated)
      return null;
    return {'Authorization': 'Bearer ' + this.accessToken}
  },
  accessToken: null
};

export default auth
