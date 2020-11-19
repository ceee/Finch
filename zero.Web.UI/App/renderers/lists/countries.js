
import List from 'zero/core/list.ts';
import CountriesApi from 'zero/resources/countries.js';

const list = new List('countries');
const prefix = '@country.fields.';

list.templateLabel = x => prefix + x;
list.link = zero.alias.settings.countries + '-edit';

list.onFetch(filter => CountriesApi.getByQuery(filter));

list.column('flag', { width: 62, canSort: false, hideLabel: true }).custom((value, model) => `<i class="flag flag-${model.code.toLowerCase()}"></i>`, true, 'flag');
list.column('name').name();
list.column('code').text();
list.column('isPreferred', { width: 200 }).boolean();
list.column('isActive').active();

export default list;