
import Editor from 'zero/core/editor.ts';
import ApplicationFeatures from 'zero/pages/settings/application-features.vue';

const editor = new Editor('application', '@application.fields.');
editor.options.coreDatabase = true;

const general = editor.tab('general', '@ui.tab_general');
const domains = editor.tab('domains', '@application.tab_domains', x => x.domains.length);
const features = editor.tab('features', '@application.tab_features', x => x.features.length);

general.field('name', { label: '@ui.name' }).text(50).required();
general.field('fullName').text(120);
general.field('email').text(120).required();
general.field('imageId').image();
general.field('iconId').image();
domains.field('domains', { helpText: '@application.fields.domains_help' }).inputList(10, null, '@application.fields.domains_add').required();
features.field('features').custom(ApplicationFeatures);

export default editor;