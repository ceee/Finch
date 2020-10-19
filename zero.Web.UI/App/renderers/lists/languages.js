
import List from 'zero/core/list.js';
import LanguagesApi from 'zero/resources/languages';

const list = new List('languages');
const prefix = '@language.fields.';

list.templateLabel = x => prefix + x;
list.link = zero.alias.settings.languages + '-edit';

list.onFetch(filter => LanguagesApi.getAll(filter));

list.column('name').name();
list.column('code').text();
list.column('isDefault', { width: 200 }).boolean();

export default list;