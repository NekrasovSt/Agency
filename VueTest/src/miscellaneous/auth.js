const auth = {
  get isAuthenticated() {
    return !!this.accessToken;
  },
  get header() {
    if (!this.isAuthenticated)
      return null;
    return {'Authorization': 'Bearer ' + this.accessToken}
  },
  get accessToken() {
    return localStorage.getItem('access_token');
  },
  set accessToken(value) {
    if (value)
      localStorage.setItem('access_token', value);
    else
      localStorage.removeItem('access_token');
  }
};

export default auth
