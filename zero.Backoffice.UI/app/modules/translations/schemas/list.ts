
import List from '../../../schemas/list/list';
import api from '../api';

const list = new List('translations');
const prefix = '@translation.fields.';

list.templateLabel = x => prefix + x;
list.link = 'translations-edit';

list.onFetch(filter => api.getByQuery(filter));

list.column('key', { width: 420 }).name();
list.column('value').text();

export default list;