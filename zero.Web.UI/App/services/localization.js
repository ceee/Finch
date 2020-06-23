import { extend as _extend, each as _each } from 'underscore';

export default {

  localize(key, options)
  {
    let params = _extend({
      force: false,
      tokens: {},
      hideEmpty: false
    }, options || {});

    if (!key)
    {
      return '';
    }

    const hasAtSign = key.indexOf('@') === 0;

    if (!params.force && !hasAtSign)
    {
      return this.replaceTokens(key, params.tokens);
    }

    key = hasAtSign ? key.slice(1) : key;
    const value = zero.translations[key.toLowerCase()];

    // TODO only return key if in debug mode
    if (!params.hideEmpty && (!value || typeof value !== 'string'))
    {
      return '[' + key + ']';
    }

    return this.replaceTokens(value, params.tokens);
  },


  replaceTokens(value, tokens)
  {
    if (!value || value.indexOf('{') < 0)
    {
      return value;
    }

    _each(tokens, (replacement, key) =>
    {
      value = value.replace("{" + key + "}", replacement);
    });

    return value;
  }
};