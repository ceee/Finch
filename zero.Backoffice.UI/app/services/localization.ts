
import { extendObject } from '../utils/objects';
import { useTranslationStore } from '../stores/translations';

export interface LocalizeOptions
{
  force?: boolean,
  tokens?: Record<string, any>,
  hideEmpty?: boolean
}


export const localize = (key: string, options?: LocalizeOptions): string =>
{
  let params = extendObject({
    force: false,
    tokens: {},
    hideEmpty: false
  }, options || {}) as LocalizeOptions;

  if (!key)
  {
    return '';
  }

  if (!isNaN(key))
  {
    return key;
  }

  const hasAtSign = key.indexOf('@') === 0;

  if (!params.force && !hasAtSign)
  {
    return replaceTokens(key, params.tokens);
  }

  key = hasAtSign ? key.slice(1) : key;
  const store = useTranslationStore();
  const value = store.find(key);

  // TODO only return key if in debug mode
  if (!params.hideEmpty && (!value || typeof value !== 'string'))
  {
    return '[' + key + ']';
  }

  return replaceTokens(value, params.tokens);
};


export const replaceTokens = (value: string, tokens: Record<string, string>): string =>
{
  if (!value || value.indexOf('{') < 0)
  {
    return value;
  }

  for (const key in tokens)
  {
    value = value.replace("{" + key + "}", tokens[key]);
  }

  return value;
};