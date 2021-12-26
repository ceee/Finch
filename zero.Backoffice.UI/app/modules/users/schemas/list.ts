
import List from '../../../schemas/list/list';
import api from '../api';

const list = new List('users');
const prefix = '@user.fields.';

list.templateLabel = x => prefix + x;
list.link = x =>
{
  return {
    name: 'users-edit',
    params: { id: x.id },
    query: { scope: 'shared' }
  };
};

list.onFetch(filter => api.getByQuery(filter));

list.column('avatarId', { width: 70, canSort: false, hideLabel: true }).image();
list.column('name').name();
list.column('email').text();
list.column('isActive').active();

export default list;