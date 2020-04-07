import { extend as _extend, each as _each } from 'underscore';

export default {

  cache: zero.translations,

  localize(key, options)
  {
    let params = _extend({
      force: false,
      tokens: {}
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
    let value = this.cache;

    for (let part of key.split('.'))
    {
      if (!value[part])
      {
        break;
      }

      value = value[part];
    }

    if (!value || value === this.cache || typeof value !== 'string')
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