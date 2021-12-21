
import List from '../../../schemas/list/list';
import api from '../api';

const list = new List('mailTemplates');

list.templateLabel = x => '@mailTemplate.fields.' + x;
list.link = 'mailtemplates-edit';

list.onFetch(filter => api.getByQuery(filter));

list.column('name').name();
list.column('subject').text();
list.column('key').text();

export default list;