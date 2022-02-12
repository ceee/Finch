
import { ZeroEditor } from "../../../editor/editor";
import UiPasswordHash from '../../../components/ui-password-hash.vue';
//import Permissions from 'zero/components/permissions.vue';

const editor = new ZeroEditor('users');

editor.resourcePrefix = '@user.fields.';

//const permissionsCount = x => (x.claims || []).filter(claim =>
//{
//  const value = claim.value.split(':')[1];
//  return value !== 'none' && value !== 'false' && !!value;
//}).length;

const general = editor.tab('general', '@ui.tab_general');
const permissions = editor.tab('permissions', '@user.tab_permissions');
permissions.hidden = x => !x.id;

general.field('name', { label: '@ui.name' }).text({ maxLength: 80 });
general.field('email').text({ maxLength: 120 });
general.field('passwordHash', { hidden: x => !x.id }).component(UiPasswordHash);
general.field('languageId').culturePicker();
general.field('avatarId', { optional: true }).image();
//permissions.field('claims', { hideLabel: true }).custom(Permissions);

export default editor;