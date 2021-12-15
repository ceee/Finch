
import List from '../../../schemas/list/list';
import api from '../api';

const list = new List('languages');
const prefix = '@language.fields.';

list.templateLabel = x => prefix + x;
list.link = 'languages-edit';

list.onFetch(filter => api.getByQuery(filter));

list.column('name').name();
list.column('code').text();
list.column('isDefault', { width: 200 }).boolean();

export default list;