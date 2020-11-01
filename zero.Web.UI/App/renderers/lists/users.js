
import List from 'zero/core/list.ts';
import UsersApi from 'zero/resources/users.js';

const list = new List('users');
const prefix = '@user.fields.';

list.templateLabel = x => prefix + x;
list.link = zero.alias.settings.users + '-edit';

list.onFetch(filter => UsersApi.getAll(filter));

list.column('avatarId', { width: 70, canSort: false }).image();
list.column('name').name();
list.column('email').text();
list.column('isActive').active();

export default list;