
import Editor from '../core/editor.js';
import ApplicationFeatures from '../pages/settings/application-features.vue';

const editor = new Editor('application');
const prefix = '@application.fields.';

editor.onLabelCreate = x => prefix + x;
editor.onDescriptionCreate = x => prefix + x + '_text';

editor.tab('general', '@ui.tab_general');
const domains = editor.tab('domains', '@application.tab_domains', x => x.domains.length);
const features = editor.tab('features', '@application.tab_features', x => x.features.length);

editor.field('name', { label: '@ui.name' }).text(50);
editor.field('fullName').text(120);
editor.field('email').text(120);
editor.field('imageId').image();
editor.field('iconId').image();
domains.field('domains', { helpText: prefix + 'domains_help' }).inputList(10, null, prefix + 'domains_add');
features.field('features').component(ApplicationFeatures);

export default editor;