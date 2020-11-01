
import Editor from 'zero/core/editor.ts';
import Permissions from 'zero/components/permissions.vue';

const editor = new Editor('userRole', '@role.fields.');

const permissionsCount = x => (x.claims || []).filter(claim =>
{
  const value = claim.value.split(':')[1];
  return value !== 'none' && value !== 'false' && !!value;
}).length;

const general = editor.tab('general', '@ui.tab_general');
const permissions = editor.tab('permissions', '@role.tab_permissions', permissionsCount);

general.field('name', { label: '@ui.name' }).text(60).required();
general.field('description').text(120);
general.field('icon').iconPicker();
general.field('avatarId').image();
permissions.field('claims', { hideLabel: true }).custom(Permissions);

export default editor;