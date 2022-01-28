
import { ZeroEditor } from "../../../editor/editor";

const editor = new ZeroEditor('applications');

editor.resourcePrefix = '@application.fields.';

const general = editor.tab('general', '@ui.tab_general');
const domains = editor.tab('domains', '@application.tab_domains');
domains.count = x => x.domains && x.domains.length;

//const features = editor.tab('features', '@application.tab_features', x => x.features.length);

general.field('name', { label: '@ui.name' }).text({ maxLength: 50 });
general.field('fullName', { optional: true }).text({ maxLength: 120 });
general.field('email').text({ maxLength: 120 });
general.field('imageId').image();
general.field('iconId', { optional: true }).image();
domains.field('domains', { helpText: '@application.fields.domains_help' }).inputlist({ limit: 10, maxItemLength: 320, addLabel: '@application.fields.domains_add' });
//features.field('features', { hideLabel: true }).custom(ApplicationFeatures);

export default editor;