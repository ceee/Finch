
import List from 'zero/core/list.ts';
import MailTemplatesApi from 'zero/resources/mailTemplates.js';

const list = new List('mailTemplates');

list.templateLabel = x => '@mailTemplate.fields.' + x;
list.link = zero.alias.settings.mails + '-edit';

list.onFetch(filter => MailTemplatesApi.getByQuery(filter));

list.column('name').name();
list.column('subject').text();
list.column('key').text();

export default list;