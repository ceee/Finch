
import List from '../../../schemas/list/list';
import api from '../api';

const list = new List('countries');
const prefix = '@country.fields.';

list.templateLabel = x => prefix + x;
list.link = 'countries-edit';

list.onFetch(filter => api.getByQuery(filter));

list.column('flag', { width: 62, canSort: false, hideLabel: true }).custom((value, model) => `<i class="ui-icon" data-symbol="flag-${model.code.toLowerCase()}"></i>`, true, 'flag');
list.column('name').name();
list.column('code').custom(val => val.toUpperCase());
list.column('isPreferred').boolean();
list.column('isActive').active();

export default list;