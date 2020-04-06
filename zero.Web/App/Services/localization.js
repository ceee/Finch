
export default {

  cache: zero.translations,

  localize(key, force)
  {
    if (!key)
    {
      return '';
    }

    const hasAtSign = key.indexOf('@') === 0;

    if (!force && !hasAtSign)
    {
      return key;
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

    return value;
  }
};