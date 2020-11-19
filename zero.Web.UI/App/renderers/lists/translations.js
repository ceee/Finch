
import List from 'zero/core/list.ts';
import TranslationsApi from 'zero/resources/translations.js';

const list = new List('translations');
const prefix = '@translation.fields.';

list.templateLabel = x => prefix + x;
list.link = zero.alias.settings.translations + '-edit';

list.onFetch(filter => TranslationsApi.getByQuery(filter));

list.column('key').name();
list.column('value').text();

export default list;