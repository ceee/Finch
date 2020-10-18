
import Editor from '../core/editor.js';
import ApplicationFeatures from '../pages/settings/application-features.vue';

const editor = new Editor('application');
const prefix = '@application.fields.';

editor.templateLabel = x => prefix + x;
editor.templateDescription = x => prefix + x + '_text';

const general = editor.tab('general', '@ui.tab_general');
const domains = editor.tab('domains', '@application.tab_domains', x => x.domains.length);
const features = editor.tab('features', '@application.tab_features', x => x.features.length);

general.field('name', { label: '@ui.name' }).text(50).required();
general.field('fullName').text(120);
general.field('email').text(120).required();
general.field('imageId').image();
general.field('iconId').image();
domains.field('domains', { helpText: prefix + 'domains_help' }).inputList(10, null, prefix + 'domains_add').required();
features.field('features').custom(ApplicationFeatures);

export default editor;