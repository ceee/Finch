
import Editor from '../core/editor.js';
import ApplicationFeatures from '../pages/settings/application-features.vue';

//import Text from 'text.vue';

const editor = new Editor('application');

editor.onLabelCreate = field => '@application.fields.' + field;
editor.onDescriptionCreate = field => '@application.fields.' + field + '_text';

editor.tab('general', '@ui.tab_general');
const domains = editor.tab('domains', '@application.tab_domains', x => x.domains.length);
const features = editor.tab('features', '@application.tab_features', x => x.features.length);

editor.field('name', new Text(50), { label: '@ui.name' });
editor.field('fullName', new Text(120));
editor.field('email', new Text(120));
editor.field('imageId', new Image());
editor.field('iconId', new Image());

domains.field('domains', new InputList({
  limit: 10,
  addLabel: '@application.fields.domains_add',
  helpText: '@application.fields.domains_help'
}));

features.field('features', ApplicationFeatures);

export default editor;