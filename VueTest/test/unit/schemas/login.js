export default {
  type: 'object',
  properties: {
    Login: {type: 'string'},
    Password: {type: 'string'},
  },
  required: [
    'Login',
    'Password'],
};
