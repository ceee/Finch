
import Editor from 'zero/core/editor.js';
import Permissions from 'zero/components/permissions.vue';

const editor = new Editor('user', '@user.fields.');

const permissionsCount = x => (x.claims || []).filter(claim =>
{
  const value = claim.value.split(':')[1];
  return value !== 'none' && value !== 'false' && !!value;
}).length;

const general = editor.tab('general', '@ui.tab_general');
const permissions = editor.tab('permissions', '@user.tab_permissions', permissionsCount);

general.field('name', { label: '@ui.name' }).text(80).required();
general.field('email').text(120).required();
general.field('languageId').culturePicker().required();
general.field('avatarId').image();
permissions.field('claims', { hideLabel: true }).custom(Permissions);

export default editor;